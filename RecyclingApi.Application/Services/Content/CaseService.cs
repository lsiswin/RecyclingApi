using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Domain.Entities.Content;
using RecyclingApi.Domain.Entities.Data;
using AutoMapper;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 案例服务实现类
    /// </summary>
    public class CaseService : ICaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CaseService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取案例分页列表
        /// </summary>
        public async Task<PagedResponseDto<CaseDto>> GetPagedListAsync(CaseRequestDto requestDto)
        {
            var query = _context.Cases.AsQueryable();

            // 应用筛选条件
            if (!string.IsNullOrEmpty(requestDto.Keyword))
            {
                query = query.Where(c => c.Title.Contains(requestDto.Keyword));
            }

            if (!string.IsNullOrEmpty(requestDto.Category))
            {
                query = query.Where(c => c.Category == requestDto.Category);
            }

            if (requestDto.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == requestDto.IsActive.Value);
            }

            // 应用排序
            query = requestDto.IsDesc == true
                ? query.OrderByDescending(c => c.Sort).ThenByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.Sort).ThenByDescending(c => c.CreatedAt);

            // 计算总数
            var totalCount = await query.CountAsync();

            // 应用分页
            var items = await query
                .Skip((requestDto.PageIndex - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize)
                .ToListAsync();

            // 映射结果
            var dtos = _mapper.Map<List<CaseDto>>(items);

            return new PagedResponseDto<CaseDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize
            };
        }

        /// <summary>
        /// 根据ID获取案例
        /// </summary>
        public async Task<CaseDto> GetByIdAsync(int id)
        {
            var entity = await _context.Cases.FindAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 创建案例
        /// </summary>
        public async Task<CaseDto> CreateAsync(CreateUpdateCaseDto input)
        {
            var entity = _mapper.Map<Case>(input);
            entity.CreatedAt = DateTime.Now;

            _context.Cases.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 更新案例
        /// </summary>
        public async Task<CaseDto> UpdateAsync(int id, CreateUpdateCaseDto input)
        {
            var entity = await _context.Cases.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            _context.Cases.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 删除案例
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Cases.FindAsync(id);
            if (entity == null)
                return false;

            _context.Cases.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 切换案例状态
        /// </summary>
        public async Task<CaseDto> ToggleStatusAsync(int id)
        {
            var entity = await _context.Cases.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            entity.IsActive = !entity.IsActive;
            entity.UpdatedAt = DateTime.Now;

            _context.Cases.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CaseDto>(entity);
        }

        /// <summary>
        /// 增加案例浏览次数
        /// </summary>
        public async Task<int> IncrementViewsAsync(int id)
        {
            var entity = await _context.Cases.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的案例");

            entity.Views += 1;
            entity.UpdatedAt = DateTime.Now;

            _context.Cases.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Views;
        }
    }
} 