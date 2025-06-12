using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Application.Services.Content;

namespace RecyclingApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        /// <summary>
        /// 获取案例分页列表
        /// </summary>
        [HttpGet]
        public async Task<ApiResponse<PagedResponseDto<CaseDto>>> GetList([FromQuery] CaseRequestDto requestDto)
        {
            var result = await _caseService.GetPagedListAsync(requestDto);
            return new ApiResponse<PagedResponseDto<CaseDto>>(result);
        }

        /// <summary>
        /// 根据ID获取案例
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ApiResponse<CaseDto>> GetById(int id)
        {
            var caseDto = await _caseService.GetByIdAsync(id);
            if (caseDto == null)
                return new ApiResponse<CaseDto>($"未找到ID为{id}的案例");

            return new ApiResponse<CaseDto>(caseDto);
        }

        /// <summary>
        /// 创建案例
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<CaseDto>> Create([FromBody] CreateUpdateCaseDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<CaseDto>(ModelState.ToString());

            var result = await _caseService.CreateAsync(input);
            return new ApiResponse<CaseDto>(result);
        }

        /// <summary>
        /// 更新案例
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<CaseDto>> Update(int id, [FromBody] CreateUpdateCaseDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<CaseDto>(ModelState.ToString());

            try
            {
                var result = await _caseService.UpdateAsync(id, input);
                return new ApiResponse<CaseDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<CaseDto>(ex.Message);
            }
        }

        /// <summary>
        /// 删除案例
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var result = await _caseService.DeleteAsync(id);
            if (!result)
                return new ApiResponse<string>($"未找到ID为{id}的案例");

            return new ApiResponse<string>("删除成功");
        }

        /// <summary>
        /// 切换案例状态
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<CaseDto>> ToggleStatus(int id)
        {
            try
            {
                var result = await _caseService.ToggleStatusAsync(id);
                return new ApiResponse<CaseDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<CaseDto>(ex.Message);
            }
        }

        /// <summary>
        /// 增加案例浏览次数
        /// </summary>
        [HttpPatch("{id}/increment-views")]
        public async Task<ApiResponse<int>> IncrementViews(int id)
        {
            try
            {
                var views = await _caseService.IncrementViewsAsync(id);
                return new ApiResponse<int>(views);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<int>(ex.Message);
            }
        }
    }
} 