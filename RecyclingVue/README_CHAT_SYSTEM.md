# IT设备回收在线客服系统

基于 ASP.NET Core SignalR 和 Vue 3 的实时在线客服系统。

## 系统特性

- ✅ **实时通信**: 基于 SignalR 的双向实时通信，支持 WebSocket
- ✅ **智能分配**: 自动将访客分配给可用客服，支持负载均衡
- ✅ **状态管理**: 实时监控客服状态（可用、忙碌、离开）
- ✅ **断线重连**: 自动重连机制，确保服务稳定性
- ✅ **消息路由**: 智能消息路由，支持访客-客服一对一对话
- ✅ **对话转接**: 支持客服之间的对话转接
- ✅ **历史记录**: 聊天历史记录保存和加载

## 技术架构

### 前端技术栈
- Vue 3 + Composition API
- Element Plus UI 组件库
- @microsoft/signalr 客户端
- Vite 构建工具

### 后端技术栈
- ASP.NET Core 6+
- SignalR 实时通信
- Entity Framework Core
- Redis 分布式缓存（推荐生产环境）

## 快速开始

### 1. 启动后端服务

确保 ASP.NET Core 后端服务正在运行：

```bash
cd RecyclingApi.Web
dotnet run
```

默认运行在 `http://localhost:5279`

### 2. 启动前端服务

```bash
cd RecyclingVue
npm install
npm run dev
```

### 3. 访问演示页面

打开浏览器访问：`http://localhost:5173/chat-demo`

## 使用指南

### 访客端使用

1. **访问网站**: 打开任意页面
2. **点击客服按钮**: 页面右下角的"在线客服"按钮
3. **开始对话**: 系统自动连接并分配客服
4. **发送消息**: 在聊天窗口中输入消息

### 客服端使用

1. **打开工作台**: 访问 `/staff-workbench` 页面
2. **上线服务**: 系统自动将客服标记为可用状态
3. **接待访客**: 在访客列表中选择访客进行对话
4. **管理状态**: 可以切换可用/忙碌/离开状态
5. **转接对话**: 可以将访客转接给其他客服

## 核心组件说明

### 1. ChatHub (后端)

位置: `RecyclingApi.Web/Hubs/ChatHub.cs`

主要功能：
- 管理客服和访客连接
- 处理消息路由
- 实现智能分配算法
- 维护在线状态

关键方法：
```csharp
// 访客加入聊天
Task JoinChat(string userId, string userName, string avatar)

// 客服加入队列
Task JoinStaffQueue(string staffId, string staffName, string department, List<string> skills)

// 访客发送消息给客服
Task SendMessageToStaff(string visitorId, ChatMessageDto messageData)

// 客服发送消息给访客
Task SendMessageToVisitor(string visitorConnectionId, ChatMessageDto messageData)

// 客服状态管理
Task MarkStaffBusy(string staffId)
Task MarkStaffAvailable(string staffId)

// 手动分配访客
Task AssignVisitorToStaff(string visitorConnectionId, string staffId)
```

### 2. ChatService (前端)

位置: `RecyclingVue/src/services/chatService.js`

主要功能：
- 管理 SignalR 连接
- 处理消息发送和接收
- 维护本地状态
- 提供统一的 API 接口

关键方法：
```javascript
// 连接到服务器
async connect(userType = 'visitor')

// 访客发送消息
async sendMessageToStaff(content)

// 客服发送消息
async sendMessageToVisitor(visitorConnectionId, content)

// 状态管理
async markStaffBusy()
async markStaffAvailable()

// 事件监听
on(event, callback)
```

功能：
- 全局聊天窗口
- 访客端界面
- 消息显示和发送
- 连接状态管理
功能：
- 客服工作台界面
- 访客列表管理
- 多对话窗口
- 状态切换和转接

## 消息流程

### 新访客对话流程

1. **访客连接**: 访客打开聊天窗口，建立 SignalR 连接
2. **发送消息**: 访客发送第一条消息
3. **智能分配**: 服务器根据算法分配可用客服
4. **通知客服**: 客服收到新访客分配通知
5. **开始对话**: 客服和访客开始实时对话

### 消息路由机制

```
访客消息 → ChatHub.SendMessageToStaff() → 查找分配的客服 → 转发给客服
客服消息 → ChatHub.SendMessageToVisitor() → 直接发送给指定访客
```

### 客服分配策略

当前实现的分配策略：
1. **最少对话数优先**: 优先分配给当前对话数最少的客服
2. **状态过滤**: 只分配给状态为"可用"的客服
3. **负载均衡**: 当客服对话数达到上限时自动标记为忙碌

## 配置说明

### SignalR 连接配置

前端连接配置 (`chatService.js`):
```javascript
this.connection = new signalR.HubConnectionBuilder()
  .withUrl('http://localhost:5279/chathub')  // 后端地址
  .withAutomaticReconnect()  // 自动重连
  .build()
```

### 客服分配参数

在 `ChatHub.cs` 中可以调整的参数：
```csharp
// 每个客服最大同时对话数
if (StaffConversationCount[availableStaff.StaffId] >= 5) {
    await MarkStaffBusy(availableStaff.StaffId);
}
```

## 部署注意事项

### 生产环境配置

1. **分布式缓存**: 使用 Redis 存储客服状态和访客映射
2. **SignalR Backplane**: 配置 Redis backplane 支持多实例部署
3. **数据库持久化**: 将聊天记录保存到数据库
4. **身份验证**: 为客服端添加严格的身份验证

### Redis 配置示例

```csharp
// Startup.cs 或 Program.cs
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

services.AddSignalR()
    .AddStackExchangeRedis("localhost:6379");
```

## 扩展功能

### 已实现功能
- [x] 基础实时通信
- [x] 访客自动分配
- [x] 客服状态管理
- [x] 对话转接
- [x] 断线重连

### 可扩展功能
- [ ] 文件传输支持
- [ ] 表情包支持
- [ ] 客服技能组分配
- [ ] 访客排队系统
- [ ] 对话质量评价
- [ ] 聊天机器人集成
- [ ] 多媒体消息支持
- [ ] 客服工作量统计

## 故障排除

### 常见问题

1. **连接失败**
   - 检查后端服务是否启动
   - 确认端口号是否正确
   - 检查防火墙设置

2. **消息发送失败**
   - 检查 SignalR 连接状态
   - 查看浏览器控制台错误信息
   - 确认用户权限

3. **客服分配失败**
   - 确认有客服在线
   - 检查客服状态是否为"可用"
   - 查看服务器日志

### 调试技巧

1. **启用详细日志**:
```csharp
// appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.AspNetCore.SignalR": "Debug"
    }
  }
}
```

2. **前端调试**:
```javascript
// 在浏览器控制台查看 SignalR 连接状态
console.log(chatService.getConnectionStatus())
```

## 联系支持

如有问题或建议，请联系开发团队。

---

**版本**: 1.0.0  
**更新时间**: 2024年12月  
**兼容性**: Vue 3.x, ASP.NET Core 6+ 