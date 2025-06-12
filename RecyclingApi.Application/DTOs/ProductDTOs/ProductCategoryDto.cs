using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Application.DTOs.ProductDTOs;

/// <summary>
/// 产品分类DTO
/// </summary>
public class ProductCategoryDto
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
    /// 产品数量
    /// </summary>
    public int ProductCount { get; set; }
}

/// <summary>
/// 创建或更新产品分类DTO
/// </summary>
public class CreateUpdateProductCategoryDto
{
    /// <summary>
    /// 分类名称
    /// </summary>
    [Required(ErrorMessage = "分类名称不能为空")]
    [StringLength(50, ErrorMessage = "分类名称长度不能超过50个字符")]
    public string Name { get; set; }

    /// <summary>
    /// 分类编码
    /// </summary>
    [Required(ErrorMessage = "分类编码不能为空")]
    [StringLength(50, ErrorMessage = "分类编码长度不能超过50个字符")]
    public string Code { get; set; }

    /// <summary>
    /// 分类描述
    /// </summary>
    [StringLength(200, ErrorMessage = "分类描述长度不能超过200个字符")]
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
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// 产品分类查询请求DTO
/// </summary>
public class ProductCategoryRequestDto : PagedRequestDto
{
    /// <summary>
    /// 关键字（分类名称、编码、描述）
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsActive { get; set; }
} 