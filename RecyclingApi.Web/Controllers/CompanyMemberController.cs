using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Services.Content;

namespace RecyclingApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyMemberController : ControllerBase
    {
        private readonly ICompanyInfoService _companyInfoService;

        public CompanyMemberController(ICompanyInfoService companyInfoService)
        {
            _companyInfoService = companyInfoService;
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <returns>公司信息</returns>
        [HttpGet]
        public async Task<ApiResponse<CompanyInfoDto>> GetCompanyProfile()
        {
            var result = await _companyInfoService.GetCompanyInfoAsync();
            return new ApiResponse<CompanyInfoDto>(result);
        }

        /// <summary>
        /// 创建或更新公司信息
        /// </summary>
        /// <param name="dto">公司信息DTO</param>
        /// <returns>更新后的公司信息</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<CompanyInfoDto>> CreateOrUpdateCompanyProfile(
            [FromBody] UpdateCompanyInfoDto dto)
        {
            var result = await _companyInfoService.UpdateCompanyInfoAsync(dto);
            return new ApiResponse<CompanyInfoDto>(result, "公司信息更新成功");
        }
        #region 团队成员

        /// <summary>
        /// 获取所有团队成员
        /// </summary>
        [HttpGet("team-members")]
        public async Task<ApiResponse<List<TeamMemberDto>>> GetAllTeamMembers()
        {
            var teamMembers = await _companyInfoService.GetAllTeamMembersAsync();
            return new ApiResponse<List<TeamMemberDto>>(teamMembers);
        }

        /// <summary>
        /// 根据ID获取团队成员
        /// </summary>
        [HttpGet("team-members/{id}")]
        public async Task<ApiResponse<TeamMemberDto>> GetTeamMemberById(int id)
        {
            var teamMember = await _companyInfoService.GetTeamMemberByIdAsync(id);
            if (teamMember == null)
                return new ApiResponse<TeamMemberDto>($"未找到ID为{id}的团队成员");

            return new ApiResponse<TeamMemberDto>(teamMember);
        }

        /// <summary>
        /// 创建团队成员
        /// </summary>
        [HttpPost("team-members")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<TeamMemberDto>> CreateTeamMember([FromBody] CreateUpdateTeamMemberDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<TeamMemberDto>(ModelState.ToString());

            var result = await _companyInfoService.CreateTeamMemberAsync(input);
            return new ApiResponse<TeamMemberDto>(result);
        }

        /// <summary>
        /// 更新团队成员
        /// </summary>
        [HttpPut("team-members/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<TeamMemberDto>> UpdateTeamMember(int id, [FromBody] CreateUpdateTeamMemberDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<TeamMemberDto>(ModelState.ToString());

            try
            {
                var result = await _companyInfoService.UpdateTeamMemberAsync(id, input);
                return new ApiResponse<TeamMemberDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<TeamMemberDto>(ex.Message);
            }
        }

        /// <summary>
        /// 删除团队成员
        /// </summary>
        [HttpDelete("team-members/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<string>> DeleteTeamMember(int id)
        {
            var result = await _companyInfoService.DeleteTeamMemberAsync(id);
            if (!result)
                return new ApiResponse<string>($"未找到ID为{id}的团队成员");

            return new ApiResponse<string>("删除成功");
        }

        #endregion
    }
} 