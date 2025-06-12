using System;

namespace RecyclingApi.Application.DTOs.ContentDTOs
{
    /// <summary>
    /// 轮播图DTO
    /// </summary>
    public class BannerDto
    {
        /// <summary>
        /// 轮播图ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 轮播图标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 轮播图描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
} 