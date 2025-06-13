using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;

namespace RecyclingApi.Application.Services.HR
{
    /// <summary>
    /// 职位服务接口
    /// </summary>
    public interface IJobPositionService
    {
        /// <summary>
        /// 获取所有职位
        /// </summary>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>职位DTO列表</returns>
        Task<List<JobPositionDTO>> GetAllJobPositionsAsync(bool includeInactive = false);

        /// <summary>
        /// 获取职位列表（分页）
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<JobPositionDTO>> GetJobPositionsAsync(int pageNumber, int pageSize, bool includeInactive = false);

        /// <summary>
        /// 根据部门获取职位列表
        /// </summary>
        /// <param name="department">部门名称</param>
        /// <param name="includeInactive">是否包含未激活的职位</param>
        /// <returns>职位DTO列表</returns>
        Task<List<JobPositionDTO>> GetJobPositionsByDepartmentAsync(string department, bool includeInactive = false);

        /// <summary>
        /// 根据ID获取职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>职位DTO</returns>
        Task<JobPositionDTO> GetJobPositionByIdAsync(int id);

        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="dto">创建职位DTO</param>
        /// <returns>创建的职位ID</returns>
        Task<int> CreateJobPositionAsync(CreateJobPositionDTO dto);

        /// <summary>
        /// 更新职位
        /// </summary>
        /// <param name="dto">更新职位DTO</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateJobPositionAsync(UpdateJobPositionDTO dto);

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteJobPositionAsync(int id);

        /// <summary>
        /// 更改职位状态
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <param name="isActive">是否激活</param>
        /// <returns>是否更新成功</returns>
        Task<bool> ChangeJobPositionStatusAsync(int id, bool isActive);

        /// <summary>
        /// 获取职位的申请数量
        /// </summary>
        /// <param name="jobPositionId">职位ID</param>
        /// <returns>申请数量</returns>
        Task<int> GetJobPositionApplicationCountAsync(int jobPositionId);
    }
} 