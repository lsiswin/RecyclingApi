using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace RecyclingApi.Application.Common.MessageQueue;

/// <summary>
/// 聊天专用 RabbitMQ 消息队列服务实现
/// </summary>
public class ChatRabbitMQService : IChatMessageQueueService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ChatRabbitMQOptions _options;
    private readonly ILogger<ChatRabbitMQService> _logger;

    // 队列名称常量
    private const string CHAT_MESSAGES_QUEUE = "chat.messages";
    private const string STAFF_STATUS_QUEUE = "chat.staff.status";
    private const string VISITOR_ASSIGNMENT_QUEUE = "chat.visitor.assignment";
    private const string SYSTEM_NOTIFICATION_QUEUE = "chat.system.notification";

    // 交换机名称常量
    private const string CHAT_EXCHANGE = "chat.exchange";

    public ChatRabbitMQService(IOptions<ChatRabbitMQOptions> options, ILogger<ChatRabbitMQService> logger)
    {
        _options = options.Value;
        _logger = logger;

        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // 声明交换机和队列
            DeclareExchangesAndQueues();

            _logger.LogInformation("聊天 RabbitMQ 连接已建立");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "聊天 RabbitMQ 连接失败");
            throw;
        }
    }

    /// <summary>
    /// 声明交换机和队列
    /// </summary>
    private void DeclareExchangesAndQueues()
    {
        // 声明主交换机
        _channel.ExchangeDeclare(
            exchange: CHAT_EXCHANGE,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false
        );

        // 声明队列
        var queues = new[]
        {
            CHAT_MESSAGES_QUEUE,
            STAFF_STATUS_QUEUE,
            VISITOR_ASSIGNMENT_QUEUE,
            SYSTEM_NOTIFICATION_QUEUE
        };

        foreach (var queue in queues)
        {
            _channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    {"x-message-ttl", 86400000}, // 24小时TTL
                    {"x-max-length", 10000} // 最大消息数量
                }
            );

            // 绑定队列到交换机
            _channel.QueueBind(
                queue: queue,
                exchange: CHAT_EXCHANGE,
                routingKey: GetRoutingKey(queue)
            );
        }
    }

    /// <summary>
    /// 获取路由键
    /// </summary>
    private string GetRoutingKey(string queueName)
    {
        return queueName switch
        {
            CHAT_MESSAGES_QUEUE => "chat.message.*",
            STAFF_STATUS_QUEUE => "chat.staff.*",
            VISITOR_ASSIGNMENT_QUEUE => "chat.visitor.*",
            SYSTEM_NOTIFICATION_QUEUE => "chat.system.*",
            _ => queueName
        };
    }

    #region 发布消息方法

    /// <summary>
    /// 发布聊天消息到队列
    /// </summary>
    public async Task PublishChatMessageAsync(ChatMessageQueueItem message)
    {
        try
        {
            var routingKey = message.IsPrivate ? "chat.message.private" : "chat.message.public";
            await PublishMessageAsync(CHAT_EXCHANGE, routingKey, message);
            
            _logger.LogDebug("聊天消息已发布到队列: {MessageId}", message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发布聊天消息失败: {MessageId}", message.MessageId);
            throw;
        }
    }

    /// <summary>
    /// 发布客服状态变更到队列
    /// </summary>
    public async Task PublishStaffStatusChangeAsync(StaffStatusChangeItem statusChange)
    {
        try
        {
            var routingKey = $"chat.staff.{statusChange.NewStatus.ToLower()}";
            await PublishMessageAsync(CHAT_EXCHANGE, routingKey, statusChange);
            
            _logger.LogDebug("客服状态变更已发布到队列: {StaffId} -> {Status}", 
                statusChange.StaffId, statusChange.NewStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发布客服状态变更失败: {StaffId}", statusChange.StaffId);
            throw;
        }
    }

    /// <summary>
    /// 发布访客分配事件到队列
    /// </summary>
    public async Task PublishVisitorAssignmentAsync(VisitorAssignmentItem assignment)
    {
        try
        {
            var routingKey = $"chat.visitor.{assignment.AssignmentType.ToLower()}";
            await PublishMessageAsync(CHAT_EXCHANGE, routingKey, assignment);
            
            _logger.LogDebug("访客分配事件已发布到队列: {VisitorId} -> {StaffId}", 
                assignment.VisitorId, assignment.StaffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发布访客分配事件失败: {VisitorId}", assignment.VisitorId);
            throw;
        }
    }

    /// <summary>
    /// 发布系统通知到队列
    /// </summary>
    public async Task PublishSystemNotificationAsync(SystemNotificationItem notification)
    {
        try
        {
            var routingKey = $"chat.system.{notification.Type.ToLower()}";
            await PublishMessageAsync(CHAT_EXCHANGE, routingKey, notification);
            
            _logger.LogDebug("系统通知已发布到队列: {NotificationId}", notification.NotificationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发布系统通知失败: {NotificationId}", notification.NotificationId);
            throw;
        }
    }

    #endregion

    #region 订阅消息方法

    /// <summary>
    /// 订阅聊天消息
    /// </summary>
    public async Task SubscribeChatMessagesAsync(Func<ChatMessageQueueItem, Task> handler)
    {
        await SubscribeAsync(CHAT_MESSAGES_QUEUE, handler);
    }

    /// <summary>
    /// 订阅客服状态变更
    /// </summary>
    public async Task SubscribeStaffStatusChangesAsync(Func<StaffStatusChangeItem, Task> handler)
    {
        await SubscribeAsync(STAFF_STATUS_QUEUE, handler);
    }

    /// <summary>
    /// 订阅访客分配事件
    /// </summary>
    public async Task SubscribeVisitorAssignmentsAsync(Func<VisitorAssignmentItem, Task> handler)
    {
        await SubscribeAsync(VISITOR_ASSIGNMENT_QUEUE, handler);
    }

    /// <summary>
    /// 订阅系统通知
    /// </summary>
    public async Task SubscribeSystemNotificationsAsync(Func<SystemNotificationItem, Task> handler)
    {
        await SubscribeAsync(SYSTEM_NOTIFICATION_QUEUE, handler);
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 发布消息到指定交换机
    /// </summary>
    private async Task PublishMessageAsync<T>(string exchange, string routingKey, T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = Guid.NewGuid().ToString();
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发布消息失败: Exchange={Exchange}, RoutingKey={RoutingKey}", 
                exchange, routingKey);
            throw;
        }
    }

    /// <summary>
    /// 订阅指定队列的消息
    /// </summary>
    private async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler)
    {
        try
        {
            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<T>(json);

                    if (message != null)
                    {
                        await handler(message);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _logger.LogWarning("反序列化消息失败: {Json}", json);
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "处理消息失败: Queue={Queue}", queueName);
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "订阅队列失败: {Queue}", queueName);
            throw;
        }
    }

    #endregion

    public void Dispose()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
            
            _logger.LogInformation("聊天 RabbitMQ 连接已关闭");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "关闭聊天 RabbitMQ 连接失败");
        }
    }
}

/// <summary>
/// 聊天 RabbitMQ 配置选项
/// </summary>
public class ChatRabbitMQOptions
{
    public const string SectionName = "ChatRabbitMQ";

    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "chatuser";
    public string Password { get; set; } = "chatpass123";
    public string VirtualHost { get; set; } = "/";
} 