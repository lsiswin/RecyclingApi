using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 公司信息服务实现类
    /// </summary>
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyInfoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        public async Task<CompanyInfoDto> GetCompanyInfoAsync()
        {
            var repository = _unitOfWork.GetRepository<CompanyInfo>();
            Expression<Func<CompanyInfo, bool>> predicate = e => true;
            var entities = await repository.GetAsync(predicate, "Advantages", "Milestones");
            var entity = entities.FirstOrDefault();

            if (entity == null)
                throw new Exception("未找到公司信息");

            return _mapper.Map<CompanyInfoDto>(entity);
        }

        /// <summary>
        /// 更新公司信息
        /// </summary>
        public async Task<CompanyInfoDto> UpdateCompanyInfoAsync(UpdateCompanyInfoDto input)
        {
            var repository = _unitOfWork.GetRepository<CompanyInfo>();
            Expression<Func<CompanyInfo, bool>> predicate = e => true;
            var entities = await repository.GetAsync(predicate, "Advantages", "Milestones");
            var entity = entities.FirstOrDefault();
            
            if (entity == null)
            {
                // 如果不存在，则创建新的
                entity = _mapper.Map<CompanyInfo>(input);
                entity.CreatedAt = DateTime.Now;
                repository.Add(entity);
            }
            else
            {
                // 如果存在，则更新
                _mapper.Map(input, entity);
                entity.UpdatedAt = DateTime.Now;
                repository.Update(entity);
            }

            await _unitOfWork.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            entities = await repository.GetAsync(e => e.Id == entity.Id, "Advantages", "Milestones");
            entity = entities.FirstOrDefault();

            if (entity == null)
                throw new Exception("未找到更新后的公司信息");

            return _mapper.Map<CompanyInfoDto>(entity);
        }

        #region 公司优势

        /// <summary>
        /// 获取所有公司优势
        /// </summary>
        public async Task<List<CompanyAdvantageDto>> GetAllAdvantagesAsync()
        {
            var repository = _unitOfWork.GetRepository<CompanyAdvantage>();
            var entities = await repository.GetAllAsync();
            var sortedEntities = entities.OrderBy(a => a.Sort);
            return _mapper.Map<List<CompanyAdvantageDto>>(sortedEntities);
        }

        /// <summary>
        /// 根据ID获取公司优势
        /// </summary>
        public async Task<CompanyAdvantageDto> GetAdvantageByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<CompanyAdvantage>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的公司优势");

            return _mapper.Map<CompanyAdvantageDto>(entity);
        }

        /// <summary>
        /// 创建公司优势
        /// </summary>
        public async Task<CompanyAdvantageDto> CreateAdvantageAsync(CreateUpdateAdvantageDto input)
        {
            var repository = _unitOfWork.GetRepository<CompanyAdvantage>();
            var entity = _mapper.Map<CompanyAdvantage>(input);
            entity.CreatedAt = DateTime.Now;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyAdvantageDto>(entity);
        }

        /// <summary>
        /// 更新公司优势
        /// </summary>
        public async Task<CompanyAdvantageDto> UpdateAdvantageAsync(int id, CreateUpdateAdvantageDto input)
        {
            var repository = _unitOfWork.GetRepository<CompanyAdvantage>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的公司优势");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyAdvantageDto>(entity);
        }

        /// <summary>
        /// 删除公司优势
        /// </summary>
        public async Task<bool> DeleteAdvantageAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<CompanyAdvantage>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return false;

            repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region 公司发展历程

        /// <summary>
        /// 获取所有公司发展历程
        /// </summary>
        public async Task<List<CompanyMilestoneDto>> GetAllMilestonesAsync()
        {
            var repository = _unitOfWork.GetRepository<CompanyMilestone>();
            var entities = await repository.GetAllAsync();
            var sortedEntities = entities.OrderBy(m => m.Sort);
            return _mapper.Map<List<CompanyMilestoneDto>>(sortedEntities);
        }

        /// <summary>
        /// 根据ID获取公司发展历程
        /// </summary>
        public async Task<CompanyMilestoneDto> GetMilestoneByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<CompanyMilestone>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的公司发展历程");

            return _mapper.Map<CompanyMilestoneDto>(entity);
        }

        /// <summary>
        /// 创建公司发展历程
        /// </summary>
        public async Task<CompanyMilestoneDto> CreateMilestoneAsync(CreateUpdateMilestoneDto input)
        {
            var repository = _unitOfWork.GetRepository<CompanyMilestone>();
            var entity = _mapper.Map<CompanyMilestone>(input);
            entity.CreatedAt = DateTime.Now;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyMilestoneDto>(entity);
        }

        /// <summary>
        /// 更新公司发展历程
        /// </summary>
        public async Task<CompanyMilestoneDto> UpdateMilestoneAsync(int id, CreateUpdateMilestoneDto input)
        {
            var repository = _unitOfWork.GetRepository<CompanyMilestone>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的公司发展历程");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyMilestoneDto>(entity);
        }

        /// <summary>
        /// 删除公司发展历程
        /// </summary>
        public async Task<bool> DeleteMilestoneAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<CompanyMilestone>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                return false;

            repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region 团队成员

        /// <summary>
        /// 获取所有团队成员
        /// </summary>
        public async Task<List<TeamMemberDto>> GetAllTeamMembersAsync()
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            Expression<Func<TeamMember, bool>> predicate = e => true;
            var entities = await repository.GetAsync(predicate, "TeamMemberType");
            var sortedEntities = entities
                .OrderBy(t => t.TeamMemberType?.Sort)
                .ThenBy(t => t.Sort);
            return _mapper.Map<List<TeamMemberDto>>(sortedEntities);
        }

        /// <summary>
        /// 根据ID获取团队成员
        /// </summary>
        public async Task<TeamMemberDto> GetTeamMemberByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = await repository.GetByIdAsync(id, "TeamMemberType");
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员");

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 创建团队成员
        /// </summary>
        public async Task<TeamMemberDto> CreateTeamMemberAsync(CreateUpdateTeamMemberDto input)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = _mapper.Map<TeamMember>(input);
            entity.CreatedAt = DateTime.Now;

            repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            var entities = await repository.GetAsync(t => t.Id == entity.Id, "TeamMemberType");
            entity = entities.FirstOrDefault();

            if (entity == null)
                throw new Exception("未找到创建的团队成员");

            return _mapper.Map<TeamMemberDto>(entity);
        }

        /// <summary>
        /// 更新团队成员
        /// </summary>
        public async Task<TeamMemberDto> UpdateTeamMemberAsync(int id, CreateUpdateTeamMemberDto input)
        {
            var repository = _unitOfWork.GetRepository<TeamMember>();
            var entity = await repository.GetByIdAsync(id);
            
            if (entity == null)
                throw new Exception($"未找到ID为{id}的团队成员");

            _mapper.Map(input, entity);
            entity.UpdatedAt = DateTime.Now;

            repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            // 重新获取包含导航属性的实体
            var entities = await repository.GetAsync(t => t.Id == id, "TeamMemberType");
            entity = entities.FirstOrDefault();

            if (entity == null)
                throw new Exception($"未找到更新后的团队成员");

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

        #endregion
    }
}