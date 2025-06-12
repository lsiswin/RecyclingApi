import * as signalR from '@microsoft/signalr'
  import { useAuthStore } from '@/stores/auth'
  import type { ChatMessage, Visitor, StaffInfo, UserInfo } from '@/types/chat'

  /**
   * 聊天服务API - 负责与ChatHub通信
   */
  export class ChatService {
    private connection: signalR.HubConnection | null = null
    private connectionPromise: Promise<void> | null = null
    private connectionUrl = import.meta.env.VITE_API_URL
    ? `${import.meta.env.VITE_API_URL.replace('http', 'ws')}/chathub` // 关键协议替换
    : 'ws://localhost:5279/chathub'; // 开发环境地址

    /**
     * 启动连接
     */
    async start(): Promise<void> {
      if (this.connection) {
        return this.connectionPromise || Promise.resolve()
      }

      try {
        const authStore = useAuthStore()

        console.log('正在连接ChatHub:', this.connectionUrl)

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.connectionUrl, {
              accessTokenFactory: () => authStore.token || '', // 从 Pinia 获取 Token
              skipNegotiation: true, // 跳过协商（仅 WebSocket 时可用）
              transport: signalR.HttpTransportType.WebSockets // 强制 WebSocket
            })
            .withAutomaticReconnect() // 自动重连
            .configureLogging(signalR.LogLevel.Information) // 调试日志
            .build();

        // 处理重连事件
        this.connection.onreconnecting((error) => {
          console.log('正在重新连接到聊天服务...', error)
        })

        this.connection.onreconnected((connectionId) => {
          console.log('已重新连接到聊天服务，连接ID:', connectionId)
          // 重新加入客服队列
          if (authStore.user) {
            this.joinStaffQueue(
              authStore.user.id,
              authStore.user.realName || authStore.user.username,
              authStore.user.department || '客服部'
            )
          }
        })

        this.connection.onclose((error) => {
          console.log('聊天连接已关闭', error)
          this.connection = null
          this.connectionPromise = null
        })

        // 添加连接错误处理
        this.connectionPromise = this.connection.start()
          .then(() => {
            console.log('ChatHub连接成功')
          })
          .catch(err => {
            console.error('ChatHub连接失败:', err)
            this.connection = null
            this.connectionPromise = null
            throw err
          })

        return this.connectionPromise
      } catch (error) {
        console.error('启动聊天连接失败:', error)
        this.connection = null
        this.connectionPromise = null
        throw error
      }
    }

    /**
     * 停止连接
     */
    async stop(): Promise<void> {
      if (this.connection) {
        try {
          await this.connection.stop()
          console.log('聊天连接已停止')
        } catch (error) {
          console.error('停止聊天连接失败:', error)
        } finally {
          this.connection = null
          this.connectionPromise = null
        }
      }
    }

    /**
     * 添加消息接收处理器
     */
    onReceiveMessage(callback: (message: any) => void): void {
      if (!this.connection) return
      this.connection.on('ReceiveMessage', callback)
    }

    /**
     * 添加访客加入处理器
     */
    onVisitorJoined(callback: (user: UserInfo) => void): void {
      if (!this.connection) return
      this.connection.on('UserJoined', callback)
    }

    /**
     * 添加访客断开处理器
     */
    onVisitorDisconnected(callback: (visitorId: string, visitorName: string) => void): void {
      if (!this.connection) return
      this.connection.on('VisitorDisconnected', callback)
    }

    /**
     * 添加访客分配处理器
     */
    onVisitorAssigned(callback: (visitor: any) => void): void {
      if (!this.connection) return
      this.connection.on('VisitorAssigned', callback)
    }

    /**
     * 添加访客重新分配处理器
     */
    onVisitorReassigned(callback: (visitorId: string, message: string) => void): void {
      if (!this.connection) return
      this.connection.on('VisitorReassigned', callback)
    }

    /**
     * 添加在线用户更新处理器
     */
    onUpdateOnlineUsers(callback: (users: UserInfo[]) => void): void {
      if (!this.connection) return
      this.connection.on('UpdateOnlineUsers', callback)
    }

    /**
     * 添加在线客服更新处理器
     */
    onUpdateOnlineStaff(callback: (staff: StaffInfo[]) => void): void {
      if (!this.connection) return
      this.connection.on('UpdateOnlineStaff', callback)
    }

    /**
     * 加入客服队列
     */
    async joinStaffQueue(staffId: string, staffName: string, department?: string): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('JoinStaffQueue', staffId, staffName, department || '客服部')
    }

    /**
     * 向访客发送消息
     */
    async sendMessageToVisitor(
      visitorConnectionId: string,
      messageData: {
        senderId: string
        senderName: string
        content: string
        sessionId?: string
        type?: number
      }
    ): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('SendMessageToVisitor', visitorConnectionId, {
        senderId: messageData.senderId,
        senderName: messageData.senderName,
        content: messageData.content,
        sessionId: messageData.sessionId || '',
        type: messageData.type || 0
      })
    }

    /**
     * 标记客服为忙碌状态
     */
    async markStaffBusy(staffId: string): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('MarkStaffBusy', staffId)
    }

    /**
     * 标记客服为可用状态
     */
    async markStaffAvailable(staffId: string): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('MarkStaffAvailable', staffId)
    }

    /**
     * 获取在线访客列表
     */
    async getOnlineUsers(): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('GetOnlineUsers')
    }

    /**
     * 获取在线客服列表
     */
    async getOnlineStaff(): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('GetOnlineStaff')
    }

    /**
     * 用户正在输入
     */
    async userTyping(userId: string, userName: string): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('UserTyping', userId, userName)
    }

    /**
     * 用户停止输入
     */
    async userStoppedTyping(userId: string): Promise<void> {
      if (!this.connection) await this.start()
      await this.connection!.invoke('UserStoppedTyping', userId)
    }
  }

  // 导出聊天服务实例
  export const chatService = new ChatService()