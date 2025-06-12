using System;

namespace RecyclingApi.Domain.Entities.Content
{
    /// <summary>
    /// 公司展示信息实体
    /// </summary>
    public class CompanyProfile
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
        /// 企业优势描述（JSON格式，包含标题、描述和图标）
        /// </summary>
        public string AdvantagesJson { get; set; }

        /// <summary>
        /// 发展历程（JSON格式，包含年份、标题和描述）
        /// </summary>
        public string MilestonesJson { get; set; }

        /// <summary>
        /// 企业证书图片URLs（JSON数组）
        /// </summary>
        public string CertificationsJson { get; set; }

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