namespace RecyclingApi.Application.DTOs.CaseDTOs;

/// <summary>
/// 案例详细DTO
/// </summary>
public class CaseDetailDto : CaseListDto
{
    public string FullDescription { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string ProjectDetails { get; set; } = string.Empty;
    public List<string> Highlights { get; set; } = new();
} 