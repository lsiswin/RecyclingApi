using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecyclingApi.Application.Common.Redis;
using RecyclingApi.Application.Common.MessageQueue;
using RecyclingApi.Application.Services.Auth;
using StackExchange.Redis;
using RecyclingApi.Application.Services.Chat;
using RecyclingApi.Application.Mappings;
using RecyclingApi.Application.Services.Content;
using RecyclingApi.Application.Services.HR;
using RecyclingApi.Application.Services.Products;
using RecyclingApi.Application.Services.Email;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Application.Common.FileStorage;
using RecyclingApi.Application.Services.Product;

namespace RecyclingApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ChatMappingProfile>();
            cfg.AddProfile<ContentMappingProfile>();
            cfg.AddProfile<CompanyMappingProfile>();
            cfg.AddProfile<ProductMappingProfile>();
            cfg.AddProfile<HRMappingProfile>();
        },typeof(DependencyInjection));

        return services;
    }

    public static IServiceCollection AddApplicationWithRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();

        // Add Redis
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddScoped<IRedisService, RedisService>();

        return services;
    }

    public static IServiceCollection AddApplicationWithMessageQueue(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();

        // Add RabbitMQ
        services.AddScoped<IMessageQueueService, RabbitMQService>();

        return services;
    }

    public static IServiceCollection AddApplicationFull(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();

        // Add Redis
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddScoped<IRedisService, RedisService>();

        // Add RabbitMQ
        services.AddScoped<IMessageQueueService, RabbitMQService>();

        return services;
    }

    /// <summary>
    /// 添加聊天服务相关依赖
    /// </summary>
    public static IServiceCollection AddChatServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();

        // Add Redis for Chat
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        // Add Distributed Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        // Add Chat Services
        services.AddScoped<IChatCacheService, ChatRedisCacheService>();
        services.AddScoped<IChatStatisticsService, ChatStatisticsService>();

        // Add Chat RabbitMQ
        services.AddScoped<IChatMessageQueueService>(provider =>
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            var options = new ChatRabbitMQOptions
            {
                HostName = rabbitMQConfig["HostName"] ?? "localhost",
                Port = int.Parse(rabbitMQConfig["Port"] ?? "5672"),
                UserName = rabbitMQConfig["UserName"] ?? "chatuser",
                Password = rabbitMQConfig["Password"] ?? "chatpass123",
                VirtualHost = rabbitMQConfig["VirtualHost"] ?? "/"
            };
            var logger = provider.GetRequiredService<ILogger<ChatRabbitMQService>>();
            return new ChatRabbitMQService(Microsoft.Extensions.Options.Options.Create(options), logger);
        });

        return services;
    }

    /// <summary>
    /// 添加认证服务相关依赖
    /// </summary>
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();
        services.AddScoped<IResumeService, ResumeService>();
        services.AddScoped<IJobPositionService, JobPositionService>();
        services.AddScoped<ICompanyInfoService, CompanyInfoService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        // 添加邮件服务
        services.AddScoped<IEmailService, EmailService>();


        return services;
    }
} 