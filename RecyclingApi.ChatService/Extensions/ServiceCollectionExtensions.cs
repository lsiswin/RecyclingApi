using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyclingApi.Application;

namespace RecyclingApi.ChatService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatService(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Chat services with Redis and RabbitMQ
        services.AddChatServices(configuration);

        // Add SignalR
        services.AddSignalR();

        return services;
    }
} 