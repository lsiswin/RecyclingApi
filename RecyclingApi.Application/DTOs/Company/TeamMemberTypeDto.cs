using System;
using System.Collections.Generic;

namespace RecyclingApi.Application.DTOs.Company
{
    /// <summary>
    /// 团队成员类型DTO
    /// </summary>
    public class TeamMemberTypeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否在首页显示
        /// </summary>
        public bool ShowOnHome { get; set; }
    }

    /// <summary>
    /// 创建或更新团队成员类型DTO
    /// </summary>
    public class CreateUpdateTeamMemberTypeDto
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否在首页显示
        /// </summary>
        public bool ShowOnHome { get; set; }
    }
    
    /// <summary>
    /// 员工简略信息DTO（用于团队成员关联）
    /// </summary>
    public class EmployeeSimpleDto
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
        /// 头像URL
        /// </summary>
        public string AvatarUrl { get; set; }
    }
} 