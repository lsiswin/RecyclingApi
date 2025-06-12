using System;
using System.Threading.Tasks;

namespace RecyclingApi.Application.Common.MessageQueue;

/// <summary>
/// 聊天消息队列服务接口
/// </summary>
public interface IChatMessageQueueService
{
    /// <summary>
    /// 发布聊天消息到队列
    /// </summary>
    Task PublishChatMessageAsync(ChatMessageQueueItem message);

    /// <summary>
    /// 发布客服状态变更到队列
    /// </summary>
    Task PublishStaffStatusChangeAsync(StaffStatusChangeItem statusChange);

    /// <summary>
    /// 发布访客分配事件到队列
    /// </summary>
    Task PublishVisitorAssignmentAsync(VisitorAssignmentItem assignment);

    /// <summary>
    /// 发布系统通知到队列
    /// </summary>
    Task PublishSystemNotificationAsync(SystemNotificationItem notification);

    /// <summary>
    /// 订阅聊天消息
    /// </summary>
    Task SubscribeChatMessagesAsync(Func<ChatMessageQueueItem, Task> handler);

    /// <summary>
    /// 订阅客服状态变更
    /// </summary>
    Task SubscribeStaffStatusChangesAsync(Func<StaffStatusChangeItem, Task> handler);

    /// <summary>
    /// 订阅访客分配事件
    /// </summary>
    Task SubscribeVisitorAssignmentsAsync(Func<VisitorAssignmentItem, Task> handler);

    /// <summary>
    /// 订阅系统通知
    /// </summary>
    Task SubscribeSystemNotificationsAsync(Func<SystemNotificationItem, Task> handler);
}

/// <summary>
/// 聊天消息队列项
/// </summary>
public class ChatMessageQueueItem
{
    public string MessageId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public string? TargetUserId { get; set; }
    public string SessionId { get; set; } = string.Empty;
}

/// <summary>
/// 客服状态变更项
/// </summary>
public class StaffStatusChangeItem
{
    public string StaffId { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Reason { get; set; }
}

/// <summary>
/// 访客分配项
/// </summary>
public class VisitorAssignmentItem
{
    public string VisitorId { get; set; } = string.Empty;
    public string StaffId { get; set; } = string.Empty;
    public string AssignmentType { get; set; } = string.Empty; // assign, transfer, release
    public DateTime Timestamp { get; set; }
    public string? Reason { get; set; }
    public string SessionId { get; set; } = string.Empty;
}

/// <summary>
/// 系统通知项
/// </summary>
public class SystemNotificationItem
{
    public string NotificationId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? TargetUserId { get; set; }
    public string? TargetStaffId { get; set; }
    public Dictionary<string, object>? Data { get; set; }
} 