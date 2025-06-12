import * as signalR from '@microsoft/signalr'

class ChatService {
  constructor() {
    this.connection = null
    this.isConnected = false
    this.userType = 'visitor' // 'visitor' 或 'staff'
    this.callbacks = {
      onMessage: [],
      onUserJoined: [],
      onUserLeft: [],
      onUsersUpdate: [],
      onConnectionChange: [],
      // 新增客服系统回调
      onVisitorMessage: [],
      onStaffMessage: [],
      onVisitorAssigned: [],
      onVisitorDisconnected: [],
      onStaffJoined: [],
      onStaffDisconnected: [],
      onStaffStatusChanged: [],
      onStaffListUpdate: [],
      onConnectedToStaff: [],
      onVisitorReassigned: []
    }
    this.currentUser = this.loadUserInfo()
    this.chatHistory = this.loadChatHistory()
  }

  // 加载用户信息
  loadUserInfo() {
    try {
      const saved = localStorage.getItem('chat_user_info')
      if (saved) {
        return JSON.parse(saved)
      }
    } catch (error) {
      console.error('加载用户信息失败:', error)
    }
    
    // 生成新用户信息
    return {
      userId: `visitor_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`,
      userName: `访客${Math.floor(Math.random() * 1000)}`,
      avatar: '/default-avatar.png',
      visitCount: 1,
      firstVisit: new Date().toISOString(),
      pagesVisited: []
    }
  }

  // 保存用户信息
  saveUserInfo(userInfo) {
    try {
      localStorage.setItem('chat_user_info', JSON.stringify(userInfo))
      this.currentUser = userInfo
    } catch (error) {
      console.error('保存用户信息失败:', error)
    }
  }

  // 加载聊天历史
  loadChatHistory() {
    try {
      const saved = localStorage.getItem('chat_history')
      return saved ? JSON.parse(saved) : []
    } catch (error) {
      console.error('加载聊天历史失败:', error)
      return []
    }
  }

  // 保存聊天历史
  saveChatHistory() {
    try {
      localStorage.setItem('chat_history', JSON.stringify(this.chatHistory))
    } catch (error) {
      console.error('保存聊天历史失败:', error)
    }
  }

  // 添加消息到历史记录
  addMessageToHistory(message) {
    if (!message || !message.content) return
    
    this.chatHistory.push(message)
    
    // 限制历史记录数量（最多保存100条）
    if (this.chatHistory.length > 100) {
      this.chatHistory = this.chatHistory.slice(-100)
    }
    
    this.saveChatHistory()
  }

  // 清除聊天数据
  clearChatData() {
    this.chatHistory = []
    localStorage.removeItem('chat_history')
    localStorage.removeItem('chat_user_info')
    localStorage.removeItem('chat_auto_popup_shown')
  }

  // 连接到聊天服务器
  async connect(userType = 'visitor') {
    if (this.isConnected || this.connection) {
      return true
    }

    try {
      this.userType = userType
      const hubUrl = import.meta.env.VITE_API_URL ? 
        `${import.meta.env.VITE_API_URL}/chathub` : 
        'http://localhost:5279/chathub'
      
      console.log('正在连接ChatHub:', hubUrl)
      
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl, {
          skipNegotiation: false,
          transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
        })
        .withAutomaticReconnect([0, 2000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Information)
        .build()

      this.setupEventListeners()
      
      await this.connection.start()
      console.log('ChatHub连接成功')
      
      if (userType === 'visitor') {
        // 访客加入聊天室
        await this.connection.invoke('JoinChat', 
          this.currentUser.userId, 
          this.currentUser.userName, 
          this.currentUser.avatar
        )
        console.log('访客已加入聊天室')
      } else if (userType === 'staff') {
        // 客服加入队列
        await this.connection.invoke('JoinStaffQueue',
          this.currentUser.staffId || this.currentUser.userId,
          this.currentUser.staffName || this.currentUser.userName,
          this.currentUser.department,
          this.currentUser.skills
        )
        console.log('客服已加入队列')
      }
      
      this.isConnected = true
      this.notifyCallbacks('onConnectionChange', true)
      
      return true
    } catch (error) {
      console.error('连接聊天服务器失败:', error)
      this.notifyCallbacks('onConnectionChange', false)
      return false
    }
  }

  // 断开连接
  async disconnect() {
    if (this.connection) {
      try {
        await this.connection.stop()
        console.log('ChatHub连接已断开')
      } catch (error) {
        console.error('断开连接失败:', error)
      }
      this.connection = null
    }
    this.isConnected = false
    this.notifyCallbacks('onConnectionChange', false)
  }

  // 设置事件监听器
  setupEventListeners() {
    if (!this.connection) return

    // 通用消息接收（保持向后兼容）
    this.connection.on('ReceiveMessage', (message) => {
      this.addMessageToHistory(message)
      this.notifyCallbacks('onMessage', message)
    })

    // 接收私聊消息（保持向后兼容）
    this.connection.on('ReceivePrivateMessage', (message) => {
      this.addMessageToHistory({ ...message, isPrivate: true })
      this.notifyCallbacks('onMessage', { ...message, isPrivate: true })
    })

    // 新增：访客接收客服消息
    this.connection.on('ReceiveStaffMessage', (message) => {
      this.addMessageToHistory(message)
      this.notifyCallbacks('onStaffMessage', message)
      this.notifyCallbacks('onMessage', message) // 也触发通用回调
    })

    // 新增：客服接收访客消息
    this.connection.on('ReceiveVisitorMessage', (message) => {
      this.addMessageToHistory(message)
      this.notifyCallbacks('onVisitorMessage', message)
      this.notifyCallbacks('onMessage', message) // 也触发通用回调
    })

    // 新增：客服收到新访客分配
    this.connection.on('NewVisitorAssigned', (visitorInfo) => {
      this.addMessageToHistory(visitorInfo.message)
      this.notifyCallbacks('onVisitorAssigned', visitorInfo)
    })

    // 新增：访客连接到客服
    this.connection.on('ConnectedToStaff', (staffName) => {
      this.notifyCallbacks('onConnectedToStaff', staffName)
    })

    // 新增：访客被重新分配
    this.connection.on('VisitorReassigned', (visitorId, message) => {
      this.notifyCallbacks('onVisitorReassigned', { visitorId, message })
    })

    // 新增：访客断开连接（客服端接收）
    this.connection.on('VisitorDisconnected', (visitorId, visitorName) => {
      this.notifyCallbacks('onVisitorDisconnected', { visitorId, visitorName })
    })

    // 新增：客服上线
    this.connection.on('StaffJoined', (staff) => {
      this.notifyCallbacks('onStaffJoined', staff)
    })

    // 新增：客服离线
    this.connection.on('StaffDisconnected', (staff) => {
      this.notifyCallbacks('onStaffDisconnected', staff)
    })

    // 新增：客服状态变化
    this.connection.on('StaffStatusChanged', (staffId, status) => {
      this.notifyCallbacks('onStaffStatusChanged', { staffId, status })
    })

    // 新增：客服列表更新
    this.connection.on('UpdateStaffList', (staffList) => {
      this.notifyCallbacks('onStaffListUpdate', staffList)
    })

    // 用户加入
    this.connection.on('UserJoined', (user) => {
      this.notifyCallbacks('onUserJoined', user)
    })

    // 用户离开
    this.connection.on('UserDisconnected', (user) => {
      this.notifyCallbacks('onUserLeft', user)
    })

    // 更新在线用户列表
    this.connection.on('UpdateOnlineUsers', (users) => {
      this.notifyCallbacks('onUsersUpdate', users)
    })

    // 消息错误
    this.connection.on('MessageError', (error) => {
      console.error('消息错误:', error)
    })

    // 连接重新建立
    this.connection.onreconnected(() => {
      this.isConnected = true
      this.notifyCallbacks('onConnectionChange', true)
      
      // 根据用户类型重新加入
      if (this.userType === 'visitor') {
        this.connection.invoke('JoinChat', 
          this.currentUser.userId, 
          this.currentUser.userName, 
          this.currentUser.avatar
        )
      } else if (this.userType === 'staff') {
        this.connection.invoke('JoinStaffQueue',
          this.currentUser.staffId || this.currentUser.userId,
          this.currentUser.staffName || this.currentUser.userName,
          this.currentUser.department,
          this.currentUser.skills
        )
      }
    })

    // 连接断开
    this.connection.onclose(() => {
      this.isConnected = false
      this.notifyCallbacks('onConnectionChange', false)
    })
  }

  // 访客发送消息给客服
  async sendMessageToStaff(content) {
    if (!this.isConnected || !this.connection) {
      throw new Error('未连接到聊天服务器')
    }

    const messageData = {
      senderId: this.currentUser.userId,
      senderName: this.currentUser.userName,
      senderAvatar: this.currentUser.avatar,
      content: content.trim(),
      type: 0 // MessageType.Text = 0
    }

    await this.connection.invoke('SendMessageToStaff', this.currentUser.userId, messageData)
  }

  // 客服发送消息给访客
  async sendMessageToVisitor(visitorConnectionId, content) {
    if (!this.isConnected || !this.connection) {
      throw new Error('未连接到聊天服务器')
    }

    const messageData = {
      senderId: this.currentUser.staffId || this.currentUser.userId,
      senderName: this.currentUser.staffName || this.currentUser.userName,
      senderAvatar: this.currentUser.avatar,
      content: content.trim(),
      type: 0 // MessageType.Text = 0
    }

    await this.connection.invoke('SendMessageToVisitor', visitorConnectionId, messageData)
  }

  // 客服标记为忙碌
  async markStaffBusy() {
    if (this.isConnected && this.connection && this.userType === 'staff') {
      await this.connection.invoke('MarkStaffBusy', this.currentUser.staffId || this.currentUser.userId)
    }
  }

  // 客服标记为可用
  async markStaffAvailable() {
    if (this.isConnected && this.connection && this.userType === 'staff') {
      await this.connection.invoke('MarkStaffAvailable', this.currentUser.staffId || this.currentUser.userId)
    }
  }

  // 手动分配访客给客服
  async assignVisitorToStaff(visitorConnectionId, staffId) {
    if (this.isConnected && this.connection) {
      await this.connection.invoke('AssignVisitorToStaff', visitorConnectionId, staffId)
    }
  }

  // 获取在线客服列表
  async getOnlineStaff() {
    if (this.isConnected && this.connection) {
      await this.connection.invoke('GetOnlineStaff')
    }
  }

  // 发送消息（保持向后兼容）
  async sendMessage(content) {
    if (!this.isConnected || !this.connection) {
      throw new Error('未连接到聊天服务器')
    }

    if (this.userType === 'visitor') {
      // 访客发送给客服
      await this.sendMessageToStaff(content)
    } else {
      // 其他情况使用原有方法
      const messageData = {
        senderId: this.currentUser.userId,
        senderName: this.currentUser.userName,
        senderAvatar: this.currentUser.avatar,
        content: content.trim(),
        type: 0 // MessageType.Text = 0
      }

      await this.connection.invoke('SendMessageToAll', messageData)
    }
  }

  // 发送私聊消息
  async sendPrivateMessage(targetUserId, content) {
    if (!this.isConnected || !this.connection) {
      throw new Error('未连接到聊天服务器')
    }

    const messageData = {
      senderId: this.currentUser.userId,
      senderName: this.currentUser.userName,
      senderAvatar: this.currentUser.avatar,
      content: content.trim(),
      type: 0 // MessageType.Text = 0
    }

    await this.connection.invoke('SendPrivateMessage', targetUserId, messageData)
  }

  // 用户正在输入
  async userTyping() {
    if (this.isConnected && this.connection) {
      await this.connection.invoke('UserTyping', this.currentUser.userId, this.currentUser.userName)
    }
  }

  // 用户停止输入
  async userStoppedTyping() {
    if (this.isConnected && this.connection) {
      await this.connection.invoke('UserStoppedTyping', this.currentUser.userId)
    }
  }

  // 获取在线用户
  async getOnlineUsers() {
    if (this.isConnected && this.connection) {
      await this.connection.invoke('GetOnlineUsers')
    }
  }

  // 注册回调函数
  on(event, callback) {
    if (this.callbacks[event]) {
      this.callbacks[event].push(callback)
    }
  }

  // 移除回调函数
  off(event, callback) {
    if (this.callbacks[event]) {
      const index = this.callbacks[event].indexOf(callback)
      if (index > -1) {
        this.callbacks[event].splice(index, 1)
      }
    }
  }

  // 通知回调函数
  notifyCallbacks(event, data) {
    if (this.callbacks[event]) {
      this.callbacks[event].forEach(callback => {
        try {
          callback(data)
        } catch (error) {
          console.error(`回调函数执行错误 (${event}):`, error)
        }
      })
    }
  }

  // 获取当前用户信息
  getCurrentUser() {
    return this.currentUser
  }

  // 更新用户信息
  updateUserInfo(updates) {
    this.currentUser = { ...this.currentUser, ...updates }
    this.saveUserInfo(this.currentUser)
  }

  // 获取聊天历史
  getChatHistory() {
    return this.chatHistory
  }

  // 获取连接状态
  getConnectionStatus() {
    return this.isConnected
  }

  // 检查是否首次访问
  isFirstVisit() {
    return !localStorage.getItem('chatHasVisited')
  }

  // 记录页面访问
  recordPageVisit(pageName) {
    localStorage.setItem('chatHasVisited', 'true')
    // 可以添加更多逻辑记录具体页面访问等
  }

  // 获取用户类型
  getUserType() {
    return this.userType
  }

  // 设置用户类型
  setUserType(userType) {
    this.userType = userType
    if (this.currentUser) {
      this.currentUser.userType = userType
      this.saveUserInfo(this.currentUser)
    }
  }
}

// 创建单例实例
const chatService = new ChatService()

export default chatService 