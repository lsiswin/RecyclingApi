using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.Common;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.Company;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Repositories;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TeamMemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region 团队成员类型管理

        /// <summary>
        /// 获取所有团队成员类型列表
        /// </summary>
        public async Task<List<TeamMemberTypeDto>> GetTeamMemberTypesAsync()
        {
            var repository = _unitOfWork.GetRepository<TeamMemberType>();
            var entities = await repository.GetAsync(t => true, "TeamMembers");
            var sortedEntities = entities.OrderBy(t => t.Sort).ToList();
            
            return _mapper.Map<List<TeamMemberTypeDto>>(sortedEntities);
        }

        /// <summary>
        /// 获取团队成员类型详情
        /// </summary>
        public async Task<TeamMemberTypeDto> GetTeamMemberTypeByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<TeamMemberType>();
            var entity = await repository.GetByIdAsync(id, "TeamMembers");

            if (entity == null)
                return null;

            return _mapper.Map<TeamMemberTypeDto>(entity);
        }

        /// <summary>
        /// 创建团队成员类型
        /// </summary>
        public async Task<TeamMemberTypeDto> CreateTeamMemberTypeAsync(CreateUpdateTeamMemberTypeDto dto)
        {
            var repository = _unitOfWork.GetRepository<TeamMemberType>();
            var entity = _mapper.Map<TeamMemberType>(dto);
            entity.CreatedAt = DateTime.Now;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TeamMemberTypeDto>(entity);
        }

        /// <summary>
        /// 更新团队成员类型
        /// </summary>
        public async Task<TeamMemberTypeDto> UpdateTeamMemberTypeAsync(int id, CreateUpdateTeamMemberTypeDto dto)
        {
            var repository = _unitOfWork.GetRepository<TeamMemberType>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员类型");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TeamMemberTypeDto>(entity);
        }

        /// <summary>
        /// 删除团队成员类型
        /// </summary>
        public async Task<bool> DeleteTeamMemberTypeAsync(int id)
        {
            var teamMemberRepository = _unitOfWork.GetRepository<TeamMember>();
            var typeRepository = _unitOfWork.GetRepository<TeamMemberType>();
            
            // 检查是否有团队成员使用此类型
            bool hasMembers = await teamMemberRepository.ExistsAsync(m => m.TeamMemberTypeId == id);
            if (hasMembers)
                return false; // 有关联的团队成员，不能删除

            var entity = await typeRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                typeRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        #endregion

        #region 团队成员管理

        /// <summary>
        /// 获取团队成员列表
        /// </summary>
        public async Task<List<TeamMemberDto>> GetTeamMembersAsync(int? typeId = null)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            IEnumerable<TeamMember> entities;

            if (typeId.HasValue)
            {
                entities = await repository.GetAsync(m => m.TeamMemberTypeId == typeId.Value, "TeamMemberType");
            }
            else
            {
                entities = await repository.GetAsync(m => true, "TeamMemberType");
            }

            // 排序：先按类型排序优先级，再按成员排序优先级
            var sortedEntities = entities
                .OrderBy(m => m.TeamMemberType.Sort)
                .ThenBy(m => m.Sort)
                .ToList();

            return _mapper.Map<List<TeamMemberDto>>(sortedEntities);
        }

        /// <summary>
        /// 获取团队成员分页列表
        /// </summary>
        public async Task<PagedResponseDto<TeamMemberDto>> GetPagedTeamMembersAsync(int pageIndex, int pageSize, int? typeId = null)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            Expression<Func<TeamMember, bool>> predicate = null;
            
            if (typeId.HasValue)
            {
                predicate = m => m.TeamMemberTypeId == typeId.Value;
            }

            var result = await repository.GetPagedListAsync(
                pageIndex,
                pageSize,
                predicate,
                m => m.Sort,
                false,
                "TeamMemberType");

            var dtoItems = _mapper.Map<List<TeamMemberDto>>(result.Items);

            return new PagedResponseDto<TeamMemberDto>
            {
                Items = dtoItems,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }

        /// <summary>
        /// 获取团队成员详情
        /// </summary>
        public async Task<TeamMemberDto> GetTeamMemberByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = await repository.GetByIdAsync(id, "TeamMemberType");

            if (entity == null)
                return null;

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 创建团队成员
        /// </summary>
        public async Task<TeamMemberDto> CreateTeamMemberAsync(CreateUpdateTeamMemberDto dto)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = _mapper.Map<TeamMember>(dto);
            entity.CreatedAt = DateTime.Now;
            
            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entity = await repository.GetByIdAsync(entity.Id, "TeamMemberType");

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 更新团队成员
        /// </summary>
        public async Task<TeamMemberDto> UpdateTeamMemberAsync(int id, CreateUpdateTeamMemberDto dto)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entity = await repository.GetByIdAsync(id, "TeamMemberType");

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 删除团队成员
        /// </summary>
        public async Task<bool> DeleteTeamMemberAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return false;

            repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 获取所有员工简略信息（用于选择关联）
        /// </summary>
        public async Task<List<EmployeeSimpleDto>> GetAllEmployeesSimpleAsync()
        {
            // 这是一个复杂查询，直接使用仓储可能不太合适，保留原实现
            // 或者需要另外创建Employee实体和仓储
            using var context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
            
            var employees = await context.Employees
                .OrderBy(e => e.Name)
                .Select(e => new EmployeeSimpleDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Position = e.Position
                })
                .ToListAsync();

            return employees;
        }

        #endregion
    }
} 