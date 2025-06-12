namespace RecyclingApi.Application.DTOs.CaseDTOs;

/// <summary>
/// 案例列表DTO
/// </summary>
public class CaseListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public int DeviceCount { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Scale { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int Views { get; set; }
} 