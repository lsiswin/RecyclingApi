using System.ComponentModel.DataAnnotations;
using RecyclingApi.Domain.Entities.User;

namespace RecyclingApi.Application.DTOs.Auth;

/// <summary>
/// 登录请求DTO
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// 用户名/邮箱/手机号
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 记住我
    /// </summary>
    public bool RememberMe { get; set; } = false;
}

/// <summary>
/// 注册请求DTO
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "用户名长度在3到20个字符")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "手机号不能为空")]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 确认密码
    /// </summary>
    [Required(ErrorMessage = "确认密码不能为空")]
    [Compare("Password", ErrorMessage = "两次输入密码不一致")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; } = UserType.Customer;
}

/// <summary>
/// 登录响应DTO
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// 令牌类型
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// 过期时间（秒）
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// 用户信息
    /// </summary>
    public UserInfoDto User { get; set; } = new();
}

/// <summary>
/// 用户信息DTO
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
}

/// <summary>
/// 忘记密码请求DTO
/// </summary>
public class ForgotPasswordRequestDto
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// 重置密码请求DTO
/// </summary>
public class ResetPasswordRequestDto
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 重置令牌
    /// </summary>
    [Required(ErrorMessage = "重置令牌不能为空")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// 确认新密码
    /// </summary>
    [Required(ErrorMessage = "确认密码不能为空")]
    [Compare("NewPassword", ErrorMessage = "两次输入密码不一致")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// 刷新令牌请求DTO
/// </summary>
public class RefreshTokenRequestDto
{
    /// <summary>
    /// 刷新令牌
    /// </summary>
    [Required(ErrorMessage = "刷新令牌不能为空")]
    public string RefreshToken { get; set; } = string.Empty;
} 