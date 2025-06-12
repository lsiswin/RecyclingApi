namespace RecyclingApi.Application.DTOs.CaseDTOs;

/// <summary>
/// 案例查询请求
/// </summary>
public class CaseQueryRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string? Category { get; set; }
    public string? DeviceType { get; set; }
    public string? Scale { get; set; }
} 