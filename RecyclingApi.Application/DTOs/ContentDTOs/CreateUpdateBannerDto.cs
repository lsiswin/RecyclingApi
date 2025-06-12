using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.ContentDTOs
{
    /// <summary>
    /// 创建或更新轮播图的DTO
    /// </summary>
    public class CreateUpdateBannerDto
    {
        /// <summary>
        /// 轮播图标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(100, ErrorMessage = "标题长度不能超过100个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 轮播图描述
        /// </summary>
        [Required(ErrorMessage = "描述不能为空")]
        [StringLength(500, ErrorMessage = "描述长度不能超过500个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        [Required(ErrorMessage = "图片URL不能为空")]
        [StringLength(500, ErrorMessage = "图片URL长度不能超过500个字符")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [StringLength(500, ErrorMessage = "链接地址长度不能超过500个字符")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        [Range(0, 999, ErrorMessage = "排序号必须在0-999之间")]
        public int Sort { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 