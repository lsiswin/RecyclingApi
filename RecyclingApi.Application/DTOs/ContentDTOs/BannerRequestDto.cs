using RecyclingApi.Application.DTOs;

namespace RecyclingApi.Application.DTOs.ContentDTOs
{
    /// <summary>
    /// 轮播图查询请求DTO
    /// </summary>
    public class BannerRequestDto : PagedRequestDto
    {
        /// <summary>
        /// 标题关键字
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; }
    }
} 