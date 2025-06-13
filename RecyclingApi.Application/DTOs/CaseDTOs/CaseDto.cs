using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.CaseDTOs
{
    /// <summary>
    /// 案例DTO
    /// </summary>
    public class CaseDto
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
        /// 标签列表
        /// </summary>
        public List<string> Tags { get; set; }

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
        /// 服务亮点
        /// </summary>
        public List<string> Highlights { get; set; }

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
    }

    /// <summary>
    /// 创建或更新案例DTO
    /// </summary>
    public class CreateUpdateCaseDto
    {
        /// <summary>
        /// 案例标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(100, ErrorMessage = "标题长度不能超过100个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 简短描述
        /// </summary>
        [Required(ErrorMessage = "简短描述不能为空")]
        [StringLength(500, ErrorMessage = "简短描述长度不能超过500个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 完整描述
        /// </summary>
        [Required(ErrorMessage = "完整描述不能为空")]
        public string FullDescription { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "客户名称不能为空")]
        [StringLength(100, ErrorMessage = "客户名称长度不能超过100个字符")]
        public string Client { get; set; }

        /// <summary>
        /// 案例分类（enterprise:企业回收, school:学校回收, government:政府机构, hospital:医院回收）
        /// </summary>
        [Required(ErrorMessage = "案例分类不能为空")]
        public string Category { get; set; }

        /// <summary>
        /// 设备类型（desktop:台式电脑, laptop:笔记本电脑, server:服务器, network:网络设备）
        /// </summary>
        [Required(ErrorMessage = "设备类型不能为空")]
        public string DeviceType { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        [Required(ErrorMessage = "设备数量不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "设备数量必须大于0")]
        public int DeviceCount { get; set; }

        /// <summary>
        /// 回收日期
        /// </summary>
        [Required(ErrorMessage = "回收日期不能为空")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 项目周期
        /// </summary>
        [Required(ErrorMessage = "项目周期不能为空")]
        [StringLength(50, ErrorMessage = "项目周期长度不能超过50个字符")]
        public string Duration { get; set; }

        /// <summary>
        /// 回收规模（small:小规模, medium:中规模, large:大规模）
        /// </summary>
        [Required(ErrorMessage = "回收规模不能为空")]
        public string Scale { get; set; }

        /// <summary>
        /// 案例图片URL
        /// </summary>
        [Required(ErrorMessage = "案例图片不能为空")]
        public string Image { get; set; }

        /// <summary>
        /// 标签列表
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 评分（1-5）
        /// </summary>
        [Range(1, 5, ErrorMessage = "评分必须在1-5之间")]
        public decimal Rating { get; set; }

        /// <summary>
        /// 项目详情
        /// </summary>
        [Required(ErrorMessage = "项目详情不能为空")]
        public string ProjectDetails { get; set; }

        /// <summary>
        /// 服务亮点
        /// </summary>
        public List<string> Highlights { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 案例查询请求DTO
    /// </summary>
    public class CaseRequestDto : PagedRequestDto
    {
        /// <summary>
        /// 标题关键字
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 案例分类
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string? DeviceType { get; set; }

        /// <summary>
        /// 回收规模
        /// </summary>
        public string? Scale { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; } = true;
    }
} 