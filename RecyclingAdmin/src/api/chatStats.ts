import { request } from '@/utils/request'
import type { ChatStats } from '@/types/chat'

/**
 * 聊天统计API - 与ChatStatisticsService对接
 */
export const ChatStatsApi = {
  /**
   * 获取实时统计数据
   */
  async getRealTimeStats(): Promise<{
    onlineUsers: number
    onlineStaff: number
    activeChats: number
    queuedVisitors: number
    averageWaitTime: number
  }> {
    try {
      const response = await request.get('/api/chat-stats/realtime')
      return response.data.data || response.data
    } catch (error) {
      console.error('获取实时统计数据失败:', error)
      // 返回模拟数据
      return {
        onlineUsers: 5,
        onlineStaff: 3,
        activeChats: 4,
        queuedVisitors: 1,
        averageWaitTime: 2.5
      }
    }
  },

  /**
   * 获取聊天统计概览数据
   */
  async getStatsOverview(): Promise<ChatStats> {
    try {
      const response = await request.get('/api/chat-stats/overview')
      return response.data.data || response.data
    } catch (error) {
      console.error('获取聊天统计概览失败:', error)
      // 返回模拟数据
      return {
        onlineVisitors: 5,
        todayConversations: 28,
        unreadMessages: 3,
        servedVisitors: 15,
        sentMessages: 89,
        avgResponseTime: '2.3分钟',
        onlineTime: '6小时32分钟'
      }
    }
  },

  /**
   * 获取今日统计数据
   */
  async getTodayStats(): Promise<{
    servedVisitors: number
    sentMessages: number
    avgResponseTime: string
    onlineTime: string
    totalSessions: number
    avgSessionDuration: string
  }> {
    try {
      const response = await request.get('/api/chat-stats/today')
      return response.data.data || response.data
    } catch (error) {
      console.error('获取今日统计数据失败:', error)
      // 返回模拟数据
      return {
        servedVisitors: 15,
        sentMessages: 89,
        avgResponseTime: '2.3分钟',
        onlineTime: '6小时32分钟',
        totalSessions: 18,
        avgSessionDuration: '12分钟'
      }
    }
  },

  /**
   * 获取客服统计数据
   */
  async getStaffStats(staffId: string, startDate?: string, endDate?: string): Promise<{
    totalSessions: number
    totalMessages: number
    avgResponseTime: string
    onlineHours: number
    utilizationRate: number
    satisfactionScore: number
  }> {
    try {
      const params = { staffId }
      if (startDate) Object.assign(params, { startDate })
      if (endDate) Object.assign(params, { endDate })
      
      const response = await request.get('/api/chat-stats/staff', { params })
      return response.data.data || response.data
    } catch (error) {
      console.error('获取客服统计数据失败:', error)
      // 返回模拟数据
      return {
        totalSessions: 32,
        totalMessages: 210,
        avgResponseTime: '1.8分钟',
        onlineHours: 6.5,
        utilizationRate: 85,
        satisfactionScore: 4.7
      }
    }
  },

  /**
   * 获取历史聊天记录
   */
  async getChatHistory(visitorId: string, limit: number = 50): Promise<any[]> {
    try {
      const response = await request.get(`/api/chat/history/${visitorId}`, {
        params: { limit }
      })
      return response.data.data || response.data || []
    } catch (error) {
      console.error('获取聊天历史记录失败:', error)
      return [] // 返回空数组
    }
  },

  /**
   * 获取未读消息数量
   */
  async getUnreadCount(): Promise<number> {
    try {
      const response = await request.get('/api/chat-stats/unread')
      const data = response.data.data || response.data
      return data.count || data || 0
    } catch (error) {
      console.error('获取未读消息数量失败:', error)
      return 3 // 返回模拟数据
    }
  }
}