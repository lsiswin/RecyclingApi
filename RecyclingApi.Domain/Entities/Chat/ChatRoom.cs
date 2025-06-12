using RecyclingApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Domain.Entities.Chat;

/// <summary>
/// 聊天室实体
/// 定义聊天室的基本信息和配置
/// </summary>
public class ChatRoom
{
    /// <summary>
    /// 聊天室唯一标识符
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// 聊天室名称
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 聊天室描述
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// 聊天室类型
    /// </summary>
    public ChatRoomType Type { get; set; } = ChatRoomType.CustomerService;
    
    /// <summary>
    /// 聊天室头像/图标
    /// </summary>
    [StringLength(500)]
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 是否为公开聊天室
    /// </summary>
    public bool IsPublic { get; set; } = true;
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// 最大成员数量（0表示无限制）
    /// </summary>
    public int MaxMembers { get; set; } = 0;
    
    /// <summary>
    /// 当前成员数量
    /// </summary>
    public int CurrentMemberCount { get; set; } = 0;
    
    /// <summary>
    /// 聊天室创建者ID
    /// </summary>
    [StringLength(50)]
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// 聊天室管理员ID列表（JSON格式）
    /// </summary>
    public string? AdminIds { get; set; }
    
    /// <summary>
    /// 聊天室设置（JSON格式）
    /// 包含各种配置选项如：是否允许文件上传、消息保留时间等
    /// </summary>
    public string? Settings { get; set; }
    
    /// <summary>
    /// 欢迎消息
    /// 新用户加入时显示的欢迎信息
    /// </summary>
    [StringLength(1000)]
    public string? WelcomeMessage { get; set; }
    
    /// <summary>
    /// 聊天室公告
    /// </summary>
    [StringLength(2000)]
    public string? Announcement { get; set; }
    
    /// <summary>
    /// 公告发布时间
    /// </summary>
    public DateTime? AnnouncementTime { get; set; }
    
    /// <summary>
    /// 是否需要密码
    /// </summary>
    public bool RequirePassword { get; set; } = false;
    
    /// <summary>
    /// 聊天室密码（加密存储）
    /// </summary>
    [StringLength(200)]
    public string? Password { get; set; }
    
    /// <summary>
    /// 是否允许匿名用户
    /// </summary>
    public bool AllowAnonymous { get; set; } = true;
    
    /// <summary>
    /// 是否允许文件上传
    /// </summary>
    public bool AllowFileUpload { get; set; } = true;
    
    /// <summary>
    /// 允许的文件类型（JSON数组格式）
    /// </summary>
    public string? AllowedFileTypes { get; set; }
    
    /// <summary>
    /// 最大文件大小（字节）
    /// </summary>
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
    
    /// <summary>
    /// 消息保留天数（0表示永久保留）
    /// </summary>
    public int MessageRetentionDays { get; set; } = 30;
    
    /// <summary>
    /// 是否启用消息审核
    /// </summary>
    public bool EnableModeration { get; set; } = false;
    
    /// <summary>
    /// 敏感词过滤列表（JSON数组格式）
    /// </summary>
    public string? BannedWords { get; set; }
    
    /// <summary>
    /// 最后活跃时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }
    
    /// <summary>
    /// 总消息数量
    /// </summary>
    public long TotalMessageCount { get; set; } = 0;
    
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
    /// 聊天室中的消息集合
    /// </summary>
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    
    /// <summary>
    /// 聊天室的会话集合
    /// </summary>
    public virtual ICollection<ChatSession> Sessions { get; set; } = new List<ChatSession>();
} 