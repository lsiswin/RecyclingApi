using System;
using System.Collections.Generic;

namespace RecyclingApi.Application.DTOs.Company
{
    /// <summary>
    /// 公司展示信息DTO
    /// </summary>
    public class CompanyProfileDto
    {
        /// <summary>
        /// ID
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
        /// 公司Logo
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 公司封面图片
        /// </summary>
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 成立时间
        /// </summary>
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// 企业文化/愿景
        /// </summary>
        public string Vision { get; set; }

        /// <summary>
        /// 企业使命
        /// </summary>
        public string Mission { get; set; }

        /// <summary>
        /// 企业优势列表
        /// </summary>
        public List<AdvantageItem> Advantages { get; set; }

        /// <summary>
        /// 发展历程列表
        /// </summary>
        public List<MilestoneItem> Milestones { get; set; }

        /// <summary>
        /// 企业证书图片URLs
        /// </summary>
        public List<string> Certifications { get; set; }
    }

    /// <summary>
    /// 创建或更新公司信息DTO
    /// </summary>
    public class CreateUpdateCompanyProfileDto
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 公司Logo
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 公司封面图片
        /// </summary>
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 成立时间
        /// </summary>
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// 企业文化/愿景
        /// </summary>
        public string Vision { get; set; }

        /// <summary>
        /// 企业使命
        /// </summary>
        public string Mission { get; set; }

        /// <summary>
        /// 企业优势列表
        /// </summary>
        public List<AdvantageItem> Advantages { get; set; }

        /// <summary>
        /// 发展历程列表
        /// </summary>
        public List<MilestoneItem> Milestones { get; set; }

        /// <summary>
        /// 企业证书图片URLs
        /// </summary>
        public List<string> Certifications { get; set; }
    }

    /// <summary>
    /// 企业优势项
    /// </summary>
    public class AdvantageItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标/图片URL
        /// </summary>
        public string IconUrl { get; set; }
    }

    /// <summary>
    /// 发展历程项
    /// </summary>
    public class MilestoneItem
    {
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
    }
} 