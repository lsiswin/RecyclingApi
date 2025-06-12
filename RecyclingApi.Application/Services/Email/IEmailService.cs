namespace RecyclingApi.Application.Services.Email;

/// <summary>
/// 邮件服务接口
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="to">收件人</param>
    /// <param name="subject">主题</param>
    /// <param name="content">内容</param>
    /// <param name="isHtml">是否HTML格式</param>
    /// <returns>发送结果</returns>
    Task<bool> SendEmailAsync(string to, string subject, string content, bool isHtml = true);
    
    /// <summary>
    /// 发送带附件的邮件
    /// </summary>
    /// <param name="to">收件人</param>
    /// <param name="subject">主题</param>
    /// <param name="content">内容</param>
    /// <param name="attachmentPaths">附件路径列表</param>
    /// <param name="isHtml">是否HTML格式</param>
    /// <returns>发送结果</returns>
    Task<bool> SendEmailWithAttachmentsAsync(
        string to, 
        string subject, 
        string content, 
        List<string> attachmentPaths, 
        bool isHtml = true);
} 