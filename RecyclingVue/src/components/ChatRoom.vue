<template>
  <div class="chat-room">
    <!-- 聊天室头部 -->
    <div class="chat-header">
      <div class="header-left">
        <h3>在线客服</h3>
        <span class="online-count">在线用户: {{ onlineUsers.length }}</span>
      </div>
      <div class="header-right">
        <el-button 
          v-if="!isConnected" 
          type="primary" 
          size="small" 
          @click="connectToChat"
          :loading="connecting"
        >
          {{ connecting ? '连接中...' : '连接聊天' }}
        </el-button>
        <el-button 
          v-else 
          type="danger" 
          size="small" 
          @click="disconnectFromChat"
        >
          断开连接
        </el-button>
        <el-button 
          type="info" 
          size="small" 
          @click="toggleUserList"
          class="user-list-btn"
        >
          <el-icon><User /></el-icon>
          {{ showUserList ? '隐藏用户' : '显示用户' }}
        </el-button>
      </div>
    </div>

    <div class="chat-body">
      <!-- 在线用户列表 -->
      <div v-if="showUserList" class="user-list">
        <h4>在线用户 ({{ onlineUsers.length }})</h4>
        <div class="user-item" v-for="user in onlineUsers" :key="user.userId">
          <el-avatar :size="32" :src="user.avatar">
            {{ user.userName.charAt(0) }}
          </el-avatar>
          <div class="user-info">
            <span class="user-name">{{ user.userName }}</span>
            <span class="join-time">{{ formatTime(user.joinTime) }}</span>
          </div>
        </div>
      </div>

      <!-- 消息区域 -->
      <div class="message-area" ref="messageArea">
        <div class="connection-status" v-if="!isConnected">
          <el-alert
            title="未连接到聊天服务器"
            type="warning"
            description="请点击连接聊天按钮开始聊天"
            show-icon
            :closable="false"
          />
        </div>

        <div class="messages" v-else>
          <div 
            v-for="message in messages" 
            :key="message.id"
            :class="['message-item', { 'own-message': message.userId === currentUser.userId }]"
          >
            <div class="message-avatar">
              <el-avatar :size="36" :src="message.avatar">
                {{ message.userName.charAt(0) }}
              </el-avatar>
            </div>
            <div class="message-content">
              <div class="message-header">
                <span class="message-user">{{ message.userName }}</span>
                <span class="message-time">{{ formatMessageTime(message.timestamp) }}</span>
              </div>
              <div class="message-text" :class="getMessageTypeClass(message.type)">
                {{ message.content }}
              </div>
            </div>
          </div>

          <!-- 正在输入提示 -->
          <div v-if="typingUsers.length > 0" class="typing-indicator">
            <span>{{ getTypingText() }}</span>
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
        <div class="input-toolbar">
          <el-button size="small" @click="sendQuickMessage('你好，我想咨询IT设备回收相关问题')">
            快速回复: 咨询回收
          </el-button>
          <el-button size="small" @click="sendQuickMessage('请问你们的回收价格如何计算？')">
            快速回复: 价格咨询
          </el-button>
        </div>
        <div class="input-box">
          <el-input
            v-model="newMessage"
            type="textarea"
            :rows="3"
            placeholder="输入消息..."
            @keydown.enter.prevent="handleEnterKey"
            @input="handleTyping"
            :disabled="!isConnected"
          />
          <div class="input-actions">
            <el-button 
              type="primary" 
              @click="sendMessage"
              :disabled="!newMessage.trim() || !isConnected"
            >
              发送
            </el-button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, onUnmounted, nextTick, watch } from 'vue'
import { ElMessage, ElNotification } from 'element-plus'
import { User } from '@element-plus/icons-vue'
import * as signalR from '@microsoft/signalr'

// 响应式数据
const isConnected = ref(false)
const connecting = ref(false)
const showUserList = ref(false)
const messages = ref([])
const onlineUsers = ref([])
const typingUsers = ref([])
const newMessage = ref('')
const messageArea = ref(null)

// 当前用户信息
const currentUser = reactive({
  userId: `user_${Date.now()}`,
  userName: `用户${Math.floor(Math.random() * 1000)}`,
  avatar: '/default-avatar.png'
})

// SignalR连接
let connection = null
let typingTimeout = null

// 连接到聊天服务器
const connectToChat = async () => {
  if (connecting.value) return
  
  connecting.value = true
  
  try {
    const hubUrl = import.meta.env.VITE_API_URL ? 
      `${import.meta.env.VITE_API_URL}/chathub` : 
      'http://localhost:5279/chathub'
    
    console.log('正在连接ChatHub:', hubUrl)
    
    connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        skipNegotiation: false,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
      })
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .configureLogging(signalR.LogLevel.Information)
      .build()

    // 设置事件监听器
    setupEventListeners()

    // 启动连接
    await connection.start()
    
    // 加入聊天室
    await connection.invoke('JoinChat', currentUser.userId, currentUser.userName, currentUser.avatar)
    
    isConnected.value = true
    connecting.value = false
    
    ElMessage.success('已连接到聊天服务器')
    
    // 发送欢迎消息
    addSystemMessage('欢迎来到IT设备回收在线客服！有什么可以帮助您的吗？')
    
  } catch (error) {
    console.error('连接失败:', error)
    connecting.value = false
    ElMessage.error('连接聊天服务器失败，请稍后重试')
  }
}

// 断开连接
const disconnectFromChat = async () => {
  if (connection) {
    try {
      await connection.stop()
      isConnected.value = false
      messages.value = []
      onlineUsers.value = []
      typingUsers.value = []
      ElMessage.info('已断开聊天连接')
    } catch (error) {
      console.error('断开连接失败:', error)
    }
  }
}

// 设置SignalR事件监听器
const setupEventListeners = () => {
  if (!connection) return

  // 接收消息
  connection.on('ReceiveMessage', (message) => {
    messages.value.push(message)
    scrollToBottom()
    
    // 如果不是自己的消息，显示通知
    if (message.userId !== currentUser.userId) {
      showMessageNotification(message)
    }
  })

  // 接收私聊消息
  connection.on('ReceivePrivateMessage', (message) => {
    messages.value.push({
      ...message,
      isPrivate: true
    })
    scrollToBottom()
    showMessageNotification(message, true)
  })

  // 用户加入
  connection.on('UserJoined', (user) => {
    addSystemMessage(`${user.userName} 加入了聊天室`)
  })

  // 用户离开
  connection.on('UserDisconnected', (user) => {
    addSystemMessage(`${user.userName} 离开了聊天室`)
  })

  // 更新在线用户列表
  connection.on('UpdateOnlineUsers', (users) => {
    onlineUsers.value = users
  })

  // 用户正在输入
  connection.on('UserTyping', (userId, userName) => {
    if (userId !== currentUser.userId) {
      if (!typingUsers.value.find(u => u.userId === userId)) {
        typingUsers.value.push({ userId, userName })
      }
    }
  })

  // 用户停止输入
  connection.on('UserStoppedTyping', (userId) => {
    typingUsers.value = typingUsers.value.filter(u => u.userId !== userId)
  })

  // 消息错误
  connection.on('MessageError', (error) => {
    ElMessage.error(error)
  })

  // 被踢出
  connection.on('KickedOut', (reason) => {
    ElMessage.error(`您已被踢出聊天室: ${reason}`)
    disconnectFromChat()
  })

  // 被禁言
  connection.on('Muted', (duration, reason) => {
    ElMessage.warning(`您已被禁言 ${duration} 分钟: ${reason}`)
  })

  // 连接重新建立
  connection.onreconnected(() => {
    ElMessage.success('重新连接成功')
    connection.invoke('JoinChat', currentUser.userId, currentUser.userName, currentUser.avatar)
  })

  // 连接断开
  connection.onclose(() => {
    isConnected.value = false
    ElMessage.warning('连接已断开')
  })
}

// 发送消息
const sendMessage = async () => {
  if (!newMessage.value.trim() || !isConnected.value) return

  try {
    const messageData = {
      senderId: currentUser.userId,
      senderName: currentUser.userName,
      senderAvatar: currentUser.avatar,
      content: newMessage.value.trim(),
      type: 0 // MessageType.Text = 0
    }

    await connection.invoke('SendMessageToAll', messageData)
    newMessage.value = ''
    
    // 停止输入状态
    if (typingTimeout) {
      clearTimeout(typingTimeout)
      typingTimeout = null
      await connection.invoke('UserStoppedTyping', currentUser.userId)
    }
    
  } catch (error) {
    console.error('发送消息失败:', error)
    ElMessage.error('发送消息失败')
  }
}

// 发送快速消息
const sendQuickMessage = (content) => {
  newMessage.value = content
  sendMessage()
}

// 处理输入
const handleTyping = async () => {
  if (!isConnected.value) return

  try {
    // 发送正在输入状态
    await connection.invoke('UserTyping', currentUser.userId, currentUser.userName)
    
    // 清除之前的定时器
    if (typingTimeout) {
      clearTimeout(typingTimeout)
    }
    
    // 设置新的定时器，3秒后停止输入状态
    typingTimeout = setTimeout(async () => {
      try {
        await connection.invoke('UserStoppedTyping', currentUser.userId)
      } catch (error) {
        console.error('停止输入状态失败:', error)
      }
    }, 3000)
    
  } catch (error) {
    console.error('发送输入状态失败:', error)
  }
}

// 处理回车键
const handleEnterKey = (event) => {
  if (event.ctrlKey || event.shiftKey) {
    // Ctrl+Enter 或 Shift+Enter 换行
    return
  } else {
    // 单独的 Enter 发送消息
    sendMessage()
  }
}

// 切换用户列表显示
const toggleUserList = () => {
  showUserList.value = !showUserList.value
}

// 添加系统消息
const addSystemMessage = (content) => {
  messages.value.push({
    id: `system_${Date.now()}`,
    userId: 'system',
    userName: '系统消息',
    avatar: '/system-avatar.png',
    content,
    timestamp: new Date(),
    type: 'System'
  })
  scrollToBottom()
}

// 显示消息通知
const showMessageNotification = (message, isPrivate = false) => {
  ElNotification({
    title: isPrivate ? `私聊消息 - ${message.userName}` : `新消息 - ${message.userName}`,
    message: message.content,
    type: 'info',
    duration: 3000,
    position: 'bottom-right'
  })
}

// 滚动到底部
const scrollToBottom = () => {
  nextTick(() => {
    if (messageArea.value) {
      messageArea.value.scrollTop = messageArea.value.scrollHeight
    }
  })
}

// 格式化时间
const formatTime = (time) => {
  return new Date(time).toLocaleTimeString('zh-CN', {
    hour: '2-digit',
    minute: '2-digit'
  })
}

// 格式化消息时间
const formatMessageTime = (time) => {
  const now = new Date()
  const messageTime = new Date(time)
  const diffInMinutes = Math.floor((now - messageTime) / (1000 * 60))
  
  if (diffInMinutes < 1) {
    return '刚刚'
  } else if (diffInMinutes < 60) {
    return `${diffInMinutes}分钟前`
  } else {
    return messageTime.toLocaleTimeString('zh-CN', {
      hour: '2-digit',
      minute: '2-digit'
    })
  }
}

// 获取正在输入的文本
const getTypingText = () => {
  if (typingUsers.value.length === 1) {
    return `${typingUsers.value[0].userName} 正在输入`
  } else if (typingUsers.value.length > 1) {
    return `${typingUsers.value.length} 人正在输入`
  }
  return ''
}

// 获取消息类型样式
const getMessageTypeClass = (type) => {
  return {
    'system-message': type === 'System',
    'private-message': type === 'Private'
  }
}

// 监听消息变化，自动滚动
watch(messages, () => {
  scrollToBottom()
}, { deep: true })

// 组件挂载时自动连接
onMounted(() => {
  // 可以选择自动连接或手动连接
  // connectToChat()
})

// 组件卸载时断开连接
onUnmounted(() => {
  disconnectFromChat()
})
</script>

<style scoped lang="scss">
.chat-room {
  display: flex;
  flex-direction: column;
  height: 600px;
  border: 1px solid #e4e7ed;
  border-radius: 8px;
  background: #fff;
  overflow: hidden;
}

.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  background: #f5f7fa;
  border-bottom: 1px solid #e4e7ed;
  
  .header-left {
    h3 {
      margin: 0 0 4px 0;
      color: #303133;
      font-size: 16px;
    }
    
    .online-count {
      color: #909399;
      font-size: 12px;
    }
  }
  
  .header-right {
    display: flex;
    gap: 8px;
    align-items: center;
    
    .user-list-btn {
      display: flex;
      align-items: center;
      gap: 4px;
    }
  }
}

.chat-body {
  display: flex;
  flex: 1;
  overflow: hidden;
}

.user-list {
  width: 200px;
  border-right: 1px solid #e4e7ed;
  background: #fafafa;
  padding: 16px;
  overflow-y: auto;
  
  h4 {
    margin: 0 0 16px 0;
    color: #303133;
    font-size: 14px;
  }
  
  .user-item {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 0;
    border-bottom: 1px solid #f0f0f0;
    
    &:last-child {
      border-bottom: none;
    }
    
    .user-info {
      flex: 1;
      min-width: 0;
      
      .user-name {
        display: block;
        font-size: 13px;
        color: #303133;
        font-weight: 500;
      }
      
      .join-time {
        display: block;
        font-size: 11px;
        color: #909399;
        margin-top: 2px;
      }
    }
  }
}

.message-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  
  .connection-status {
    padding: 20px;
  }
  
  .messages {
    flex: 1;
    padding: 16px;
    overflow-y: auto;
    
    .message-item {
      display: flex;
      gap: 12px;
      margin-bottom: 16px;
      
      &.own-message {
        flex-direction: row-reverse;
        
        .message-content {
          .message-header {
            text-align: right;
          }
          
          .message-text {
            background: #409eff;
            color: white;
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
          background: #f0f0f0;
          border-radius: 8px;
          font-size: 14px;
          line-height: 1.4;
          word-wrap: break-word;
          
          &.system-message {
            background: #e1f3d8;
            color: #67c23a;
            font-style: italic;
          }
          
          &.private-message {
            background: #fdf6ec;
            color: #e6a23c;
            border-left: 3px solid #e6a23c;
          }
        }
      }
    }
    
    .typing-indicator {
      display: flex;
      align-items: center;
      gap: 8px;
      color: #909399;
      font-size: 12px;
      font-style: italic;
      margin-bottom: 16px;
      
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
}

.input-area {
  border-top: 1px solid #e4e7ed;
  background: #fafafa;
  
  .input-toolbar {
    padding: 8px 16px;
    border-bottom: 1px solid #f0f0f0;
    display: flex;
    gap: 8px;
  }
  
  .input-box {
    padding: 16px;
    
    .input-actions {
      display: flex;
      justify-content: flex-end;
      margin-top: 8px;
    }
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
  .chat-room {
    height: 500px;
  }
  
  .user-list {
    width: 150px;
    padding: 12px;
  }
  
  .message-area .messages {
    padding: 12px;
  }
  
  .input-area {
    .input-toolbar {
      padding: 6px 12px;
      flex-wrap: wrap;
    }
    
    .input-box {
      padding: 12px;
    }
  }
}
</style> 