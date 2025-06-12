using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RecyclingApi.Application.Common.MessageQueue;

public class RabbitMQService : IMessageQueueService, IDisposable
{
    private readonly ILogger<RabbitMQService> _logger;
    private readonly string _connectionString;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly object _lock = new();

    public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("RabbitMQ") ?? "amqp://localhost:5672";
        InitializeConnection();
    }

    private void InitializeConnection()
    {
        try
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(_connectionString);
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _logger.LogInformation("RabbitMQ connection established");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to establish RabbitMQ connection");
        }
    }

    public async Task PublishAsync<T>(string queueName, T message) where T : class
    {
        var json = JsonConvert.SerializeObject(message);
        await PublishAsync(queueName, json);
    }

    public async Task PublishAsync(string queueName, string message)
    {
        await Task.Run(() =>
        {
            lock (_lock)
            {
                EnsureConnection();
                _channel?.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                
                var body = Encoding.UTF8.GetBytes(message);
                var properties = _channel?.CreateBasicProperties();
                if (properties != null)
                {
                    properties.Persistent = true;
                }

                _channel?.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
                _logger.LogDebug("Message published to queue {QueueName}", queueName);
            }
        });
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class
    {
        await SubscribeAsync(queueName, async (message) =>
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(message);
                if (obj != null)
                {
                    await handler(obj);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing message in queue {QueueName}", queueName);
            }
        });
    }

    public async Task SubscribeAsync(string queueName, Func<string, Task> handler)
    {
        await Task.Run(() =>
        {
            lock (_lock)
            {
                EnsureConnection();
                _channel?.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        await handler(message);
                        _channel?.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message in queue {QueueName}", queueName);
                        _channel?.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                _channel?.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                _logger.LogInformation("Started consuming from queue {QueueName}", queueName);
            }
        });
    }

    public async Task PublishToExchangeAsync<T>(string exchangeName, string routingKey, T message) where T : class
    {
        var json = JsonConvert.SerializeObject(message);
        await PublishToExchangeAsync(exchangeName, routingKey, json);
    }

    public async Task PublishToExchangeAsync(string exchangeName, string routingKey, string message)
    {
        await Task.Run(() =>
        {
            lock (_lock)
            {
                EnsureConnection();
                var body = Encoding.UTF8.GetBytes(message);
                var properties = _channel?.CreateBasicProperties();
                if (properties != null)
                {
                    properties.Persistent = true;
                }

                _channel?.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: properties, body: body);
                _logger.LogDebug("Message published to exchange {ExchangeName} with routing key {RoutingKey}", exchangeName, routingKey);
            }
        });
    }

    // 简化实现其他方法
    public async Task<bool> CreateQueueAsync(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false)
    {
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteQueueAsync(string queueName) => await Task.FromResult(true);
    public async Task<bool> PurgeQueueAsync(string queueName) => await Task.FromResult(true);
    public async Task<int> GetQueueMessageCountAsync(string queueName) => await Task.FromResult(0);
    public async Task<bool> CreateExchangeAsync(string exchangeName, string type = "direct", bool durable = true, bool autoDelete = false) => await Task.FromResult(true);
    public async Task<bool> DeleteExchangeAsync(string exchangeName) => await Task.FromResult(true);
    public async Task<bool> BindQueueToExchangeAsync(string queueName, string exchangeName, string routingKey = "") => await Task.FromResult(true);
    public async Task PublishToDeadLetterAsync<T>(string originalQueue, T message, string reason) where T : class => await Task.CompletedTask;
    public async Task PublishDelayedAsync<T>(string queueName, T message, TimeSpan delay) where T : class => await Task.CompletedTask;
    public async Task PublishBatchAsync<T>(string queueName, IEnumerable<T> messages) where T : class => await Task.CompletedTask;

    public async Task<bool> IsConnectedAsync()
    {
        return await Task.FromResult(_connection?.IsOpen == true);
    }

    public async Task ReconnectAsync()
    {
        await Task.Run(() =>
        {
            lock (_lock)
            {
                Dispose();
                InitializeConnection();
            }
        });
    }

    private void EnsureConnection()
    {
        if (_connection?.IsOpen != true || _channel?.IsOpen != true)
        {
            InitializeConnection();
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
} 