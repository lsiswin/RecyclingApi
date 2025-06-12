using System;
using System.Collections.Generic;
using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Domain.Entities.HR
{
    /// <summary>
    /// 员工实体类
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 员工简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// 是否在职
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
        /// 关联的团队成员（导航属性）
        /// </summary>
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
} 