import { defineStore } from 'pinia'
import { AuthApi } from '@/api/auth'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    token: localStorage.getItem('auth_token'),
    refreshToken: localStorage.getItem('refresh_token'),
    isLoggedIn: false
  }),

  getters: {
    username: (state) => state.user?.username || '',
    userRole: (state) => state.user?.role || 'guest',
    realName: (state) => state.user?.realName || state.user?.username || '',
    avatar: (state) => state.user?.avatar || ''
  },

  actions: {
    async login(credentials) {
      try {
        const response = await AuthApi.login(credentials)
        
        if (response.success) {
          const { data } = response
          this.user = data.user
          this.token = data.token
          this.refreshToken = data.refreshToken
          this.isLoggedIn = true
          
          localStorage.setItem('auth_token', data.token)
          if (data.refreshToken) {
            localStorage.setItem('refresh_token', data.refreshToken)
          }
          
          return { success: true, message: response.message }
        } else {
          return { success: false, message: response.message || '登录失败' }
        }
      } catch (error) {
        return { success: false, message: error.message || '登录失败，请检查网络连接' }
      }
    },

    async register(registerData) {
      try {
        const response = await AuthApi.register(registerData)
        return { 
          success: response.success, 
          message: response.message 
        }
      } catch (error) {
        return { success: false, message: error.message || '注册失败，请检查网络连接' }
      }
    },

    async checkAuth() {
      if (!this.token) return false
      
      try {
        const response = await AuthApi.verifyToken()
        
        if (response.success) {
          this.user = response.data
          this.isLoggedIn = true
          return true
        } else {
          // 如果验证失败，尝试使用刷新令牌
          if (this.refreshToken) {
            return await this.refreshUserToken()
          } else {
            this.logout()
            return false
          }
        }
      } catch (error) {
        // 如果验证请求失败，尝试使用刷新令牌
        if (this.refreshToken) {
          return await this.refreshUserToken()
        } else {
          this.logout()
          return false
        }
      }
    },
    
    async refreshUserToken() {
      try {
        const response = await AuthApi.refreshToken(this.refreshToken)
        
        if (response.success) {
          this.token = response.data.token
          localStorage.setItem('auth_token', response.data.token)
          
          if (response.data.refreshToken) {
            this.refreshToken = response.data.refreshToken
            localStorage.setItem('refresh_token', response.data.refreshToken)
          }
          
          return true
        } else {
          this.logout()
          return false
        }
      } catch (error) {
        this.logout()
        return false
      }
    },

    async logout() {
      // 尝试调用登出API，但不管结果如何都清除本地状态
      try {
        if (this.token) {
          await AuthApi.logout()
        }
      } catch (error) {
        console.error('登出API调用失败:', error)
      } finally {
        this.user = null
        this.token = null
        this.refreshToken = null
        this.isLoggedIn = false
        localStorage.removeItem('auth_token')
        localStorage.removeItem('refresh_token')
      }
    },
    
    async forgotPassword(email) {
      try {
        const response = await AuthApi.forgotPassword({ email })
        return { 
          success: response.success, 
          message: response.message 
        }
      } catch (error) {
        return { success: false, message: error.message || '发送重置密码请求失败' }
      }
    },
    
    async resetPassword(resetData) {
      try {
        const response = await AuthApi.resetPassword(resetData)
        return { 
          success: response.success, 
          message: response.message 
        }
      } catch (error) {
        return { success: false, message: error.message || '重置密码失败' }
      }
    },
    
    async sendVerificationCode(email) {
      try {
        const response = await AuthApi.sendVerificationCode(email)
        return { 
          success: response.success, 
          message: response.message 
        }
      } catch (error) {
        return { success: false, message: error.message || '发送验证码失败' }
      }
    }
  }
})