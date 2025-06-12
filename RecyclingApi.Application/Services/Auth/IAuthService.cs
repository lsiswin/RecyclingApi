using RecyclingApi.Application.DTOs.Auth;

namespace RecyclingApi.Application.Services.Auth;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="request">登录请求</param>
    /// <returns>登录响应</returns>
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="request">注册请求</param>
    /// <returns>注册结果</returns>
    Task<bool> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <param name="request">刷新令牌请求</param>
    /// <returns>新的登录响应</returns>
    Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

    /// <summary>
    /// 忘记密码
    /// </summary>
    /// <param name="request">忘记密码请求</param>
    /// <returns>是否成功发送重置邮件</returns>
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request">重置密码请求</param>
    /// <returns>是否重置成功</returns>
    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);

    /// <summary>
    /// 验证令牌
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <returns>用户信息</returns>
    Task<UserInfoDto?> VerifyTokenAsync(string token);

    /// <summary>
    /// 用户登出
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>是否成功</returns>
    Task<bool> LogoutAsync(string userId);
} 