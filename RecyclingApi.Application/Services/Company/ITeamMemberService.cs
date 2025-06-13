using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs.Company;
using RecyclingApi.Application.Common;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.DTOs;

namespace RecyclingApi.Application.Services.Company
{
    /// <summary>
    /// 团队成员服务接口
    /// </summary>
    public interface ITeamMemberService
    {
        #region 团队成员类型管理

        /// <summary>
        /// 获取所有团队成员类型列表
        /// </summary>
        /// <returns>类型列表</returns>
        Task<List<TeamMemberTypeDto>> GetTeamMemberTypesAsync();

        /// <summary>
        /// 获取团队成员类型详情
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns>类型详情</returns>
        Task<TeamMemberTypeDto> GetTeamMemberTypeByIdAsync(int id);

        /// <summary>
        /// 创建团队成员类型
        /// </summary>
        /// <param name="dto">创建DTO</param>
        /// <returns>创建后的类型</returns>
        Task<TeamMemberTypeDto> CreateTeamMemberTypeAsync(CreateUpdateTeamMemberTypeDto dto);

        /// <summary>
        /// 更新团队成员类型
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dto">更新DTO</param>
        /// <returns>更新后的类型</returns>
        Task<TeamMemberTypeDto> UpdateTeamMemberTypeAsync(int id, CreateUpdateTeamMemberTypeDto dto);

        /// <summary>
        /// 删除团队成员类型
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteTeamMemberTypeAsync(int id);

        #endregion

        #region 团队成员管理

        /// <summary>
        /// 获取团队成员列表
        /// </summary>
        /// <param name="typeId">可选类型ID筛选</param>
        /// <returns>成员列表</returns>
        Task<List<TeamMemberDto>> GetTeamMembersAsync(int? typeId = null);

        /// <summary>
        /// 获取团队成员详情
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <returns>成员详情</returns>
        Task<TeamMemberDto> GetTeamMemberByIdAsync(int id);

        /// <summary>
        /// 创建团队成员
        /// </summary>
        /// <param name="dto">创建DTO</param>
        /// <returns>创建后的成员</returns>
        Task<TeamMemberDto> CreateTeamMemberAsync(CreateUpdateTeamMemberDto dto);

        /// <summary>
        /// 更新团队成员
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <param name="dto">更新DTO</param>
        /// <returns>更新后的成员</returns>
        Task<TeamMemberDto> UpdateTeamMemberAsync(int id, CreateUpdateTeamMemberDto dto);

        /// <summary>
        /// 删除团队成员
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteTeamMemberAsync(int id);

        /// <summary>
        /// 获取所有员工简略信息（用于选择关联）
        /// </summary>
        /// <returns>员工列表</returns>
        Task<List<EmployeeSimpleDto>> GetAllEmployeesSimpleAsync();

        /// <summary>
        /// 获取团队成员分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="typeId">可选类型ID筛选</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<TeamMemberDto>> GetPagedTeamMembersAsync(int pageIndex, int pageSize, int? typeId = null);

        #endregion
    }
} 