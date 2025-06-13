using System;
using System.Collections.Generic;
using RecyclingApi.Domain.Entities.User;

namespace RecyclingApi.Domain.Entities.HR
{
    /// <summary>
    /// 职位申请实体
    /// </summary>
    public class JobApplication
    {
        /// <summary>
        /// 申请ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 申请人邮箱
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 申请人电话
        /// </summary>
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
        /// 申请职位
        /// </summary>
        public virtual JobPosition JobPosition { get; set; }
        
        /// <summary>
        /// 申请状态
        /// </summary>
        public ApplicationStatus Status { get; set; }
        
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
        
        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }

    /// <summary>
    /// 申请状态枚举
    /// </summary>
    public enum ApplicationStatus
    {
        /// <summary>
        /// 新申请
        /// </summary>
        New = 0,
        
        /// <summary>
        /// 简历筛选中
        /// </summary>
        Reviewing = 1,
        
        /// <summary>
        /// 面试阶段
        /// </summary>
        Interview = 2,
        
        /// <summary>
        /// 已发送录用通知
        /// </summary>
        Offered = 3,
        
        /// <summary>
        /// 已录用
        /// </summary>
        Hired = 4,
        
        /// <summary>
        /// 已拒绝
        /// </summary>
        Rejected = 5
    }
} 