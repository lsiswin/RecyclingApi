using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Redis;
using RecyclingApi.Application.Common.MessageQueue;
using RecyclingApi.Application.Common.Responses;

namespace RecyclingApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatMessageQueueService _messageQueueService;
        private readonly IChatCacheService _cacheService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            IChatMessageQueueService messageQueueService,
            IChatCacheService cacheService,
            ILogger<ChatController> logger)
        {
            _messageQueueService = messageQueueService;
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="request">系统消息请求</param>
        /// <returns>发送结果</returns>
        [HttpPost("system-message")]
        public async Task<ApiResponse<bool>> SendSystemMessage([FromBody] SystemMessageRequest request)
        {
            try
            {
                var notification = new SystemNotificationItem
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    Type = request.Type ?? "info",
                    Title = "系统消息",
                    Content = request.Message,
                    Timestamp = DateTime.UtcNow
                };

                await _messageQueueService.PublishSystemNotificationAsync(notification);
                
                return new ApiResponse<bool>(true, "系统消息发送成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送系统消息失败: {Message}", request.Message);
                return new ApiResponse<bool>("发送系统消息失败");
            }
        }

        /// <summary>
        /// 获取聊天统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        [HttpGet("stats")]
        public async Task<ApiResponse<object>> GetChatStats()
        {
            try
            {
                var onlineUsersCount = await _cacheService.GetOnlineUserCountAsync();
                var onlineStaffCount = await _cacheService.GetOnlineStaffCountAsync();
                var activeSessionsCount = await _cacheService.GetActiveSessionCountAsync();

                var stats = new
                {
                    OnlineUsersCount = onlineUsersCount,
                    OnlineStaffCount = onlineStaffCount,
                    ActiveSessionsCount = activeSessionsCount,
                    TotalMessagesToday = await GetTodayMessageCount(),
                    TotalUsers = onlineUsersCount + onlineStaffCount,
                    PeakOnlineUsers = await GetPeakOnlineUsers()
                };

                return new ApiResponse<object>(stats, "获取聊天统计成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取聊天统计失败");
                return new ApiResponse<object>("获取聊天统计失败");
            }
        }

        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns>在线用户列表</returns>
        [HttpGet("online-users")]
        public async Task<ApiResponse<object>> GetOnlineUsers()
        {
            try
            {
                var users = await _cacheService.GetAllOnlineUsersAsync();
                var userList = users.Select(u => new
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Avatar = u.Avatar,
                    UserType = u.UserType,
                    JoinTime = u.JoinTime,
                    SessionId = u.SessionId
                }).ToList();

                return new ApiResponse<object>(userList, "获取在线用户列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取在线用户列表失败");
                return new ApiResponse<object>("获取在线用户列表失败");
            }
        }

        /// <summary>
        /// 获取在线客服列表
        /// </summary>
        /// <returns>在线客服列表</returns>
        [HttpGet("online-staff")]
        public async Task<ApiResponse<object>> GetOnlineStaff()
        {
            try
            {
                var staff = await _cacheService.GetAllOnlineStaffAsync();
                var staffList = staff.Select(s => new
                {
                    StaffId = s.StaffId,
                    StaffName = s.StaffName,
                    Avatar = s.Avatar,
                    Department = s.Department,
                    Status = s.Status.ToString(),
                    JoinTime = s.JoinTime,
                    MaxConcurrentChats = s.MaxConcurrentChats
                }).ToList();

                return new ApiResponse<object>(staffList, "获取在线客服列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取在线客服列表失败");
                return new ApiResponse<object>("获取在线客服列表失败");
            }
        }

        /// <summary>
        /// 踢出用户
        /// </summary>
        /// <param name="request">踢出用户请求</param>
        /// <returns>操作结果</returns>
        [HttpPost("kick-user")]
        public async Task<ApiResponse<bool>> KickUser([FromBody] KickUserRequest request)
        {
            try
            {
                // 发送踢出用户的系统通知
                var notification = new SystemNotificationItem
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    Type = "kick",
                    Title = "用户被踢出",
                    Content = $"用户 {request.UserId} 被踢出聊天室，原因：{request.Reason}",
                    Timestamp = DateTime.UtcNow,
                    TargetUserId = request.UserId
                };

                await _messageQueueService.PublishSystemNotificationAsync(notification);
                
                // 从缓存中移除用户
                await _cacheService.RemoveOnlineUserAsync(request.UserId);

                return new ApiResponse<bool>(true, "用户已被踢出");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "踢出用户失败: {UserId}", request.UserId);
                return new ApiResponse<bool>("踢出用户失败");
            }
        }

        /// <summary>
        /// 禁言用户
        /// </summary>
        /// <param name="request">禁言用户请求</param>
        /// <returns>操作结果</returns>
        [HttpPost("mute-user")]
        public async Task<ApiResponse<bool>> MuteUser([FromBody] MuteUserRequest request)
        {
            try
            {
                // 发送禁言通知
                var notification = new SystemNotificationItem
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    Type = "mute",
                    Title = "用户被禁言",
                    Content = $"用户 {request.UserId} 被禁言 {request.Duration} 分钟，原因：{request.Reason}",
                    Timestamp = DateTime.UtcNow,
                    TargetUserId = request.UserId,
                    Data = new Dictionary<string, object>
                    {
                        { "duration", request.Duration },
                        { "reason", request.Reason }
                    }
                };

                await _messageQueueService.PublishSystemNotificationAsync(notification);

                return new ApiResponse<bool>(true, "用户已被禁言");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "禁言用户失败: {UserId}", request.UserId);
                return new ApiResponse<bool>("禁言用户失败");
            }
        }

        /// <summary>
        /// 获取聊天历史记录
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>聊天历史记录</returns>
        [HttpGet("history/{sessionId}")]
        public async Task<ApiResponse<object>> GetChatHistory(string sessionId, int page = 1, int pageSize = 50)
        {
            try
            {
                var messages = await _cacheService.GetChatHistoryAsync(sessionId, pageSize);
                
                // 简单的分页处理（实际应该在服务层实现更复杂的分页逻辑）
                var pagedMessages = messages
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new
                {
                    Messages = pagedMessages,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = messages.Count
                };

                return new ApiResponse<object>(result, "获取聊天历史记录成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取聊天历史记录失败: {SessionId}", sessionId);
                return new ApiResponse<object>("获取聊天历史记录失败");
            }
        }

        /// <summary>
        /// 获取用户聊天历史记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="limit">限制数量</param>
        /// <returns>用户聊天历史记录</returns>
        [HttpGet("user-history/{userId}")]
        public async Task<ApiResponse<object>> GetUserChatHistory(string userId, int limit = 50)
        {
            try
            {
                var messages = await _cacheService.GetUserChatHistoryAsync(userId, limit);
                return new ApiResponse<object>(messages, "获取用户聊天历史记录成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户聊天历史记录失败: {UserId}", userId);
                return new ApiResponse<object>("获取用户聊天历史记录失败");
            }
        }

        /// <summary>
        /// 获取可用客服列表
        /// </summary>
        /// <returns>可用客服列表</returns>
        [HttpGet("available-staff")]
        public async Task<ApiResponse<object>> GetAvailableStaff()
        {
            try
            {
                var availableStaff = await _cacheService.GetAvailableStaffAsync();
                var staffList = availableStaff.Select(s => new
                {
                    StaffId = s.StaffId,
                    StaffName = s.StaffName,
                    Avatar = s.Avatar,
                    Department = s.Department,
                    Status = s.Status.ToString(),
                    MaxConcurrentChats = s.MaxConcurrentChats,
                    CurrentConversations = _cacheService.GetStaffConversationCountAsync(s.StaffId).Result
                }).ToList();

                return new ApiResponse<object>(staffList, "获取可用客服列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取可用客服列表失败");
                return new ApiResponse<object>("获取可用客服列表失败");
            }
        }

        /// <summary>
        /// 更新客服状态
        /// </summary>
        /// <param name="request">更新客服状态请求</param>
        /// <returns>操作结果</returns>
        [HttpPost("update-staff-status")]
        public async Task<ApiResponse<bool>> UpdateStaffStatus([FromBody] UpdateStaffStatusRequest request)
        {
            try
            {
                // 获取当前客服信息
                var currentStaff = await _cacheService.GetOnlineStaffAsync(request.StaffId);
                if (currentStaff == null)
                {
                    return new ApiResponse<bool>("客服不存在或不在线");
                }

                // 更新客服状态
                await _cacheService.UpdateStaffStatusAsync(request.StaffId, request.Status);

                // 发布状态变更事件
                var statusChange = new StaffStatusChangeItem
                {
                    StaffId = request.StaffId,
                    StaffName = currentStaff.StaffName,
                    OldStatus = currentStaff.Status.ToString(),
                    NewStatus = request.Status.ToString(),
                    Timestamp = DateTime.UtcNow,
                    Reason = request.Reason
                };

                await _messageQueueService.PublishStaffStatusChangeAsync(statusChange);

                return new ApiResponse<bool>(true, "客服状态更新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新客服状态失败: {StaffId}", request.StaffId);
                return new ApiResponse<bool>("更新客服状态失败");
            }
        }

        private async Task<int> GetTodayMessageCount()
        {
            try
            {
                // 这里可以实现获取今日消息数量的逻辑
                // 暂时返回0，实际实现需要根据具体需求
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<int> GetPeakOnlineUsers()
        {
            try
            {
                // 这里可以实现获取峰值在线用户数的逻辑
                // 暂时返回当前在线用户数，实际实现需要根据具体需求
                var userCount = await _cacheService.GetOnlineUserCountAsync();
                var staffCount = await _cacheService.GetOnlineStaffCountAsync();
                return userCount + staffCount;
            }
            catch
            {
                return 0;
            }
        }
    }

    // 请求模型
    public class SystemMessageRequest
    {
        public string Message { get; set; } = string.Empty;
        public string? Type { get; set; }
    }

    public class KickUserRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class MuteUserRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class UpdateStaffStatusRequest
    {
        public string StaffId { get; set; } = string.Empty;
        public StaffStatus Status { get; set; }
        public string? Reason { get; set; }
    }
} 