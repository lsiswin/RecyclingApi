using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Application.Services.HR;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs;

namespace RecyclingApi.Web.Controllers.HR
{
    /// <summary>
    /// 职位管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPositionService _jobPositionService;
        private readonly ILogger<JobPositionController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jobPositionService">职位服务</param>
        /// <param name="logger">日志记录器</param>
        public JobPositionController(
            IJobPositionService jobPositionService,
            ILogger<JobPositionController> logger)
        {
            _jobPositionService = jobPositionService;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有职位
        /// </summary>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>职位DTO列表</returns>
        [HttpGet("all")]
        public async Task<ApiResponse<List<JobPositionDTO>>> GetAllJobPositions([FromQuery] bool includeInactive = false)
        {
            try
            {
                var result = await _jobPositionService.GetAllJobPositionsAsync(includeInactive);
                return new ApiResponse<List<JobPositionDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有职位时发生错误");
                return new ApiResponse<List<JobPositionDTO>>("获取所有职位时发生错误");
            }
        }

        /// <summary>
        /// 获取职位列表（分页）
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>分页结果</returns>
        [HttpGet]
        public async Task<ApiResponse<PagedResponseDto<JobPositionDTO>>> GetJobPositions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeInactive = true)
        {
            try
            {
                var result = await _jobPositionService.GetJobPositionsAsync(pageNumber, pageSize, includeInactive);
                return new ApiResponse<PagedResponseDto<JobPositionDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取职位列表时发生错误");
                return new ApiResponse<PagedResponseDto<JobPositionDTO>>("获取职位列表时发生错误");
            }
        }

        /// <summary>
        /// 根据部门获取职位列表
        /// </summary>
        /// <param name="department">部门名称</param>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>职位DTO列表</returns>
        [HttpGet("department/{department}")]
        public async Task<ApiResponse<List<JobPositionDTO>>> GetJobPositionsByDepartment(
            string department,
            [FromQuery] bool includeInactive = false)
        {
            try
            {
                var result = await _jobPositionService.GetJobPositionsByDepartmentAsync(department, includeInactive);
                return new ApiResponse<List<JobPositionDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取部门职位列表时发生错误");
                return new ApiResponse<List<JobPositionDTO>>("获取部门职位列表时发生错误");
            }
        }

        /// <summary>
        /// 根据ID获取职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>职位DTO</returns>
        [HttpGet("{id}")]
        public async Task<ApiResponse<JobPositionDTO>> GetJobPositionById(int id)
        {
            try
            {
                var jobPosition = await _jobPositionService.GetJobPositionByIdAsync(id);
                if (jobPosition == null)
                {
                    return new ApiResponse<JobPositionDTO>("职位不存在");
                }
                return new ApiResponse<JobPositionDTO>(jobPosition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取职位详情时发生错误");
                return new ApiResponse<JobPositionDTO>("获取职位详情时发生错误");
            }
        }

        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="dto">创建职位DTO</param>
        /// <returns>创建的职位ID</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ApiResponse<int>> CreateJobPosition([FromBody] CreateJobPositionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ApiResponse<int>(ModelState.ToString());
                }

                var jobPositionId = await _jobPositionService.CreateJobPositionAsync(dto);
                return new ApiResponse<int>(jobPositionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建职位时发生错误");
                return new ApiResponse<int>("创建职位时发生错误");
            }
        }

        /// <summary>
        /// 更新职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <param name="dto">更新职位DTO</param>
        /// <returns>操作结果</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ApiResponse<bool>> UpdateJobPosition(int id, [FromBody] UpdateJobPositionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ApiResponse<bool>(ModelState.ToString());
                }

                if (id != dto.Id)
                {
                    return new ApiResponse<bool>("ID不匹配");
                }

                var result = await _jobPositionService.UpdateJobPositionAsync(dto);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新职位时发生错误");
                return new ApiResponse<bool>("更新职位时发生错误");
            }
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<bool>> DeleteJobPosition(int id)
        {
            try
            {
                var result = await _jobPositionService.DeleteJobPositionAsync(id);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除职位时发生错误");
                return new ApiResponse<bool>("删除职位时发生错误");
            }
        }

        /// <summary>
        /// 更改职位状态
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <param name="isActive">是否激活</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ApiResponse<bool>> ChangeJobPositionStatus(int id, [FromQuery] bool isActive)
        {
            try
            {
                var result = await _jobPositionService.ChangeJobPositionStatusAsync(id, isActive);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更改职位状态时发生错误");
                return new ApiResponse<bool>("更改职位状态时发生错误");
            }
        }

        /// <summary>
        /// 获取职位的申请数量
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>申请数量</returns>
        [HttpGet("{id}/applications/count")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ApiResponse<int>> GetJobPositionApplicationCount(int id)
        {
            try
            {
                var count = await _jobPositionService.GetJobPositionApplicationCountAsync(id);
                return new ApiResponse<int>(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取职位申请数量时发生错误");
                return new ApiResponse<int>("获取职位申请数量时发生错误");
            }
        }
    }
} 