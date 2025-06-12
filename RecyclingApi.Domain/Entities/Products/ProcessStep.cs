using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Domain.Entities.Products;

/// <summary>
/// 处理步骤实体类
/// 记录产品回收处理过程中的各个步骤
/// </summary>
public class ProcessStep
{
    /// <summary>
    /// 步骤唯一标识符
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 步骤名称
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// 步骤描述
    /// </summary>
    [StringLength(500)]
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

    /// <summary>
    /// 关联产品ID（外键）
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 关联产品导航属性
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}