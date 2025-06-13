using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.HR
{
    /// <summary>
    /// 职位数据传输对象
    /// </summary>
    public class JobPositionDTO
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 职位名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        [StringLength(100, ErrorMessage = "职位名称不能超过100个字符")]
        public string Title { get; set; }
        
        /// <summary>
        /// 职位描述
        /// </summary>
        [Required(ErrorMessage = "职位描述不能为空")]
        public string Description { get; set; }
        
        /// <summary>
        /// 职位要求
        /// </summary>
        public string Requirements { get; set; }
        
        /// <summary>
        /// 所属部门
        /// </summary>
        [Required(ErrorMessage = "部门不能为空")]
        [StringLength(50, ErrorMessage = "部门名称不能超过50个字符")]
        public string Department { get; set; }
        
        /// <summary>
        /// 工作地点
        /// </summary>
        [Required(ErrorMessage = "工作地点不能为空")]
        [StringLength(100, ErrorMessage = "工作地点不能超过100个字符")]
        public string Location { get; set; }
        
        /// <summary>
        /// 薪资下限
        /// </summary>
        public decimal? SalaryMin { get; set; }
        
        /// <summary>
        /// 薪资上限
        /// </summary>
        public decimal? SalaryMax { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PostedDate { get; set; }
        
        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
        
        /// <summary>
        /// 申请人数
        /// </summary>
        public int ApplicationCount { get; set; }
    }
    
    /// <summary>
    /// 创建职位数据传输对象
    /// </summary>
    public class CreateJobPositionDTO
    {
        /// <summary>
        /// 职位名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        [StringLength(100, ErrorMessage = "职位名称不能超过100个字符")]
        public string Title { get; set; }
        
        /// <summary>
        /// 职位描述
        /// </summary>
        [Required(ErrorMessage = "职位描述不能为空")]
        public string Description { get; set; }
        
        /// <summary>
        /// 职位要求
        /// </summary>
        public string Requirements { get; set; }
        
        /// <summary>
        /// 所属部门
        /// </summary>
        [Required(ErrorMessage = "部门不能为空")]
        [StringLength(50, ErrorMessage = "部门名称不能超过50个字符")]
        public string Department { get; set; }
        
        /// <summary>
        /// 工作地点
        /// </summary>
        [Required(ErrorMessage = "工作地点不能为空")]
        [StringLength(100, ErrorMessage = "工作地点不能超过100个字符")]
        public string Location { get; set; }
        
        /// <summary>
        /// 薪资下限
        /// </summary>
        public decimal? SalaryMin { get; set; }
        
        /// <summary>
        /// 薪资上限
        /// </summary>
        public decimal? SalaryMax { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
    }
    
    /// <summary>
    /// 更新职位数据传输对象
    /// </summary>
    public class UpdateJobPositionDTO
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 职位名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        [StringLength(100, ErrorMessage = "职位名称不能超过100个字符")]
        public string Title { get; set; }
        
        /// <summary>
        /// 职位描述
        /// </summary>
        [Required(ErrorMessage = "职位描述不能为空")]
        public string Description { get; set; }
        
        /// <summary>
        /// 职位要求
        /// </summary>
        public string Requirements { get; set; }
        
        /// <summary>
        /// 所属部门
        /// </summary>
        [Required(ErrorMessage = "部门不能为空")]
        [StringLength(50, ErrorMessage = "部门名称不能超过50个字符")]
        public string Department { get; set; }
        
        /// <summary>
        /// 工作地点
        /// </summary>
        [Required(ErrorMessage = "工作地点不能为空")]
        [StringLength(100, ErrorMessage = "工作地点不能超过100个字符")]
        public string Location { get; set; }
        
        /// <summary>
        /// 薪资下限
        /// </summary>
        public decimal? SalaryMin { get; set; }
        
        /// <summary>
        /// 薪资上限
        /// </summary>
        public decimal? SalaryMax { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
    }
} 