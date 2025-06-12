using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Domain.Entities.Content;
using RecyclingApi.Domain.Entities.Data;
using AutoMapper;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 公司信息服务实现类
    /// </summary>
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyInfoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        public async Task<CompanyInfoDto> GetCompanyInfoAsync()
        {
            var entity = await _context.CompanyInfos.FirstOrDefaultAsync();
            if (entity == null)
                return null;

            return _mapper.Map<CompanyInfoDto>(entity);
        }

        /// <summary>
        /// 更新公司信息
        /// </summary>
        public async Task<CompanyInfoDto> UpdateCompanyInfoAsync(UpdateCompanyInfoDto input)
        {
            var entity = await _context.CompanyInfos.FirstOrDefaultAsync();
            
            if (entity == null)
            {
                // 如果不存在，则创建新的
                entity = _mapper.Map<CompanyInfo>(input);
                entity.CreatedAt = DateTime.Now;
                _context.CompanyInfos.Add(entity);
            }
            else
            {
                // 如果存在，则更新
                _mapper.Map(input, entity);
                entity.UpdatedAt = DateTime.Now;
                _context.CompanyInfos.Update(entity);
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<CompanyInfoDto>(entity);
        }

        #region 公司优势

        /// <summary>
        /// 获取所有公司优势
        /// </summary>
        public async Task<List<CompanyAdvantageDto>> GetAllAdvantagesAsync()
        {
            var entities = await _context.CompanyAdvantages
                .OrderBy(a => a.Sort)
                .ToListAsync();

            return _mapper.Map<List<CompanyAdvantageDto>>(entities);
        }

        /// <summary>
        /// 根据ID获取公司优势
        /// </summary>
        public async Task<CompanyAdvantageDto> GetAdvantageByIdAsync(int id)
        {
            var entity = await _context.CompanyAdvantages.FindAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<CompanyAdvantageDto>(entity);
        }

        /// <summary>
        /// 创建公司优势
        /// </summary>
        public async Task<CompanyAdvantageDto> CreateAdvantageAsync(CreateUpdateAdvantageDto input)
        {
            var entity = _mapper.Map<CompanyAdvantage>(input);
            entity.CreatedAt = DateTime.Now;

            _context.CompanyAdvantages.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyAdvantageDto>(entity);
        }

        /// <summary>
        /// 更新公司优势
        /// </summary>
        public async Task<CompanyAdvantageDto> UpdateAdvantageAsync(int id, CreateUpdateAdvantageDto input)
        {
            var entity = await _context.CompanyAdvantages.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的公司优势");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            _context.CompanyAdvantages.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyAdvantageDto>(entity);
        }

        /// <summary>
        /// 删除公司优势
        /// </summary>
        public async Task<bool> DeleteAdvantageAsync(int id)
        {
            var entity = await _context.CompanyAdvantages.FindAsync(id);
            if (entity == null)
                return false;

            _context.CompanyAdvantages.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region 公司发展历程

        /// <summary>
        /// 获取所有公司发展历程
        /// </summary>
        public async Task<List<CompanyMilestoneDto>> GetAllMilestonesAsync()
        {
            var entities = await _context.CompanyMilestones
                .OrderBy(m => m.Sort)
                .ToListAsync();

            return _mapper.Map<List<CompanyMilestoneDto>>(entities);
        }

        /// <summary>
        /// 根据ID获取公司发展历程
        /// </summary>
        public async Task<CompanyMilestoneDto> GetMilestoneByIdAsync(int id)
        {
            var entity = await _context.CompanyMilestones.FindAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<CompanyMilestoneDto>(entity);
        }

        /// <summary>
        /// 创建公司发展历程
        /// </summary>
        public async Task<CompanyMilestoneDto> CreateMilestoneAsync(CreateUpdateMilestoneDto input)
        {
            var entity = _mapper.Map<CompanyMilestone>(input);
            entity.CreatedAt = DateTime.Now;

            _context.CompanyMilestones.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyMilestoneDto>(entity);
        }

        /// <summary>
        /// 更新公司发展历程
        /// </summary>
        public async Task<CompanyMilestoneDto> UpdateMilestoneAsync(int id, CreateUpdateMilestoneDto input)
        {
            var entity = await _context.CompanyMilestones.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的公司发展历程");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            _context.CompanyMilestones.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyMilestoneDto>(entity);
        }

        /// <summary>
        /// 删除公司发展历程
        /// </summary>
        public async Task<bool> DeleteMilestoneAsync(int id)
        {
            var entity = await _context.CompanyMilestones.FindAsync(id);
            if (entity == null)
                return false;

            _context.CompanyMilestones.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region 团队成员

        /// <summary>
        /// 获取所有团队成员
        /// </summary>
        public async Task<List<TeamMemberDto>> GetAllTeamMembersAsync()
        {
            var entities = await _context.TeamMembers
                .Include(t => t.TeamMemberType)
                .Include(t => t.Employee)
                .OrderBy(t => t.TeamMemberType.Sort)
                .ThenBy(t => t.Sort)
                .ToListAsync();

            return _mapper.Map<List<TeamMemberDto>>(entities);
        }

        /// <summary>
        /// 根据ID获取团队成员
        /// </summary>
        public async Task<TeamMemberDto> GetTeamMemberByIdAsync(int id)
        {
            var entity = await _context.TeamMembers
                .Include(t => t.TeamMemberType)
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null)
                return null;

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 创建团队成员
        /// </summary>
        public async Task<TeamMemberDto> CreateTeamMemberAsync(CreateUpdateTeamMemberDto input)
        {
            var entity = _mapper.Map<TeamMember>(input);
            entity.CreatedAt = DateTime.Now;

            _context.TeamMembers.Add(entity);
            await _context.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entity = await _context.TeamMembers
                .Include(t => t.TeamMemberType)
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.Id == entity.Id);

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 更新团队成员
        /// </summary>
        public async Task<TeamMemberDto> UpdateTeamMemberAsync(int id, CreateUpdateTeamMemberDto input)
        {
            var entity = await _context.TeamMembers.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            _context.TeamMembers.Update(entity);
            await _context.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entity = await _context.TeamMembers
                .Include(t => t.TeamMemberType)
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 删除团队成员
        /// </summary>
        public async Task<bool> DeleteTeamMemberAsync(int id)
        {
            var entity = await _context.TeamMembers.FindAsync(id);
            if (entity == null)
                return false;

            _context.TeamMembers.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
} 