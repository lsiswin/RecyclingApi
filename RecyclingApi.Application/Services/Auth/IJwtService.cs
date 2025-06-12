using RecyclingApi.Domain.Entities.User;
using System.Security.Claims;

namespace RecyclingApi.Application.Services.Auth;

/// <summary>
/// JWT服务接口
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// 生成访问令牌
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <param name="roles">用户角色</param>
    /// <returns>访问令牌</returns>
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);

    /// <summary>
    /// 生成刷新令牌
    /// </summary>
    /// <returns>刷新令牌</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// 从令牌中获取用户声明
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <returns>用户声明</returns>
    ClaimsPrincipal? GetPrincipalFromToken(string token);

    /// <summary>
    /// 验证令牌是否有效
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <returns>是否有效</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// 从令牌中获取用户ID
    /// </summary>
    /// <param name="token">JWT令牌</param>
    /// <returns>用户ID</returns>
    string? GetUserIdFromToken(string token);
} 