using System;
using System.Collections.Generic;

namespace RecyclingApi.Domain.Entities.HR
{
    /// <summary>
    /// 职位实体
    /// </summary>
    public class JobPosition
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 职位名称
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 职位描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 职位要求
        /// </summary>
        public string Requirements { get; set; }
        
        /// <summary>
        /// 所属部门
        /// </summary>
        public string Department { get; set; }
        
        /// <summary>
        /// 工作地点
        /// </summary>
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
        /// 职位申请集合
        /// </summary>
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
} 