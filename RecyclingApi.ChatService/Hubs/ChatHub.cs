

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RecyclingApi.Application.Common.MessageQueue;
using RecyclingApi.Application.Common.Redis;
using RecyclingApi.Application.Services.Chat;

namespace RecyclingApi.ChatService.Hubs;

/// <summary>
/// 聊天Hub，处理实时聊天功能 - 集成 RabbitMQ 和 Redis
/// </summary>
public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IChatCacheService _cacheService;
    private readonly IChatMessageQueueService _messageQueueService;
    private readonly IChatStatisticsService _statisticsService;

    public ChatHub(
        ILogger<ChatHub> logger,
        IChatCacheService cacheService,
        IChatMessageQueueService messageQueueService,
        IChatStatisticsService statisticsService)
    {
        _logger = logger;
        _cacheService = cacheService;
        _messageQueueService = messageQueueService;
        _statisticsService = statisticsService;
    }

    /// <summary>
    /// 用户连接时调用
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("用户连接: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// 用户断开连接时调用
    /// </summary>
    /// <param name="exception"></param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        
        try
        {
            // 检查是否是访客断开
            var visitor = await _cacheService.GetUserByConnectionIdAsync(connectionId);
            if (visitor != null)
            {
                await _cacheService.RemoveOnlineUserAsync(visitor.UserId);
                
                // 如果访客有分配的客服，通知客服访客已离线
                var staffId = await _cacheService.GetVisitorStaffMappingAsync(visitor.UserId);
                if (!string.IsNullOrEmpty(staffId))
                {
                    await _cacheService.RemoveVisitorStaffMappingAsync(visitor.UserId);
                    await _cacheService.DecrementStaffConversationCountAsync(staffId);
                    
                    // 通知客服访客已离线
                    var assignedStaff = await _cacheService.GetOnlineStaffAsync(staffId);
                    if (assignedStaff != null)
                    {
                        await Clients.Client(assignedStaff.ConnectionId).SendAsync("VisitorDisconnected", visitor.UserId, visitor.UserName);
                    }

                    // 发布访客断开事件到消息队列
                    await _messageQueueService.PublishSystemNotificationAsync(new SystemNotificationItem
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        Type = "Info",
                        Title = "访客离线",
                        Content = $"访客 {visitor.UserName} 已离线",
                        TargetUserId = staffId,
                        Timestamp = DateTime.UtcNow
                    });
                }
                
                await Clients.All.SendAsync("UserDisconnected", visitor);
                var onlineUsers = await _cacheService.GetAllOnlineUsersAsync();
                await Clients.All.SendAsync("UpdateOnlineUsers", onlineUsers);
            }
            
            // 检查是否是客服断开
            var allStaff = await _cacheService.GetAllOnlineStaffAsync();
            var disconnectedStaff = allStaff.FirstOrDefault(s => s.ConnectionId == connectionId);
            if (disconnectedStaff != null)
            {
                await _cacheService.RemoveOnlineStaffAsync(disconnectedStaff.StaffId);
                
                // 找到该客服负责的所有访客，重新分配
                var assignedVisitors = await _cacheService.GetStaffVisitorsAsync(disconnectedStaff.StaffId);
                foreach (var visitorId in assignedVisitors)
                {
                    await _cacheService.RemoveVisitorStaffMappingAsync(visitorId);
                    
                    // 尝试重新分配给其他客服
                    var newStaff = await AssignVisitorToAvailableStaff(visitorId);
                    if (newStaff != null)
                    {
                        await Clients.Client(newStaff.ConnectionId).SendAsync("VisitorReassigned", visitorId, $"客服 {disconnectedStaff.StaffName} 已离线，对话已转接给您");
                        
                        // 发布访客重新分配事件
                        await _messageQueueService.PublishVisitorAssignmentAsync(new VisitorAssignmentItem
                        {
                            VisitorId = visitorId,
                            StaffId = newStaff.StaffId,
                            AssignmentType = "Transfer",
                            Timestamp = DateTime.UtcNow,
                            Reason = $"客服 {disconnectedStaff.StaffName} 离线"
                        });
                    }
                }
                
                // 发布客服状态变更事件
                await _messageQueueService.PublishStaffStatusChangeAsync(new StaffStatusChangeItem
                {
                    StaffId = disconnectedStaff.StaffId,
                    StaffName = disconnectedStaff.StaffName,
                    OldStatus = disconnectedStaff.Status.ToString(),
                    NewStatus = "Offline",
                    Timestamp = DateTime.UtcNow,
                    Reason = "Disconnected"
                });
                
                // 通知其他客服该客服已离线
                await Clients.Group("Staff").SendAsync("StaffDisconnected", disconnectedStaff);
                await NotifyStaffListUpdate();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理用户断开连接时发生错误: {ConnectionId}", connectionId);
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    #region 访客相关方法

    /// <summary>
    /// 访客加入聊天室
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="avatar">头像</param>
    public async Task JoinChat(string userId, string userName, string? avatar = null)
    {
        try
        {
            var userInfo = new UserInfo
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar ?? "/default-avatar.png",
                ConnectionId = Context.ConnectionId,
                JoinTime = DateTime.Now,
                UserType = "Visitor"
            };

            await _cacheService.SetOnlineUserAsync(userInfo);

            // 通知其他用户有新用户加入
            await Clients.Others.SendAsync("UserJoined", userInfo);
            
            // 向当前用户发送在线用户列表
            var onlineUsers = await _cacheService.GetAllOnlineUsersAsync();
            await Clients.Caller.SendAsync("UpdateOnlineUsers", onlineUsers);
            
            // 向所有用户更新在线用户列表
            await Clients.All.SendAsync("UpdateOnlineUsers", onlineUsers);
            
            // 记录用户活动
            await _statisticsService.RecordUserActivityAsync(userId, "Join", DateTime.UtcNow);
            
            _logger.LogInformation("访客加入: {UserName} ({UserId})", userName, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "访客加入聊天室失败: {UserId}", userId);
            await Clients.Caller.SendAsync("JoinChatError", $"加入聊天室失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 访客发送消息给客服
    /// </summary>
    /// <param name="visitorId">访客ID</param>
    /// <param name="messageData">消息数据</param>
    public async Task SendMessageToStaff(string visitorId, ChatMessageDto messageData)
    {
        try
        {
            var message = new ChatMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                SessionId = messageData.SessionId ?? "",
                UserId = messageData.SenderId,
                UserName = messageData.SenderName,
                Content = messageData.Content,
                Timestamp = DateTime.UtcNow,
                MessageType = messageData.Type.ToString(),
                IsFromStaff = false
            };

            // 保存消息到缓存
            await _cacheService.SaveChatMessageAsync(message);

            // 获取分配的客服
            var staffId = await _cacheService.GetVisitorStaffMappingAsync(visitorId);
            if (string.IsNullOrEmpty(staffId))
            {
                // 如果没有分配客服，自动分配一个
                var assignedStaff = await AssignVisitorToAvailableStaff(visitorId);
                if (assignedStaff != null)
                {
                    staffId = assignedStaff.StaffId;
                    await Clients.Caller.SendAsync("StaffAssigned", assignedStaff);
                }
                else
                {
                    await Clients.Caller.SendAsync("NoStaffAvailable", "当前没有可用客服，请稍后再试");
                    return;
                }
            }

            // 发送消息给指定客服
            var staff = await _cacheService.GetOnlineStaffAsync(staffId);
            if (staff != null)
            {
                await Clients.Client(staff.ConnectionId).SendAsync("ReceiveMessage", message);
                
                // 发布消息到队列
                await _messageQueueService.PublishChatMessageAsync(new ChatMessageQueueItem
                {
                    MessageId = message.MessageId,
                    UserId = message.UserId,
                    UserName = message.UserName,
                    Content = message.Content,
                    Timestamp = message.Timestamp,
                    MessageType = message.MessageType,
                    IsPrivate = true,
                    TargetUserId = staffId,
                    SessionId = message.SessionId
                });

                // 记录消息统计
                await _statisticsService.RecordMessageAsync(visitorId, messageData.Type.ToString(), DateTime.UtcNow);
            }

            _logger.LogInformation("访客消息发送: {VisitorId} -> {StaffId}", visitorId, staffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "访客发送消息失败: {VisitorId}", visitorId);
            await Clients.Caller.SendAsync("SendMessageError", $"发送消息失败: {ex.Message}");
        }
    }

    #endregion

    #region 客服相关方法

    /// <summary>
    /// 客服加入工作队列
    /// </summary>
    /// <param name="staffId">客服ID</param>
    /// <param name="staffName">客服姓名</param>
    /// <param name="department">部门</param>
    public async Task JoinStaffQueue(string staffId, string staffName, string? department = null)
    {
        try
        {
            var staffInfo = new StaffInfo
            {
                StaffId = staffId,
                StaffName = staffName,
                Department = department ?? "客服部",
                ConnectionId = Context.ConnectionId,
                Status = StaffStatus.Online,
                JoinTime = DateTime.Now,
                MaxConcurrentChats = 5
            };

            await _cacheService.SetOnlineStaffAsync(staffInfo);
            await Groups.AddToGroupAsync(Context.ConnectionId, "Staff");

            // 通知其他客服有新客服上线
            await Clients.Group("Staff").SendAsync("StaffJoined", staffInfo);
            await NotifyStaffListUpdate();

            // 记录客服工作统计
            await _statisticsService.RecordStaffWorkAsync(staffId, "Online", DateTime.UtcNow);

            _logger.LogInformation("客服上线: {StaffName} ({StaffId})", staffName, staffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "客服加入工作队列失败: {StaffId}", staffId);
            await Clients.Caller.SendAsync("JoinStaffQueueError", $"加入工作队列失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 客服发送消息给访客
    /// </summary>
    /// <param name="visitorConnectionId">访客连接ID</param>
    /// <param name="messageData">消息数据</param>
    public async Task SendMessageToVisitor(string visitorConnectionId, ChatMessageDto messageData)
    {
        try
        {
            var message = new ChatMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                SessionId = messageData.SessionId ?? "",
                UserId = messageData.SenderId,
                UserName = messageData.SenderName,
                Content = messageData.Content,
                Timestamp = DateTime.UtcNow,
                MessageType = messageData.Type.ToString(),
                IsFromStaff = true
            };

            // 保存消息到缓存
            await _cacheService.SaveChatMessageAsync(message);

            // 发送消息给指定访客
            await Clients.Client(visitorConnectionId).SendAsync("ReceiveMessage", message);

            // 发布消息到队列
            await _messageQueueService.PublishChatMessageAsync(new ChatMessageQueueItem
            {
                MessageId = message.MessageId,
                UserId = message.UserId,
                UserName = message.UserName,
                Content = message.Content,
                Timestamp = message.Timestamp,
                MessageType = message.MessageType,
                IsPrivate = true,
                SessionId = message.SessionId
            });

            // 记录消息统计
            await _statisticsService.RecordMessageAsync(messageData.SenderId, messageData.Type.ToString(), DateTime.UtcNow);

            _logger.LogInformation("客服消息发送: {StaffId} -> {VisitorConnectionId}", messageData.SenderId, visitorConnectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "客服发送消息失败: {StaffId}", messageData.SenderId);
            await Clients.Caller.SendAsync("SendMessageError", $"发送消息失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 标记客服为忙碌状态
    /// </summary>
    /// <param name="staffId">客服ID</param>
    public async Task MarkStaffBusy(string staffId)
    {
        await _cacheService.UpdateStaffStatusAsync(staffId, StaffStatus.Busy);
        await NotifyStaffStatusChanged(staffId, StaffStatus.Busy);
    }

    /// <summary>
    /// 标记客服为可用状态
    /// </summary>
    /// <param name="staffId">客服ID</param>
    public async Task MarkStaffAvailable(string staffId)
    {
        await _cacheService.UpdateStaffStatusAsync(staffId, StaffStatus.Online);
        await NotifyStaffStatusChanged(staffId, StaffStatus.Online);
    }

    #endregion

    #region 通用方法

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="messageData">消息数据</param>
    public async Task SendMessageToAll(ChatMessageDto messageData)
    {
        try
        {
            var message = new ChatMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserId = messageData.SenderId,
                UserName = messageData.SenderName,
                Content = messageData.Content,
                Timestamp = DateTime.UtcNow,
                MessageType = messageData.Type.ToString(),
                IsFromStaff = false
            };

            // 发送消息给所有连接的客户端
            await Clients.All.SendAsync("ReceiveMessage", message);

            // 发布消息到队列
            await _messageQueueService.PublishChatMessageAsync(new ChatMessageQueueItem
            {
                MessageId = message.MessageId,
                UserId = message.UserId,
                UserName = message.UserName,
                Content = message.Content,
                Timestamp = message.Timestamp,
                MessageType = message.MessageType,
                IsPrivate = false
            });

            // 记录消息统计
            await _statisticsService.RecordMessageAsync(messageData.SenderId, messageData.Type.ToString(), DateTime.UtcNow);

            _logger.LogInformation("群发消息: {UserName} -> {Content}", messageData.SenderName, messageData.Content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送群发消息失败: {UserId}", messageData.SenderId);
            await Clients.Caller.SendAsync("SendMessageError", $"发送消息失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 用户正在输入
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    public async Task UserTyping(string userId, string userName)
    {
        await Clients.Others.SendAsync("UserTyping", userId, userName);
    }

    /// <summary>
    /// 用户停止输入
    /// </summary>
    /// <param name="userId">用户ID</param>
    public async Task UserStoppedTyping(string userId)
    {
        await Clients.Others.SendAsync("UserStoppedTyping", userId);
    }

    /// <summary>
    /// 获取在线用户列表
    /// </summary>
    public async Task GetOnlineUsers()
    {
        var onlineUsers = await _cacheService.GetAllOnlineUsersAsync();
        await Clients.Caller.SendAsync("UpdateOnlineUsers", onlineUsers);
    }

    /// <summary>
    /// 获取在线客服列表
    /// </summary>
    public async Task GetOnlineStaff()
    {
        var onlineStaff = await _cacheService.GetAllOnlineStaffAsync();
        await Clients.Caller.SendAsync("UpdateOnlineStaff", onlineStaff);
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 为访客分配可用的客服
    /// </summary>
    /// <param name="visitorId">访客ID</param>
    /// <returns>分配的客服信息</returns>
    private async Task<StaffInfo?> AssignVisitorToAvailableStaff(string visitorId)
    {
        try
        {
            var availableStaff = await _cacheService.GetAvailableStaffAsync();
            if (!availableStaff.Any())
                return null;

            // 选择当前对话数量最少的客服
            StaffInfo? selectedStaff = null;
            var minConversations = int.MaxValue;

            foreach (var staff in availableStaff)
            {
                var conversationCount = await _cacheService.GetStaffConversationCountAsync(staff.StaffId);
                if (conversationCount < staff.MaxConcurrentChats && conversationCount < minConversations)
                {
                    minConversations = conversationCount;
                    selectedStaff = staff;
                }
            }

            if (selectedStaff != null)
            {
                // 建立访客-客服映射
                await _cacheService.SetVisitorStaffMappingAsync(visitorId, selectedStaff.StaffId);
                await _cacheService.IncrementStaffConversationCountAsync(selectedStaff.StaffId);

                // 创建聊天会话
                var sessionId = await _cacheService.CreateChatSessionAsync(visitorId, selectedStaff.StaffId);

                // 发布访客分配事件
                await _messageQueueService.PublishVisitorAssignmentAsync(new VisitorAssignmentItem
                {
                    VisitorId = visitorId,
                    StaffId = selectedStaff.StaffId,
                    AssignmentType = "Assign",
                    Timestamp = DateTime.UtcNow,
                    SessionId = sessionId
                });

                _logger.LogInformation("访客已分配给客服: {VisitorId} -> {StaffId}", visitorId, selectedStaff.StaffId);
            }

            return selectedStaff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "分配客服失败: {VisitorId}", visitorId);
            return null;
        }
    }

    /// <summary>
    /// 通知客服状态变更
    /// </summary>
    /// <param name="staffId">客服ID</param>
    /// <param name="status">新状态</param>
    private async Task NotifyStaffStatusChanged(string staffId, StaffStatus status)
    {
        try
        {
            var staff = await _cacheService.GetOnlineStaffAsync(staffId);
            if (staff != null)
            {
                // 发布状态变更事件
                await _messageQueueService.PublishStaffStatusChangeAsync(new StaffStatusChangeItem
                {
                    StaffId = staffId,
                    StaffName = staff.StaffName,
                    OldStatus = staff.Status.ToString(),
                    NewStatus = status.ToString(),
                    Timestamp = DateTime.UtcNow
                });

                // 通知其他客服
                await Clients.Group("Staff").SendAsync("StaffStatusChanged", staffId, status);
                await NotifyStaffListUpdate();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "通知客服状态变更失败: {StaffId}", staffId);
        }
    }

    /// <summary>
    /// 通知客服列表更新
    /// </summary>
    private async Task NotifyStaffListUpdate()
    {
        try
        {
            var onlineStaff = await _cacheService.GetAllOnlineStaffAsync();
            await Clients.All.SendAsync("UpdateOnlineStaff", onlineStaff);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "通知客服列表更新失败");
        }
    }

    #endregion
}

/// <summary>
/// 聊天消息DTO
/// </summary>
public class ChatMessageDto
{
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string? SenderAvatar { get; set; }
    public string Content { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Text;
    public string? ChatRoomId { get; set; }
    public string? SessionId { get; set; }
}

/// <summary>
/// 消息类型枚举
/// </summary>
public enum MessageType
{
    Text = 0,
    Image = 1,
    File = 2,
    System = 3
}