using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Services.Content;

namespace RecyclingApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        /// <summary>
        /// 获取轮播图分页列表
        /// </summary>
        [HttpGet]
        public async Task<ApiResponse<PagedResponseDto<BannerDto>>> GetList([FromQuery] BannerRequestDto requestDto)
        {
            var result = await _bannerService.GetPagedListAsync(requestDto);
            return new ApiResponse<PagedResponseDto<BannerDto>>(result);
        }

        /// <summary>
        /// 根据ID获取轮播图
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ApiResponse<BannerDto>> GetById(int id)
        {
            var banner = await _bannerService.GetByIdAsync(id);
            if (banner == null)
                return new ApiResponse<BannerDto>($"未找到ID为{id}的轮播图");

            return new ApiResponse<BannerDto>(banner);
        }

        /// <summary>
        /// 创建轮播图
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<BannerDto>> Create([FromBody] CreateUpdateBannerDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<BannerDto>(ModelState.ToString());

            var result = await _bannerService.CreateAsync(input);
            if (result == null)
                return new ApiResponse<BannerDto>("创建轮播图失败");
            return new ApiResponse<BannerDto>(result);
        }

        /// <summary>
        /// 更新轮播图
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<BannerDto>> Update(int id, [FromBody] CreateUpdateBannerDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<BannerDto>(ModelState.ToString());

            try
            {
                var result = await _bannerService.UpdateAsync(id, input);
                return new ApiResponse<BannerDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<BannerDto>(ex.Message);
            }
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var result = await _bannerService.DeleteAsync(id);
            if (!result)
                return new ApiResponse<string>($"未找到ID为{id}的轮播图");

            return new ApiResponse<string>("删除成功");
        }

        /// <summary>
        /// 切换轮播图状态
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<BannerDto>> ToggleStatus(int id)
        {
            try
            {
                var result = await _bannerService.ToggleStatusAsync(id);
                return new ApiResponse<BannerDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<BannerDto>(ex.Message);
            }
        }
    }
} 