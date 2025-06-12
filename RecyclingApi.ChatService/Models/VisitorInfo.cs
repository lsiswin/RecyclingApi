namespace RecyclingApi.ChatService.Models;

public class VisitorInfo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "访客";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public string CurrentPage { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActiveTime { get; set; } = DateTime.UtcNow;
    public bool IsOnline { get; set; } = true;
    public int UnreadCount { get; set; }
    public string? AssignedStaffId { get; set; }
    public VisitorStatus Status { get; set; } = VisitorStatus.Waiting;
}

public enum VisitorStatus
{
    Waiting,
    InChat,
    Ended
}