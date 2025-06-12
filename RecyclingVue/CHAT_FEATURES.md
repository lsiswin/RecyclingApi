# IT设备回收项目 - 聊天功能说明

## 功能概述

本项目集成了完整的实时聊天功能，支持用户与客服进行在线咨询。聊天功能基于SignalR实现，提供实时通信能力。

## 主要特性

### 1. 全局聊天组件
- **位置**: 固定在页面右下角
- **状态**: 支持最小化和展开两种状态
- **响应式**: 在移动端自动全屏显示

### 2. 自动弹出机制
- **触发条件**: 页面加载后1分钟自动弹出（仅首次访问用户）
- **智能判断**: 如果用户已主动打开过聊天，则不再自动弹出
- **视觉提示**: 弹出前会有脉冲动画提示

### 3. 用户信息管理
- **自动生成**: 首次访问时自动生成用户ID和昵称
- **本地存储**: 用户信息保存在localStorage中
- **访问统计**: 记录用户访问次数和页面浏览记录
- **信息完善**: 支持用户补充姓名、电话、邮箱等信息

### 4. 聊天记录持久化
- **本地存储**: 聊天记录自动保存到localStorage
- **历史加载**: 下次访问时自动加载历史聊天记录
- **容量限制**: 最多保存100条最近消息
- **跨页面**: 在不同页面间保持聊天状态

### 5. 实时通信功能
- **SignalR连接**: 基于WebSocket的实时通信
- **消息推送**: 支持实时接收客服消息
- **连接状态**: 显示在线/离线状态
- **自动重连**: 网络断开后自动重新连接

### 6. 用户体验优化
- **快速回复**: 提供常用问题快速回复按钮
- **消息通知**: 窗口最小化时显示桌面通知
- **未读提醒**: 显示未读消息数量徽章
- **输入提示**: 显示对方正在输入状态

## 技术实现

### 前端技术栈
- **Vue 3**: 使用Composition API
- **Element Plus**: UI组件库
- **SignalR Client**: 实时通信客户端
- **LocalStorage**: 本地数据存储

### 后端技术栈
- **ASP.NET Core**: Web API框架
- **SignalR**: 实时通信服务
- **Entity Framework**: 数据访问层（可选）

## 文件结构

```
RecyclingVue/
├── src/
│   ├── components/
│   │   ├── GlobalChatWidget.vue      # 全局聊天组件
│   │   └── ChatRoom.vue              # 聊天室组件（备用）
│   ├── services/
│   │   └── chatService.js            # 聊天服务类
│   ├── views/
│   │   ├── TestChatView.vue          # 聊天功能测试页面
│   │   └── ContactView.vue           # 联系我们页面（集成聊天）
│   └── App.vue                       # 主应用（集成全局聊天）

RecyclingApi.Web/
├── Hubs/
│   └── ChatHub.cs                    # SignalR Hub
├── Controllers/
│   └── ChatController.cs             # 聊天控制器
└── Program.cs                        # 服务配置
```

## 使用方法

### 1. 启动后端服务
```bash
cd RecyclingApi.Web
dotnet run
```

### 2. 启动前端服务
```bash
cd RecyclingVue
npm run dev
```

### 3. 访问测试页面
打开浏览器访问: `http://localhost:5173/test-chat`

### 4. 测试功能
- 点击"打开聊天窗口"按钮测试聊天界面
- 点击"发送测试消息"测试消息发送
- 点击"显示用户信息"查看用户数据
- 点击"清除聊天数据"重置所有数据

## 集成到其他页面

### 1. 全局集成（推荐）
聊天组件已在`App.vue`中全局集成，所有页面都可使用。

### 2. 手动触发聊天
在任何页面中可以通过以下方式打开聊天窗口：

```javascript
// 方法1: 直接点击聊天按钮
const chatWidget = document.querySelector('.global-chat-widget')
if (chatWidget) {
  const chatButton = chatWidget.querySelector('.chat-button')
  if (chatButton) {
    chatButton.click()
  }
}

// 方法2: 使用聊天服务
import chatService from '@/services/chatService'
// 发送快速消息
chatService.sendMessage('您好，我需要咨询')
```

### 3. 自定义快速咨询按钮
参考`ContactView.vue`中的实现：

```vue
<el-button type="primary" @click="openChat">
  <el-icon><Message /></el-icon>
  立即在线咨询
</el-button>
```

## 配置选项

### 聊天服务配置
在`chatService.js`中可以修改以下配置：

```javascript
// SignalR服务器地址
.withUrl('http://localhost:5000/chathub')

// 自动弹出延迟时间（毫秒）
setTimeout(() => {
  // 1分钟 = 60000毫秒
}, 60000)

// 最大聊天记录数量
const recentMessages = this.chatHistory.slice(-100)
```

### 样式自定义
聊天组件支持通过CSS变量自定义样式：

```scss
.global-chat-widget {
  --chat-primary-color: #409EFF;
  --chat-success-color: #67C23A;
  --chat-border-radius: 12px;
  --chat-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
}
```

## 注意事项

1. **后端服务**: 确保SignalR服务正常运行
2. **CORS配置**: 开发环境需要配置跨域访问
3. **浏览器兼容**: 需要支持WebSocket的现代浏览器
4. **存储限制**: localStorage有容量限制，注意清理旧数据
5. **网络状态**: 网络断开时会自动重连，但可能丢失部分消息

## 扩展功能

### 计划中的功能
- [ ] 文件上传支持
- [ ] 表情包支持
- [ ] 消息撤回功能
- [ ] 聊天记录导出
- [ ] 多客服分配
- [ ] 消息加密传输

### 自定义扩展
可以通过修改`chatService.js`和`GlobalChatWidget.vue`来添加自定义功能。

## 故障排除

### 常见问题
1. **连接失败**: 检查后端服务是否启动
2. **消息不显示**: 检查浏览器控制台错误信息
3. **自动弹出不工作**: 清除浏览器localStorage重试
4. **样式异常**: 检查Element Plus样式是否正确加载

### 调试方法
1. 打开浏览器开发者工具
2. 查看Console面板的错误信息
3. 检查Network面板的WebSocket连接
4. 查看Application面板的localStorage数据

## 联系支持

如有问题或建议，请联系开发团队。 