using Microsoft.AspNetCore.Identity;

namespace RecyclingApi.Domain.Entities.User;

/// <summary>
/// 应用程序用户实体
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; } = UserType.Customer;

    /// <summary>
    /// 部门（仅员工）
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// 职位（仅员工）
    /// </summary>
    public string? Position { get; set; }
}

/// <summary>
/// 用户类型枚举
/// </summary>
public enum UserType
{
    /// <summary>
    /// 客户
    /// </summary>
    Customer = 0,

    /// <summary>
    /// 员工
    /// </summary>
    Staff = 1,

    /// <summary>
    /// 管理员
    /// </summary>
    Admin = 2
} 