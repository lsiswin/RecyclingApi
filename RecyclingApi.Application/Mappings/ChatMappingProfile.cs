using AutoMapper;
using RecyclingApi.Domain.Entities.Chat;
using RecyclingApi.Application.DTOs.ChatDTOs;
using System.Text.Json;

namespace RecyclingApi.Application.Mappings;

/// <summary>
/// 聊天相关实体的AutoMapper映射配置
/// 定义实体类与DTO之间的映射关系
/// </summary>
public class ChatMappingProfile : Profile
{
    /// <summary>
    /// 初始化聊天映射配置
    /// </summary>
    public ChatMappingProfile()
    {
        // ChatUser映射配置
        CreateMap<ChatUser, ChatUserDto>()
            .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.ConnectionId));
            
        CreateMap<CreateChatUserDto, ChatUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.UserStatus.Online))
            .ForMember(dest => dest.JoinTime, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            
        CreateMap<UpdateChatUserDto, ChatUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // ChatMessage映射配置
        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MessageId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SenderId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.SenderName))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.SenderAvatar))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => 
                DeserializeAttachments(src.Attachments)))
            .ForMember(dest => dest.Reactions, opt => opt.MapFrom(src => 
                DeserializeReactions(src.Reactions)));

        CreateMap<ChatMessageDto, ChatMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.SenderAvatar, opt => opt.MapFrom(src => src.Avatar))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => 
                SerializeAttachments(src.Attachments)))
            .ForMember(dest => dest.Reactions, opt => opt.MapFrom(src => 
                SerializeReactions(src.Reactions)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // ChatSession映射配置
        CreateMap<ChatSession, ChatSessionDto>()
            .ForMember(dest => dest.ChatRoomName, opt => opt.MapFrom(src => src.ChatRoom != null ? src.ChatRoom.Name : ""))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new ChatUserDto
            {
                UserId = src.UserId,
                UserName = src.UserName,
                Avatar = src.UserAvatar
            }))
            .ForMember(dest => dest.CustomerService, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.CustomerServiceId) ? null : new ChatUserDto
                {
                    UserId = src.CustomerServiceId,
                    UserName = src.CustomerServiceName ?? "",
                    IsCustomerService = true
                }))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => 
                DeserializeTags(src.Tags)));

        CreateMap<CreateChatSessionDto, ChatSession>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.LastActiveTime, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 0)); // 活跃状态

        CreateMap<UpdateChatSessionDto, ChatSession>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SessionId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => 
                SerializeTags(src.Tags)))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // ChatRoom映射配置
        CreateMap<ChatRoom, ChatRoomDto>()
            .ForMember(dest => dest.AdminIds, opt => opt.MapFrom(src => 
                DeserializeStringList(src.AdminIds)))
            .ForMember(dest => dest.AllowedFileTypes, opt => opt.MapFrom(src => 
                DeserializeStringList(src.AllowedFileTypes)))
            .ForMember(dest => dest.BannedWords, opt => opt.MapFrom(src => 
                DeserializeStringList(src.BannedWords)));

        CreateMap<CreateChatRoomDto, ChatRoom>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AdminIds, opt => opt.MapFrom(src => 
                SerializeStringList(src.AdminIds)))
            .ForMember(dest => dest.AllowedFileTypes, opt => opt.MapFrom(src => 
                SerializeStringList(src.AllowedFileTypes)))
            .ForMember(dest => dest.BannedWords, opt => opt.MapFrom(src => 
                SerializeStringList(src.BannedWords)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }

    /// <summary>
    /// 反序列化附件信息
    /// </summary>
    private static List<ChatAttachmentDto>? DeserializeAttachments(string? json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try
        {
            return JsonSerializer.Deserialize<List<ChatAttachmentDto>>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 序列化附件信息
    /// </summary>
    private static string? SerializeAttachments(List<ChatAttachmentDto>? attachments)
    {
        if (attachments == null || !attachments.Any()) return null;
        try
        {
            return JsonSerializer.Serialize(attachments);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 反序列化消息反应
    /// </summary>
    private static List<ChatReactionDto>? DeserializeReactions(string? json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try
        {
            return JsonSerializer.Deserialize<List<ChatReactionDto>>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 序列化消息反应
    /// </summary>
    private static string? SerializeReactions(List<ChatReactionDto>? reactions)
    {
        if (reactions == null || !reactions.Any()) return null;
        try
        {
            return JsonSerializer.Serialize(reactions);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 反序列化标签列表
    /// </summary>
    private static List<string>? DeserializeTags(string? json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try
        {
            return JsonSerializer.Deserialize<List<string>>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 序列化标签列表
    /// </summary>
    private static string? SerializeTags(List<string>? tags)
    {
        if (tags == null || !tags.Any()) return null;
        try
        {
            return JsonSerializer.Serialize(tags);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 反序列化字符串列表
    /// </summary>
    private static List<string> DeserializeStringList(string? json)
    {
        if (string.IsNullOrEmpty(json)) return new List<string>();
        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    /// <summary>
    /// 序列化字符串列表
    /// </summary>
    private static string? SerializeStringList(List<string>? list)
    {
        if (list == null || !list.Any()) return null;
        try
        {
            return JsonSerializer.Serialize(list);
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// 聊天室数据传输对象
/// </summary>
public class ChatRoomDto
{
    /// <summary>
    /// 聊天室ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 聊天室名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 聊天室描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 聊天室类型
    /// </summary>
    public Domain.Enums.ChatRoomType Type { get; set; }
    
    /// <summary>
    /// 聊天室头像
    /// </summary>
    public string? Avatar { get; set; }
    
    /// <summary>
    /// 是否为公开聊天室
    /// </summary>
    public bool IsPublic { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// 最大成员数量
    /// </summary>
    public int MaxMembers { get; set; }
    
    /// <summary>
    /// 当前成员数量
    /// </summary>
    public int CurrentMemberCount { get; set; }
    
    /// <summary>
    /// 管理员ID列表
    /// </summary>
    public List<string> AdminIds { get; set; } = new();
    
    /// <summary>
    /// 欢迎消息
    /// </summary>
    public string? WelcomeMessage { get; set; }
    
    /// <summary>
    /// 聊天室公告
    /// </summary>
    public string? Announcement { get; set; }
    
    /// <summary>
    /// 是否允许文件上传
    /// </summary>
    public bool AllowFileUpload { get; set; }
    
    /// <summary>
    /// 允许的文件类型
    /// </summary>
    public List<string> AllowedFileTypes { get; set; } = new();
    
    /// <summary>
    /// 最大文件大小
    /// </summary>
    public long MaxFileSize { get; set; }
    
    /// <summary>
    /// 敏感词过滤列表
    /// </summary>
    public List<string> BannedWords { get; set; } = new();
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建聊天室请求DTO
/// </summary>
public class CreateChatRoomDto
{
    /// <summary>
    /// 聊天室名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 聊天室描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 聊天室类型
    /// </summary>
    public Domain.Enums.ChatRoomType Type { get; set; }
    
    /// <summary>
    /// 是否为公开聊天室
    /// </summary>
    public bool IsPublic { get; set; } = true;
    
    /// <summary>
    /// 最大成员数量
    /// </summary>
    public int MaxMembers { get; set; } = 0;
    
    /// <summary>
    /// 管理员ID列表
    /// </summary>
    public List<string>? AdminIds { get; set; }
    
    /// <summary>
    /// 欢迎消息
    /// </summary>
    public string? WelcomeMessage { get; set; }
    
    /// <summary>
    /// 是否允许文件上传
    /// </summary>
    public bool AllowFileUpload { get; set; } = true;
    
    /// <summary>
    /// 允许的文件类型
    /// </summary>
    public List<string>? AllowedFileTypes { get; set; }
    
    /// <summary>
    /// 最大文件大小
    /// </summary>
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024;
    
    /// <summary>
    /// 敏感词过滤列表
    /// </summary>
    public List<string>? BannedWords { get; set; }
    
    /// <summary>
    /// 创建者ID
    /// </summary>
    public string? CreatedBy { get; set; }
} 