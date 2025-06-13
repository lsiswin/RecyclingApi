using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Application.Services.HR
{
    /// <summary>
    /// 职位申请服务接口
    /// </summary>
    public interface IJobApplicationService
    {
        /// <summary>
        /// 获取职位申请列表（分页）
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<JobApplicationDTO>> GetJobApplicationsAsync(JobApplicationFilterDTO filter);

        /// <summary>
        /// 获取用户的所有申请
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>申请DTO列表</returns>
        Task<List<JobApplicationDTO>> GetUserApplicationsAsync(string userId);

        /// <summary>
        /// 获取职位的所有申请
        /// </summary>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>申请DTO列表</returns>
        Task<List<JobApplicationDTO>> GetJobPositionApplicationsAsync(int jobPositionId);

        /// <summary>
        /// 根据ID获取申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>申请DTO</returns>
        Task<JobApplicationDTO> GetJobApplicationByIdAsync(int id);

        /// <summary>
        /// 创建职位申请
        /// </summary>
        /// <param name="dto">创建申请DTO</param>
        /// <param name="userId">用户ID（可选）</param>
        /// <returns>创建的申请ID</returns>
        Task<int> CreateJobApplicationAsync(CreateJobApplicationDTO dto, string userId = null);

        /// <summary>
        /// 更新申请状态
        /// </summary>
        /// <param name="dto">更新状态DTO</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateJobApplicationStatusAsync(UpdateJobApplicationStatusDTO dto);

        /// <summary>
        /// 更新申请备注
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <param name="notes">备注内容</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateJobApplicationNotesAsync(int id, string notes);

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteJobApplicationAsync(int id);

        /// <summary>
        /// 获取申请状态统计
        /// </summary>
        /// <returns>各状态的申请数量</returns>
        Task<Dictionary<ApplicationStatus, int>> GetApplicationStatusStatisticsAsync();

        /// <summary>
        /// 检查用户是否已申请该职位
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>是否已申请</returns>
        Task<bool> HasUserAppliedForPositionAsync(string userId, int jobPositionId);
    }
} 