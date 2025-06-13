using System;
using System.Collections.Generic;

namespace RecyclingApi.Domain.Entities.Content
{
    /// <summary>
    /// 公司信息实体
    /// </summary>
    public class CompanyInfo
    {
        /// <summary>
        /// 公司信息ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 公司介绍图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 成立时间
        /// </summary>
        public DateTime EstablishmentDate { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 公司优势列表
        /// </summary>
        public List<CompanyAdvantage> Advantages { get; set; }

        /// <summary>
        /// 公司发展历程列表
        /// </summary>
        public List<CompanyMilestone> Milestones { get; set; }

        /// <summary>
        /// 团队成员列表
        /// </summary>
        public List<TeamMember> TeamMembers { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// 公司优势实体
    /// </summary>
    public class CompanyAdvantage
    {
        /// <summary>
        /// 优势ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        public string Icon { get; set; }

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

        public int CompanyInfoId { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
    }


    /// <summary>
    /// 公司发展历程实体
    /// </summary>
    public class CompanyMilestone
    {
        /// <summary>
        /// 历程ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

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

        public int CompanyInfoId { get; set; } // 必须与 CompanyInfos.Id 类型一致
        public CompanyInfo CompanyInfo { get; set; }
    }
} 