using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.Common;
using RecyclingApi.Application.DTOs.Company;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Domain.Entities.Content;
using RecyclingApi.Domain.Entities.Data;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Application.Services.Company
{
    /// <summary>
    /// 团队成员服务实现类
    /// </summary>
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TeamMemberService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region 团队成员类型管理

        /// <summary>
        /// 获取所有团队成员类型列表
        /// </summary>
        public async Task<List<TeamMemberTypeDto>> GetTeamMemberTypesAsync()
        {
            var entities = await _context.TeamMemberTypes
                .OrderBy(t => t.Sort)
                .ToListAsync();

            return _mapper.Map<List<TeamMemberTypeDto>>(entities);
        }

        /// <summary>
        /// 获取团队成员类型详情
        /// </summary>
        public async Task<TeamMemberTypeDto> GetTeamMemberTypeByIdAsync(int id)
        {
            var entity = await _context.TeamMemberTypes
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null)
                return null;

            return _mapper.Map<TeamMemberTypeDto>(entity);
        }

        /// <summary>
        /// 创建团队成员类型
        /// </summary>
        public async Task<TeamMemberTypeDto> CreateTeamMemberTypeAsync(CreateUpdateTeamMemberTypeDto dto)
        {
            var entity = _mapper.Map<TeamMemberType>(dto);
            entity.CreatedAt = DateTime.Now;

            _context.TeamMemberTypes.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TeamMemberTypeDto>(entity);
        }

        /// <summary>
        /// 更新团队成员类型
        /// </summary>
        public async Task<TeamMemberTypeDto> UpdateTeamMemberTypeAsync(int id, CreateUpdateTeamMemberTypeDto dto)
        {
            var entity = await _context.TeamMemberTypes.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员类型");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;

            _context.TeamMemberTypes.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TeamMemberTypeDto>(entity);
        }

        /// <summary>
        /// 删除团队成员类型
        /// </summary>
        public async Task<bool> DeleteTeamMemberTypeAsync(int id)
        {
            // 检查是否有团队成员使用此类型
            bool hasMembers = await _context.TeamMembers.AnyAsync(m => m.TeamMemberTypeId == id);
            if (hasMembers)
                return false; // 有关联的团队成员，不能删除

            var entity = await _context.TeamMemberTypes.FindAsync(id);
            if (entity == null)
                return false;

            _context.TeamMemberTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region 团队成员管理

        /// <summary>
        /// 获取团队成员列表
        /// </summary>
        public async Task<List<TeamMemberDto>> GetTeamMembersAsync(int? typeId = null)
        {
            var query = _context.TeamMembers
                .Include(m => m.TeamMemberType)
                .Include(m => m.Employee)
                .AsQueryable();

            // 按类型筛选（如果提供了类型ID）
            if (typeId.HasValue)
            {
                query = query.Where(m => m.TeamMemberTypeId == typeId.Value);
            }

            // 排序：先按类型排序优先级，再按成员排序优先级
            var entities = await query
                .OrderBy(m => m.TeamMemberType.Sort)
                .ThenBy(m => m.Sort)
                .ToListAsync();

            return _mapper.Map<List<TeamMemberDto>>(entities);
        }

        /// <summary>
        /// 获取团队成员详情
        /// </summary>
        public async Task<TeamMemberDto> GetTeamMemberByIdAsync(int id)
        {
            var entity = await _context.TeamMembers
                .Include(m => m.TeamMemberType)
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null)
                return null;

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 创建团队成员
        /// </summary>
        public async Task<TeamMemberDto> CreateTeamMemberAsync(CreateUpdateTeamMemberDto dto)
        {
            var entity = _mapper.Map<TeamMember>(dto);
            entity.CreatedAt = DateTime.Now;
            _context.TeamMembers.Add(entity);
            await _context.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entity = await _context.TeamMembers
                .Include(m => m.TeamMemberType)
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == entity.Id);

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 更新团队成员
        /// </summary>
        public async Task<TeamMemberDto> UpdateTeamMemberAsync(int id, CreateUpdateTeamMemberDto dto)
        {
            var entity = await _context.TeamMembers.FindAsync(id);
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;

            _context.TeamMembers.Update(entity);
            await _context.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entity = await _context.TeamMembers
                .Include(m => m.TeamMemberType)
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

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

        /// <summary>
        /// 获取所有员工简略信息
        /// </summary>
        public async Task<List<EmployeeSimpleDto>> GetAllEmployeesSimpleAsync()
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();

            return _mapper.Map<List<EmployeeSimpleDto>>(employees);
        }

        #endregion
    }
} 