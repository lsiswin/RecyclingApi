namespace RecyclingApi.Application.DTOs.ProductDTOs;

/// <summary>
/// 处理步骤数据传输对象
/// 用于返回产品回收处理步骤的信息
/// </summary>
public class ProcessStepDto
{
    /// <summary>
    /// 步骤唯一标识符
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 步骤名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 步骤描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 步骤执行顺序
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; set; }
    
    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// 完成人员
    /// </summary>
    public string? CompletedBy { get; set; }
} 