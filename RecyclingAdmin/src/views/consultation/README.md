# 回收系统后台管理 - 客服聊天模块

## 模块概述

客服聊天模块是回收系统后台管理的核心功能之一，用于客服人员与网站访客进行实时沟通交流。该模块基于SignalR技术实现实时通信，具有以下主要功能：

1. 访客列表管理：显示在线访客及其状态
2. 实时消息收发：支持与访客进行实时对话
3. 快捷回复：预设常用回复内容，提高工作效率
4. 统计数据：显示接待情况、响应时间等关键指标

## 技术实现

### 前端组件

- `CustomerServiceWorkbench.vue`: 客服工作台主界面
- `chat.ts`: 基于SignalR的聊天服务API
- `chatStats.ts`: 聊天统计数据API

### 后端服务

- `ChatHub.cs`: SignalR Hub，负责实时消息处理
- `ChatCacheService.cs`: 聊天缓存服务，基于Redis实现
- `ChatStatisticsService.cs`: 聊天统计服务，收集和分析聊天数据

## 数据流

1. 访客在前台网站发起咨询请求
2. `ChatHub` 接收请求并分配给合适的客服
3. 客服工作台显示新访客并提示
4. 客服与访客进行对话，所有消息通过 `ChatHub` 中转
5. 会话数据保存在Redis缓存和数据库中
6. `ChatStatisticsService` 记录统计数据

## 客服工作流程

1. 登录系统，进入客服工作台
2. 系统自动连接ChatHub并加入客服队列
3. 接收分配的访客咨询请求
4. 与访客进行对话
5. 使用快捷回复处理常见问题
6. 查看当日工作统计数据

## 类型定义

主要数据类型包括：

- `ChatMessage`: 聊天消息
- `Visitor`: 访客信息
- `StaffInfo`: 客服信息
- `UserInfo`: 聊天用户信息
- `ChatStats`: 聊天统计数据

## 连接服务器

工作台组件会自动连接到后端ChatHub：

```typescript
// 启动聊天连接
await chatService.start()

// 加入客服队列
await chatService.joinStaffQueue(
  authStore.user.id, 
  authStore.user.realName || authStore.user.username,
  '客服部'
)
```

## 发送消息

```typescript
// 向访客发送消息
await chatService.sendMessageToVisitor(
  selectedVisitor.connectionId,
  {
    senderId: authStore.user.id,
    senderName: authStore.user.realName || authStore.user.username,
    content: messageText,
    type: 0
  }
)
```