using RecyclingApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyclingApi.Domain.Entities.Chat;

/// <summary>
/// 聊天消息实体
/// 存储聊天系统中的消息详细信息
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// 消息唯一标识符
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// 消息UUID（用于前端标识）
    /// </summary>
    [Required]
    [StringLength(50)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// 发送者用户ID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SenderId { get; set; } = string.Empty;
    
    /// <summary>
    /// 发送者用户名
    /// </summary>
    [Required]
    [StringLength(100)]
    public string SenderName { get; set; } = string.Empty;
    
    /// <summary>
    /// 发送者头像
    /// </summary>
    [StringLength(500)]
    public string? SenderAvatar { get; set; }
    
    /// <summary>
    /// 聊天室ID
    /// </summary>
    public int ChatRoomId { get; set; }
    
    /// <summary>
    /// 会话ID（可选，用于私聊或特定会话）
    /// </summary>
    public int? SessionId { get; set; }
    
    /// <summary>
    /// 消息内容
    /// </summary>
    [Required]
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
    /// 私聊目标用户ID（仅私聊消息有效）
    /// </summary>
    [StringLength(50)]
    public string? TargetUserId { get; set; }
    
    /// <summary>
    /// 私聊目标用户名（仅私聊消息有效）
    /// </summary>
    [StringLength(100)]
    public string? TargetUserName { get; set; }
    
    /// <summary>
    /// 回复的消息ID（用于消息回复功能）
    /// </summary>
    public int? ReplyToMessageId { get; set; }
    
    /// <summary>
    /// 附件信息（JSON格式）
    /// 包含文件名、大小、类型、URL等信息
    /// </summary>
    public string? Attachments { get; set; }
    
    /// <summary>
    /// 消息元数据（JSON格式）
    /// 包含额外的消息信息，如位置、链接预览等
    /// </summary>
    public string? Metadata { get; set; }
    
    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; } = false;
    
    /// <summary>
    /// 已读用户列表（JSON格式）
    /// 存储已读此消息的用户ID和时间
    /// </summary>
    public string? ReadByUsers { get; set; }
    
    /// <summary>
    /// 是否已编辑
    /// </summary>
    public bool IsEdited { get; set; } = false;
    
    /// <summary>
    /// 编辑时间
    /// </summary>
    public DateTime? EditedAt { get; set; }
    
    /// <summary>
    /// 编辑历史（JSON格式）
    /// 存储消息的编辑历史记录
    /// </summary>
    public string? EditHistory { get; set; }
    
    /// <summary>
    /// 是否已删除
    /// </summary>
    public bool IsDeleted { get; set; } = false;
    
    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// 删除者ID
    /// </summary>
    [StringLength(50)]
    public string? DeletedBy { get; set; }
    
    /// <summary>
    /// 删除原因
    /// </summary>
    [StringLength(500)]
    public string? DeleteReason { get; set; }
    
    /// <summary>
    /// 消息优先级（0-普通，1-重要，2-紧急）
    /// </summary>
    public int Priority { get; set; } = 0;
    
    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsPinned { get; set; } = false;
    
    /// <summary>
    /// 置顶时间
    /// </summary>
    public DateTime? PinnedAt { get; set; }
    
    /// <summary>
    /// 置顶者ID
    /// </summary>
    [StringLength(50)]
    public string? PinnedBy { get; set; }
    
    /// <summary>
    /// 消息反应（JSON格式）
    /// 存储用户对消息的表情反应
    /// </summary>
    public string? Reactions { get; set; }
    
    /// <summary>
    /// 发送者IP地址
    /// </summary>
    [StringLength(50)]
    public string? SenderIpAddress { get; set; }
    
    /// <summary>
    /// 发送者用户代理
    /// </summary>
    [StringLength(500)]
    public string? SenderUserAgent { get; set; }
    
    /// <summary>
    /// 消息发送时间
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // 导航属性
    
    /// <summary>
    /// 所属聊天室
    /// </summary>
    [ForeignKey("ChatRoomId")]
    public virtual ChatRoom ChatRoom { get; set; } = null!;
    
    /// <summary>
    /// 所属会话（可选）
    /// </summary>
    [ForeignKey("SessionId")]
    public virtual ChatSession? Session { get; set; }
    
    /// <summary>
    /// 发送者信息
    /// </summary>
    public virtual ChatUser? Sender { get; set; }
    
    /// <summary>
    /// 回复的消息（可选）
    /// </summary>
    [ForeignKey("ReplyToMessageId")]
    public virtual ChatMessage? ReplyToMessage { get; set; }
    
    /// <summary>
    /// 回复此消息的消息集合
    /// </summary>
    public virtual ICollection<ChatMessage> Replies { get; set; } = new List<ChatMessage>();
} 