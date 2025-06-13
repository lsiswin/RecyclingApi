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
    /// 简历管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly ILogger<ResumeController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resumeService">简历服务</param>
        /// <param name="logger">日志记录器</param>
        public ResumeController(
            IResumeService resumeService,
            ILogger<ResumeController> logger)
        {
            _resumeService = resumeService;
            _logger = logger;
        }

        /// <summary>
        /// 获取简历列表（分页）
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页结果</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ApiResponse<PagedResponseDto<ResumeDTO>>> GetResumes([FromQuery] ResumeFilterDTO filter)
        {
            try
            {
                var result = await _resumeService.GetResumesAsync(filter);
                return new ApiResponse<PagedResponseDto<ResumeDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取简历列表时发生错误");
                return new ApiResponse<PagedResponseDto<ResumeDTO>>("获取简历列表时发生错误");
            }
        }

        /// <summary>
        /// 根据ID获取简历
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <returns>简历DTO</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ApiResponse<ResumeDTO>> GetResumeById(int id)
        {
            try
            {
                var resume = await _resumeService.GetResumeByIdAsync(id);
                if (resume == null)
                {
                    return new ApiResponse<ResumeDTO>("简历不存在");
                }

                // 检查权限：只有管理员、员工或者简历所有者可以访问
                if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
                {
                    var userId = User.FindFirst("sub")?.Value;
                    if (userId != resume.UserId)
                    {
                        return new ApiResponse<ResumeDTO>("无权访问此简历");
                    }
                }

                return new ApiResponse<ResumeDTO>(resume);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取简历详情时发生错误");
                return new ApiResponse<ResumeDTO>("获取简历详情时发生错误");
            }
        }

        /// <summary>
        /// 获取用户的所有简历
        /// </summary>
        /// <returns>简历DTO列表</returns>
        [HttpGet("my")]
        [Authorize]
        public async Task<ApiResponse<List<ResumeDTO>>> GetMyResumes()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                var resumes = await _resumeService.GetResumesByUserIdAsync(userId);
                return new ApiResponse<List<ResumeDTO>>(resumes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户简历时发生错误");
                return new ApiResponse<List<ResumeDTO>>("获取用户简历时发生错误");
            }
        }

        /// <summary>
        /// 创建简历
        /// </summary>
        /// <param name="dto">创建简历DTO</param>
        /// <returns>创建的简历ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<int>> CreateResume([FromBody] CreateResumeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ApiResponse<int>(ModelState.ToString());
                }

                var userId = User.FindFirst("sub")?.Value;
                var resumeId = await _resumeService.CreateResumeAsync(dto, userId);
                return new ApiResponse<int>(resumeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建简历时发生错误");
                return new ApiResponse<int>("创建简历时发生错误");
            }
        }

        /// <summary>
        /// 更新简历
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <param name="dto">更新简历DTO</param>
        /// <returns>操作结果</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ApiResponse<bool>> UpdateResume(int id, [FromBody] UpdateResumeDTO dto)
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

                // 检查权限
                var resume = await _resumeService.GetResumeByIdAsync(id);
                if (resume == null)
                {
                    return new ApiResponse<bool>("简历不存在");
                }

                if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
                {
                    var userId = User.FindFirst("sub")?.Value;
                    if (userId != resume.UserId)
                    {
                        return new ApiResponse<bool>("无权修改此简历");
                    }
                }

                var result = await _resumeService.UpdateResumeAsync(dto);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新简历时发生错误");
                return new ApiResponse<bool>("更新简历时发生错误");
            }
        }

        /// <summary>
        /// 删除简历
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ApiResponse<bool>> DeleteResume(int id)
        {
            try
            {
                // 检查权限
                var resume = await _resumeService.GetResumeByIdAsync(id);
                if (resume == null)
                {
                    return new ApiResponse<bool>("简历不存在");
                }

                if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
                {
                    var userId = User.FindFirst("sub")?.Value;
                    if (userId != resume.UserId)
                    {
                        return new ApiResponse<bool>("无权删除此简历");
                    }
                }

                var result = await _resumeService.DeleteResumeAsync(id);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除简历时发生错误");
                return new ApiResponse<bool>("删除简历时发生错误");
            }
        }

        /// <summary>
        /// 更改简历状态
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <param name="isActive">是否激活</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<ApiResponse<bool>> ChangeResumeStatus(int id, [FromQuery] bool isActive)
        {
            try
            {
                // 检查权限
                var resume = await _resumeService.GetResumeByIdAsync(id);
                if (resume == null)
                {
                    return new ApiResponse<bool>("简历不存在");
                }

                if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
                {
                    var userId = User.FindFirst("sub")?.Value;
                    if (userId != resume.UserId)
                    {
                        return new ApiResponse<bool>("无权修改此简历状态");
                    }
                }

                var result = await _resumeService.ChangeResumeStatusAsync(id, isActive);
                return new ApiResponse<bool>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更改简历状态时发生错误");
                return new ApiResponse<bool>("更改简历状态时发生错误");
            }
        }
    }
} 