using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs.ContentDTOs;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 公司信息服务接口
    /// </summary>
    public interface ICompanyInfoService
    {
        /// <summary>
        /// 获取公司信息
        /// </summary>
        Task<CompanyInfoDto> GetCompanyInfoAsync();

        /// <summary>
        /// 更新公司信息
        /// </summary>
        Task<CompanyInfoDto> UpdateCompanyInfoAsync(UpdateCompanyInfoDto input);

        #region 公司优势

        /// <summary>
        /// 获取所有公司优势
        /// </summary>
        Task<List<CompanyAdvantageDto>> GetAllAdvantagesAsync();

        /// <summary>
        /// 根据ID获取公司优势
        /// </summary>
        Task<CompanyAdvantageDto> GetAdvantageByIdAsync(int id);

        /// <summary>
        /// 创建公司优势
        /// </summary>
        Task<CompanyAdvantageDto> CreateAdvantageAsync(CreateUpdateAdvantageDto input);

        /// <summary>
        /// 更新公司优势
        /// </summary>
        Task<CompanyAdvantageDto> UpdateAdvantageAsync(int id, CreateUpdateAdvantageDto input);

        /// <summary>
        /// 删除公司优势
        /// </summary>
        Task<bool> DeleteAdvantageAsync(int id);

        #endregion

        #region 公司发展历程

        /// <summary>
        /// 获取所有公司发展历程
        /// </summary>
        Task<List<CompanyMilestoneDto>> GetAllMilestonesAsync();

        /// <summary>
        /// 根据ID获取公司发展历程
        /// </summary>
        Task<CompanyMilestoneDto> GetMilestoneByIdAsync(int id);

        /// <summary>
        /// 创建公司发展历程
        /// </summary>
        Task<CompanyMilestoneDto> CreateMilestoneAsync(CreateUpdateMilestoneDto input);

        /// <summary>
        /// 更新公司发展历程
        /// </summary>
        Task<CompanyMilestoneDto> UpdateMilestoneAsync(int id, CreateUpdateMilestoneDto input);

        /// <summary>
        /// 删除公司发展历程
        /// </summary>
        Task<bool> DeleteMilestoneAsync(int id);

        #endregion

        #region 团队成员

        /// <summary>
        /// 获取所有团队成员
        /// </summary>
        Task<List<TeamMemberDto>> GetAllTeamMembersAsync();

        /// <summary>
        /// 根据ID获取团队成员
        /// </summary>
        Task<TeamMemberDto> GetTeamMemberByIdAsync(int id);

        /// <summary>
        /// 创建团队成员
        /// </summary>
        Task<TeamMemberDto> CreateTeamMemberAsync(CreateUpdateTeamMemberDto input);

        /// <summary>
        /// 更新团队成员
        /// </summary>
        Task<TeamMemberDto> UpdateTeamMemberAsync(int id, CreateUpdateTeamMemberDto input);

        /// <summary>
        /// 删除团队成员
        /// </summary>
        Task<bool> DeleteTeamMemberAsync(int id);

        #endregion
    }
} 