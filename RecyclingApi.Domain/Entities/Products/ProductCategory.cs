using System;
using System.Collections.Generic;

namespace RecyclingApi.Domain.Entities.Products;

/// <summary>
/// 产品分类实体
/// </summary>
public class ProductCategory
{
    /// <summary>
    /// 分类ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 分类编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 分类描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 图标名称
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 排序号（值越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 产品列表（导航属性）
    /// </summary>
    public ICollection<Product> Products { get; set; }
}