using System;
using System.Collections.Generic;

namespace RecyclingApi.Domain.Entities.Content
{
    /// <summary>
    /// 案例实体
    /// </summary>
    public class Case
    {
        /// <summary>
        /// 案例ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 案例标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 简短描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 完整描述
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// 案例分类（enterprise:企业回收, school:学校回收, government:政府机构, hospital:医院回收）
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 设备类型（desktop:台式电脑, laptop:笔记本电脑, server:服务器, network:网络设备）
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 回收日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 项目周期
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 回收规模（small:小规模, medium:中规模, large:大规模）
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 案例图片URL
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 标签列表（以逗号分隔）
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 评分（1-5）
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// 项目详情
        /// </summary>
        public string ProjectDetails { get; set; }

        /// <summary>
        /// 服务亮点（以分号分隔）
        /// </summary>
        public string Highlights { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
} 