namespace RecyclingApi.Application.Common.Exceptions;

/// <summary>
/// 资源未找到异常
/// 当请求的资源不存在时抛出此异常
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// 初始化NotFoundException实例
    /// </summary>
    /// <param name="message">异常消息</param>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// 初始化NotFoundException实例
    /// </summary>
    /// <param name="message">异常消息</param>
    /// <param name="innerException">内部异常</param>
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
} 