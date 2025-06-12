using System;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Domain.Entities.Content
{
    /// <summary>
    /// 团队成员实体（关联员工模型）
    /// </summary>
    public class TeamMember
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 关联员工ID
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// 姓名（当不关联员工时使用）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职位（当不关联员工时使用）
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 简介/描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 团队成员类型ID
        /// </summary>
        public int TeamMemberTypeId { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 团队成员类型（导航属性）
        /// </summary>
        public TeamMemberType TeamMemberType { get; set; }

        /// <summary>
        /// 关联员工（导航属性）
        /// </summary>
        public Employee Employee { get; set; }
    }
} 