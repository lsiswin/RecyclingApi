using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Application.DTOs.ChatDTOs;

/// <summary>
/// 聊天用户数据传输对象
/// 用于传输用户的聊天相关信息
/// </summary>
public class ChatUserDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户真实姓名
    /// </summary>
    public string? RealName { get; set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 用户邮箱
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// 用户电话
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Offline;
    
    /// <summary>
    /// 是否为客服人员
    /// </summary>
    public bool IsCustomerService { get; set; } = false;
    
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin { get; set; } = false;
    
    /// <summary>
    /// 最后在线时间
    /// </summary>
    public DateTime? LastOnlineTime { get; set; }
    
    /// <summary>
    /// 加入聊天时间
    /// </summary>
    public DateTime JoinTime { get; set; }
    
    /// <summary>
    /// 连接ID
    /// </summary>
    public string? ConnectionId { get; set; }
}

/// <summary>
/// 创建聊天用户请求DTO
/// </summary>
public class CreateChatUserDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 用户邮箱
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// 用户电话
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// 用户真实姓名
    /// </summary>
    public string? RealName { get; set; }
}

/// <summary>
/// 更新聊天用户信息DTO
/// </summary>
public class UpdateChatUserDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus? Status { get; set; }
    
    /// <summary>
    /// 用户邮箱
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// 用户电话
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// 用户真实姓名
    /// </summary>
    public string? RealName { get; set; }
}

/// <summary>
/// 在线用户信息DTO
/// </summary>
public class OnlineUserDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Online;
    
    /// <summary>
    /// 是否为客服人员
    /// </summary>
    public bool IsCustomerService { get; set; } = false;
    
    /// <summary>
    /// 加入时间
    /// </summary>
    public DateTime JoinTime { get; set; }
    
    /// <summary>
    /// 连接ID
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;
} 