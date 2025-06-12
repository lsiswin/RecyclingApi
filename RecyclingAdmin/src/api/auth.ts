import { request } from '@/utils/request'
import type { User, LoginRequest, LoginResponse, ApiResponse } from '@/types/user'

/**
 * 认证相关的API接口类
 */
export class AuthApi {
  /**
   * 用户登录
   */
  static async login(loginData: LoginRequest): Promise<ApiResponse<LoginResponse>> {
    try {
      const response = await request.post('/api/auth/login', loginData)
      console.log("登录响应",response.data);      
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '登录失败',
        data: undefined
      }
    }
  }

  /**
   * 用户注册
   */
  static async register(registerData: {
    username: string
    password: string
    confirmPassword: string
    email: string
    phoneNumber: string
    realName?: string
    userType?: string
  }): Promise<ApiResponse<User>> {
    try {
      const response = await request.post('/api/auth/register', registerData)
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '注册失败',
        data: undefined
      }
    }
  }

  /**
   * 刷新令牌
   */
  static async refreshToken(data: { refreshToken: string }): Promise<ApiResponse<string>> {
    try {
      const response = await request.post('/api/auth/refresh-token', data)
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '刷新令牌失败',
        data: undefined
      }
    }
  }

  /**
   * 验证令牌
   */
  static async verifyToken(): Promise<ApiResponse<User>> {
    try {
      const response = await request.get('/api/auth/verify')
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '令牌验证失败',
        data: undefined
      }
    }
  }

  /**
   * 获取当前用户信息
   */
  static async getCurrentUser(): Promise<ApiResponse<User>> {
    try {
      const response = await request.get('/api/auth/me')
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '获取用户信息失败',
        data: undefined
      }
    }
  }

  /**
   * 用户登出
   */
  static async logout(): Promise<ApiResponse<void>> {
    try {
      const response = await request.post('/api/auth/logout')
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '登出失败',
        data: undefined
      }
    }
  }

  /**
   * 忘记密码
   */
  static async forgotPassword(data: { email: string }): Promise<ApiResponse<void>> {
    try {
      const response = await request.post('/api/auth/forgot-password', data)
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '发送重置邮件失败',
        data: undefined
      }
    }
  }

  /**
   * 重置密码
   */
  static async resetPassword(resetData: {
    token: string
    email: string
    newPassword: string
    confirmPassword: string
  }): Promise<ApiResponse<void>> {
    try {
      const response = await request.post('/api/auth/reset-password', resetData)
      return response.data
    } catch (error: any) {
      return {
        success: false,
        message: error.message || '重置密码失败',
        data: undefined
      }
    }
  }
} 