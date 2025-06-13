using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.HR
{
    /// <summary>
    /// 简历数据传输对象
    /// </summary>
    public class ResumeDTO
    {
        /// <summary>
        /// 简历ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 简历标题
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 文件URL
        /// </summary>
        public string FileUrl { get; set; }
        
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        
        /// <summary>
        /// 技能描述
        /// </summary>
        public string Skills { get; set; }
        
        /// <summary>
        /// 教育经历
        /// </summary>
        public string Education { get; set; }
        
        /// <summary>
        /// 工作经验
        /// </summary>
        public string WorkExperience { get; set; }
        
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo { get; set; }
        
        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate { get; set; }
        
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
    
    /// <summary>
    /// 创建简历数据传输对象
    /// </summary>
    public class CreateResumeDTO
    {
        /// <summary>
        /// 简历标题
        /// </summary>
        [Required(ErrorMessage = "简历标题不能为空")]
        [StringLength(100, ErrorMessage = "简历标题不能超过100个字符")]
        public string Title { get; set; }
        
        /// <summary>
        /// 简历文件（前端传输）
        /// </summary>
        public string ResumeFileBase64 { get; set; }
        
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 技能描述
        /// </summary>
        public string Skills { get; set; }
        
        /// <summary>
        /// 教育经历
        /// </summary>
        public string Education { get; set; }
        
        /// <summary>
        /// 工作经验
        /// </summary>
        public string WorkExperience { get; set; }
        
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo { get; set; }
    }
    
    /// <summary>
    /// 更新简历数据传输对象
    /// </summary>
    public class UpdateResumeDTO
    {
        /// <summary>
        /// 简历ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 简历标题
        /// </summary>
        [Required(ErrorMessage = "简历标题不能为空")]
        [StringLength(100, ErrorMessage = "简历标题不能超过100个字符")]
        public string Title { get; set; }
        
        /// <summary>
        /// 简历文件（Base64编码）
        /// </summary>
        public string ResumeFileBase64 { get; set; }
        
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 技能描述
        /// </summary>
        public string Skills { get; set; }
        
        /// <summary>
        /// 教育经历
        /// </summary>
        public string Education { get; set; }
        
        /// <summary>
        /// 工作经验
        /// </summary>
        public string WorkExperience { get; set; }
        
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
    
    /// <summary>
    /// 简历筛选条件数据传输对象
    /// </summary>
    public class ResumeFilterDTO : PagedRequestDto
    {
        /// <summary>
        /// 简历标题
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        
        /// <summary>
        /// 技能关键词
        /// </summary>
        public string? Skills { get; set; }
        
        /// <summary>
        /// 上传日期开始
        /// </summary>
        public DateTime? UploadDateFrom { get; set; }
        
        /// <summary>
        /// 上传日期结束
        /// </summary>
        public DateTime? UploadDateTo { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool? IsActive { get; set; }
    }
} 