using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.HR;
using RecyclingApi.Domain.Entities.User;

namespace RecyclingApi.Application.Services.HR
{
    /// <summary>
    /// 职位申请服务实现
    /// </summary>
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IRepository<JobApplication> _jobApplicationRepository;
        private readonly IRepository<JobPosition> _jobPositionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jobApplicationRepository">职位申请仓储</param>
        /// <param name="jobPositionRepository">职位仓储</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="mapper">对象映射器</param>
        public JobApplicationService(
            IRepository<JobApplication> jobApplicationRepository,
            IRepository<JobPosition> jobPositionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _jobApplicationRepository = jobApplicationRepository ?? throw new ArgumentNullException(nameof(jobApplicationRepository));
            _jobPositionRepository = jobPositionRepository ?? throw new ArgumentNullException(nameof(jobPositionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 获取职位申请列表（分页）
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页结果</returns>
        public async Task<PagedResponseDto<JobApplicationDTO>> GetJobApplicationsAsync(JobApplicationFilterDTO filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            try
            {
                // 创建分页请求
                var pagedRequest = new PagedRequestDto
                {
                    PageIndex = filter.PageIndex,
                    PageSize = filter.PageSize,
                    SortBy = "AppliedDate",
                    IsDesc = true
                };

                // 构建查询条件
                Expression<Func<JobApplication, bool>> predicate = null;

                // 基础查询
                var applications = await GetApplicationsWithFilters(filter);

                // 排序和分页
                var orderedApplications = applications
                    .OrderByDescending(a => a.AppliedDate)
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                // 获取总数
                var totalCount = applications.Count();

                // 映射结果
                var applicationsDto = _mapper.Map<List<JobApplicationDTO>>(orderedApplications);

                // 返回分页结果
                return new PagedResponseDto<JobApplicationDTO>
                {
                    PageIndex = filter.PageIndex,
                    PageSize = filter.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                    Items = applicationsDto
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("获取职位申请列表失败", ex);
            }
        }

        /// <summary>
        /// 获取用户的所有申请
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>申请DTO列表</returns>
        public async Task<List<JobApplicationDTO>> GetUserApplicationsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("用户ID不能为空", nameof(userId));
            }

            var applications = await _jobApplicationRepository.GetAsync(
                a => a.UserId == userId,
                "JobPosition");

            return _mapper.Map<List<JobApplicationDTO>>(applications);
        }

        /// <summary>
        /// 获取职位的所有申请
        /// </summary>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>申请DTO列表</returns>
        public async Task<List<JobApplicationDTO>> GetJobPositionApplicationsAsync(int jobPositionId)
        {
            if (jobPositionId <= 0)
            {
                throw new ArgumentException("职位ID必须大于0", nameof(jobPositionId));
            }

            var applications = await _jobApplicationRepository.GetAsync(
                a => a.JobPositionId == jobPositionId, 
                "JobPosition", "User");

            return _mapper.Map<List<JobApplicationDTO>>(applications);
        }

        /// <summary>
        /// 根据ID获取申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>申请DTO</returns>
        public async Task<JobApplicationDTO> GetJobApplicationByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("申请ID必须大于0", nameof(id));
            }

            var application = await _jobApplicationRepository.GetByIdAsync(id, "JobPosition", "User");
            if (application == null)
            {
                return null;
            }

            return _mapper.Map<JobApplicationDTO>(application);
        }

        /// <summary>
        /// 创建职位申请
        /// </summary>
        /// <param name="dto">创建申请DTO</param>
        /// <param name="userId">用户ID（可选）</param>
        /// <returns>创建的申请ID</returns>
        public async Task<int> CreateJobApplicationAsync(CreateJobApplicationDTO dto, string userId = null)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // 检查职位是否存在
            var jobPosition = await _jobPositionRepository.GetByIdAsync(dto.JobPositionId);
            if (jobPosition == null)
            {
                throw new InvalidOperationException("职位不存在");
            }

            // 检查是否已经申请过
            if (!string.IsNullOrEmpty(userId))
            {
                var existingApplications = await _jobApplicationRepository.GetAsync(
                    a => a.JobPositionId == dto.JobPositionId && a.UserId == userId);

                if (existingApplications.Any())
                {
                    throw new InvalidOperationException("您已经申请过这个职位");
                }
            }

            // 创建申请实体
            var application = new JobApplication
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                ResumeUrl = dto.ResumeUrl,
                CoverLetter = dto.CoverLetter,
                JobPositionId = dto.JobPositionId,
                Status = ApplicationStatus.New,
                AppliedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                UserId = userId
            };

            // 保存申请信息到数据库
            _jobApplicationRepository.Add(application);
            await _unitOfWork.SaveChangesAsync();

            return application.Id;
        }

        /// <summary>
        /// 更新申请状态
        /// </summary>
        /// <param name="dto">更新状态DTO</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateJobApplicationStatusAsync(UpdateJobApplicationStatusDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var application = await _jobApplicationRepository.GetByIdAsync(dto.Id);
            if (application == null)
            {
                return false;
            }

            application.Status = dto.Status;
            application.Notes = dto.Notes;
            application.LastUpdatedDate = DateTime.Now;

            _jobApplicationRepository.Update(application);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 更新申请备注
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <param name="notes">备注内容</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateJobApplicationNotesAsync(int id, string notes)
        {
            if (id <= 0)
            {
                throw new ArgumentException("申请ID必须大于0", nameof(id));
            }

            var application = await _jobApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return false;
            }

            application.Notes = notes;
            application.LastUpdatedDate = DateTime.Now;

            _jobApplicationRepository.Update(application);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteJobApplicationAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("申请ID必须大于0", nameof(id));
            }

            var application = await _jobApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return false;
            }

            _jobApplicationRepository.Delete(application);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 获取申请状态统计
        /// </summary>
        /// <returns>各状态的申请数量</returns>
        public async Task<Dictionary<ApplicationStatus, int>> GetApplicationStatusStatisticsAsync()
        {
            var applications = await _jobApplicationRepository.GetAllAsync();
            var statistics = applications
                .GroupBy(a => a.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            // 确保所有状态都有值
            foreach (ApplicationStatus status in Enum.GetValues(typeof(ApplicationStatus)))
            {
                if (!statistics.ContainsKey(status))
                {
                    statistics[status] = 0;
                }
            }

            return statistics;
        }

        /// <summary>
        /// 检查用户是否已申请该职位
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>是否已申请</returns>
        public async Task<bool> HasUserAppliedForPositionAsync(string userId, int jobPositionId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            if (jobPositionId <= 0)
            {
                throw new ArgumentException("职位ID必须大于0", nameof(jobPositionId));
            }

            var applications = await _jobApplicationRepository.GetAsync(
                a => a.JobPositionId == jobPositionId && a.UserId == userId);

            return applications.Any();
        }

        /// <summary>
        /// 根据筛选条件获取申请列表
        /// </summary>
        private async Task<IEnumerable<JobApplication>> GetApplicationsWithFilters(JobApplicationFilterDTO filter)
        {
            // 获取所有应用并加载相关实体
            var applications = await _jobApplicationRepository.GetAsync(a => true, "JobPosition", "User");

            // 应用筛选条件
            if (!string.IsNullOrEmpty(filter.Name))
            {
                applications = applications.Where(a => a.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                applications = applications.Where(a => a.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.JobPositionId.HasValue)
            {
                applications = applications.Where(a => a.JobPositionId == filter.JobPositionId.Value);
            }

            if (filter.Status.HasValue)
            {
                applications = applications.Where(a => a.Status == filter.Status.Value);
            }

            if (filter.AppliedDateFrom.HasValue)
            {
                applications = applications.Where(a => a.AppliedDate >= filter.AppliedDateFrom.Value);
            }

            if (filter.AppliedDateTo.HasValue)
            {
                applications = applications.Where(a => a.AppliedDate <= filter.AppliedDateTo.Value);
            }

            return applications;
        }
    }
} 