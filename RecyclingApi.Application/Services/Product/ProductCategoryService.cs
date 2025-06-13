using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.Products;

namespace RecyclingApi.Application.Services.Product
{
    /// <summary>
    /// 产品分类服务实现类
    /// </summary>
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productCategoryRepository">产品分类仓储</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="mapper">对象映射器</param>
        public ProductCategoryService(
            IRepository<ProductCategory> productCategoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository ?? throw new ArgumentNullException(nameof(productCategoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 创建产品分类
        /// </summary>
        /// <param name="input">产品分类信息</param>
        /// <returns>创建的产品分类</returns>
        public async Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // 创建产品分类实体
            var category = _mapper.Map<ProductCategory>(input);
            category.IsActive = true;
            category.CreatedAt = DateTime.Now;

            // 保存到数据库
            _productCategoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync();

            // 返回创建的产品分类
            return _mapper.Map<ProductCategoryDto>(category);
        }

        /// <summary>
        /// 删除产品分类
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("产品分类ID必须大于0", nameof(id));
            }

            // 检查产品分类是否存在
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            // 删除产品分类
            _productCategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 获取所有激活的产品分类
        /// </summary>
        /// <returns>产品分类列表</returns>
        public async Task<List<ProductCategoryDto>> GetAllActiveAsync()
        {
            // 获取所有激活的产品分类
            var categories = await _productCategoryRepository.GetAsync(c => c.IsActive);

            // 映射为DTO
            return _mapper.Map<List<ProductCategoryDto>>(categories);
        }

        /// <summary>
        /// 根据ID获取产品分类
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <returns>产品分类</returns>
        public async Task<ProductCategoryDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("产品分类ID必须大于0", nameof(id));
            }

            // 获取产品分类
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            // 映射为DTO
            return _mapper.Map<ProductCategoryDto>(category);
        }

        /// <summary>
        /// 获取分页产品分类列表
        /// </summary>
        /// <param name="requestDto">请求参数</param>
        /// <returns>分页结果</returns>
        public async Task<PagedResponseDto<ProductCategoryDto>> GetPagedListAsync(ProductCategoryRequestDto requestDto)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException(nameof(requestDto));
            }

            // 验证分页参数
            int pageNumber = requestDto.PageIndex <= 0 ? 1 : requestDto.PageIndex;
            int pageSize = requestDto.PageSize <= 0 ? 10 : (requestDto.PageSize > 100 ? 100 : requestDto.PageSize);

            // 构建查询条件
            Expression<Func<ProductCategory, bool>> predicate = null;

            if (requestDto.IsActive == false)
            {
                predicate = c => c.IsActive;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                var keyword = requestDto.Keyword.Trim();
                Expression<Func<ProductCategory, bool>> keywordPredicate = c =>
                    c.Name.Contains(keyword) || 
                    c.Description.Contains(keyword);

                predicate = predicate == null 
                    ? keywordPredicate 
                    : Expression.Lambda<Func<ProductCategory, bool>>(
                        Expression.AndAlso(
                            predicate.Body,
                            Expression.Invoke(keywordPredicate, predicate.Parameters)
                        ),
                        predicate.Parameters
                    );
            }

            // 获取分页数据
            var pagedResult = await _productCategoryRepository.GetPagedListAsync(
                pageNumber,
                pageSize,
                predicate,
                c => c.CreatedAt,
                false);

            // 映射为DTO
            var categoryDtos = _mapper.Map<List<ProductCategoryDto>>(pagedResult.Items);

            // 返回分页结果
            return new PagedResponseDto<ProductCategoryDto>
            {
                PageIndex = pagedResult.PageIndex,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages,
                Items = categoryDtos
            };
        }

        /// <summary>
        /// 切换产品分类状态
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <returns>更新后的产品分类</returns>
        public async Task<ProductCategoryDto> ToggleStatusAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("产品分类ID必须大于0", nameof(id));
            }

            // 获取产品分类
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            // 切换状态
            category.IsActive = !category.IsActive;
            category.UpdatedAt = DateTime.Now;

            // 更新数据库
            _productCategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            // 返回更新后的产品分类
            return _mapper.Map<ProductCategoryDto>(category);
        }

        /// <summary>
        /// 更新产品分类
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <param name="input">更新信息</param>
        /// <returns>更新后的产品分类</returns>
        public async Task<ProductCategoryDto> UpdateAsync(int id, CreateUpdateProductCategoryDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (id <= 0)
            {
                throw new ArgumentException("产品分类ID必须大于0", nameof(id));
            }

            // 获取产品分类
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            // 更新属性
            _mapper.Map(input, category);
            category.UpdatedAt = DateTime.Now;

            // 更新数据库
            _productCategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            // 返回更新后的产品分类
            return _mapper.Map<ProductCategoryDto>(category);
        }
    }
}
