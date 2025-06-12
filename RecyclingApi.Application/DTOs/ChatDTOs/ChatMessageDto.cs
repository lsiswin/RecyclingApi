using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Application.DTOs.ChatDTOs;

/// <summary>
/// 聊天消息数据传输对象
/// 用于API接口的消息数据传输
/// </summary>
public class ChatMessageDto
{
    /// <summary>
    /// 消息唯一标识符
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// 发送者用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 发送者头像
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// 消息类型
    /// </summary>
    public MessageType Type { get; set; } = MessageType.Text;
    
    /// <summary>
    /// 是否为私聊消息
    /// </summary>
    public bool IsPrivate { get; set; } = false;
    
    /// <summary>
    /// 私聊目标用户ID
    /// </summary>
    public string? TargetUserId { get; set; }
    
    /// <summary>
    /// 回复的消息ID
    /// </summary>
    public string? ReplyToMessageId { get; set; }
    
    /// <summary>
    /// 附件信息
    /// </summary>
    public List<ChatAttachmentDto>? Attachments { get; set; }
    
    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; } = false;
    
    /// <summary>
    /// 是否已编辑
    /// </summary>
    public bool IsEdited { get; set; } = false;
    
    /// <summary>
    /// 编辑时间
    /// </summary>
    public DateTime? EditedAt { get; set; }
    
    /// <summary>
    /// 消息优先级
    /// </summary>
    public int Priority { get; set; } = 0;
    
    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsPinned { get; set; } = false;
    
    /// <summary>
    /// 消息反应
    /// </summary>
    public List<ChatReactionDto>? Reactions { get; set; }
    
    /// <summary>
    /// 消息发送时间
    /// </summary>
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// 聊天附件数据传输对象
/// </summary>
public class ChatAttachmentDto
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// 文件类型
    /// </summary>
    public string FileType { get; set; } = string.Empty;
    
    /// <summary>
    /// 文件URL
    /// </summary>
    public string FileUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// 缩略图URL（如果是图片或视频）
    /// </summary>
    public string? ThumbnailUrl { get; set; }
}

/// <summary>
/// 聊天消息反应数据传输对象
/// </summary>
public class ChatReactionDto
{
    /// <summary>
    /// 表情符号
    /// </summary>
    public string Emoji { get; set; } = string.Empty;
    
    /// <summary>
    /// 反应的用户列表
    /// </summary>
    public List<string> UserIds { get; set; } = new();
    
    /// <summary>
    /// 反应数量
    /// </summary>
    public int Count { get; set; }
} 