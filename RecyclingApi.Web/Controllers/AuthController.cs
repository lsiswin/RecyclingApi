using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.DTOs.Auth;
using RecyclingApi.Application.Services.Auth;
using RecyclingApi.Application.Common.Responses;
using System.Security.Claims;

namespace RecyclingApi.Web.Controllers;

/// <summary>
/// 认证控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="request">登录请求</param>
    /// <returns>登录响应</returns>
    [HttpPost("login")]
    public async Task<ApiResponse<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<LoginResponseDto>("请求参数无效");
            }

            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return new ApiResponse<LoginResponseDto>("用户名或密码错误");
            }

            return new ApiResponse<LoginResponseDto>(result, "登录成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登录接口发生错误");
            return new ApiResponse<LoginResponseDto>("服务器内部错误");
        }
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="request">注册请求</param>
    /// <returns>注册结果</returns>
    [HttpPost("register")]
    public async Task<ApiResponse<bool>> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<bool>("请求参数无效");
            }

            var result = await _authService.RegisterAsync(request);
            if (!result)
            {
                return new ApiResponse<bool>("注册失败，用户名或邮箱可能已存在");
            }

            return new ApiResponse<bool>(true, "注册成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "注册接口发生错误");
            return new ApiResponse<bool>("服务器内部错误");
        }
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <param name="request">刷新令牌请求</param>
    /// <returns>新的令牌</returns>
    [HttpPost("refresh")]
    public async Task<ApiResponse<string>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<string>("请求参数无效");
            }

            var result = await _authService.RefreshTokenAsync(request);
            if (result == null)
            {
                return new ApiResponse<string>("刷新令牌无效");
            }

            return new ApiResponse<string>("令牌刷新成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "刷新令牌接口发生错误");
            return new ApiResponse<string>("服务器内部错误");
        }
    }

    /// <summary>
    /// 忘记密码
    /// </summary>
    /// <param name="request">忘记密码请求</param>
    /// <returns>处理结果</returns>
    [HttpPost("forgot-password")]
    public async Task<ApiResponse<bool>> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<bool>("请求参数无效");
            }

            var result = await _authService.ForgotPasswordAsync(request);
            if (!result)
            {
                return new ApiResponse<bool>("发送重置邮件失败");
            }

            return new ApiResponse<bool>(true, "重置密码邮件已发送，请查收");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "忘记密码接口发生错误");
            return new ApiResponse<bool>("服务器内部错误");
        }
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request">重置密码请求</param>
    /// <returns>重置结果</returns>
    [HttpPost("reset-password")]
    public async Task<ApiResponse<bool>> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<bool>("请求参数无效");
            }

            var result = await _authService.ResetPasswordAsync(request);
            if (!result)
            {
                return new ApiResponse<bool>("密码重置失败，请检查重置令牌是否有效");
            }

            return new ApiResponse<bool>(true, "密码重置成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "重置密码接口发生错误");
            return new ApiResponse<bool>("服务器内部错误");
        }
    }

    /// <summary>
    /// 验证令牌
    /// </summary>
    /// <returns>用户信息</returns>
    [HttpGet("verify")]
    [Authorize]
    public async Task<ApiResponse<UserInfoDto>> VerifyToken()
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<UserInfoDto>("令牌不能为空");
            }

            var result = await _authService.VerifyTokenAsync(token);
            if (result == null)
            {
                return new ApiResponse<UserInfoDto>("令牌无效");
            }

            return new ApiResponse<UserInfoDto>(result, "令牌验证成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "验证令牌接口发生错误");
            return new ApiResponse<UserInfoDto>("服务器内部错误");
        }
    }

    /// <summary>
    /// 用户登出
    /// </summary>
    /// <returns>登出结果</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ApiResponse<bool>> Logout()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<bool>("用户未登录");
            }

            var result = await _authService.LogoutAsync(userId);
            if (!result)
            {
                return new ApiResponse<bool>("登出失败");
            }

            return new ApiResponse<bool>(true, "登出成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登出接口发生错误");
            return new ApiResponse<bool>("服务器内部错误");
        }
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    /// <returns>用户信息</returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<ApiResponse<UserInfoDto>> GetCurrentUser()
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<UserInfoDto>("令牌不能为空");
            }

            var result = await _authService.VerifyTokenAsync(token);
            if (result == null)
            {
                return new ApiResponse<UserInfoDto>("令牌无效");
            }

            return new ApiResponse<UserInfoDto>(result, "获取用户信息成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户信息接口发生错误");
            return new ApiResponse<UserInfoDto>("服务器内部错误");
        }
    }
} 