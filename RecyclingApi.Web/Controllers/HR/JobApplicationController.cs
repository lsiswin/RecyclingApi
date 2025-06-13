using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Application.Services.HR;
using RecyclingApi.Application.Common.Responses;

namespace RecyclingApi.Web.Controllers.HR
{
    /// <summary>
    /// 职位申请控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;
        private readonly ILogger<JobApplicationController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jobApplicationService">职位申请服务</param>
        /// <param name="logger">日志记录器</param>
        public JobApplicationController(
            IJobApplicationService jobApplicationService,
            ILogger<JobApplicationController> logger)
        {
            _jobApplicationService = jobApplicationService;
            _logger = logger;
        }

        /// <summary>
        /// 获取职位申请列表（分页）
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页结果</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<PagedResponseDto<JobApplicationDTO>>> GetJobApplications([FromQuery] JobApplicationFilterDTO filter)
        {
            try
            {
                // 设置默认分页参数
                if (filter.PageIndex <= 0) filter.PageIndex = 1;
                if (filter.PageSize <= 0) filter.PageSize = 10;

                var result = await _jobApplicationService.GetJobApplicationsAsync(filter);
                return new ApiResponse<PagedResponseDto<JobApplicationDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取职位申请列表失败");
                return new ApiResponse<PagedResponseDto<JobApplicationDTO>>("获取职位申请列表失败");
            }
        }

        /// <summary>
        /// 根据ID获取职位申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>申请详情</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<JobApplicationDTO>> GetJobApplicationById(int id)
        {
            try
            {
                var application = await _jobApplicationService.GetJobApplicationByIdAsync(id);
                if (application == null)
                {
                    return new ApiResponse<JobApplicationDTO>("职位申请不存在");
                }

                return new ApiResponse<JobApplicationDTO>(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取职位申请详情失败");
                return new ApiResponse<JobApplicationDTO>("获取职位申请详情失败");
            }
        }

        /// <summary>
        /// 获取某个职位的所有申请
        /// </summary>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>申请列表</returns>
        [HttpGet("byPosition/{jobPositionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<List<JobApplicationDTO>>> GetJobPositionApplications(int jobPositionId)
        {
            try
            {
                var applications = await _jobApplicationService.GetJobPositionApplicationsAsync(jobPositionId);
                return new ApiResponse<List<JobApplicationDTO>>(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取职位申请列表失败");
                return new ApiResponse<List<JobApplicationDTO>>("获取职位申请列表失败");
            }
        }

        /// <summary>
        /// 获取当前用户的所有申请
        /// </summary>
        /// <returns>申请列表</returns>
        [HttpGet("myApplications")]
        [Authorize]
        public async Task<ApiResponse<List<JobApplicationDTO>>> GetMyApplications()
        {
            try
            {
                // 获取当前用户ID
                var userId = User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiResponse<List<JobApplicationDTO>>("用户未登录");
                }

                var applications = await _jobApplicationService.GetUserApplicationsAsync(userId);
                return new ApiResponse<List<JobApplicationDTO>>(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户申请列表失败");
                return new ApiResponse<List<JobApplicationDTO>>("获取用户申请列表失败");
            }
        }

        /// <summary>
        /// 创建职位申请
        /// </summary>
        /// <param name="dto">创建申请DTO</param>
        /// <returns>创建结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<int>> CreateJobApplication([FromBody] CreateJobApplicationDTO dto)
        {
            try
            {
                // 获取当前用户ID（如果已登录）
                var userId = User.FindFirst("userId")?.Value;

                // 创建申请
                var applicationId = await _jobApplicationService.CreateJobApplicationAsync(dto, userId);

                return new ApiResponse<int>(applicationId, "申请成功");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "创建职位申请失败");
                return new ApiResponse<int>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建职位申请失败");
                return new ApiResponse<int>("创建职位申请失败");
            }
        }

        /// <summary>
        /// 更新申请状态
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <param name="dto">更新状态DTO</param>
        /// <returns>更新结果</returns>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<bool>> UpdateJobApplicationStatus(int id, [FromBody] UpdateJobApplicationStatusDTO dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return new ApiResponse<bool>("ID不匹配");
                }

                var result = await _jobApplicationService.UpdateJobApplicationStatusAsync(dto);
                if (!result)
                {
                    return new ApiResponse<bool>("职位申请不存在");
                }

                return new ApiResponse<bool>(true, "更新状态成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新申请状态失败");
                return new ApiResponse<bool>("更新申请状态失败");
            }
        }

        /// <summary>
        /// 更新申请备注
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <param name="dto">备注DTO</param>
        /// <returns>更新结果</returns>
        [HttpPut("{id}/notes")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<bool>> UpdateJobApplicationNotes(int id, [FromBody] UpdateNotesDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return new ApiResponse<bool>("ID不匹配");
                }

                var result = await _jobApplicationService.UpdateJobApplicationNotesAsync(id, dto.Notes);
                if (!result)
                {
                    return new ApiResponse<bool>("职位申请不存在");
                }

                return new ApiResponse<bool>(true, "更新备注成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新申请备注失败");
                return new ApiResponse<bool>("更新申请备注失败");
            }
        }

        /// <summary>
        /// 删除职位申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<bool>> DeleteJobApplication(int id)
        {
            try
            {
                var result = await _jobApplicationService.DeleteJobApplicationAsync(id);
                if (!result)
                {
                    return new ApiResponse<bool>("职位申请不存在");
                }

                return new ApiResponse<bool>(true, "删除成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除职位申请失败");
                return new ApiResponse<bool>("删除职位申请失败");
            }
        }

        /// <summary>
        /// 获取申请状态统计
        /// </summary>
        /// <returns>统计数据</returns>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<Dictionary<int, int>>> GetApplicationStatistics()
        {
            try
            {
                var statistics = await _jobApplicationService.GetApplicationStatusStatisticsAsync();
                
                // 将枚举转换为整数
                var result = statistics.ToDictionary(kv => (int)kv.Key, kv => kv.Value);
                
                return new ApiResponse<Dictionary<int, int>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取申请统计失败");
                return new ApiResponse<Dictionary<int, int>>("获取申请统计失败");
            }
        }
    }

    /// <summary>
    /// 更新备注DTO
    /// </summary>
    public class UpdateNotesDto
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 备注内容
        /// </summary>
        public string Notes { get; set; }
    }
} 