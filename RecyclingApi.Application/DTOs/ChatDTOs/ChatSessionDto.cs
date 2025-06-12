namespace RecyclingApi.Application.DTOs.ChatDTOs;

/// <summary>
/// 聊天会话数据传输对象
/// 用于传输聊天会话的详细信息
/// </summary>
public class ChatSessionDto
{
    /// <summary>
    /// 会话ID
    /// </summary>
    public string SessionId { get; set; } = string.Empty;
    
    /// <summary>
    /// 聊天室ID
    /// </summary>
    public int ChatRoomId { get; set; }
    
    /// <summary>
    /// 聊天室名称
    /// </summary>
    public string ChatRoomName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户信息
    /// </summary>
    public ChatUserDto User { get; set; } = new();
    
    /// <summary>
    /// 客服人员信息
    /// </summary>
    public ChatUserDto? CustomerService { get; set; }
    
    /// <summary>
    /// 会话标题
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// 会话描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 会话状态（0-活跃，1-等待，2-已结束，3-已关闭）
    /// </summary>
    public int Status { get; set; }
    
    /// <summary>
    /// 会话优先级（0-普通，1-重要，2-紧急）
    /// </summary>
    public int Priority { get; set; }
    
    /// <summary>
    /// 会话标签
    /// </summary>
    public List<string>? Tags { get; set; }
    
    /// <summary>
    /// 会话分类
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// 用户问题描述
    /// </summary>
    public string? UserProblem { get; set; }
    
    /// <summary>
    /// 解决方案
    /// </summary>
    public string? Solution { get; set; }
    
    /// <summary>
    /// 会话备注
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// 会话开始时间
    /// </summary>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// 会话结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 最后活跃时间
    /// </summary>
    public DateTime LastActiveTime { get; set; }
    
    /// <summary>
    /// 最后消息时间
    /// </summary>
    public DateTime? LastMessageTime { get; set; }
    
    /// <summary>
    /// 最后消息内容
    /// </summary>
    public string? LastMessageContent { get; set; }
    
    /// <summary>
    /// 最后消息发送者
    /// </summary>
    public string? LastMessageSender { get; set; }
    
    /// <summary>
    /// 消息总数
    /// </summary>
    public int MessageCount { get; set; }
    
    /// <summary>
    /// 未读消息数
    /// </summary>
    public int UnreadCount { get; set; }
    
    /// <summary>
    /// 用户满意度评分
    /// </summary>
    public int? SatisfactionRating { get; set; }
    
    /// <summary>
    /// 用户反馈
    /// </summary>
    public string? UserFeedback { get; set; }
    
    /// <summary>
    /// 会话持续时间（秒）
    /// </summary>
    public int? Duration { get; set; }
    
    /// <summary>
    /// 会话来源
    /// </summary>
    public string? Source { get; set; }
}

/// <summary>
/// 创建聊天会话请求DTO
/// </summary>
public class CreateChatSessionDto
{
    /// <summary>
    /// 聊天室ID
    /// </summary>
    public int ChatRoomId { get; set; }
    
    /// <summary>
    /// 用户信息
    /// </summary>
    public CreateChatUserDto User { get; set; } = new();
    
    /// <summary>
    /// 会话标题
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// 会话描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 会话优先级
    /// </summary>
    public int Priority { get; set; } = 0;
    
    /// <summary>
    /// 会话分类
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// 用户问题描述
    /// </summary>
    public string? UserProblem { get; set; }
    
    /// <summary>
    /// 会话来源
    /// </summary>
    public string? Source { get; set; }
    
    /// <summary>
    /// 来源页面URL
    /// </summary>
    public string? SourceUrl { get; set; }
}

/// <summary>
/// 更新聊天会话DTO
/// </summary>
public class UpdateChatSessionDto
{
    /// <summary>
    /// 会话标题
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// 会话描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 会话状态
    /// </summary>
    public int? Status { get; set; }
    
    /// <summary>
    /// 会话优先级
    /// </summary>
    public int? Priority { get; set; }
    
    /// <summary>
    /// 会话标签
    /// </summary>
    public List<string>? Tags { get; set; }
    
    /// <summary>
    /// 会话分类
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// 用户问题描述
    /// </summary>
    public string? UserProblem { get; set; }
    
    /// <summary>
    /// 解决方案
    /// </summary>
    public string? Solution { get; set; }
    
    /// <summary>
    /// 会话备注
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// 客服人员ID
    /// </summary>
    public string? CustomerServiceId { get; set; }
}

/// <summary>
/// 会话反馈DTO
/// </summary>
public class SessionFeedbackDto
{
    /// <summary>
    /// 用户满意度评分（1-5分）
    /// </summary>
    public int SatisfactionRating { get; set; }
    
    /// <summary>
    /// 用户反馈内容
    /// </summary>
    public string? UserFeedback { get; set; }
} 