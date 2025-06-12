using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.Auth;

/// <summary>
/// 发送验证码请求DTO
/// </summary>
public class SendVerificationCodeRequestDto
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// 验证码类型
    /// </summary>
    public string Type { get; set; } = "default";
}

/// <summary>
/// 发送短信验证码请求DTO
/// </summary>
public class SendVerificationCodeSmsRequestDto
{
    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "手机号不能为空")]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// 验证码类型
    /// </summary>
    public string Type { get; set; } = "default";
}

/// <summary>
/// 验证码验证请求DTO
/// </summary>
public class VerifyCodeRequestDto
{
    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "验证码不能为空")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "验证码长度必须为6位")]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// 验证码类型
    /// </summary>
    public string Type { get; set; } = "default";
} 