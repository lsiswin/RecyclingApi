using System.Linq.Expressions;
using AutoMapper;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Application.Common.Exceptions;
using RecyclingApi.Domain.Entities.Products;
using RecyclingApi.Domain.Entities.Content;
using RecyclingApi.Domain.Enums;
using RecyclingApi.Application.Repositories;

namespace RecyclingApi.Application.Services.Products
{
    /// <summary>
    /// 产品服务实现类
    /// 实现产品相关的业务逻辑操作
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取产品分页列表
        /// </summary>
        public async Task<PagedResponseDto<ProductDto>> GetPagedListAsync(ProductRequestDto requestDto)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            Expression<Func<Domain.Entities.Products.Product, bool>> predicate = p => true;

            // 构建查询条件
            if (!string.IsNullOrEmpty(requestDto.Keyword))
            {
                predicate = CombineExpressions(predicate, 
                    p => p.Name.Contains(requestDto.Keyword) || 
                         p.Description.Contains(requestDto.Keyword));
            }

            if (requestDto.CategoryId.HasValue)
            {
                predicate = CombineExpressions(predicate, 
                    p => p.CategoryId == requestDto.CategoryId.Value);
            }

            // 执行查询
            var result = await repository.GetPagedListAsync(
                requestDto.PageIndex,
                requestDto.PageSize,
                predicate,
                p => p.Sort,
                requestDto.IsDesc);

            // 转换结果
            var dtos = _mapper.Map<List<ProductDto>>(result.Items);

            return new PagedResponseDto<ProductDto>
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize
            };
        }

        /// <summary>
        /// 根据ID获取产品
        /// </summary>
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var entity = await repository.GetByIdAsync(id, "Category");
            
            if (entity == null)
                throw new NotFoundException($"产品ID {id} 未找到");

            return _mapper.Map<ProductDto>(entity);
        }

        /// <summary>
        /// 创建产品
        /// </summary>
        public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var entity = _mapper.Map<Domain.Entities.Products.Product>(input);
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(entity);
        }

        /// <summary>
        /// 更新产品
        /// </summary>
        public async Task<ProductDto> UpdateAsync(int id, CreateUpdateProductDto input)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new NotFoundException($"产品ID {id} 未找到");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(entity);
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return false;

            repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 切换产品状态
        /// </summary>
        public async Task<ProductDto> ToggleStatusAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new NotFoundException($"产品ID {id} 未找到");

            entity.IsActive = !entity.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(entity);
        }

        /// <summary>
        /// 获取所有产品分类
        /// </summary>
        public async Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync()
        {
            var repository = _unitOfWork.GetRepository<ProductCategory>();
            var categories = await repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryDto>>(categories);
        }

        /// <summary>
        /// 根据分类ID获取该分类下的所有产品
        /// </summary>
        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var entities = await repository.GetAsync(p => p.CategoryId == categoryId && p.IsActive);
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        /// <summary>
        /// 根据ID获取产品详细信息
        /// </summary>
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        /// <summary>
        /// 计算产品的估值
        /// </summary>
        public async Task<decimal> CalculateEstimatedValueAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Domain.Entities.Products.Product>();
            var product = await repository.GetByIdAsync(id);
            
            if (product == null)
                throw new NotFoundException($"产品ID {id} 未找到");

            // 基础价值
            decimal baseValue = GetBaseValueByType(product.Type);

            // 年份折旧系数（每年折旧10%，最低保留20%价值）
            int age = DateTime.Now.Year - product.ManufactureYear;
            decimal depreciationRate = Math.Max(0.2m, 1 - age * 0.1m);

            // 条件和品牌系数
            decimal conditionMultiplier = GetConditionMultiplier(product.Condition);
            decimal brandMultiplier = GetBrandMultiplier(product.Brand);

            // 计算最终估值
            return Math.Round(baseValue * depreciationRate * conditionMultiplier * brandMultiplier, 2);
        }

        /// <summary>
        /// 分页查询案例列表
        /// </summary>
        public async Task<PagedResult<CaseListDto>> GetCasesPagedAsync(CaseQueryRequest request)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            Expression<Func<Case, bool>> predicate = c => true;

            // 构建查询条件
            if (!string.IsNullOrEmpty(request.Category))
            {
                predicate = CombineExpressions(predicate, c => c.Category == request.Category);
            }

            if (!string.IsNullOrEmpty(request.DeviceType))
            {
                predicate = CombineExpressions(predicate, c => c.DeviceType == request.DeviceType);
            }

            if (!string.IsNullOrEmpty(request.Scale))
            {
                predicate = CombineExpressions(predicate, c => c.Scale == request.Scale);
            }

            // 执行查询
            var result = await repository.GetPagedListAsync(
                request.Page,
                request.PageSize,
                predicate,
                c => c.CreatedAt,
                true);

            // 转换为DTO
            var items = _mapper.Map<List<CaseListDto>>(result.Items);

            return new PagedResult<CaseListDto>
            {
                Items = items,
                TotalCount = result.TotalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)result.TotalCount / request.PageSize)
            };
        }

        /// <summary>
        /// 根据ID获取案例详细信息
        /// </summary>
        public async Task<CaseDetailDto> GetCaseByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
                throw new NotFoundException($"案例ID {id} 未找到");

            return _mapper.Map<CaseDetailDto>(entity);
        }

        #region 私有辅助方法

        /// <summary>
        /// 合并两个表达式树
        /// </summary>
        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left!, right!), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }

        /// <summary>
        /// 根据产品类型获取基础价值
        /// </summary>
        private static decimal GetBaseValueByType(ProductType productType)
        {
            return productType switch
            {
                ProductType.Desktop => 3000m,
                ProductType.Laptop => 5000m,
                ProductType.Mobile => 4000m,
                ProductType.Tablet => 2500m,
                ProductType.Server => 15000m,
                ProductType.NetworkEquipment => 3500m,
                ProductType.Monitor => 1500m,
                ProductType.Printer => 1000m,
                _ => 1000m
            };
        }

        /// <summary>
        /// 根据设备条件获取条件系数
        /// </summary>
        private static decimal GetConditionMultiplier(string? condition)
        {
            return condition?.ToLower() switch
            {
                "优秀" or "excellent" => 1.0m,
                "良好" or "good" => 0.8m,
                "一般" or "fair" => 0.6m,
                "较差" or "poor" => 0.4m,
                "损坏" or "damaged" => 0.2m,
                _ => 0.7m
            };
        }

        /// <summary>
        /// 根据品牌获取品牌系数
        /// </summary>
        private static decimal GetBrandMultiplier(string? brand)
        {
            return brand?.ToLower() switch
            {
                "apple" => 1.3m,
                "dell" => 1.1m,
                "hp" => 1.05m,
                "lenovo" => 1.1m,
                "asus" => 1.05m,
                "acer" => 0.95m,
                _ => 1.0m
            };
        }

        #endregion
    }
}