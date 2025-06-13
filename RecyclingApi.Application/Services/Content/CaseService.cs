using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 案例服务实现类
    /// </summary>
    public class CaseService : ICaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取案例分页列表
        /// </summary>
        public async Task<PagedResponseDto<CaseDto>> GetPagedListAsync(CaseRequestDto requestDto)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            
            // 构建默认表达式，始终为true
            Expression<Func<Case, bool>> predicate = c => true;

            // 应用筛选条件
            if (!string.IsNullOrEmpty(requestDto.Keyword))
            {
                predicate = CombineExpressions(predicate, c => c.Title.Contains(requestDto.Keyword));
            }

            if (!string.IsNullOrEmpty(requestDto.Category))
            {
                predicate = CombineExpressions(predicate, c => c.Category == requestDto.Category);
            }

            if (requestDto.IsActive.HasValue)
            {
                predicate = CombineExpressions(predicate, c => c.IsActive == requestDto.IsActive.Value);
            }

            // 确定排序字段
            Expression<Func<Case, object>> orderBy = c => c.Sort;
            var result = await repository.GetPagedListAsync(
                requestDto.PageIndex,
                requestDto.PageSize,
                predicate,
                orderBy,
                requestDto.IsDesc);

            // 映射结果
            var dtos = _mapper.Map<List<CaseDto>>(result.Items);

            return new PagedResponseDto<CaseDto>
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize
            };
        }

        /// <summary>
        /// 根据ID获取案例
        /// </summary>
        public async Task<CaseDto> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 创建案例
        /// </summary>
        public async Task<CaseDto> CreateAsync(CreateUpdateCaseDto input)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = _mapper.Map<Case>(input);
            entity.CreatedAt = DateTime.Now;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 更新案例
        /// </summary>
        public async Task<CaseDto> UpdateAsync(int id, CreateUpdateCaseDto input)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 删除案例
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return false;

            repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 切换案例状态
        /// </summary>
        public async Task<CaseDto> ToggleStatusAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            entity.IsActive = !entity.IsActive;
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 增加案例浏览次数
        /// </summary>
        public async Task<int> IncrementViewsAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Case>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            entity.Views += 1;
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Views;
        }

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
    }
}