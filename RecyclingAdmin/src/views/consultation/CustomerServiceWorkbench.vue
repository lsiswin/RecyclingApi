<template>
  <div class="customer-service-workbench">
    <!-- 工作台头部 -->
    <div class="workbench-header">
      <div class="header-stats">
        <div class="stat-item">
          <el-icon class="stat-icon"><User /></el-icon>
          <div class="stat-info">
            <span class="stat-number">{{ stats.onlineVisitors }}</span>
            <span class="stat-label">在线访客</span>
          </div>
        </div>
        <div class="stat-item">
          <el-icon class="stat-icon"><ChatDotRound /></el-icon>
          <div class="stat-info">
            <span class="stat-number">{{ stats.todayConversations }}</span>
            <span class="stat-label">今日对话</span>
          </div>
        </div>
        <div class="stat-item">
          <el-icon class="stat-icon"><Message /></el-icon>
          <div class="stat-info">
            <span class="stat-number">{{ stats.unreadMessages }}</span>
            <span class="stat-label">未读消息</span>
          </div>
        </div>
      </div>
      <div class="header-actions">
        <el-button type="primary" @click="refreshData">
          <el-icon><Refresh /></el-icon>
          刷新
        </el-button>
      </div>
    </div>

    <!-- 主工作区 -->
    <div class="workbench-content">
      <!-- 访客列表 -->
      <div class="visitor-panel">
        <div class="panel-header">
          <h3>访客列表</h3>
          <el-badge :value="visitors.length" class="visitor-count" />
        </div>
        <div class="visitor-list">
          <div
            v-for="visitor in visitors"
            :key="visitor.id"
            :class="['visitor-item', { active: selectedVisitor?.id === visitor.id }]"
            @click="selectVisitor(visitor)"
          >
            <el-avatar :size="40" class="visitor-avatar">
              <el-icon><User /></el-icon>
            </el-avatar>
            <div class="visitor-info">
              <div class="visitor-name">{{ visitor.name }}</div>
              <div class="visitor-status">
                <el-tag :type="visitor.isOnline ? 'success' : 'info'" size="small">
                  {{ visitor.isOnline ? '在线' : '离线' }}
                </el-tag>
                <span class="visitor-time">{{ visitor.lastActiveTime }}</span>
              </div>
            </div>
            <div v-if="visitor.unreadCount > 0" class="unread-badge">
              {{ visitor.unreadCount }}
            </div>
          </div>
        </div>
      </div>

      <!-- 聊天区域 -->
      <div class="chat-panel">
        <div v-if="selectedVisitor" class="chat-container">
          <!-- 聊天头部 -->
          <div class="chat-header">
            <div class="visitor-profile">
              <el-avatar :size="32">
                <el-icon><User /></el-icon>
              </el-avatar>
              <div class="profile-info">
                <span class="visitor-name">{{ selectedVisitor.name }}</span>
                <span class="visitor-location">{{ selectedVisitor.location }}</span>
              </div>
            </div>
            <div class="chat-actions">
              <el-button size="small" type="primary" @click="endConversation">结束对话</el-button>
            </div>
          </div>

          <!-- 消息列表 -->
          <div class="message-list" ref="messageListRef">
            <div
              v-for="message in currentMessages"
              :key="message.id"
              :class="['message-item', message.isFromStaff ? 'staff' : 'visitor']"
            >
              <div class="message-avatar">
                <el-avatar :size="32">
                  <el-icon v-if="message.isFromStaff"><Service /></el-icon>
                  <el-icon v-else><User /></el-icon>
                </el-avatar>
              </div>
              <div class="message-content">
                <div class="message-bubble">
                  {{ message.content }}
                </div>
                <div class="message-time">{{ message.timestamp }}</div>
              </div>
            </div>
          </div>

          <!-- 快捷回复 -->
          <div class="quick-replies">
            <el-button
              v-for="reply in quickReplies"
              :key="reply.id"
              size="small"
              @click="sendQuickReply(reply.content)"
            >
              {{ reply.title }}
            </el-button>
          </div>

          <!-- 消息输入 -->
          <div class="message-input">
            <div class="input-toolbar">
              <el-button size="small" circle>
                <el-icon><Picture /></el-icon>
              </el-button>
              <el-button size="small" circle>
                <el-icon><Paperclip /></el-icon>
              </el-button>
            </div>
            <el-input
              v-model="messageText"
              type="textarea"
              :rows="3"
              placeholder="输入消息..."
              @keyup.ctrl.enter="sendMessage"
            />
            <el-button type="primary" @click="sendMessage" :disabled="!messageText.trim()">
              发送 (Ctrl+Enter)
            </el-button>
          </div>
        </div>
        <div v-else class="no-visitor-selected">
          <el-empty description="请选择一个访客开始对话" />
        </div>
      </div>

      <!-- 工具面板 -->
      <div class="tools-panel">
        <!-- 访客信息 -->
        <el-card v-if="selectedVisitor" class="visitor-details">
          <template #header>
            <span>访客信息</span>
          </template>
          <div class="detail-item">
            <span class="label">访客ID:</span>
            <span class="value">{{ selectedVisitor.id }}</span>
          </div>
          <div class="detail-item">
            <span class="label">连接时间:</span>
            <span class="value">{{ selectedVisitor.connectTime }}</span>
          </div>
          <div class="detail-item">
            <span class="label">对话时长:</span>
            <span class="value">{{ selectedVisitor.duration }}</span>
          </div>
          <div class="detail-item">
            <span class="label">消息数:</span>
            <span class="value">{{ selectedVisitor.messageCount }}</span>
          </div>
        </el-card>

        <!-- 今日统计 -->
        <el-card class="today-stats">
          <template #header>
            <span>今日统计</span>
          </template>
          <div class="stat-grid">
            <div class="stat-cell">
              <div class="stat-value">{{ todayStats.servedVisitors }}</div>
              <div class="stat-name">接待访客</div>
            </div>
            <div class="stat-cell">
              <div class="stat-value">{{ todayStats.sentMessages }}</div>
              <div class="stat-name">发送消息</div>
            </div>
            <div class="stat-cell">
              <div class="stat-value">{{ todayStats.avgResponseTime }}</div>
              <div class="stat-name">平均响应时间</div>
            </div>
            <div class="stat-cell">
              <div class="stat-value">{{ todayStats.onlineTime }}</div>
              <div class="stat-name">在线时长</div>
            </div>
          </div>
        </el-card>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, nextTick, onMounted, onBeforeUnmount, watch } from 'vue'
import { ElMessage } from 'element-plus'
import { useAuthStore } from '@/stores/auth'
import { chatService } from '@/api/chat'
import { ChatStatsApi } from '@/api/chatStats'
import type { Visitor, ChatMessage, QuickReply, ChatStats, UserInfo } from '@/types/chat'

// 获取认证状态
const authStore = useAuthStore()

// 组件状态
const selectedVisitor = ref<Visitor | null>(null)
const messageText = ref('')
const messageListRef = ref<HTMLElement>()
const isConnected = ref(false)
const isTyping = ref(false)
const typingTimer = ref<number | null>(null)

// 聊天统计数据
const stats = reactive<ChatStats>({
  onlineVisitors: 0,
  todayConversations: 0,
  unreadMessages: 0,
  servedVisitors: 0,
  sentMessages: 0,
  avgResponseTime: '',
  onlineTime: ''
})

// 今日统计数据
const todayStats = reactive({
  servedVisitors: 0,
  sentMessages: 0,
  avgResponseTime: '',
  onlineTime: ''
})

// 访客列表
const visitors = ref<Visitor[]>([])

// 消息列表
const messages = ref<ChatMessage[]>([])

// 快捷回复列表
const quickReplies = ref<QuickReply[]>([
  { id: 'q1', title: '欢迎咨询', content: '您好！欢迎咨询我们的设备回收服务，有什么可以帮助您的吗？' },
  { id: 'q2', title: '回收流程', content: '我们的回收流程：1.在线评估 2.预约上门 3.现场检测 4.确认价格 5.完成回收' },
  { id: 'q3', title: '价格咨询', content: '价格根据设备型号、使用年限、外观成色等因素评估，请提供设备详细信息。' },
  { id: 'q4', title: '上门服务', content: '我们提供免费上门评估服务，工作日和周末都可以预约。' },
  { id: 'q5', title: '支付方式', content: '支持现金、银行转账、支付宝、微信等多种支付方式。' },
  { id: 'q6', title: '感谢咨询', content: '感谢您的咨询！如有其他问题，随时联系我们。' }
])

// 当前选中访客的消息列表
const currentMessages = computed(() => {
  if (!selectedVisitor.value) return []
  return messages.value.filter(m => m.visitorId === selectedVisitor.value!.id)
})

/**
 * 初始化聊天连接和事件处理
 */
const initChatService = async () => {
  try {
    // 启动聊天连接
    await chatService.start()
    isConnected.value = true
    
    // 注册消息接收事件
    chatService.onReceiveMessage((message) => {
      // 转换ChatHub消息格式为本地格式
      const newMessage: ChatMessage = {
        id: message.messageId || `m${Date.now()}`,
        messageId: message.messageId,
        content: message.content,
        isFromStaff: message.isFromStaff,
        timestamp: new Date(message.timestamp).toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
        visitorId: message.userId,
        userId: message.userId,
        userName: message.userName,
        sessionId: message.sessionId,
        messageType: message.messageType
      }
      
      messages.value.push(newMessage)
      
      // 如果是当前选中的访客，滚动到底部
      if (selectedVisitor.value && selectedVisitor.value.id === newMessage.visitorId) {
        nextTick(() => {
          scrollToBottom()
        })
      } else {
        // 增加未读消息计数
        const visitorIndex = visitors.value.findIndex(v => v.id === newMessage.visitorId)
        if (visitorIndex > -1) {
          visitors.value[visitorIndex].unreadCount++
        }
      }
    })
    
    // 注册访客加入事件
    chatService.onVisitorJoined((user: UserInfo) => {
      // 只处理访客类型的用户
      if (user.userType === 0 || user.userType === 'Visitor') {
        const newVisitor: Visitor = {
          id: user.userId,
          userId: user.userId,
          name: user.userName,
          userName: user.userName,
          isOnline: true,
          lastActiveTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
          location: '未知',
          connectTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
          duration: '刚刚',
          messageCount: 0,
          unreadCount: 0,
          connectionId: user.connectionId,
          avatar: user.avatar,
          joinTime: user.joinTime
        }
        
        visitors.value.push(newVisitor)
        stats.onlineVisitors = visitors.value.filter(v => v.isOnline).length
        
        ElMessage.success(`新访客加入: ${user.userName}`)
      }
    })
    
    // 注册访客断开事件
    chatService.onVisitorDisconnected((visitorId, visitorName) => {
      const visitorIndex = visitors.value.findIndex(v => v.id === visitorId || v.userId === visitorId)
      if (visitorIndex > -1) {
        visitors.value[visitorIndex].isOnline = false
        visitors.value[visitorIndex].lastActiveTime = new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' })
        stats.onlineVisitors = visitors.value.filter(v => v.isOnline).length
      }
      
      ElMessage.info(`访客离线: ${visitorName}`)
    })
    
    // 注册访客分配事件
    chatService.onVisitorAssigned((visitor) => {
      ElMessage.success(`访客 ${visitor.userName || visitor.name} 已分配给您`)
    })
    
    // 注册访客重新分配事件
    chatService.onVisitorReassigned((visitorId, message) => {
      ElMessage.info(message)
    })
    
    // 注册在线用户更新事件
    chatService.onUpdateOnlineUsers((users: UserInfo[]) => {
      // 转换为访客格式并更新列表
      const onlineVisitors = users
        .filter(u => u.userType === 0 || u.userType === 'Visitor')
        .map(u => {
          // 查找现有访客信息
          const existingVisitor = visitors.value.find(v => v.id === u.userId || v.userId === u.userId)
          
          return {
            id: u.userId,
            userId: u.userId,
            name: u.userName,
            userName: u.userName,
            isOnline: true,
            lastActiveTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
            location: existingVisitor?.location || '未知',
            connectTime: u.joinTime ? new Date(u.joinTime).toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }) : '未知',
            duration: u.joinTime ? calculateDuration(new Date(u.joinTime), new Date()) : '未知',
            messageCount: existingVisitor?.messageCount || 0,
            unreadCount: existingVisitor?.unreadCount || 0,
            connectionId: u.connectionId,
            avatar: u.avatar,
            joinTime: u.joinTime
          } as Visitor
        })
      
      // 更新访客列表，保留现有的离线访客
      visitors.value = [
        ...onlineVisitors,
        ...visitors.value.filter(v => !v.isOnline && !onlineVisitors.some(ov => ov.id === v.id || ov.userId === v.id))
      ]
      
      stats.onlineVisitors = visitors.value.filter(v => v.isOnline).length
    })
    
    // 加入客服队列
    if (authStore.user) {
      try {
        await chatService.joinStaffQueue(
          authStore.user.id, 
          authStore.user.realName || authStore.user.username,
          '客服部'
        )
      } catch (error) {
        console.error('加入客服队列失败:', error)
      }
      
      // 获取在线访客列表
      try {
        await chatService.getOnlineUsers()
      } catch (error) {
        console.error('获取在线用户失败:', error)
        // 添加一些模拟数据
        loadMockVisitors()
      }
    }
  } catch (error) {
    console.error('初始化聊天服务失败:', error)
    ElMessage.error('连接聊天服务失败，已加载模拟数据')
    isConnected.value = false
    
    // 加载模拟数据
    loadMockVisitors()
  }
}

/**
 * 计算时间间隔
 */
const calculateDuration = (start: Date, end: Date): string => {
  const diffMs = end.getTime() - start.getTime()
  const diffMins = Math.floor(diffMs / 60000)
  
  if (diffMins < 1) {
    return '刚刚'
  } else if (diffMins < 60) {
    return `${diffMins}分钟`
  } else {
    const hours = Math.floor(diffMins / 60)
    const mins = diffMins % 60
    return `${hours}小时${mins ? mins + '分钟' : ''}`
  }
}

/**
 * 加载模拟访客数据
 */
const loadMockVisitors = () => {
  // 创建一些模拟访客
  const mockVisitors: Visitor[] = [
    {
      id: 'v1',
      userId: 'v1',
      name: '访客1',
      userName: '访客1',
      isOnline: true,
      lastActiveTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      location: '北京',
      connectTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      duration: '10分钟',
      messageCount: 5,
      unreadCount: 2,
      connectionId: 'mock-conn-1'
    },
    {
      id: 'v2',
      userId: 'v2',
      name: '访客2',
      userName: '访客2',
      isOnline: true,
      lastActiveTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      location: '上海',
      connectTime: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      duration: '5分钟',
      messageCount: 3,
      unreadCount: 0,
      connectionId: 'mock-conn-2'
    }
  ]
  
  visitors.value = mockVisitors
  stats.onlineVisitors = mockVisitors.length
  
  // 创建一些模拟消息
  const mockMessages: ChatMessage[] = [
    {
      id: 'm1',
      content: '您好，我想咨询一下关于设备回收的问题',
      isFromStaff: false,
      timestamp: new Date(Date.now() - 1000 * 60 * 5).toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      visitorId: 'v1',
      userId: 'v1',
      userName: '访客1'
    },
    {
      id: 'm2',
      content: '您好，有什么可以帮助您的吗？',
      isFromStaff: true,
      timestamp: new Date(Date.now() - 1000 * 60 * 4).toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      visitorId: 'v1',
      userId: 'staff1',
      userName: '客服小李'
    },
    {
      id: 'm3',
      content: '我有一些旧电脑需要回收，价格怎么算？',
      isFromStaff: false,
      timestamp: new Date(Date.now() - 1000 * 60 * 3).toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      visitorId: 'v1',
      userId: 'v1',
      userName: '访客1'
    }
  ]
  
  messages.value = mockMessages
}

/**
 * 加载聊天统计数据
 */
const loadChatStats = async () => {
  try {
    // 获取实时统计数据
    const realTimeStats = await ChatStatsApi.getRealTimeStats()
    
    // 更新基本统计数据
    stats.onlineVisitors = realTimeStats.onlineUsers
    stats.unreadMessages = 0 // 这个会在消息处理中累加
    
    // 获取今日统计数据
    const todayStatsData = await ChatStatsApi.getTodayStats()
    
    // 更新聊天统计
    stats.todayConversations = todayStatsData.totalSessions
    
    // 更新今日统计数据
    todayStats.servedVisitors = todayStatsData.servedVisitors
    todayStats.sentMessages = todayStatsData.sentMessages
    todayStats.avgResponseTime = todayStatsData.avgResponseTime
    todayStats.onlineTime = todayStatsData.onlineTime
    
    // 如果有当前客服的ID，获取客服详细统计
    if (authStore.user) {
      try {
        const staffStats = await ChatStatsApi.getStaffStats(authStore.user.id)
        console.log('客服统计:', staffStats)
      } catch (error) {
        console.error('获取客服统计失败:', error)
      }
    }
  } catch (error) {
    console.error('加载聊天统计数据失败:', error)
    // 出错时使用默认值
    stats.onlineVisitors = visitors.value.filter(v => v.isOnline).length
    stats.todayConversations = 0
    stats.unreadMessages = 0
    
    todayStats.servedVisitors = 0
    todayStats.sentMessages = 0
    todayStats.avgResponseTime = '0分钟'
    todayStats.onlineTime = '0小时'
  }
}

/**
 * 选择访客
 */
const selectVisitor = (visitor: Visitor) => {
  selectedVisitor.value = visitor
  visitor.unreadCount = 0
  nextTick(() => {
    scrollToBottom()
  })
}

/**
 * 发送消息
 */
const sendMessage = async () => {
  if (!messageText.value.trim() || !selectedVisitor.value || !authStore.user) return

  try {
    // 创建本地消息对象
    const newMessage: ChatMessage = {
      id: `m${Date.now()}`,
      content: messageText.value,
      isFromStaff: true,
      timestamp: new Date().toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' }),
      visitorId: selectedVisitor.value.id,
      userId: authStore.user.id,
      userName: authStore.user.realName || authStore.user.username
    }

    // 添加到本地消息列表
    messages.value.push(newMessage)
    
    // 增加访客的消息计数
    const visitorIndex = visitors.value.findIndex(v => v.id === selectedVisitor.value!.id)
    if (visitorIndex > -1) {
      visitors.value[visitorIndex].messageCount++
    }
    
    // 清空输入框
    messageText.value = ''
    
    // 滚动到底部
    nextTick(() => {
      scrollToBottom()
    })

    // 发送消息到聊天服务
    await chatService.sendMessageToVisitor(
      selectedVisitor.value.connectionId,
      {
        senderId: authStore.user.id,
        senderName: authStore.user.realName || authStore.user.username,
        content: newMessage.content,
        type: 0
      }
    )
    
    // 更新统计数据
    todayStats.sentMessages++
  } catch (error) {
    console.error('发送消息失败:', error)
    ElMessage.error('发送消息失败，请重试')
  }
}

/**
 * 发送快捷回复
 */
const sendQuickReply = (content: string) => {
  messageText.value = content
  sendMessage()
}

/**
 * 滚动消息列表到底部
 */
const scrollToBottom = () => {
  if (messageListRef.value) {
    messageListRef.value.scrollTop = messageListRef.value.scrollHeight
  }
}

/**
 * 刷新数据
 */
const refreshData = async () => {
  try {
    // 刷新访客列表
    await chatService.getOnlineUsers()
    
    // 刷新统计数据
    await loadChatStats()
    
    ElMessage.success('数据已刷新')
  } catch (error) {
    console.error('刷新数据失败:', error)
    ElMessage.error('刷新数据失败，请重试')
  }
}

/**
 * 处理输入框输入事件，发送正在输入状态
 */
const handleInputTyping = () => {
  if (!isTyping.value && authStore.user && selectedVisitor.value) {
    isTyping.value = true
    chatService.userTyping(authStore.user.id, authStore.user.realName || authStore.user.username)
  }
  
  // 清除之前的定时器
  if (typingTimer.value) {
    clearTimeout(typingTimer.value)
  }
  
  // 设置新的定时器，2秒后停止输入状态
  typingTimer.value = window.setTimeout(() => {
    if (isTyping.value && authStore.user) {
      isTyping.value = false
      chatService.userStoppedTyping(authStore.user.id)
    }
  }, 2000)
}

// 结束对话
const endConversation = async () => {
  if (!selectedVisitor.value) return
  
  try {
    // 这里需要实现结束对话的逻辑
    ElMessage.success(`已结束与访客 ${selectedVisitor.value.name} 的对话`)
    
    // 更新访客状态
    const visitorIndex = visitors.value.findIndex(v => v.id === selectedVisitor.value!.id)
    if (visitorIndex > -1) {
      visitors.value[visitorIndex].isOnline = false
    }
    
    // 清除选中状态
    selectedVisitor.value = null
  } catch (error) {
    console.error('结束对话失败:', error)
    ElMessage.error('结束对话失败，请重试')
  }
}

// 监听输入框变化
watch(messageText, () => {
  handleInputTyping()
})

// 组件挂载时
onMounted(async () => {
  await initChatService()
  await loadChatStats()
})

// 组件卸载前
onBeforeUnmount(async () => {
  // 停止聊天连接
  await chatService.stop()
  
  // 清除定时器
  if (typingTimer.value) {
    clearTimeout(typingTimer.value)
  }
})
</script>

<style scoped>
.customer-service-workbench {
  height: calc(100vh - 140px);
  display: flex;
  flex-direction: column;
}

.workbench-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  background: white;
  border-radius: 8px;
  margin-bottom: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.header-stats {
  display: flex;
  gap: 40px;
}

.stat-item {
  display: flex;
  align-items: center;
  gap: 12px;
}

.stat-icon {
  font-size: 24px;
  color: #409eff;
}

.stat-info {
  display: flex;
  flex-direction: column;
}

.stat-number {
  font-size: 20px;
  font-weight: 600;
  color: #303133;
}

.stat-label {
  font-size: 12px;
  color: #909399;
}

.workbench-content {
  display: flex;
  gap: 20px;
  flex: 1;
  min-height: 0;
}

.visitor-panel {
  width: 280px;
  background: white;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
}

.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 20px;
  border-bottom: 1px solid #e4e7ed;
}

.panel-header h3 {
  margin: 0;
  font-size: 16px;
  color: #303133;
}

.visitor-list {
  flex: 1;
  overflow-y: auto;
}

.visitor-item {
  display: flex;
  align-items: center;
  padding: 16px 20px;
  cursor: pointer;
  border-bottom: 1px solid #f5f7fa;
  transition: background-color 0.3s;
  position: relative;
}

.visitor-item:hover {
  background-color: #f5f7fa;
}

.visitor-item.active {
  background-color: #e6f7ff;
  border-left: 3px solid #409eff;
}

.visitor-avatar {
  margin-right: 12px;
}

.visitor-info {
  flex: 1;
}

.visitor-name {
  font-size: 14px;
  font-weight: 500;
  color: #303133;
  margin-bottom: 4px;
}

.visitor-status {
  display: flex;
  align-items: center;
  gap: 8px;
}

.visitor-time {
  font-size: 12px;
  color: #909399;
}

.unread-badge {
  background: #f56c6c;
  color: white;
  border-radius: 10px;
  padding: 2px 6px;
  font-size: 12px;
  min-width: 18px;
  text-align: center;
}

.chat-panel {
  flex: 1;
  background: white;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
}

.chat-container {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 20px;
  border-bottom: 1px solid #e4e7ed;
}

.visitor-profile {
  display: flex;
  align-items: center;
  gap: 12px;
}

.profile-info {
  display: flex;
  flex-direction: column;
}

.visitor-name {
  font-size: 14px;
  font-weight: 500;
  color: #303133;
}

.visitor-location {
  font-size: 12px;
  color: #909399;
}

.message-list {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.message-item {
  display: flex;
  gap: 12px;
}

.message-item.staff {
  flex-direction: row-reverse;
}

.message-content {
  max-width: 70%;
}

.message-item.staff .message-content {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
}

.message-bubble {
  padding: 12px 16px;
  border-radius: 12px;
  font-size: 14px;
  line-height: 1.4;
}

.message-item.visitor .message-bubble {
  background: #f5f7fa;
  color: #303133;
}

.message-item.staff .message-bubble {
  background: #409eff;
  color: white;
}

.message-time {
  font-size: 12px;
  color: #909399;
  margin-top: 4px;
}

.quick-replies {
  padding: 16px 20px;
  border-top: 1px solid #f5f7fa;
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.message-input {
  padding: 20px;
  border-top: 1px solid #e4e7ed;
  display: flex;
  gap: 12px;
  align-items: flex-end;
}

.input-toolbar {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.message-input .el-textarea {
  flex: 1;
}

.no-visitor-selected {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
}

.tools-panel {
  width: 280px;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.visitor-details .detail-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 12px;
}

.detail-item .label {
  color: #909399;
  font-size: 14px;
}

.detail-item .value {
  color: #303133;
  font-size: 14px;
  font-weight: 500;
}

.stat-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.stat-cell {
  text-align: center;
  padding: 12px;
  background: #f8f9fa;
  border-radius: 6px;
}

.stat-value {
  font-size: 18px;
  font-weight: 600;
  color: #409eff;
  margin-bottom: 4px;
}

.stat-name {
  font-size: 12px;
  color: #909399;
}
</style> 