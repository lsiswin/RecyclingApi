using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecyclingApi.Domain.Entities.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RecyclingApi.Application.Services.Auth;

/// <summary>
/// JWT服务实现
/// </summary>
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");
        _issuer = _configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
        _audience = _configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        _accessTokenExpirationMinutes = int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "60");
        _refreshTokenExpirationDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"] ?? "7");
    }

    /// <summary>
    /// 生成访问令牌
    /// </summary>
    public string GenerateAccessToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new("user_type", user.UserType.ToString()),
            new("real_name", user.RealName ?? string.Empty),
            new("avatar", user.Avatar ?? string.Empty),
            new("department", user.Department ?? string.Empty),
            new("position", user.Position ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // 添加角色声明
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 生成刷新令牌
    /// </summary>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// 从令牌中获取用户声明
    /// </summary>
    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = false, // 不验证过期时间，因为可能是刷新令牌场景
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 验证令牌是否有效
    /// </summary>
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 从令牌中获取用户ID
    /// </summary>
    public string? GetUserIdFromToken(string token)
    {
        var principal = GetPrincipalFromToken(token);
        return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
} 