using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;

namespace RecyclingApi.Application.Services.HR
{
    /// <summary>
    /// 简历服务接口
    /// </summary>
    public interface IResumeService
    {
        /// <summary>
        /// 获取简历列表（分页）
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<ResumeDTO>> GetResumesAsync(ResumeFilterDTO filter);

        /// <summary>
        /// 根据ID获取简历
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <returns>简历DTO</returns>
        Task<ResumeDTO> GetResumeByIdAsync(int id);

        /// <summary>
        /// 获取用户的所有简历
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>简历DTO列表</returns>
        Task<List<ResumeDTO>> GetResumesByUserIdAsync(string userId);

        /// <summary>
        /// 创建简历
        /// </summary>
        /// <param name="dto">创建简历DTO</param>
        /// <param name="userId">用户ID</param>
        /// <returns>创建的简历ID</returns>
        Task<int> CreateResumeAsync(CreateResumeDTO dto, string userId);

        /// <summary>
        /// 更新简历
        /// </summary>
        /// <param name="dto">更新简历DTO</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateResumeAsync(UpdateResumeDTO dto);

        /// <summary>
        /// 删除简历
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteResumeAsync(int id);

        /// <summary>
        /// 更改简历状态
        /// </summary>
        /// <param name="id">简历ID</param>
        /// <param name="isActive">是否激活</param>
        /// <returns>是否更新成功</returns>
        Task<bool> ChangeResumeStatusAsync(int id, bool isActive);
    }
} 