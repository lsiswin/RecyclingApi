

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs.Company;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Services.Company;

namespace RecyclingApi.Web.Controllers
{
    /// <summary>
    /// 团队成员控制器
    /// </summary>
    [Route("api/team")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="teamMemberService">团队成员服务</param>
        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        #region 团队成员管理

        /// <summary>
        /// 获取团队成员列表
        /// </summary>
        /// <param name="typeId">可选类型ID筛选</param>
        /// <returns>成员列表</returns>
        [HttpGet("getmembers")]
        public async Task<ApiResponse<List<TeamMemberDto>>> GetTeamMembers([FromQuery] int? typeId = null)
        {
            var result = await _teamMemberService.GetTeamMembersAsync(typeId);
            return new ApiResponse<List<TeamMemberDto>>(result);
        }

        /// <summary>
        /// 获取团队成员详情
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <returns>成员详情</returns>
        [HttpGet("members/{id}")]
        public async Task<ApiResponse<TeamMemberDto>> GetTeamMemberById(int id)
        {
            var result = await _teamMemberService.GetTeamMemberByIdAsync(id);
            return new ApiResponse<TeamMemberDto>(result);
        }

        /// <summary>
        /// 创建团队成员
        /// </summary>
        /// <param name="dto">创建DTO</param>
        /// <returns>创建后的成员</returns>
        [HttpPost("members")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<TeamMemberDto>> CreateTeamMember(
            [FromBody] CreateUpdateTeamMemberDto dto)
        {
            var result = await _teamMemberService.CreateTeamMemberAsync(dto);
            return new ApiResponse<TeamMemberDto>(result, "团队成员创建成功");
        }

        /// <summary>
        /// 更新团队成员
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <param name="dto">更新DTO</param>
        /// <returns>更新后的成员</returns>
        [HttpPut("members/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<TeamMemberDto>> UpdateTeamMember(
            int id, [FromBody] CreateUpdateTeamMemberDto dto)
        {
            var result = await _teamMemberService.UpdateTeamMemberAsync(id, dto);
            return new ApiResponse<TeamMemberDto>(result, "团队成员更新成功");
        }

        /// <summary>
        /// 删除团队成员
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("members/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<bool>> DeleteTeamMember(int id)
        {
            var result = await _teamMemberService.DeleteTeamMemberAsync(id);
            return new ApiResponse<bool>(result, "团队成员删除成功");
        }

        /// <summary>
        /// 获取所有员工简略信息（用于选择关联）
        /// </summary>
        /// <returns>员工列表</returns>
        [HttpGet("employees")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<EmployeeSimpleDto>>> GetAllEmployeesSimple()
        {
            var result = await _teamMemberService.GetAllEmployeesSimpleAsync();
            return  new ApiResponse<List<EmployeeSimpleDto>>(result);
        }

        #endregion
    }
} 