namespace RecyclingApi.Application.Common.Responses;

/// <summary>
/// API统一响应格式
/// 用于包装所有API接口的返回数据
/// </summary>
/// <typeparam name="T">响应数据的类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 操作是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 响应消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// 响应数据
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// 响应时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 创建成功响应
    /// </summary>
    /// <param name="data">响应数据</param>
    /// <param name="message">成功消息</param>
    public ApiResponse(T data, string message = "Success")
    {
        Success = true;
        Message = message;
        Data = data;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// 创建失败响应
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    public ApiResponse(string errorMessage)
    {
        Success = false;
        Message = errorMessage;
        Data = default(T);
        Timestamp = DateTime.UtcNow;
    }
} 