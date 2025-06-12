using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyclingApi.Application.Common.Redis;

/// <summary>
/// 聊天缓存服务接口
/// </summary>
public interface IChatCacheService
{
    #region 用户管理
    Task SetOnlineUserAsync(UserInfo user, TimeSpan? expiry = null);
    Task<UserInfo?> GetOnlineUserAsync(string userId);
    Task RemoveOnlineUserAsync(string userId);
    Task<List<UserInfo>> GetAllOnlineUsersAsync();
    Task<UserInfo?> GetUserByConnectionIdAsync(string connectionId);
    #endregion

    #region 客服管理
    Task SetOnlineStaffAsync(StaffInfo staff, TimeSpan? expiry = null);
    Task<StaffInfo?> GetOnlineStaffAsync(string staffId);
    Task RemoveOnlineStaffAsync(string staffId);
    Task<List<StaffInfo>> GetAllOnlineStaffAsync();
    Task<List<StaffInfo>> GetAvailableStaffAsync();
    Task UpdateStaffStatusAsync(string staffId, StaffStatus status);
    #endregion

    #region 访客-客服映射
    Task SetVisitorStaffMappingAsync(string visitorId, string staffId, TimeSpan? expiry = null);
    Task<string?> GetVisitorStaffMappingAsync(string visitorId);
    Task RemoveVisitorStaffMappingAsync(string visitorId);
    Task<List<string>> GetStaffVisitorsAsync(string staffId);
    #endregion

    #region 客服会话计数
    Task SetStaffConversationCountAsync(string staffId, int count);
    Task<int> GetStaffConversationCountAsync(string staffId);
    Task IncrementStaffConversationCountAsync(string staffId);
    Task DecrementStaffConversationCountAsync(string staffId);
    #endregion

    #region 消息存储
    Task SaveChatMessageAsync(ChatMessage message);
    Task<List<ChatMessage>> GetChatHistoryAsync(string sessionId, int limit = 50);
    Task<List<ChatMessage>> GetUserChatHistoryAsync(string userId, int limit = 50);
    #endregion

    #region 会话管理
    Task<string> CreateChatSessionAsync(string visitorId, string staffId);
    Task<ChatSession?> GetChatSessionAsync(string sessionId);
    Task UpdateChatSessionStatusAsync(string sessionId, ChatSessionStatus status);
    Task EndChatSessionAsync(string sessionId);
    #endregion

    #region 统计信息
    Task<int> GetOnlineUserCountAsync();
    Task<int> GetOnlineStaffCountAsync();
    Task<int> GetActiveSessionCountAsync();
    #endregion

    #region 缓存管理
    Task ClearCacheAsync(string pattern);
    Task<bool> IsConnectedAsync();
    #endregion
}

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public DateTime JoinTime { get; set; }
    public string? SessionId { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// 客服信息
/// </summary>
public class StaffInfo
{
    public string StaffId { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public StaffStatus Status { get; set; }
    public DateTime JoinTime { get; set; }
    public int MaxConcurrentChats { get; set; } = 5;
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// 客服状态枚举
/// </summary>
public enum StaffStatus
{
    Online,
    Busy,
    Away,
    Offline
}

/// <summary>
/// 聊天消息
/// </summary>
public class ChatMessage
{
    public string MessageId { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public bool IsFromStaff { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// 聊天会话
/// </summary>
public class ChatSession
{
    public string SessionId { get; set; } = string.Empty;
    public string VisitorId { get; set; } = string.Empty;
    public string StaffId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ChatSessionStatus Status { get; set; }
    public string? Summary { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// 聊天会话状态
/// </summary>
public enum ChatSessionStatus
{
    Active,
    Waiting,
    Ended,
    Transferred
} 