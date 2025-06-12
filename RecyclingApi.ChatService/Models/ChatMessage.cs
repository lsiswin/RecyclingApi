namespace RecyclingApi.ChatService.Models;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string VisitorId { get; set; } = string.Empty;
    public string StaffId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsFromStaff { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public string? AttachmentUrl { get; set; }
    public MessageType Type { get; set; } = MessageType.Text;
}

public enum MessageType
{
    Text,
    Image,
    File,
    System
}