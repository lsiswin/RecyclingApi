import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { User } from '@/types/user'
import { UserType } from '@/types/user'
import { AuthApi } from '@/api/auth'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(localStorage.getItem('token'))
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'))
  const loading = ref(false)

  const isAuthenticated = computed(() => !!token.value && !!user.value)

  /**
   * 模拟登录（开发模式回退）
   */
  const mockLogin = async (username: string, password: string) => {
    // 模拟登录API调用
    if ((username === 'admin' && password === 'Admin123!') || 
        (username === 'staff' && password === 'Staff123!')) {
      const userData: User = {
        id: username === 'admin' ? '1' : '2',
        username,
        realName: username === 'admin' ? '系统管理员' : '客服员工',
        userType: username === 'admin' ? UserType.Admin : UserType.Staff,
        email: `${username}@recycling.com`,
        isActive: true,
        createdAt: new Date().toISOString()
      }
      
      user.value = userData
      token.value = `mock-token-${username}`
      refreshToken.value = `mock-refresh-token-${username}`
      localStorage.setItem('token', token.value)
      localStorage.setItem('refreshToken', refreshToken.value)
      localStorage.setItem('user', JSON.stringify(userData))
      
      return { success: true, message: '登录成功（开发模式）' }
    } else {
      return { success: false, message: '用户名或密码错误' }
    }
  }

  /**
   * 用户登录
   */
  const login = async (username: string, password: string) => {
    loading.value = true
    try {
      // 首先尝试真实API
      const result = await AuthApi.login({ username, password })      
      if (result.success && result.data) {
        user.value = result.data.user
        token.value = result.data.accessToken
        refreshToken.value = result.data.refreshToken || result.data.accessToken
        
        // 保存到本地存储
        localStorage.setItem('token', result.data.accessToken)
        if (result.data.refreshToken) {
          localStorage.setItem('refreshToken', result.data.refreshToken)
        }
        localStorage.setItem('user', JSON.stringify(result.data.user))
        
        return { success: true, message: result.message }
      } else {
        return { success: false, message: result.message }
      }
    } catch (error: any) {
      // 如果API连接失败，使用模拟登录
      console.warn('API连接失败，使用模拟登录:', error.message)
      return await mockLogin(username, password)
    } finally {
      loading.value = false
    }
  }

  /**
   * 用户注册
   */
  const register = async (registerData: {
    username: string
    password: string
    confirmPassword: string
    email: string
    phoneNumber: string
    realName?: string
    userType?: string
  }) => {
    loading.value = true
    try {
      const result = await AuthApi.register(registerData)
      return result
    } catch (error) {
      // 模拟注册成功
      console.warn('API连接失败，使用模拟注册')
      return { success: true, message: '注册成功（开发模式）' }
    } finally {
      loading.value = false
    }
  }

  /**
   * 刷新令牌
   */
  const refreshTokens = async () => {
    if (!refreshToken.value) {
      return { success: false, message: '刷新令牌不存在' }
    }

    try {
      const result = await AuthApi.refreshToken({ refreshToken: refreshToken.value })
      
      if (result.success && result.data) {
        token.value = result.data
        localStorage.setItem('token', result.data)
        
        return { success: true, message: result.message }
      } else {
        // 刷新失败，清除认证信息
        logout()
        return { success: false, message: result.message }
      }
    } catch (error) {
      // 开发模式下保持登录状态
      console.warn('刷新令牌失败，开发模式下保持登录状态')
      return { success: true, message: '令牌刷新成功（开发模式）' }
    }
  }

  /**
   * 验证令牌
   */
  const verifyToken = async () => {
    if (!token.value) {
      return { success: false, message: '令牌不存在' }
    }

    try {
      const result = await AuthApi.verifyToken()
      
      if (result.success && result.data) {
        user.value = result.data
        localStorage.setItem('user', JSON.stringify(result.data))
        return { success: true, message: result.message }
      } else {
        // 验证失败，清除认证信息
        logout()
        return { success: false, message: result.message }
      }
    } catch (error) {
      // 开发模式下，如果本地有用户信息就认为验证成功
      const savedUser = localStorage.getItem('user')
      if (savedUser) {
        try {
          user.value = JSON.parse(savedUser)
          return { success: true, message: '令牌验证成功（开发模式）' }
        } catch {
          logout()
          return { success: false, message: '令牌验证失败' }
        }
      } else {
        logout()
        return { success: false, message: '令牌验证失败' }
      }
    }
  }

  /**
   * 获取当前用户信息
   */
  const getCurrentUser = async () => {
    try {
      const result = await AuthApi.getCurrentUser()
      
      if (result.success && result.data) {
        user.value = result.data
        localStorage.setItem('user', JSON.stringify(result.data))
        return { success: true, message: result.message, user: result.data }
      } else {
        return { success: false, message: result.message }
      }
    } catch (error) {
      // 开发模式下返回当前用户
      if (user.value) {
        return { success: true, message: '获取用户信息成功（开发模式）', user: user.value }
      } else {
        return { success: false, message: '获取用户信息失败' }
      }
    }
  }

  /**
   * 用户登出
   */
  const logout = async () => {
    try {
      // 调用后端登出接口
      await AuthApi.logout()
    } catch (error) {
      // 即使后端登出失败，也要清除本地数据
      console.warn('后端登出失败:', error)
    } finally {
      // 清除本地存储
      user.value = null
      token.value = null
      refreshToken.value = null
      localStorage.removeItem('token')
      localStorage.removeItem('refreshToken')
      localStorage.removeItem('user')
    }
  }

  /**
   * 初始化认证状态
   */
  const initAuth = async () => {
    const savedToken = localStorage.getItem('token')
    const savedUser = localStorage.getItem('user')
    
    if (savedToken) {
      token.value = savedToken
      
      if (savedUser) {
        try {
          user.value = JSON.parse(savedUser)
        } catch {
          user.value = null
        }
      }
      
      // 验证令牌
      await verifyToken()
    }
  }

  return {
    user,
    token,
    refreshToken,
    loading,
    isAuthenticated,
    login,
    register,
    refreshTokens,
    verifyToken,
    getCurrentUser,
    logout,
    initAuth
  }
}) 