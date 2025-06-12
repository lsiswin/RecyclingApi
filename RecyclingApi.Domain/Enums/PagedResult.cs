namespace RecyclingApi.Domain.Enums;

/// <summary>
/// 分页结果数据传输对象
/// 用于返回分页查询的结果数据
/// </summary>
/// <typeparam name="T">数据项的类型</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// 当前页的数据项列表
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// 总记录数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页显示数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }
}