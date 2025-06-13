using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 轮播图服务实现
    /// </summary>
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="mapper">对象映射器</param>
        public BannerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取轮播图分页列表
        /// </summary>
        public async Task<PagedResponseDto<BannerDto>> GetPagedListAsync(BannerRequestDto requestDto)
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            Expression<Func<Banner, bool>> predicate = null;

            // 构建查询条件
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                predicate = b => b.Title.Contains(requestDto.Keyword) || 
                                b.Description.Contains(requestDto.Keyword);
            }

            if (requestDto.IsActive.HasValue)
            {
                var isActive = requestDto.IsActive.Value;
                predicate = predicate == null 
                    ? b => b.IsActive == isActive
                    : b => b.IsActive == isActive && predicate.Compile()(b);
            }

            // 确定排序字段
            Expression<Func<Banner, object>> orderBy = GetSortProperty(requestDto.SortBy);

            // 获取分页结果
            var result = await repository.GetPagedListAsync(
                requestDto.PageIndex,
                requestDto.PageSize,
                predicate,
                orderBy,
                requestDto.IsDesc);

            // 转换为DTO
            var dtoItems = _mapper.Map<List<BannerDto>>(result.Items);

            return new PagedResponseDto<BannerDto>
            {
                Items = dtoItems,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }

        /// <summary>
        /// 获取所有轮播图列表
        /// </summary>
        public async Task<List<BannerDto>> GetAllAsync()
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entities = await repository.GetAllAsync();
            return _mapper.Map<List<BannerDto>>(entities);
        }

        /// <summary>
        /// 获取所有激活的轮播图列表
        /// </summary>
        public async Task<List<BannerDto>> GetActiveAsync()
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entities = await repository.GetAsync(b => b.IsActive);
            return _mapper.Map<List<BannerDto>>(entities);
        }

        /// <summary>
        /// 根据ID获取轮播图
        /// </summary>
        public async Task<BannerDto> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return null;

            return _mapper.Map<BannerDto>(entity);
        }

        /// <summary>
        /// 创建轮播图
        /// </summary>
        public async Task<BannerDto> CreateAsync(CreateUpdateBannerDto input)
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entity = _mapper.Map<Banner>(input);
            entity.CreatedAt = DateTime.Now;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BannerDto>(entity);
        }

        /// <summary>
        /// 更新轮播图
        /// </summary>
        public async Task<BannerDto> UpdateAsync(int id, CreateUpdateBannerDto input)
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的轮播图");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BannerDto>(entity);
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return false;

            repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// 切换轮播图状态
        /// </summary>
        public async Task<BannerDto> ToggleStatusAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Banner>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的轮播图");

            // 切换状态
            entity.IsActive = !entity.IsActive;
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BannerDto>(entity);
        }

        /// <summary>
        /// 获取排序属性
        /// </summary>
        private static Expression<Func<Banner, object>> GetSortProperty(string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "title" => b => b.Title,
                "sort" => b => b.Sort,
                "createdat" => b => b.CreatedAt,
                "isactive" => b => b.IsActive,
                _ => b => b.Id
            };
        }
    }
} 