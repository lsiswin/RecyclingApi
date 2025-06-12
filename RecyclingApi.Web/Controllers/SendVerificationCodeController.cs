using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs.Auth;
using RecyclingApi.Application.Services.Auth;
using RecyclingApi.Application.Services.Email;

namespace RecyclingApi.Web.Controllers;

/// <summary>
/// 验证码控制器
/// </summary>
[ApiController]
[Route("api/auth")]
public class SendVerificationCodeController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendVerificationCodeController> _logger;
    private readonly Random _random = new Random();
    
    // 验证码缓存
    private static readonly Dictionary<string, (string Code, DateTime ExpireTime)> _verificationCodes = new();
    
    public SendVerificationCodeController(
        IEmailService emailService,
        ILogger<SendVerificationCodeController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }
    
    /// <summary>
    /// 发送验证码到邮箱
    /// </summary>
    /// <param name="request">发送验证码请求</param>
    /// <returns>发送结果</returns>
    [HttpPost("send-verification-code")]
    public async Task<ApiResponse<bool>> SendVerificationCode([FromBody] SendVerificationCodeRequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return new ApiResponse<bool>("邮箱地址不能为空");
            }
            
            // 生成6位数字验证码
            string code = GenerateVerificationCode();
            
            // 存储验证码，有效期10分钟
            StoreVerificationCode(request.Email, code);
            
            // 发送验证码邮件
            string subject = "验证码 - IT设备回收系统";
            string content = $"您的验证码是：<b>{code}</b>，有效期10分钟。如非本人操作请忽略此邮件。";
            
            await _emailService.SendEmailAsync(request.Email, subject, content);
            
            return new ApiResponse<bool>(true, "验证码已发送至您的邮箱");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送验证码失败");
            return new ApiResponse<bool>("发送验证码失败，请稍后重试");
        }
    }
    
    /// <summary>
    /// 发送验证码到手机
    /// </summary>
    /// <param name="request">发送短信验证码请求</param>
    /// <returns>发送结果</returns>
    [HttpPost("send-verification-code-sms")]
    public ApiResponse<bool> SendVerificationCodeSms([FromBody] SendVerificationCodeSmsRequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse<bool>("手机号不能为空");
            }
            
            // 生成6位数字验证码
            string code = GenerateVerificationCode();
            
            // 存储验证码，有效期10分钟
            StoreVerificationCode(request.PhoneNumber, code);
            
            // TODO: 接入短信发送服务
            // 此处仅模拟发送成功
            
            return new ApiResponse<bool>(true, "验证码已发送至您的手机");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送短信验证码失败");
            return new ApiResponse<bool>("发送验证码失败，请稍后重试");
        }
    }
    
    /// <summary>
    /// 验证验证码
    /// </summary>
    /// <param name="request">验证码验证请求</param>
    /// <returns>验证结果</returns>
    [HttpPost("verify-code")]
    public ApiResponse<bool> VerifyCode([FromBody] VerifyCodeRequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.PhoneNumber))
            {
                return new ApiResponse<bool>("邮箱或手机号不能为空");
            }
            
            if (string.IsNullOrEmpty(request.Code))
            {
                return new ApiResponse<bool>("验证码不能为空");
            }
            
            // 使用邮箱或手机号作为键
            string key = string.IsNullOrEmpty(request.Email) ? request.PhoneNumber : request.Email;
            
            // 验证码验证
            if (!_verificationCodes.TryGetValue(key, out var codeInfo))
            {
                return new ApiResponse<bool>("验证码已过期或不存在");
            }
            
            if (DateTime.Now > codeInfo.ExpireTime)
            {
                _verificationCodes.Remove(key);
                return new ApiResponse<bool>("验证码已过期");
            }
            
            if (codeInfo.Code != request.Code)
            {
                return new ApiResponse<bool>("验证码错误");
            }
            
            // 验证成功后移除验证码
            _verificationCodes.Remove(key);
            
            return new ApiResponse<bool>(true, "验证成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "验证码验证失败");
            return new ApiResponse<bool>("验证失败，请稍后重试");
        }
    }
    
    /// <summary>
    /// 生成6位数字验证码
    /// </summary>
    private string GenerateVerificationCode()
    {
        return _random.Next(100000, 999999).ToString();
    }
    
    /// <summary>
    /// 存储验证码
    /// </summary>
    private void StoreVerificationCode(string key, string code)
    {
        // 验证码有效期10分钟
        _verificationCodes[key] = (code, DateTime.Now.AddMinutes(10));
    }
} 