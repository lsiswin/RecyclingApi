namespace RecyclingApi.Domain.Enums;

/// <summary>
/// 用户状态枚举
/// 定义聊天系统中用户的在线状态
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 离线状态
    /// 用户未连接到聊天系统
    /// </summary>
    Offline = 0,
    
    /// <summary>
    /// 在线状态
    /// 用户已连接并活跃在聊天系统中
    /// </summary>
    Online = 1,
    
    /// <summary>
    /// 忙碌状态
    /// 用户在线但标记为忙碌
    /// </summary>
    Busy = 2,
    
    /// <summary>
    /// 离开状态
    /// 用户暂时离开但保持连接
    /// </summary>
    Away = 3,
    
    /// <summary>
    /// 隐身状态
    /// 用户在线但对其他用户显示为离线
    /// </summary>
    Invisible = 4,
    
    /// <summary>
    /// 勿扰状态
    /// 用户在线但不希望被打扰
    /// </summary>
    DoNotDisturb = 5
} 