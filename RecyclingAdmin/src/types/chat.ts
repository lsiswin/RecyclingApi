/**
 * 聊天消息类型
 */
export interface ChatMessage {
  id: string
  messageId?: string
  sessionId?: string
  content: string
  isFromStaff: boolean
  timestamp: string
  visitorId: string
  userId: string
  userName: string
  messageType?: string
  metadata?: Record<string, any>
}

/**
 * 访客类型
 */
export interface Visitor {
  id: string
  userId?: string
  name: string
  userName?: string
  isOnline: boolean
  lastActiveTime: string
  location: string
  connectTime: string
  duration: string
  messageCount: number
  unreadCount: number
  connectionId: string
  avatar?: string
  sessionId?: string
  userType?: number
  joinTime?: string
  metadata?: Record<string, any>
}

/**
 * 聊天用户信息接口（用于接收后端数据）
 */
export interface UserInfo {
  userId: string
  userName: string
  avatar: string
  connectionId: string
  userType: number | string
  joinTime: string
  sessionId?: string
  metadata?: Record<string, any>
}

/**
 * 客服信息类型
 */
export interface StaffInfo {
  staffId: string
  staffName: string
  avatar?: string
  connectionId: string
  department: string
  status: StaffStatus
  joinTime: string
  maxConcurrentChats: number
  metadata?: Record<string, any>
}

/**
 * 客服状态枚举
 */
export enum StaffStatus {
  Online = 0,
  Busy = 1,
  Away = 2,
  Offline = 3
}

/**
 * 聊天会话类型
 */
export interface ChatSession {
  sessionId: string
  visitorId: string
  staffId: string
  startTime: string
  endTime?: string
  status: ChatSessionStatus
  summary?: string
  metadata?: Record<string, any>
}

/**
 * 聊天会话状态枚举
 */
export enum ChatSessionStatus {
  Active = 0,
  Waiting = 1,
  Ended = 2,
  Transferred = 3
}

/**
 * 快捷回复类型
 */
export interface QuickReply {
  id: string
  title: string
  content: string
  category?: string
}

/**
 * 统计数据类型
 */
export interface ChatStats {
  onlineVisitors: number
  todayConversations: number
  unreadMessages: number
  servedVisitors: number
  sentMessages: number
  avgResponseTime: string
  onlineTime: string
}