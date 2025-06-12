# 聊天系统 RabbitMQ 和 Redis 集成文档

## 概述

本文档描述了聊天系统中 RabbitMQ 消息队列和 Redis 缓存的集成实现，提供了企业级的消息处理和数据缓存解决方案。

## 架构设计

### 系统架构图

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   前端客户端     │    │   SignalR Hub   │    │   消息队列服务   │
│                │◄──►│                │◄──►│   (RabbitMQ)    │
│  - 访客端       │    │  - 实时通信     │    │                │
│  - 客服工作台   │    │  - 连接管理     │    │  - 消息路由     │
└─────────────────┘    └─────────────────┘    │  - 事件处理     │
                                              └─────────────────┘
                              │                         │
                              ▼                         ▼
                       ┌─────────────────┐    ┌─────────────────┐
                       │   缓存服务      │    │   后台处理服务   │
                       │   (Redis)       │    │                │
                       │                │    │  - 消息处理     │
                       │  - 用户状态     │    │  - 统计分析     │
                       │  - 会话管理     │    │  - 通知分发     │
                       │  - 消息缓存     │    └─────────────────┘
                       └─────────────────┘
```

### 核心组件

1. **RabbitMQ 消息队列**
   - 聊天消息队列
   - 客服状态变更队列
   - 访客分配队列
   - 系统通知队列

2. **Redis 缓存服务**
   - 在线用户管理
   - 客服状态管理
   - 访客-客服映射
   - 聊天历史缓存
   - 统计数据缓存

3. **后台处理服务**
   - 消息队列订阅
   - 统计数据收集
   - 系统监控

## 环境准备

### 1. 安装 Redis

#### Windows (使用 Docker)
```bash
# 拉取 Redis 镜像
docker pull redis:latest

# 运行 Redis 容器
docker run -d --name redis-chat -p 6379:6379 redis:latest

# 验证连接
docker exec -it redis-chat redis-cli ping
```

#### Linux/macOS
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install redis-server

# CentOS/RHEL
sudo yum install redis

# macOS (使用 Homebrew)
brew install redis

# 启动 Redis
redis-server

# 验证连接
redis-cli ping
```

### 2. 安装 RabbitMQ

#### Windows (使用 Docker)
```bash
# 拉取 RabbitMQ 镜像
docker pull rabbitmq:3-management

# 运行 RabbitMQ 容器
docker run -d --name rabbitmq-chat \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management

# 访问管理界面: http://localhost:15672
# 默认用户名/密码: guest/guest
```

#### Linux/macOS
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install rabbitmq-server

# CentOS/RHEL
sudo yum install rabbitmq-server

# macOS (使用 Homebrew)
brew install rabbitmq

# 启动 RabbitMQ
sudo systemctl start rabbitmq-server

# 启用管理插件
sudo rabbitmq-plugins enable rabbitmq_management
```

## 配置说明

### appsettings.json 配置

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RecyclingApi;...",
    "Redis": "localhost:6379"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

### 生产环境配置

```json
{
  "ConnectionStrings": {
    "Redis": "your-redis-cluster:6379"
  },
  "RabbitMQ": {
    "HostName": "your-rabbitmq-cluster",
    "Port": 5672,
    "UserName": "your-username",
    "Password": "your-password",
    "VirtualHost": "/chat"
  }
}
```

## 核心功能

### 1. 消息队列处理

#### 消息类型
- **聊天消息**: 用户间的实时消息
- **状态变更**: 客服在线状态变化
- **访客分配**: 访客与客服的分配事件
- **系统通知**: 系统级别的通知消息

#### 队列配置
```csharp
// 队列名称
private const string CHAT_MESSAGES_QUEUE = "chat.messages";
private const string STAFF_STATUS_QUEUE = "chat.staff.status";
private const string VISITOR_ASSIGNMENT_QUEUE = "chat.visitor.assignment";
private const string SYSTEM_NOTIFICATION_QUEUE = "chat.system.notification";

// 交换机配置
private const string CHAT_EXCHANGE = "chat.exchange";
```

### 2. Redis 缓存管理

#### 缓存键结构
```
chat:user:{userId}              # 用户信息
chat:staff:{staffId}            # 客服信息
chat:mapping:{visitorId}        # 访客-客服映射
chat:count:{staffId}            # 客服对话计数
chat:message:{messageId}        # 消息缓存
chat:session:{sessionId}        # 会话信息
chat:connection:{connectionId}  # 连接映射
```

#### 缓存过期策略
- 用户/客服信息: 24小时
- 会话信息: 7天
- 消息缓存: 30天
- 统计数据: 根据类型设定

### 3. 统计和监控

#### 实时统计
- 在线用户数量
- 在线客服数量
- 活跃会话数量
- 系统负载情况

#### 历史统计
- 每日消息统计
- 客服工作统计
- 用户活跃度分析
- 响应时间分析

## API 接口

### 统计 API

```http
# 获取实时统计
GET /api/chatstatistics/realtime

# 获取今日统计
GET /api/chatstatistics/today

# 获取指定范围统计
GET /api/chatstatistics/range?startDate=2024-01-01&endDate=2024-01-31

# 获取客服统计
GET /api/chatstatistics/staff?startDate=2024-01-01&endDate=2024-01-31

# 获取系统健康状态
GET /api/chatstatistics/health

# 获取在线用户列表
GET /api/chatstatistics/online-users
```

### 响应示例

#### 实时统计
```json
{
  "onlineUsers": 25,
  "onlineStaff": 5,
  "activeSessions": 18,
  "queuedVisitors": 3,
  "averageWaitTime": 2.5,
  "systemLoad": 0.72
}
```

#### 系统健康状态
```json
{
  "status": "Healthy",
  "timestamp": "2024-01-15T10:30:00Z",
  "onlineUsers": 25,
  "onlineStaff": 5,
  "activeSessions": 18,
  "systemLoad": 0.72,
  "cacheConnected": true,
  "services": {
    "redis": "Connected",
    "signalR": "Running",
    "messageQueue": "Running"
  }
}
```

## 部署指南

### 1. 开发环境部署

```bash
# 1. 启动 Redis
docker run -d --name redis-dev -p 6379:6379 redis:latest

# 2. 启动 RabbitMQ
docker run -d --name rabbitmq-dev \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management

# 3. 启动后端服务
cd RecyclingApi.Web
dotnet run

# 4. 启动前端服务
cd RecyclingVue
npm run dev
```

### 2. 生产环境部署

#### Docker Compose 配置

```yaml
version: '3.8'
services:
  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    command: redis-server --appendonly yes

  rabbitmq:
    image: rabbitmq:3-management-alpine
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      RABBITMQ_DEFAULT_USER: chatuser
      RABBITMQ_DEFAULT_PASS: chatpass
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq

  chat-api:
    build: ./RecyclingApi.Web
    ports:
      - "5279:5279"
    environment:
      - ConnectionStrings__Redis=redis:6379
      - RabbitMQ__HostName=rabbitmq
      - RabbitMQ__UserName=chatuser
      - RabbitMQ__Password=chatpass
    depends_on:
      - redis
      - rabbitmq

volumes:
  redis_data:
  rabbitmq_data:
```

### 3. 集群部署

#### Redis 集群
```bash
# Redis Sentinel 配置
sentinel monitor mymaster 192.168.1.100 6379 2
sentinel down-after-milliseconds mymaster 5000
sentinel failover-timeout mymaster 10000
```

#### RabbitMQ 集群
```bash
# 节点配置
rabbitmqctl join_cluster rabbit@node1
rabbitmqctl set_policy ha-all "^" '{"ha-mode":"all"}'
```

## 性能优化

### 1. Redis 优化

```redis
# 内存优化
maxmemory 2gb
maxmemory-policy allkeys-lru

# 持久化配置
save 900 1
save 300 10
save 60 10000

# 网络优化
tcp-keepalive 300
timeout 0
```

### 2. RabbitMQ 优化

```erlang
# 内存限制
vm_memory_high_watermark.relative = 0.6

# 磁盘限制
disk_free_limit.relative = 2.0

# 连接优化
heartbeat = 60
```

### 3. 应用层优化

```csharp
// 连接池配置
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = connectionString;
    options.ConfigurationOptions = new ConfigurationOptions
    {
        ConnectRetry = 3,
        ConnectTimeout = 5000,
        SyncTimeout = 5000,
        AsyncTimeout = 5000
    };
});
```

## 监控和日志

### 1. 系统监控

- **Redis 监控**: 内存使用、连接数、命令执行
- **RabbitMQ 监控**: 队列长度、消息速率、连接状态
- **应用监控**: 响应时间、错误率、吞吐量

### 2. 日志配置

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "RecyclingApi.Web.Services": "Debug",
      "RecyclingApi.Web.Hubs": "Debug"
    }
  }
}
```

### 3. 健康检查

```csharp
// 添加健康检查
builder.Services.AddHealthChecks()
    .AddRedis(connectionString)
    .AddRabbitMQ(rabbitConnectionString);
```

## 故障排除

### 常见问题

1. **Redis 连接失败**
   ```bash
   # 检查 Redis 状态
   redis-cli ping
   
   # 检查端口
   netstat -an | grep 6379
   ```

2. **RabbitMQ 连接失败**
   ```bash
   # 检查 RabbitMQ 状态
   rabbitmqctl status
   
   # 检查端口
   netstat -an | grep 5672
   ```

3. **消息队列堆积**
   ```bash
   # 查看队列状态
   rabbitmqctl list_queues
   
   # 清空队列
   rabbitmqctl purge_queue chat.messages
   ```

### 性能问题

1. **Redis 内存不足**
   - 增加内存限制
   - 优化缓存策略
   - 清理过期数据

2. **RabbitMQ 消息堆积**
   - 增加消费者数量
   - 优化消息处理逻辑
   - 调整队列参数

## 安全考虑

### 1. 网络安全
- 使用 VPN 或私有网络
- 配置防火墙规则
- 启用 SSL/TLS 加密

### 2. 访问控制
```bash
# Redis 密码认证
requirepass your-strong-password

# RabbitMQ 用户管理
rabbitmqctl add_user chatuser strongpassword
rabbitmqctl set_permissions -p / chatuser ".*" ".*" ".*"
```

### 3. 数据加密
- 敏感数据加密存储
- 传输层加密
- 定期密钥轮换

## 总结

通过集成 RabbitMQ 和 Redis，聊天系统实现了：

1. **高可用性**: 消息队列确保消息不丢失
2. **高性能**: Redis 缓存提供快速数据访问
3. **可扩展性**: 支持水平扩展和集群部署
4. **可监控性**: 完整的统计和监控体系
5. **可维护性**: 清晰的架构和完善的文档

这套解决方案为企业级聊天系统提供了坚实的技术基础，能够支撑大规模的并发用户和消息处理需求。 