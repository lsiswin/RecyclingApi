using RecyclingApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyclingApi.Domain.Entities.Chat;

/// <summary>
/// 聊天会话实体
/// 管理用户的聊天会话信息和状态
/// </summary>
public class ChatSession
{
    /// <summary>
    /// 会话唯一标识符
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// 会话UUID（用于前端标识）
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// 聊天室ID
    /// </summary>
    public int ChatRoomId { get; set; }
    
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required]
    [StringLength(50)]
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户头像
    /// </summary>
    [StringLength(500)]
    public string? UserAvatar { get; set; }
    
    /// <summary>
    /// 客服人员ID（如果是客服会话）
    /// </summary>
    [StringLength(50)]
    public string? CustomerServiceId { get; set; }
    
    /// <summary>
    /// 客服人员名称
    /// </summary>
    [StringLength(100)]
    public string? CustomerServiceName { get; set; }
    
    /// <summary>
    /// 会话标题
    /// </summary>
    [StringLength(200)]
    public string? Title { get; set; }
    
    /// <summary>
    /// 会话描述
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// 会话状态（0-活跃，1-等待，2-已结束，3-已关闭）
    /// </summary>
    public int Status { get; set; } = 0;
    
    /// <summary>
    /// 会话优先级（0-普通，1-重要，2-紧急）
    /// </summary>
    public int Priority { get; set; } = 0;
    
    /// <summary>
    /// 会话标签（JSON数组格式）
    /// 用于分类和筛选会话
    /// </summary>
    public string? Tags { get; set; }
    
    /// <summary>
    /// 会话分类
    /// </summary>
    [StringLength(100)]
    public string? Category { get; set; }
    
    /// <summary>
    /// 用户联系信息（JSON格式）
    /// 包含电话、邮箱等联系方式
    /// </summary>
    public string? ContactInfo { get; set; }
    
    /// <summary>
    /// 用户问题描述
    /// </summary>
    [StringLength(2000)]
    public string? UserProblem { get; set; }
    
    /// <summary>
    /// 解决方案
    /// </summary>
    [StringLength(2000)]
    public string? Solution { get; set; }
    
    /// <summary>
    /// 会话备注
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// 是否为匿名会话
    /// </summary>
    public bool IsAnonymous { get; set; } = false;
    
    /// <summary>
    /// 是否为机器人会话
    /// </summary>
    public bool IsBotSession { get; set; } = false;
    
    /// <summary>
    /// 机器人ID（如果是机器人会话）
    /// </summary>
    [StringLength(50)]
    public string? BotId { get; set; }
    
    /// <summary>
    /// 会话开始时间
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 会话结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 最后活跃时间
    /// </summary>
    public DateTime LastActiveTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 最后消息时间
    /// </summary>
    public DateTime? LastMessageTime { get; set; }
    
    /// <summary>
    /// 最后消息内容
    /// </summary>
    [StringLength(500)]
    public string? LastMessageContent { get; set; }
    
    /// <summary>
    /// 最后消息发送者
    /// </summary>
    [StringLength(100)]
    public string? LastMessageSender { get; set; }
    
    /// <summary>
    /// 消息总数
    /// </summary>
    public int MessageCount { get; set; } = 0;
    
    /// <summary>
    /// 未读消息数
    /// </summary>
    public int UnreadCount { get; set; } = 0;
    
    /// <summary>
    /// 用户满意度评分（1-5分）
    /// </summary>
    public int? SatisfactionRating { get; set; }
    
    /// <summary>
    /// 用户反馈
    /// </summary>
    [StringLength(1000)]
    public string? UserFeedback { get; set; }
    
    /// <summary>
    /// 反馈时间
    /// </summary>
    public DateTime? FeedbackTime { get; set; }
    
    /// <summary>
    /// 会话持续时间（秒）
    /// </summary>
    public int? Duration { get; set; }
    
    /// <summary>
    /// 平均响应时间（秒）
    /// </summary>
    public int? AverageResponseTime { get; set; }
    
    /// <summary>
    /// 首次响应时间（秒）
    /// </summary>
    public int? FirstResponseTime { get; set; }
    
    /// <summary>
    /// 会话来源（网站、移动端、API等）
    /// </summary>
    [StringLength(100)]
    public string? Source { get; set; }
    
    /// <summary>
    /// 来源页面URL
    /// </summary>
    [StringLength(500)]
    public string? SourceUrl { get; set; }
    
    /// <summary>
    /// 用户IP地址
    /// </summary>
    [StringLength(50)]
    public string? UserIpAddress { get; set; }
    
    /// <summary>
    /// 用户代理信息
    /// </summary>
    [StringLength(500)]
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// 会话元数据（JSON格式）
    /// 存储额外的会话信息
    /// </summary>
    public string? Metadata { get; set; }
    
    /// <summary>
    /// 是否已归档
    /// </summary>
    public bool IsArchived { get; set; } = false;
    
    /// <summary>
    /// 归档时间
    /// </summary>
    public DateTime? ArchivedAt { get; set; }
    
    /// <summary>
    /// 归档者ID
    /// </summary>
    [StringLength(50)]
    public string? ArchivedBy { get; set; }
    
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
    /// 参与会话的用户
    /// </summary>
    public virtual ChatUser? User { get; set; }
    
    /// <summary>
    /// 客服人员信息
    /// </summary>
    public virtual ChatUser? CustomerService { get; set; }
    
    /// <summary>
    /// 会话中的消息集合
    /// </summary>
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
} 