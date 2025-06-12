<template>
  <div class="global-chat-widget">
    <!-- 聊天按钮 - 最小化状态 -->
    <div 
      v-if="!isExpanded" 
      class="chat-button"
      @click="expandChat"
      :class="{ 'has-unread': unreadCount > 0, 'pulse': shouldPulse }"
    >
      <el-badge :value="unreadCount" :hidden="unreadCount === 0" :max="99">
        <el-icon :size="24"><ChatDotRound /></el-icon>
      </el-badge>
      <span class="chat-text">在线客服</span>
    </div>

    <!-- 聊天窗口 - 展开状态 -->
    <div v-if="isExpanded" class="chat-window" :class="{ 'mobile': isMobile }">
      <!-- 窗口头部 -->
      <div class="chat-header">
        <div class="header-left">
          <el-avatar :size="32" src="/customer-service.svg">客服</el-avatar>
          <div class="header-info">
            <h4>在线客服</h4>
            <span class="status" :class="{ 'online': isConnected }">
              {{ isConnected ? '在线' : '离线' }}
            </span>
          </div>
        </div>
        <div class="header-actions">
          <el-button 
            type="text" 
            @click="minimizeChat"
            :icon="Minus"
            size="small"
          />
          <el-button 
            type="text" 
            @click="closeChat"
            :icon="Close"
            size="small"
          />
        </div>
      </div>

      <!-- 连接状态提示 -->
      <div v-if="!isConnected && !connecting" class="connection-banner">
        <el-alert
          title="未连接到客服"
          type="warning"
          :closable="false"
          show-icon
        >
          <template #default>
            <div class="connection-actions">
              <span>点击连接开始咨询，您也可以在未连接状态下浏览我们的网站</span>
              <div class="action-buttons">
                <el-button size="small" type="primary" @click="connectToChat">
                  连接客服
                </el-button>
                <el-button size="small" @click="retryConnection">
                  重试连接
                </el-button>
                <el-button size="small" @click="reloadPage">
                  刷新页面
                </el-button>
              </div>
            </div>
          </template>
        </el-alert>
      </div>

      <div v-if="connecting" class="connection-banner">
        <el-alert
          title="正在连接客服..."
          type="info"
          :closable="false"
          show-icon
        />
      </div>

      <!-- 显示离线帮助信息 -->
      <div v-if="!isConnected && !connecting && messages.length === 0" class="offline-help">
        <h4>离线帮助</h4>
        <p>您当前未连接客服，但您可以了解以下常见问题：</p>
        <ul>
          <li>我们回收各类IT设备，包括电脑、服务器、显示器等</li>
          <li>价格根据设备型号、年限和状态评估</li>
          <li>提供免费上门回收服务</li>
        </ul>
        <p>您也可以通过以下方式联系我们：</p>
        <p>电话：400-123-4567</p>
        <p>邮箱：recycling@example.com</p>
      </div>

      <!-- 消息区域 -->
      <div class="messages-container" ref="messagesContainer">
        <!-- 欢迎消息 -->
        <div v-if="messages.length === 0" class="welcome-message">
          <div class="welcome-content">
            <el-icon :size="48" color="#409EFF"></el-icon>
            <h3>欢迎咨询IT设备回收服务</h3>
            <p>我们的客服团队随时为您提供专业的回收咨询服务</p>
            <div class="quick-questions">
              <el-button 
                size="small" 
                @click="sendQuickMessage('我想了解设备回收流程')"
                :disabled="!isConnected"
              >
                回收流程
              </el-button>
              <el-button 
                size="small" 
                @click="sendQuickMessage('请问回收价格如何计算？')"
                :disabled="!isConnected"
              >
                价格咨询
              </el-button>
              <el-button 
                size="small" 
                @click="sendQuickMessage('我需要上门回收服务')"
                :disabled="!isConnected"
              >
                上门服务
              </el-button>
            </div>
          </div>
        </div>

        <!-- 聊天消息 -->
        <div class="messages-list">
          <div 
            v-for="message in messages" 
            :key="message.id"
            :class="['message-item', { 
              'own-message': message.userId === currentUser?.userId,
              'system-message': message.type === 'System'
            }]"
          >
            <div class="message-avatar">
              <el-avatar :size="32" :src="message.avatar">
                {{ message.userName.charAt(0) }}
              </el-avatar>
            </div>
            <div class="message-content">
              <div class="message-header">
                <span class="message-user">{{ message.userName }}</span>
                <span class="message-time">{{ formatMessageTime(message.timestamp) }}</span>
              </div>
              <div class="message-text" :class="getMessageClass(message)">
                {{ message.content }}
              </div>
            </div>
          </div>
        </div>

        <!-- 正在输入提示 -->
        <div v-if="typingUsers.length > 0" class="typing-indicator">
          <div class="typing-content">
            <el-avatar :size="24" src="/customer-service.svg">客</el-avatar>
            <span>客服正在输入</span>
            <div class="typing-dots">
              <span></span>
              <span></span>
              <span></span>
            </div>
          </div>
        </div>
      </div>

      <!-- 输入区域 -->
      <div class="input-area" v-if="isConnected">
        <div class="input-box">
          <el-input
            v-model="newMessage"
            type="textarea"
            :rows="2"
            placeholder="输入消息..."
            @keydown.enter.prevent="handleEnterKey"
            :disabled="!isConnected"
            resize="none"
          />
          <div class="input-actions">
            <el-button 
              type="primary" 
              size="small"
              @click="sendMessage"
              :disabled="!newMessage.trim() || !isConnected"
            >
              发送
            </el-button>
          </div>
        </div>
      </div>

      <!-- 用户信息设置 -->
      <div v-if="showUserForm" class="user-form-overlay">
        <div class="user-form">
          <h4>完善您的信息</h4>
          <el-form :model="userForm" label-width="60px" size="small">
            <el-form-item label="姓名">
              <el-input v-model="userForm.name" placeholder="请输入您的姓名" />
            </el-form-item>
            <el-form-item label="电话">
              <el-input v-model="userForm.phone" placeholder="请输入联系电话" />
            </el-form-item>
            <el-form-item label="邮箱">
              <el-input v-model="userForm.email" placeholder="请输入邮箱地址" />
            </el-form-item>
          </el-form>
          <div class="form-actions">
            <el-button size="small" @click="showUserForm = false">跳过</el-button>
            <el-button type="primary" size="small" @click="saveUserInfo">保存</el-button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted, onUnmounted, nextTick, watch } from 'vue'
import { ElMessage, ElNotification } from 'element-plus'
import { 
  ChatDotRound, 
  Close, 
  Minus   
} from '@element-plus/icons-vue'
import chatService from '@/services/chatService'

// 响应式数据
const isExpanded = ref(false)
const isConnected = ref(false)
const connecting = ref(false)
const messages = ref([])
const typingUsers = ref([])
const newMessage = ref('')
const unreadCount = ref(0)
const shouldPulse = ref(false)
const showUserForm = ref(false)
const messagesContainer = ref(null)

// 用户信息
const currentUser = ref(chatService.getCurrentUser() || {
  userId: `guest_${Date.now()}`,
  userName: '访客',
  avatar: '/default-avatar.png'
})
const userForm = reactive({
  name: '',
  phone: '',
  email: ''
})

// 自动弹出定时器
let autoPopupTimer = null
let pulseTimer = null

// 检测移动端
const isMobile = computed(() => {
  return window.innerWidth <= 768
})

// 页面加载后1分钟自动弹出
const startAutoPopup = () => {
  // 如果用户已经主动打开过聊天，则不自动弹出
  const hasOpenedChat = localStorage.getItem('chat_has_opened')
  if (hasOpenedChat) return

  autoPopupTimer = setTimeout(() => {
    if (!isExpanded.value) {
      shouldPulse.value = true
      // 3秒后自动展开
      setTimeout(() => {
        expandChat()
        shouldPulse.value = false
      }, 3000)
    }
  }, 60000) // 1分钟
}

// 展开聊天窗口
const expandChat = async () => {
  isExpanded.value = true
  unreadCount.value = 0
  shouldPulse.value = false
  
  // 记录用户已打开聊天
  localStorage.setItem('chat_has_opened', 'true')
  
  // 加载历史消息
  loadChatHistory()
  
  // 如果未连接，尝试连接
  if (!isConnected.value) {
    await connectToChat()
  }
  
  // 滚动到底部
  nextTick(() => {
    scrollToBottom()
  })

  // 如果是首次访问，显示用户信息表单
  if (chatService.isFirstVisit() && !currentUser.value?.name) {
    setTimeout(() => {
      showUserForm.value = true
    }, 2000)
  }
}

// 最小化聊天窗口
const minimizeChat = () => {
  isExpanded.value = false
}

// 关闭聊天窗口
const closeChat = () => {
  isExpanded.value = false
  // 可以选择断开连接或保持连接
  // chatService.disconnect()
}

// 连接到聊天服务器
const connectToChat = async () => {
  if (connecting.value) return
  
  connecting.value = true
  
  try {
    const success = await chatService.connect()
    if (success) {
      isConnected.value = true
      ElMessage.success('已连接到客服')
      
      // 发送欢迎消息
      addSystemMessage('您好！欢迎咨询IT设备回收服务，我是您的专属客服，有什么可以帮助您的吗？')
    } else {
      ElMessage.error('连接客服失败，请稍后重试')
    }
  } catch (error) {
    console.error('连接失败:', error)
    ElMessage.error('连接客服失败')
  } finally {
    connecting.value = false
  }
}

// 加载聊天历史记录
const loadChatHistory = () => {
  try {
    const history = chatService.getChatHistory() || []
    messages.value = [...history]
  } catch (error) {
    console.error('加载聊天历史失败:', error)
    messages.value = []
    ElMessage.warning('加载历史消息失败')
  }
}

// 发送消息
const sendMessage = async () => {
  if (!newMessage.value.trim() || !isConnected.value) return

  const messageContent = newMessage.value.trim()
  
  // 立即添加消息到界面和历史记录
  const userMessage = {
    id: `msg_${Date.now()}`,
    userId: currentUser.value.userId,
    userName: currentUser.value.userName,
    avatar: currentUser.value.avatar,
    content: messageContent,
    timestamp: new Date().toISOString(),
    type: 'Text',
    userType: 'Visitor'
  }
  
  messages.value.push(userMessage)
  // 同时添加到 chatService 的历史记录
  chatService.addMessageToHistory(userMessage)
  newMessage.value = ''
  scrollToBottom()

  try {
    await chatService.sendMessage(messageContent)
  } catch (error) {
    console.error('发送消息失败:', error)
    ElMessage.error('发送消息失败')
    
    // 发送失败时，可以选择移除消息或标记为失败
    // 这里我们保留消息但可以添加失败标识
    userMessage.failed = true
  }
}

// 发送快速消息
const sendQuickMessage = async (content) => {
  if (!isConnected.value) {
    await connectToChat()
    if (!isConnected.value) {
      ElMessage.error('连接失败，无法发送消息')
      return
    }
  }
  
  newMessage.value = content
  await sendMessage()
}

// 处理回车键
const handleEnterKey = (event) => {
  if (event.shiftKey) {
    // Shift+Enter 换行
    return
  } else {
    // Enter 发送消息
    sendMessage()
  }
}

// 添加系统消息
const addSystemMessage = (content) => {
  const systemMessage = {
    id: `system_${Date.now()}`,
    userId: 'system',
    userName: '系统消息',
    avatar: '/customer-service.svg',
    content,
    timestamp: new Date().toISOString(),
    type: 'System'
  }
  
  messages.value.push(systemMessage)
  scrollToBottom()
}

// 保存用户信息
const saveUserInfo = () => {
  const updates = {
    userName: userForm.name || currentUser.value?.userName || '访客',
    name: userForm.name,
    phone: userForm.phone,
    email: userForm.email
  }
  
  chatService.updateUserInfo(updates)
  currentUser.value = chatService.getCurrentUser()
  showUserForm.value = false
  
  ElMessage.success('信息已保存')
}

// 滚动到底部
const scrollToBottom = () => {
  nextTick(() => {
    if (messagesContainer.value) {
      messagesContainer.value.scrollTop = messagesContainer.value.scrollHeight
    }
  })
}

// 格式化消息时间
const formatMessageTime = (timestamp) => {
  const now = new Date()
  const messageTime = new Date(timestamp)
  const diffInMinutes = Math.floor((now - messageTime) / (1000 * 60))
  
  if (diffInMinutes < 1) {
    return '刚刚'
  } else if (diffInMinutes < 60) {
    return `${diffInMinutes}分钟前`
  } else if (diffInMinutes < 1440) {
    const hours = Math.floor(diffInMinutes / 60)
    return `${hours}小时前`
  } else {
    return messageTime.toLocaleDateString('zh-CN')
  }
}

// 获取消息样式类
const getMessageClass = (message) => {
  return {
    'system-message': message.type === 'System',
    'private-message': message.isPrivate
  }
}

// 设置聊天服务事件监听器
const setupChatListeners = () => {
  // 监听新消息（通用）
  chatService.on('onMessage', (message) => {
    // 避免重复显示访客自己发送的消息（已在sendMessage中添加）
    if (message.userId !== currentUser.value?.userId || message.userType !== 'Visitor') {
      messages.value.push(message)
      scrollToBottom()
    }
    
    // 如果窗口未展开，增加未读计数
    if (!isExpanded.value && message.userId !== currentUser.value?.userId) {
      unreadCount.value++
      
      // 显示通知
      ElNotification({
        title: '新消息',
        message: `${message.userName}: ${message.content}`,
        type: 'info',
        duration: 3000,
        position: 'bottom-right',
        onClick: () => {
          expandChat()
        }
      })
    }
  })

  // 监听客服消息（访客端专用）
  chatService.on('onStaffMessage', (message) => {
    // 已在 onMessage 中处理，这里可以添加特殊逻辑
    console.log('收到客服消息:', message)
  })

  // 监听连接到客服的通知
  chatService.on('onConnectedToStaff', (staffName) => {
    addSystemMessage(`您已连接到客服 ${staffName}，请开始咨询`)
    ElMessage.success(`已连接到客服 ${staffName}`)
  })

  // 监听访客重新分配
  chatService.on('onVisitorReassigned', (data) => {
    addSystemMessage(data.message)
    ElMessage.info('对话已转接')
  })

  // 监听连接状态变化
  chatService.on('onConnectionChange', (connected) => {
    isConnected.value = connected
    if (!connected) {
      connecting.value = false
    }
  })

  // 监听用户加入
  chatService.on('onUserJoined', (user) => {
    if (user.userId !== currentUser.value?.userId) {
      addSystemMessage(`${user.userName} 加入了对话`)
    }
  })

  // 监听用户离开
  chatService.on('onUserLeft', (user) => {
    if (user.userId !== currentUser.value?.userId) {
      addSystemMessage(`${user.userName} 离开了对话`)
    }
  })
}

// 记录页面访问
const recordPageVisit = () => {
  const pageName = window.location.pathname
  chatService.recordPageVisit(pageName)
}

// 监听消息变化，自动滚动
watch(messages, () => {
  scrollToBottom()
}, { deep: true })

// 组件挂载
onMounted(() => {
  setupChatListeners()
  recordPageVisit()
  startAutoPopup()
  
  // 如果有历史记录，加载它们
  loadChatHistory()
})

// 组件卸载
onUnmounted(() => {
  if (autoPopupTimer) {
    clearTimeout(autoPopupTimer)
  }
  if (pulseTimer) {
    clearTimeout(pulseTimer)
  }
  
  // 不断开连接，保持后台连接状态
  // chatService.disconnect()
})

// 暴露方法给父组件
defineExpose({
  expandChat,
  sendQuickMessage
})

// 聊天服务连接重试
const retryConnection = () => {
  if (connecting.value) return
  
  ElMessage.info('正在重新尝试连接...')
  connectToChat()
}

// 重新加载页面
const reloadPage = () => {
  ElMessage.info('正在重新加载页面...')
  setTimeout(() => {
    window.location.reload()
  }, 500)
}
</script>

<style scoped lang="scss">
.global-chat-widget {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 9999;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}

.chat-button {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px 16px;
  background: linear-gradient(135deg, #409EFF, #67C23A);
  color: white;
  border-radius: 25px;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(64, 158, 255, 0.3);
  transition: all 0.3s ease;
  
  &:hover {
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(64, 158, 255, 0.4);
  }
  
  &.has-unread {
    animation: bounce 2s infinite;
  }
  
  &.pulse {
    animation: pulse 1.5s infinite;
  }
  
  .chat-text {
    font-size: 14px;
    font-weight: 500;
    white-space: nowrap;
  }
}

.chat-window {
  width: 380px;
  height: 500px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
  display: flex;
  flex-direction: column;
  overflow: hidden;
  
  &.mobile {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    width: 100%;
    height: 100%;
    border-radius: 0;
    z-index: 10000;
  }
}

.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  background: linear-gradient(135deg, #409EFF, #67C23A);
  color: white;
  
  .header-left {
    display: flex;
    align-items: center;
    gap: 12px;
    
    .header-info {
      h4 {
        margin: 0;
        font-size: 16px;
        font-weight: 600;
      }
      
      .status {
        font-size: 12px;
        opacity: 0.9;
        
        &.online {
          &::before {
            content: '●';
            color: #67C23A;
            margin-right: 4px;
          }
        }
      }
    }
  }
  
  .header-actions {
    display: flex;
    gap: 4px;
    
    .el-button {
      color: white;
      
      &:hover {
        background: rgba(255, 255, 255, 0.1);
      }
    }
  }
}

.connection-banner {
  padding: 12px;
  
  .connection-actions {
    display: flex;
    flex-direction: column;
    gap: 10px;
    
    .action-buttons {
      display: flex;
      gap: 8px;
      flex-wrap: wrap;
    }
  }
}

.messages-container {
  flex: 1;
  overflow-y: auto;
  padding: 16px;
  background: #f8f9fa;
}

.welcome-message {
  text-align: center;
  padding: 40px 20px;
  
  .welcome-content {
    h3 {
      margin: 16px 0 8px 0;
      color: #303133;
      font-size: 18px;
    }
    
    p {
      color: #606266;
      margin-bottom: 24px;
      line-height: 1.5;
    }
    
    .quick-questions {
      display: flex;
      flex-direction: column;
      gap: 8px;
      
      .el-button {
        border-radius: 20px;
      }
    }
  }
}

.messages-list {
  .message-item {
    display: flex;
    gap: 8px;
    margin-bottom: 16px;
    
    &.own-message {
      flex-direction: row-reverse;
      
      .message-content {
        .message-header {
          text-align: right;
        }
        
        .message-text {
          background: #409EFF;
          color: white;
        }
      }
    }
    
    &.system-message {
      justify-content: center;
      
      .message-content {
        .message-text {
          background: #e1f3d8;
          color: #67c23a;
          font-style: italic;
          text-align: center;
        }
      }
    }
    
    .message-avatar {
      flex-shrink: 0;
    }
    
    .message-content {
      flex: 1;
      min-width: 0;
      
      .message-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 4px;
        
        .message-user {
          font-size: 12px;
          color: #909399;
          font-weight: 500;
        }
        
        .message-time {
          font-size: 11px;
          color: #c0c4cc;
        }
      }
      
      .message-text {
        padding: 8px 12px;
        background: white;
        border-radius: 12px;
        font-size: 14px;
        line-height: 1.4;
        word-wrap: break-word;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
        
        &.private-message {
          background: #fdf6ec;
          color: #e6a23c;
          border-left: 3px solid #e6a23c;
        }
      }
    }
  }
}

.typing-indicator {
  margin-bottom: 16px;
  
  .typing-content {
    display: flex;
    align-items: center;
    gap: 8px;
    color: #909399;
    font-size: 12px;
    font-style: italic;
    
    .typing-dots {
      display: flex;
      gap: 2px;
      
      span {
        width: 4px;
        height: 4px;
        background: #909399;
        border-radius: 50%;
        animation: typing 1.4s infinite ease-in-out;
        
        &:nth-child(1) { animation-delay: -0.32s; }
        &:nth-child(2) { animation-delay: -0.16s; }
      }
    }
  }
}

.offline-help {
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
  margin: 10px;
  border-left: 4px solid #e6a23c;
  
  h4 {
    color: #e6a23c;
    margin-top: 0;
    margin-bottom: 12px;
  }
  
  p {
    margin: 8px 0;
    color: #606266;
  }
  
  ul {
    margin: 10px 0;
    padding-left: 20px;
    
    li {
      margin-bottom: 8px;
      color: #606266;
    }
  }
}

.input-area {
  border-top: 1px solid #e4e7ed;
  background: white;
  padding: 16px;
  
  .input-box {
    .input-actions {
      display: flex;
      justify-content: flex-end;
      margin-top: 8px;
    }
  }
}

.user-form-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  
  .user-form {
    background: white;
    padding: 24px;
    border-radius: 8px;
    width: 300px;
    
    h4 {
      margin: 0 0 16px 0;
      text-align: center;
      color: #303133;
    }
    
    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 8px;
      margin-top: 16px;
    }
  }
}

@keyframes bounce {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-5px);
  }
  60% {
    transform: translateY(-3px);
  }
}

@keyframes pulse {
  0% {
    box-shadow: 0 4px 12px rgba(64, 158, 255, 0.3);
  }
  50% {
    box-shadow: 0 4px 20px rgba(64, 158, 255, 0.6);
  }
  100% {
    box-shadow: 0 4px 12px rgba(64, 158, 255, 0.3);
  }
}

@keyframes typing {
  0%, 80%, 100% {
    transform: scale(0);
  }
  40% {
    transform: scale(1);
  }
}

@media (max-width: 768px) {
  .global-chat-widget {
    bottom: 10px;
    right: 10px;
  }
  
  .chat-button {
    padding: 10px 14px;
    
    .chat-text {
      font-size: 13px;
    }
  }
}
</style> 