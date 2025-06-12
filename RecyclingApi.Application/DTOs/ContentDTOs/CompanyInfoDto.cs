using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.ContentDTOs
{
    /// <summary>
    /// 公司信息DTO
    /// </summary>
    public class CompanyInfoDto
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
        public List<CompanyAdvantageDto> Advantages { get; set; }

        /// <summary>
        /// 公司发展历程列表
        /// </summary>
        public List<CompanyMilestoneDto> Milestones { get; set; }

        /// <summary>
        /// 团队成员列表
        /// </summary>
        public List<TeamMemberDto> TeamMembers { get; set; }
    }

    /// <summary>
    /// 更新公司信息DTO
    /// </summary>
    public class UpdateCompanyInfoDto
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required(ErrorMessage = "公司名称不能为空")]
        [StringLength(100, ErrorMessage = "公司名称长度不能超过100个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        [Required(ErrorMessage = "公司简介不能为空")]
        public string Introduction { get; set; }

        /// <summary>
        /// 公司介绍图片
        /// </summary>
        [Required(ErrorMessage = "公司介绍图片不能为空")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 成立时间
        /// </summary>
        public DateTime EstablishmentDate { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Required(ErrorMessage = "联系电话不能为空")]
        [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Required(ErrorMessage = "电子邮箱不能为空")]
        [EmailAddress(ErrorMessage = "电子邮箱格式不正确")]
        public string Email { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [Required(ErrorMessage = "公司地址不能为空")]
        public string Address { get; set; }
    }

    /// <summary>
    /// 公司优势DTO
    /// </summary>
    public class CompanyAdvantageDto
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
    }

    /// <summary>
    /// 创建或更新公司优势DTO
    /// </summary>
    public class CreateUpdateAdvantageDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(50, ErrorMessage = "标题长度不能超过50个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "描述不能为空")]
        [StringLength(200, ErrorMessage = "描述长度不能超过200个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        [Required(ErrorMessage = "图标名称不能为空")]
        public string Icon { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        [Range(0, 999, ErrorMessage = "排序号必须在0-999之间")]
        public int Sort { get; set; }
    }

    /// <summary>
    /// 公司发展历程DTO
    /// </summary>
    public class CompanyMilestoneDto
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
    }

    /// <summary>
    /// 创建或更新公司发展历程DTO
    /// </summary>
    public class CreateUpdateMilestoneDto
    {
        /// <summary>
        /// 年份
        /// </summary>
        [Required(ErrorMessage = "年份不能为空")]
        [StringLength(10, ErrorMessage = "年份长度不能超过10个字符")]
        public string Year { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(100, ErrorMessage = "标题长度不能超过100个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "描述不能为空")]
        [StringLength(500, ErrorMessage = "描述长度不能超过500个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        [Range(0, 999, ErrorMessage = "排序号必须在0-999之间")]
        public int Sort { get; set; }
    }

    /// <summary>
    /// 团队成员DTO
    /// </summary>
    public class TeamMemberDto
    {
        /// <summary>
        /// 成员ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 创建或更新团队成员DTO
    /// </summary>
    public class CreateUpdateTeamMemberDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(50, ErrorMessage = "姓名长度不能超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [Required(ErrorMessage = "职位不能为空")]
        [StringLength(50, ErrorMessage = "职位长度不能超过50个字符")]
        public string Position { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required(ErrorMessage = "简介不能为空")]
        [StringLength(500, ErrorMessage = "简介长度不能超过500个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Required(ErrorMessage = "头像不能为空")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        [Range(0, 999, ErrorMessage = "排序号必须在0-999之间")]
        public int Sort { get; set; }
    }
} 