namespace RecyclingApi.Domain.Enums;

/// <summary>
/// 聊天室类型枚举
/// 定义聊天系统中支持的聊天室类型
/// </summary>
public enum ChatRoomType
{
    /// <summary>
    /// 客服聊天室
    /// 用户与客服人员的一对一聊天
    /// </summary>
    CustomerService = 0,
    
    /// <summary>
    /// 群聊聊天室
    /// 多个用户参与的群组聊天
    /// </summary>
    Group = 1,
    
    /// <summary>
    /// 私聊聊天室
    /// 两个用户之间的私人聊天
    /// </summary>
    Private = 2,
    
    /// <summary>
    /// 公共聊天室
    /// 开放给所有用户的公共聊天空间
    /// </summary>
    Public = 3,
    
    /// <summary>
    /// 系统通知室
    /// 用于发送系统通知和公告
    /// </summary>
    SystemNotification = 4
} 