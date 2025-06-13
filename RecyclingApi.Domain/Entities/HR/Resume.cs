using System;
using System.Collections.Generic;
using RecyclingApi.Domain.Entities.User;

namespace RecyclingApi.Domain.Entities.HR
{
    /// <summary>
    /// 简历实体
    /// </summary>
    public class Resume
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
        /// 用户信息
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
} 