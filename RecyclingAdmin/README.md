# RecyclingAdmin - IT设备回收后台管理系统

基于 Vue 3 + TypeScript + Element Plus 构建的现代化后台管理系统。

## 🚀 项目特性

- **现代化技术栈**: Vue 3 + TypeScript + Vite + Element Plus
- **响应式设计**: 适配不同屏幕尺寸
- **权限管理**: 基于角色的访问控制
- **模块化架构**: 清晰的代码组织结构
- **丰富的组件**: 完整的CRUD操作界面

## 📋 功能模块

### 内容管理
- ✅ 首页轮播图管理
- 🚧 公司信息编辑
- 🚧 产品分类管理
- 🚧 案例内容管理

### 招聘管理
- ✅ 岗位发布/下架
- 🚧 简历管理
- 🚧 应聘者状态跟踪

### 咨询管理
- 🚧 客服账号管理
- 🚧 对话监控
- 🚧 数据统计

### 表单管理
- 🚧 联系表单查看
- 🚧 导出功能

### 系统管理
- ✅ 用户管理
- ✅ 权限控制

## 🛠️ 技术栈

- **前端框架**: Vue 3.x
- **开发语言**: TypeScript
- **构建工具**: Vite
- **UI组件库**: Element Plus
- **状态管理**: Pinia
- **路由管理**: Vue Router 4
- **图标库**: Element Plus Icons

## 📦 安装和运行

### 环境要求
- Node.js >= 16.0.0
- npm >= 8.0.0

### 安装依赖
```bash
npm install
```

### 开发环境运行
```bash
npm run dev
```

### 生产环境构建
```bash
npm run build
```

### 预览构建结果
```bash
npm run preview
```

## 🔐 登录账号

系统提供以下测试账号：

| 角色 | 用户名 | 密码 | 权限 |
|------|--------|------|------|
| 管理员 | admin | Admin123! | 全部功能 |
| 客服员工 | staff | Staff123! | 部分功能 |

## 📁 项目结构

```
src/
├── assets/          # 静态资源
├── components/      # 公共组件
├── layouts/         # 布局组件
├── router/          # 路由配置
├── stores/          # 状态管理
├── types/           # TypeScript类型定义
├── views/           # 页面组件
│   ├── content/     # 内容管理页面
│   ├── recruitment/ # 招聘管理页面
│   ├── consultation/# 咨询管理页面
│   ├── forms/       # 表单管理页面
│   └── ...
├── App.vue          # 根组件
└── main.ts          # 入口文件
```

## 🎨 界面预览

### 登录页面
- 现代化渐变背景设计
- 表单验证和错误提示
- 测试账号信息展示

### 仪表板
- 数据统计卡片展示
- 功能模块快速导航
- 系统活动时间线
- 服务状态监控

### 用户管理
- 用户列表展示和搜索
- 用户类型统计
- 用户状态管理
- 添加/编辑用户功能

### 轮播图管理
- 图片预览和上传
- 状态切换和排序
- 完整的CRUD操作

## 🔧 开发说明

### 添加新页面
1. 在 `src/views/` 下创建页面组件
2. 在 `src/router/index.ts` 中添加路由配置
3. 在侧边栏菜单中添加导航项

### 状态管理
使用 Pinia 进行状态管理，stores 位于 `src/stores/` 目录下。

### 类型定义
TypeScript 类型定义位于 `src/types/` 目录下。

## 📝 开发计划

- [ ] 完善所有功能模块
- [ ] 添加数据可视化图表
- [ ] 实现文件上传和管理
- [ ] 添加系统设置功能
- [ ] 优化移动端适配
- [ ] 添加国际化支持

## 🤝 贡献指南

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 📞 联系方式

如有问题或建议，请通过以下方式联系：

- 项目地址: [GitHub Repository]
- 问题反馈: [Issues]

---

⭐ 如果这个项目对你有帮助，请给它一个星标！

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
