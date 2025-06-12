namespace RecyclingApi.ChatService.Models;

public class StaffInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActiveTime { get; set; } = DateTime.UtcNow;
    public bool IsOnline { get; set; } = true;
    public int ActiveConversations { get; set; }
    public int MaxConversations { get; set; } = 5;
    public StaffStatus Status { get; set; } = StaffStatus.Available;
}

public enum StaffStatus
{
    Available,
    Busy,
    Away,
    Offline
}