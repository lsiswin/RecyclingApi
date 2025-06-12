using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RecyclingApi.Application.Common.Redis;

/// <summary>
/// 聊天专用 Redis 缓存服务实现
/// </summary>
public class ChatRedisCacheService : IChatCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IDatabase _database;
    private readonly ILogger<ChatRedisCacheService> _logger;

    // 缓存键前缀
    private const string USER_PREFIX = "chat:user:";
    private const string STAFF_PREFIX = "chat:staff:";
    private const string MAPPING_PREFIX = "chat:mapping:";
    private const string COUNT_PREFIX = "chat:count:";
    private const string MESSAGE_PREFIX = "chat:message:";
    private const string SESSION_PREFIX = "chat:session:";
    private const string CONNECTION_PREFIX = "chat:connection:";

    // 缓存过期时间
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromHours(24);
    private static readonly TimeSpan SessionExpiry = TimeSpan.FromDays(7);
    private static readonly TimeSpan MessageExpiry = TimeSpan.FromDays(30);

    public ChatRedisCacheService(
        IDistributedCache distributedCache,
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<ChatRedisCacheService> logger)
    {
        _distributedCache = distributedCache;
        _database = connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    #region 用户管理

    public async Task SetOnlineUserAsync(UserInfo user, TimeSpan? expiry = null)
    {
        try
        {
            var key = USER_PREFIX + user.UserId;
            var connectionKey = CONNECTION_PREFIX + user.ConnectionId;
            var json = JsonSerializer.Serialize(user);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? DefaultExpiry
            };

            await _distributedCache.SetStringAsync(key, json, options);
            await _distributedCache.SetStringAsync(connectionKey, user.UserId, options);

            _logger.LogDebug("用户缓存已设置: {UserId}", user.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "设置用户缓存失败: {UserId}", user.UserId);
            throw;
        }
    }

    public async Task<UserInfo?> GetOnlineUserAsync(string userId)
    {
        try
        {
            var key = USER_PREFIX + userId;
            var json = await _distributedCache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<UserInfo>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户缓存失败: {UserId}", userId);
            return null;
        }
    }

    public async Task RemoveOnlineUserAsync(string userId)
    {
        try
        {
            var user = await GetOnlineUserAsync(userId);
            if (user != null)
            {
                var userKey = USER_PREFIX + userId;
                var connectionKey = CONNECTION_PREFIX + user.ConnectionId;
                
                await _distributedCache.RemoveAsync(userKey);
                await _distributedCache.RemoveAsync(connectionKey);
            }

            _logger.LogDebug("用户缓存已移除: {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "移除用户缓存失败: {UserId}", userId);
            throw;
        }
    }

    public async Task<List<UserInfo>> GetAllOnlineUsersAsync()
    {
        try
        {
            var pattern = USER_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            var users = new List<UserInfo>();

            foreach (var key in keys)
            {
                var json = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    var user = JsonSerializer.Deserialize<UserInfo>(json);
                    if (user != null)
                        users.Add(user);
                }
            }

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取所有在线用户失败");
            return new List<UserInfo>();
        }
    }

    public async Task<UserInfo?> GetUserByConnectionIdAsync(string connectionId)
    {
        try
        {
            var connectionKey = CONNECTION_PREFIX + connectionId;
            var userId = await _distributedCache.GetStringAsync(connectionKey);
            
            if (string.IsNullOrEmpty(userId))
                return null;

            return await GetOnlineUserAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "根据连接ID获取用户失败: {ConnectionId}", connectionId);
            return null;
        }
    }

    #endregion

    #region 客服管理

    public async Task SetOnlineStaffAsync(StaffInfo staff, TimeSpan? expiry = null)
    {
        try
        {
            var key = STAFF_PREFIX + staff.StaffId;
            var json = JsonSerializer.Serialize(staff);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? DefaultExpiry
            };

            await _distributedCache.SetStringAsync(key, json, options);
            _logger.LogDebug("客服缓存已设置: {StaffId}", staff.StaffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "设置客服缓存失败: {StaffId}", staff.StaffId);
            throw;
        }
    }

    public async Task<StaffInfo?> GetOnlineStaffAsync(string staffId)
    {
        try
        {
            var key = STAFF_PREFIX + staffId;
            var json = await _distributedCache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<StaffInfo>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取客服缓存失败: {StaffId}", staffId);
            return null;
        }
    }

    public async Task RemoveOnlineStaffAsync(string staffId)
    {
        try
        {
            var key = STAFF_PREFIX + staffId;
            await _distributedCache.RemoveAsync(key);
            _logger.LogDebug("客服缓存已移除: {StaffId}", staffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "移除客服缓存失败: {StaffId}", staffId);
            throw;
        }
    }

    public async Task<List<StaffInfo>> GetAllOnlineStaffAsync()
    {
        try
        {
            var pattern = STAFF_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            var staff = new List<StaffInfo>();

            foreach (var key in keys)
            {
                var json = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    var staffInfo = JsonSerializer.Deserialize<StaffInfo>(json);
                    if (staffInfo != null)
                        staff.Add(staffInfo);
                }
            }

            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取所有在线客服失败");
            return new List<StaffInfo>();
        }
    }

    public async Task<List<StaffInfo>> GetAvailableStaffAsync()
    {
        var allStaff = await GetAllOnlineStaffAsync();
        return allStaff.Where(s => s.Status == StaffStatus.Online).ToList();
    }

    public async Task UpdateStaffStatusAsync(string staffId, StaffStatus status)
    {
        try
        {
            var staff = await GetOnlineStaffAsync(staffId);
            if (staff != null)
            {
                staff.Status = status;
                await SetOnlineStaffAsync(staff);
                _logger.LogDebug("客服状态已更新: {StaffId} -> {Status}", staffId, status);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新客服状态失败: {StaffId}", staffId);
            throw;
        }
    }

    #endregion

    #region 访客-客服映射

    public async Task SetVisitorStaffMappingAsync(string visitorId, string staffId, TimeSpan? expiry = null)
    {
        try
        {
            var key = MAPPING_PREFIX + visitorId;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? DefaultExpiry
            };

            await _distributedCache.SetStringAsync(key, staffId, options);
            _logger.LogDebug("访客-客服映射已设置: {VisitorId} -> {StaffId}", visitorId, staffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "设置访客-客服映射失败: {VisitorId}", visitorId);
            throw;
        }
    }

    public async Task<string?> GetVisitorStaffMappingAsync(string visitorId)
    {
        try
        {
            var key = MAPPING_PREFIX + visitorId;
            return await _distributedCache.GetStringAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取访客-客服映射失败: {VisitorId}", visitorId);
            return null;
        }
    }

    public async Task RemoveVisitorStaffMappingAsync(string visitorId)
    {
        try
        {
            var key = MAPPING_PREFIX + visitorId;
            await _distributedCache.RemoveAsync(key);
            _logger.LogDebug("访客-客服映射已移除: {VisitorId}", visitorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "移除访客-客服映射失败: {VisitorId}", visitorId);
            throw;
        }
    }

    public async Task<List<string>> GetStaffVisitorsAsync(string staffId)
    {
        try
        {
            var pattern = MAPPING_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            var visitors = new List<string>();

            foreach (var key in keys)
            {
                var mappedStaffId = await _distributedCache.GetStringAsync(key);
                if (mappedStaffId == staffId)
                {
                    var visitorId = key.Replace(MAPPING_PREFIX, "");
                    visitors.Add(visitorId);
                }
            }

            return visitors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取客服访客列表失败: {StaffId}", staffId);
            return new List<string>();
        }
    }

    #endregion

    #region 客服会话计数

    public async Task SetStaffConversationCountAsync(string staffId, int count)
    {
        try
        {
            var key = COUNT_PREFIX + staffId;
            await _distributedCache.SetStringAsync(key, count.ToString());
            _logger.LogDebug("客服会话计数已设置: {StaffId} = {Count}", staffId, count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "设置客服会话计数失败: {StaffId}", staffId);
            throw;
        }
    }

    public async Task<int> GetStaffConversationCountAsync(string staffId)
    {
        try
        {
            var key = COUNT_PREFIX + staffId;
            var countStr = await _distributedCache.GetStringAsync(key);
            return int.TryParse(countStr, out var count) ? count : 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取客服会话计数失败: {StaffId}", staffId);
            return 0;
        }
    }

    public async Task IncrementStaffConversationCountAsync(string staffId)
    {
        try
        {
            var currentCount = await GetStaffConversationCountAsync(staffId);
            await SetStaffConversationCountAsync(staffId, currentCount + 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "增加客服会话计数失败: {StaffId}", staffId);
            throw;
        }
    }

    public async Task DecrementStaffConversationCountAsync(string staffId)
    {
        try
        {
            var currentCount = await GetStaffConversationCountAsync(staffId);
            var newCount = Math.Max(0, currentCount - 1);
            await SetStaffConversationCountAsync(staffId, newCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "减少客服会话计数失败: {StaffId}", staffId);
            throw;
        }
    }

    #endregion

    #region 消息存储

    public async Task SaveChatMessageAsync(ChatMessage message)
    {
        try
        {
            var key = MESSAGE_PREFIX + message.SessionId + ":" + message.MessageId;
            var json = JsonSerializer.Serialize(message);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = MessageExpiry
            };

            await _distributedCache.SetStringAsync(key, json, options);
            _logger.LogDebug("聊天消息已保存: {MessageId}", message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存聊天消息失败: {MessageId}", message.MessageId);
            throw;
        }
    }

    public async Task<List<ChatMessage>> GetChatHistoryAsync(string sessionId, int limit = 50)
    {
        try
        {
            var pattern = MESSAGE_PREFIX + sessionId + ":*";
            var keys = await GetKeysByPatternAsync(pattern);
            var messages = new List<ChatMessage>();

            foreach (var key in keys.Take(limit))
            {
                var json = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    var message = JsonSerializer.Deserialize<ChatMessage>(json);
                    if (message != null)
                        messages.Add(message);
                }
            }

            return messages.OrderBy(m => m.Timestamp).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取聊天历史失败: {SessionId}", sessionId);
            return new List<ChatMessage>();
        }
    }

    public async Task<List<ChatMessage>> GetUserChatHistoryAsync(string userId, int limit = 50)
    {
        try
        {
            var pattern = MESSAGE_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            var messages = new List<ChatMessage>();

            foreach (var key in keys)
            {
                var json = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    var message = JsonSerializer.Deserialize<ChatMessage>(json);
                    if (message != null && message.UserId == userId)
                        messages.Add(message);
                }
            }

            return messages.OrderByDescending(m => m.Timestamp).Take(limit).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户聊天历史失败: {UserId}", userId);
            return new List<ChatMessage>();
        }
    }

    #endregion

    #region 会话管理

    public async Task<string> CreateChatSessionAsync(string visitorId, string staffId)
    {
        try
        {
            var sessionId = Guid.NewGuid().ToString();
            var session = new ChatSession
            {
                SessionId = sessionId,
                VisitorId = visitorId,
                StaffId = staffId,
                StartTime = DateTime.UtcNow,
                Status = ChatSessionStatus.Active
            };

            var key = SESSION_PREFIX + sessionId;
            var json = JsonSerializer.Serialize(session);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = SessionExpiry
            };

            await _distributedCache.SetStringAsync(key, json, options);
            _logger.LogDebug("聊天会话已创建: {SessionId}", sessionId);

            return sessionId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建聊天会话失败: {VisitorId} -> {StaffId}", visitorId, staffId);
            throw;
        }
    }

    public async Task<ChatSession?> GetChatSessionAsync(string sessionId)
    {
        try
        {
            var key = SESSION_PREFIX + sessionId;
            var json = await _distributedCache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<ChatSession>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取聊天会话失败: {SessionId}", sessionId);
            return null;
        }
    }

    public async Task UpdateChatSessionStatusAsync(string sessionId, ChatSessionStatus status)
    {
        try
        {
            var session = await GetChatSessionAsync(sessionId);
            if (session != null)
            {
                session.Status = status;
                if (status == ChatSessionStatus.Ended)
                {
                    session.EndTime = DateTime.UtcNow;
                }

                var key = SESSION_PREFIX + sessionId;
                var json = JsonSerializer.Serialize(session);
                await _distributedCache.SetStringAsync(key, json);
                
                _logger.LogDebug("聊天会话状态已更新: {SessionId} -> {Status}", sessionId, status);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新聊天会话状态失败: {SessionId}", sessionId);
            throw;
        }
    }

    public async Task EndChatSessionAsync(string sessionId)
    {
        await UpdateChatSessionStatusAsync(sessionId, ChatSessionStatus.Ended);
    }

    #endregion

    #region 统计信息

    public async Task<int> GetOnlineUserCountAsync()
    {
        try
        {
            var pattern = USER_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            return keys.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取在线用户数量失败");
            return 0;
        }
    }

    public async Task<int> GetOnlineStaffCountAsync()
    {
        try
        {
            var pattern = STAFF_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            return keys.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取在线客服数量失败");
            return 0;
        }
    }

    public async Task<int> GetActiveSessionCountAsync()
    {
        try
        {
            var pattern = SESSION_PREFIX + "*";
            var keys = await GetKeysByPatternAsync(pattern);
            var activeCount = 0;

            foreach (var key in keys)
            {
                var json = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    var session = JsonSerializer.Deserialize<ChatSession>(json);
                    if (session?.Status == ChatSessionStatus.Active)
                        activeCount++;
                }
            }

            return activeCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取活跃会话数量失败");
            return 0;
        }
    }

    #endregion

    #region 缓存管理

    public async Task ClearCacheAsync(string pattern)
    {
        try
        {
            var keys = await GetKeysByPatternAsync(pattern);
            foreach (var key in keys)
            {
                await _distributedCache.RemoveAsync(key);
            }
            _logger.LogDebug("缓存已清除: {Pattern}", pattern);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "清除缓存失败: {Pattern}", pattern);
            throw;
        }
    }

    public async Task<bool> IsConnectedAsync()
    {
        try
        {
            await _database.PingAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region 私有方法

    private async Task<List<string>> GetKeysByPatternAsync(string pattern)
    {
        try
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern).Select(k => k.ToString()).ToList();
            return keys;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取键列表失败: {Pattern}", pattern);
            return new List<string>();
        }
    }

    #endregion
} 