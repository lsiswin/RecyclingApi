using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Application.Services.HR
{
    /// <summary>
    /// 职位服务实现类
    /// </summary>
    public class JobPositionService : IJobPositionService
    {
        private readonly IRepository<JobPosition> _jobPositionRepository;
        private readonly IRepository<JobApplication> _jobApplicationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jobPositionRepository">职位仓储</param>
        /// <param name="jobApplicationRepository">职位申请仓储</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="mapper">对象映射器</param>
        public JobPositionService(
            IRepository<JobPosition> jobPositionRepository,
            IRepository<JobApplication> jobApplicationRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _jobPositionRepository = jobPositionRepository ?? throw new ArgumentNullException(nameof(jobPositionRepository));
            _jobApplicationRepository = jobApplicationRepository ?? throw new ArgumentNullException(nameof(jobApplicationRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 获取所有职位
        /// </summary>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>职位DTO列表</returns>
        public async Task<List<JobPositionDTO>> GetAllJobPositionsAsync(bool includeInactive = false)
        {
            var positions = await _jobPositionRepository.GetAsync(
                p => includeInactive || p.IsActive,
                "Applications");

            var positionDtos = _mapper.Map<List<JobPositionDTO>>(positions);
            
            // 计算每个职位的申请人数
            foreach (var dto in positionDtos)
            {
                var position = positions.FirstOrDefault(p => p.Id == dto.Id);
                if (position != null)
                {
                    dto.ApplicationCount = position.Applications?.Count ?? 0;
                }
            }

            return positionDtos;
        }

        /// <summary>
        /// 获取职位列表（分页）
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>分页结果</returns>
        public async Task<PagedResponseDto<JobPositionDTO>> GetJobPositionsAsync(int pageNumber, int pageSize, bool includeInactive = false)
        {
            // 验证参数
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;            // 设置查询条件
            Expression<Func<JobPosition, bool>> predicate = includeInactive ? null : p => p.IsActive;

            // 获取分页结果 
            var pagedResult = await _jobPositionRepository.GetPagedListAsync(
                pageNumber,
                pageSize,
                predicate,
                p => p.PostedDate,
                true);

            // 映射结果
            var positionDtos = _mapper.Map<List<JobPositionDTO>>(pagedResult.Items);

            // 计算每个职位的申请人数
            foreach (var dto in positionDtos)
            {
                var position = pagedResult.Items.FirstOrDefault(p => p.Id == dto.Id);
                if (position != null)
                {
                    dto.ApplicationCount = position.Applications?.Count ?? 0;
                }
            }

            // 返回分页结果
            return new PagedResponseDto<JobPositionDTO>
            {
                PageIndex = pagedResult.PageIndex,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages,
                Items = positionDtos
            };
        }

        /// <summary>
        /// 根据部门获取职位列表
        /// </summary>
        /// <param name="department">部门名称</param>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>职位DTO列表</returns>
        public async Task<List<JobPositionDTO>> GetJobPositionsByDepartmentAsync(string department, bool includeInactive = false)
        {
            if (string.IsNullOrEmpty(department))
            {
                throw new ArgumentException("部门名称不能为空", nameof(department));
            }

            // 获取指定部门的职位
            var positions = await _jobPositionRepository.GetAsync(
                p => (includeInactive || p.IsActive) && p.Department == department,
                "Applications");

            var positionDtos = _mapper.Map<List<JobPositionDTO>>(positions);

            // 计算每个职位的申请人数
            foreach (var dto in positionDtos)
            {
                var position = positions.FirstOrDefault(p => p.Id == dto.Id);
                if (position != null)
                {
                    dto.ApplicationCount = position.Applications?.Count ?? 0;
                }
            }

            return positionDtos;
        }

        /// <summary>
        /// 根据ID获取职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>职位DTO</returns>
        public async Task<JobPositionDTO> GetJobPositionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("职位ID必须大于0", nameof(id));
            }

            // 获取职位
            var position = await _jobPositionRepository.GetByIdAsync(id, "Applications");
            if (position == null)
            {
                return null;
            }

            // 映射为DTO
            var positionDto = _mapper.Map<JobPositionDTO>(position);
            positionDto.ApplicationCount = position.Applications?.Count ?? 0;

            return positionDto;
        }

        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="dto">创建职位DTO</param>
        /// <returns>创建的职位ID</returns>
        public async Task<int> CreateJobPositionAsync(CreateJobPositionDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // 创建职位实体
            var position = _mapper.Map<JobPosition>(dto);
            position.PostedDate = DateTime.Now;

            // 保存到数据库
            _jobPositionRepository.Add(position);
            await _unitOfWork.SaveChangesAsync();

            return position.Id;
        }

        /// <summary>
        /// 更新职位
        /// </summary>
        /// <param name="dto">更新职位DTO</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateJobPositionAsync(UpdateJobPositionDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // 获取职位
            var position = await _jobPositionRepository.GetByIdAsync(dto.Id);
            if (position == null)
            {
                return false;
            }

            // 更新职位信息
            position.Title = dto.Title;
            position.Description = dto.Description;
            position.Requirements = dto.Requirements;
            position.Department = dto.Department;
            position.Location = dto.Location;
            position.SalaryMin = dto.SalaryMin;
            position.SalaryMax = dto.SalaryMax;
            position.IsActive = dto.IsActive;
            position.ExpiryDate = dto.ExpiryDate;

            // 保存到数据库
            _jobPositionRepository.Update(position);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteJobPositionAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("职位ID必须大于0", nameof(id));
            }

            // 获取职位
            var position = await _jobPositionRepository.GetByIdAsync(id);
            if (position == null)
            {
                return false;
            }

            // 检查是否有人申请该职位
            var hasApplications = await _jobApplicationRepository.ExistsAsync(a => a.JobPositionId == id);
            if (hasApplications)
            {
                // 如果有人申请该职位，则只将其设为未激活状态
                position.IsActive = false;
                _jobPositionRepository.Update(position);
            }
            else
            {
                // 如果没有人申请该职位，则直接删除
                _jobPositionRepository.Delete(position);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 更改职位状态
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <param name="isActive">是否激活</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> ChangeJobPositionStatusAsync(int id, bool isActive)
        {
            if (id <= 0)
            {
                throw new ArgumentException("职位ID必须大于0", nameof(id));
            }

            // 获取职位
            var position = await _jobPositionRepository.GetByIdAsync(id);
            if (position == null)
            {
                return false;
            }

            // 更新状态
            position.IsActive = isActive;
            _jobPositionRepository.Update(position);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 获取职位的申请数量
        /// </summary>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>申请数量</returns>
        public async Task<int> GetJobPositionApplicationCountAsync(int jobPositionId)
        {
            if (jobPositionId <= 0)
            {
                throw new ArgumentException("职位ID必须大于0", nameof(jobPositionId));
            }

            // 获取申请数量
            var applications = await _jobApplicationRepository.GetAsync(a => a.JobPositionId == jobPositionId);
            return applications.Count();
        }
    }
}
