namespace RecyclingApi.Application.Common.MessageQueue;

public interface IMessageQueueService
{
    // Basic publish/subscribe
    Task PublishAsync<T>(string queueName, T message) where T : class;
    Task PublishAsync(string queueName, string message);
    Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class;
    Task SubscribeAsync(string queueName, Func<string, Task> handler);

    // Exchange operations
    Task PublishToExchangeAsync<T>(string exchangeName, string routingKey, T message) where T : class;
    Task PublishToExchangeAsync(string exchangeName, string routingKey, string message);

    // Queue management
    Task<bool> CreateQueueAsync(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false);
    Task<bool> DeleteQueueAsync(string queueName);
    Task<bool> PurgeQueueAsync(string queueName);
    Task<int> GetQueueMessageCountAsync(string queueName);

    // Exchange management
    Task<bool> CreateExchangeAsync(string exchangeName, string type = "direct", bool durable = true, bool autoDelete = false);
    Task<bool> DeleteExchangeAsync(string exchangeName);
    Task<bool> BindQueueToExchangeAsync(string queueName, string exchangeName, string routingKey = "");

    // Dead letter queue
    Task PublishToDeadLetterAsync<T>(string originalQueue, T message, string reason) where T : class;

    // Delayed messages
    Task PublishDelayedAsync<T>(string queueName, T message, TimeSpan delay) where T : class;

    // Batch operations
    Task PublishBatchAsync<T>(string queueName, IEnumerable<T> messages) where T : class;

    // Connection management
    Task<bool> IsConnectedAsync();
    Task ReconnectAsync();
    void Dispose();
} 