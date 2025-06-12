using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace RecyclingApi.Application.Services.Email;

/// <summary>
/// 邮件服务实现
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly string _senderName;
    private readonly bool _enableSsl;
    
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        // 从配置中获取邮件设置
        _smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.example.com";
        _smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        _smtpUsername = _configuration["Email:Username"] ?? "";
        _smtpPassword = _configuration["Email:Password"] ?? "";
        _senderEmail = _configuration["Email:SenderEmail"] ?? "no-reply@example.com";
        _senderName = _configuration["Email:SenderName"] ?? "IT设备回收系统";
        _enableSsl = bool.Parse(_configuration["Email:EnableSsl"] ?? "true");
    }
    
    /// <summary>
    /// 发送邮件
    /// </summary>
    public async Task<bool> SendEmailAsync(string to, string subject, string content, bool isHtml = true)
    {
        try
        {
            // 创建邮件消息
            var message = CreateMailMessage(to, subject, content, isHtml);
            
            // 发送邮件
            using var client = CreateSmtpClient();
            await client.SendMailAsync(message);
            
            _logger.LogInformation("邮件已发送至 {To}, 主题: {Subject}", to, subject);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送邮件失败: {To}, {Subject}", to, subject);
            return false;
        }
    }
    
    /// <summary>
    /// 发送带附件的邮件
    /// </summary>
    public async Task<bool> SendEmailWithAttachmentsAsync(
        string to, 
        string subject, 
        string content, 
        List<string> attachmentPaths, 
        bool isHtml = true)
    {
        try
        {
            // 创建邮件消息
            var message = CreateMailMessage(to, subject, content, isHtml);
            
            // 添加附件
            foreach (var path in attachmentPaths)
            {
                if (File.Exists(path))
                {
                    message.Attachments.Add(new Attachment(path));
                }
                else
                {
                    _logger.LogWarning("附件不存在: {Path}", path);
                }
            }
            
            // 发送邮件
            using var client = CreateSmtpClient();
            await client.SendMailAsync(message);
            
            _logger.LogInformation("带附件的邮件已发送至 {To}, 主题: {Subject}, 附件数: {Count}", 
                to, subject, attachmentPaths.Count);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送带附件的邮件失败: {To}, {Subject}", to, subject);
            return false;
        }
    }
    
    /// <summary>
    /// 创建邮件消息
    /// </summary>
    private MailMessage CreateMailMessage(string to, string subject, string content, bool isHtml)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_senderEmail, _senderName),
            Subject = subject,
            Body = content,
            IsBodyHtml = isHtml
        };
        
        message.To.Add(to);
        return message;
    }
    
    /// <summary>
    /// 创建SMTP客户端
    /// </summary>
    private SmtpClient CreateSmtpClient()
    {
        var client = new SmtpClient(_smtpServer, _smtpPort)
        {
            EnableSsl = _enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
        };
        
        return client;
    }
} 