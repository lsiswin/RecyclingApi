using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RecyclingApi.Application.DTOs.Auth;
using RecyclingApi.Domain.Entities.User;

namespace RecyclingApi.Application.Services.Auth;

/// <summary>
/// 认证服务实现
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        try
        {
            // 查找用户（支持用户名、邮箱、手机号登录）
            var user = await FindUserByUsernameAsync(request.Username);
            if (user == null)
            {
                _logger.LogWarning("登录失败：用户不存在 - {Username}", request.Username);
                return null;
            }

            // 检查用户是否激活
            if (!user.IsActive)
            {
                _logger.LogWarning("登录失败：用户已被禁用 - {UserId}", user.Id);
                return null;
            }

            // 验证密码
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                _logger.LogWarning("登录失败：密码错误 - {UserId}", user.Id);
                return null;
            }

            // 更新最后登录时间
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // 获取用户角色
            var roles = await _userManager.GetRolesAsync(user);

            // 生成JWT令牌
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // 构建响应
            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600, // 1小时
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    RealName = user.RealName,
                    Avatar = user.Avatar,
                    UserType = user.UserType,
                    Department = user.Department,
                    Position = user.Position,
                    Roles = roles.ToList(),
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                }
            };

            _logger.LogInformation("用户登录成功 - {UserId}", user.Id);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登录过程中发生错误 - {Username}", request.Username);
            return null;
        }
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    public async Task<bool> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            // 检查用户名是否已存在
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("注册失败：用户名已存在 - {Username}", request.Username);
                return false;
            }

            // 检查邮箱是否已存在
            existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("注册失败：邮箱已存在 - {Email}", request.Email);
                return false;
            }

            // 创建新用户
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                RealName = request.RealName,
                UserType = request.UserType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // 创建用户
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("注册失败：{Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            // 添加默认角色
            var defaultRole = request.UserType switch
            {
                UserType.Staff => "Staff",
                UserType.Admin => "Admin",
                _ => "Customer"
            };

            await _userManager.AddToRoleAsync(user, defaultRole);

            _logger.LogInformation("用户注册成功 - {UserId}", user.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "注册过程中发生错误 - {Username}", request.Username);
            return false;
        }
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public async Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        try
        {
            // 从刷新令牌中获取用户信息
            var principal = _jwtService.GetPrincipalFromToken(request.RefreshToken);
            if (principal == null)
            {
                _logger.LogWarning("刷新令牌失败：无效的令牌");
                return null;
            }

            var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("刷新令牌失败：无法获取用户ID");
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("刷新令牌失败：用户不存在或已禁用 - {UserId}", userId);
                return null;
            }

            // 获取用户角色
            var roles = await _userManager.GetRolesAsync(user);

            // 生成新的JWT令牌
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    RealName = user.RealName,
                    Avatar = user.Avatar,
                    UserType = user.UserType,
                    Department = user.Department,
                    Position = user.Position,
                    Roles = roles.ToList(),
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                }
            };

            _logger.LogInformation("令牌刷新成功 - {UserId}", user.Id);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "刷新令牌过程中发生错误");
            return null;
        }
    }

    /// <summary>
    /// 忘记密码
    /// </summary>
    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // 为了安全，即使用户不存在也返回true
                _logger.LogWarning("忘记密码：用户不存在 - {Email}", request.Email);
                return true;
            }

            // 生成密码重置令牌
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: 发送重置密码邮件
            // 这里应该集成邮件服务发送重置链接
            _logger.LogInformation("密码重置令牌已生成 - {UserId}, Token: {Token}", user.Id, token);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "忘记密码过程中发生错误 - {Email}", request.Email);
            return false;
        }
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("重置密码失败：用户不存在 - {Email}", request.Email);
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                _logger.LogWarning("重置密码失败：{Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            _logger.LogInformation("密码重置成功 - {UserId}", user.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "重置密码过程中发生错误 - {Email}", request.Email);
            return false;
        }
    }

    /// <summary>
    /// 验证令牌
    /// </summary>
    public async Task<UserInfoDto?> VerifyTokenAsync(string token)
    {
        try
        {
            if (!_jwtService.ValidateToken(token))
            {
                return null;
            }

            var userId = _jwtService.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                RealName = user.RealName,
                Avatar = user.Avatar,
                UserType = user.UserType,
                Department = user.Department,
                Position = user.Position,
                Roles = roles.ToList(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "验证令牌过程中发生错误");
            return null;
        }
    }

    /// <summary>
    /// 用户登出
    /// </summary>
    public async Task<bool> LogoutAsync(string userId)
    {
        try
        {
            // 这里可以添加令牌黑名单逻辑
            // 或者清除用户的刷新令牌
            _logger.LogInformation("用户登出 - {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登出过程中发生错误 - {UserId}", userId);
            return false;
        }
    }

    /// <summary>
    /// 根据用户名查找用户（支持用户名、邮箱、手机号）
    /// </summary>
    private async Task<ApplicationUser?> FindUserByUsernameAsync(string username)
    {
        // 先尝试用户名
        var user = await _userManager.FindByNameAsync(username);
        if (user != null) return user;

        // 再尝试邮箱
        user = await _userManager.FindByEmailAsync(username);
        if (user != null) return user;

        // 最后尝试手机号 - 简化实现，实际项目中应该使用数据库查询
        return null;
    }
} 