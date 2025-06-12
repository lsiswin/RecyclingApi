using RecyclingApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Domain.Entities.Chat;

/// <summary>
/// 聊天用户实体
/// 存储参与聊天的用户信息和状态
/// </summary>
public class ChatUser
{
    /// <summary>
    /// 用户唯一标识符
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// 用户ID（可以关联到系统用户表）
    /// </summary>
    [Required]
    [StringLength(50)]
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户显示名称
    /// </summary>
    [Required]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户真实姓名
    /// </summary>
    [StringLength(100)]
    public string? RealName { get; set; }
    
    /// <summary>
    /// 用户头像URL
    /// </summary>
    [StringLength(500)]
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 用户邮箱
    /// </summary>
    [StringLength(200)]
    public string? Email { get; set; }
    
    /// <summary>
    /// 用户电话
    /// </summary>
    [StringLength(20)]
    public string? Phone { get; set; }
    
    /// <summary>
    /// 用户当前状态
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
    /// 加入聊天时间
    /// </summary>
    public DateTime JoinTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 最后在线时间
    /// </summary>
    public DateTime? LastOnlineTime { get; set; }
    
    /// <summary>
    /// 最后活跃时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }
    
    /// <summary>
    /// SignalR连接ID
    /// </summary>
    [StringLength(100)]
    public string? ConnectionId { get; set; }
    
    /// <summary>
    /// 用户IP地址
    /// </summary>
    [StringLength(50)]
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// 用户代理信息
    /// </summary>
    [StringLength(500)]
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// 首次访问时间
    /// </summary>
    public DateTime FirstVisitTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 访问次数
    /// </summary>
    public int VisitCount { get; set; } = 1;
    
    /// <summary>
    /// 页面访问统计（JSON格式）
    /// </summary>
    public string? PageVisitStats { get; set; }
    
    /// <summary>
    /// 用户偏好设置（JSON格式）
    /// </summary>
    public string? Preferences { get; set; }
    
    /// <summary>
    /// 是否被禁言
    /// </summary>
    public bool IsMuted { get; set; } = false;
    
    /// <summary>
    /// 禁言到期时间
    /// </summary>
    public DateTime? MutedUntil { get; set; }
    
    /// <summary>
    /// 禁言原因
    /// </summary>
    [StringLength(500)]
    public string? MuteReason { get; set; }
    
    /// <summary>
    /// 是否被封禁
    /// </summary>
    public bool IsBanned { get; set; } = false;
    
    /// <summary>
    /// 封禁到期时间
    /// </summary>
    public DateTime? BannedUntil { get; set; }
    
    /// <summary>
    /// 封禁原因
    /// </summary>
    [StringLength(500)]
    public string? BanReason { get; set; }
    
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
    /// 用户发送的消息集合
    /// </summary>
    public virtual ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
    
    /// <summary>
    /// 用户参与的聊天会话集合
    /// </summary>
    public virtual ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
} 