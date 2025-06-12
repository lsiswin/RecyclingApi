using System;
using System.Collections.Generic;

namespace RecyclingApi.Domain.Entities.Content
{
    /// <summary>
    /// 团队成员类型实体
    /// </summary>
    public class TeamMemberType
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类型名称（如：管理层、技术团队、销售团队等）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否在首页显示
        /// </summary>
        public bool ShowOnHome { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 关联员工列表（导航属性）
        /// </summary>
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
} 