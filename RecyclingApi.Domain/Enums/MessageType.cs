namespace RecyclingApi.Domain.Enums;

/// <summary>
/// 消息类型枚举
/// 定义聊天系统中支持的消息类型
/// </summary>
public enum MessageType
{
    /// <summary>
    /// 文本消息
    /// 普通的文字聊天消息
    /// </summary>
    Text = 0,
    
    /// <summary>
    /// 图片消息
    /// 包含图片内容的消息
    /// </summary>
    Image = 1,
    
    /// <summary>
    /// 文件消息
    /// 包含文件附件的消息
    /// </summary>
    File = 2,
    
    /// <summary>
    /// 系统消息
    /// 系统自动生成的通知消息
    /// </summary>
    System = 3,
    
    /// <summary>
    /// 语音消息
    /// 包含语音内容的消息
    /// </summary>
    Voice = 4,
    
    /// <summary>
    /// 视频消息
    /// 包含视频内容的消息
    /// </summary>
    Video = 5,
    
    /// <summary>
    /// 链接消息
    /// 包含网页链接的消息
    /// </summary>
    Link = 6,
    
    /// <summary>
    /// 位置消息
    /// 包含地理位置信息的消息
    /// </summary>
    Location = 7
} 