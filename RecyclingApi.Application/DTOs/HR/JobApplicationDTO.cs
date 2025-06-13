using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Application.DTOs.HR
{
    /// <summary>
    /// 职位申请数据传输对象
    /// </summary>
    public class JobApplicationDTO
    {
        /// <summary>
        /// 申请ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 申请人姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(50, ErrorMessage = "姓名不能超过50个字符")]
        public string Name { get; set; }
        
        /// <summary>
        /// 申请人邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
        
        /// <summary>
        /// 申请人电话
        /// </summary>
        [Required(ErrorMessage = "电话不能为空")]
        [Phone(ErrorMessage = "电话格式不正确")]
        public string Phone { get; set; }
        
        /// <summary>
        /// 简历URL
        /// </summary>
        public string ResumeUrl { get; set; }
        
        /// <summary>
        /// 求职信
        /// </summary>
        public string CoverLetter { get; set; }
        
        /// <summary>
        /// 申请职位ID
        /// </summary>
        public int JobPositionId { get; set; }
        
        /// <summary>
        /// 申请职位名称
        /// </summary>
        public string JobPositionTitle { get; set; }
        
        /// <summary>
        /// 申请状态
        /// </summary>
        public ApplicationStatus Status { get; set; }
        
        /// <summary>
        /// 申请状态名称
        /// </summary>
        public string StatusName => Status.ToString();
        
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime AppliedDate { get; set; }
        
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
    }
    
    /// <summary>
    /// 创建职位申请数据传输对象
    /// </summary>
    public class CreateJobApplicationDTO
    {
        /// <summary>
        /// 申请人姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(50, ErrorMessage = "姓名不能超过50个字符")]
        public string Name { get; set; }
        
        /// <summary>
        /// 申请人邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
        
        /// <summary>
        /// 申请人电话
        /// </summary>
        [Required(ErrorMessage = "电话不能为空")]
        [Phone(ErrorMessage = "电话格式不正确")]
        public string Phone { get; set; }
        
        /// <summary>
        /// 简历URL
        /// </summary>
        [Required(ErrorMessage = "请上传简历")]
        public string ResumeUrl { get; set; }
        
        /// <summary>
        /// 求职信
        /// </summary>
        public string CoverLetter { get; set; }
        
        /// <summary>
        /// 申请职位ID
        /// </summary>
        [Required(ErrorMessage = "请选择应聘职位")]
        public int JobPositionId { get; set; }
    }
    
    /// <summary>
    /// 更新职位申请状态数据传输对象
    /// </summary>
    public class UpdateJobApplicationStatusDTO
    {
        /// <summary>
        /// 申请ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 申请状态
        /// </summary>
        [Required(ErrorMessage = "请选择申请状态")]
        public ApplicationStatus Status { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }
    }
    
    /// <summary>
    /// 职位申请筛选条件数据传输对象
    /// </summary>
    public class JobApplicationFilterDTO : PagedRequestDto
    {
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// 申请人邮箱
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// 职位ID
        /// </summary>
        public int? JobPositionId { get; set; }
        
        /// <summary>
        /// 申请状态
        /// </summary>
        public ApplicationStatus? Status { get; set; }
        
        /// <summary>
        /// 申请日期开始
        /// </summary>
        public DateTime? AppliedDateFrom { get; set; }
        
        /// <summary>
        /// 申请日期结束
        /// </summary>
        public DateTime? AppliedDateTo { get; set; }
    }
} 