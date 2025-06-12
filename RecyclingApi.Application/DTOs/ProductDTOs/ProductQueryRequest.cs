namespace RecyclingApi.Application.DTOs.ProductDTOs;

/// <summary>
/// 产品查询请求数据传输对象
/// 用于接收产品查询和分页的请求参数
/// </summary>
public class ProductQueryRequest:PagedRequestDto
{    
    /// <summary>
    /// 产品分类筛选
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// 品牌筛选
    /// </summary>
    public string? Brand { get; set; }

    public int CategoryId { get; set; }
    /// <summary>
    /// 设备状况筛选
    /// </summary>
    public string? Condition { get; set; }
    
    /// <summary>
    /// 价格范围筛选（格式：min-max）
    /// </summary>
    public string? PriceRange { get; set; }
    
    /// <summary>
    /// 关键词搜索
    /// </summary>
    public string? Keyword { get; set; }
    
} 