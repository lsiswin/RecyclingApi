using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Domain.Entities.Content;
using RecyclingApi.Domain.Entities.Data;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 轮播图服务实现
    /// </summary>
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _context;

        public BannerService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取轮播图分页列表
        /// </summary>
        public async Task<PagedResponseDto<BannerDto>> GetPagedListAsync(BannerRequestDto requestDto)
        {
            // 构建查询
            var query = _context.Banners.AsQueryable();

            // 应用筛选条件
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                query = query.Where(b => b.Title.Contains(requestDto.Keyword) || 
                                        b.Description.Contains(requestDto.Keyword));
            }

            if (requestDto.IsActive.HasValue)
            {
                query = query.Where(b => b.IsActive == requestDto.IsActive.Value);
            }

            // 应用排序
            query = requestDto.IsDesc 
                ? query.OrderByDescending(GetSortProperty(requestDto.SortBy))
                : query.OrderBy(GetSortProperty(requestDto.SortBy));

            // 获取总记录数
            var totalCount = await query.CountAsync();

            // 应用分页
            var items = await query
                .Skip((requestDto.PageIndex - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize)
                .Select(b => new BannerDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    LinkUrl = b.LinkUrl,
                    Sort = b.Sort,
                    IsActive = b.IsActive,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            // 计算总页数
            var totalPages = (int)Math.Ceiling(totalCount / (double)requestDto.PageSize);

            // 构建分页响应
            return new PagedResponseDto<BannerDto>
            {
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = items
            };
        }

        /// <summary>
        /// 根据ID获取轮播图
        /// </summary>
        public async Task<BannerDto> GetByIdAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return null;

            return new BannerDto
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                Sort = banner.Sort,
                IsActive = banner.IsActive,
                CreatedAt = banner.CreatedAt
            };
        }

        /// <summary>
        /// 创建轮播图
        /// </summary>
        public async Task<BannerDto> CreateAsync(CreateUpdateBannerDto input)
        {
            var banner = new Banner
            {
                Title = input.Title,
                Description = input.Description,
                ImageUrl = input.ImageUrl,
                LinkUrl = input.LinkUrl,
                Sort = input.Sort,
                IsActive = input.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return new BannerDto
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                Sort = banner.Sort,
                IsActive = banner.IsActive,
                CreatedAt = banner.CreatedAt
            };
        }

        /// <summary>
        /// 更新轮播图
        /// </summary>
        public async Task<BannerDto> UpdateAsync(int id, CreateUpdateBannerDto input)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                throw new Exception($"未找到ID为{id}的轮播图");

            // 更新属性
            banner.Title = input.Title;
            banner.Description = input.Description;
            banner.ImageUrl = input.ImageUrl;
            banner.LinkUrl = input.LinkUrl;
            banner.Sort = input.Sort;
            banner.IsActive = input.IsActive;
            banner.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new BannerDto
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                Sort = banner.Sort,
                IsActive = banner.IsActive,
                CreatedAt = banner.CreatedAt
            };
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return false;

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 切换轮播图状态
        /// </summary>
        public async Task<BannerDto> ToggleStatusAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                throw new Exception($"未找到ID为{id}的轮播图");

            // 切换状态
            banner.IsActive = !banner.IsActive;
            banner.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new BannerDto
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                Sort = banner.Sort,
                IsActive = banner.IsActive,
                CreatedAt = banner.CreatedAt
            };
        }

        /// <summary>
        /// 获取排序属性
        /// </summary>
        private static System.Linq.Expressions.Expression<Func<Banner, object>> GetSortProperty(string sortBy)
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