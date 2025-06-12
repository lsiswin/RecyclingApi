import axios from 'axios'
import { useAuthStore } from '@/stores/auth'
import type { ApiResponse } from '@/types/user'

// 创建axios实例
export const request = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5279',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
})

// 请求拦截器
request.interceptors.request.use(
  config => {
    const authStore = useAuthStore()
    if (authStore.token) {
      config.headers['Authorization'] = `Bearer ${authStore.token}`
    }
    return config
  },
  error => {
    console.error('请求错误:', error)
    return Promise.reject(error)
  }
)

// 响应拦截器
request.interceptors.response.use(
  response => {    
    console.log("response.data",response.data);
    // 直接返回后端API响应对象，但保持axios响应结构
    return {
      ...response, 
      data: response.data as ApiResponse
    }
         
  },
  async error => {
    const authStore = useAuthStore()
    
    if (error.response) {
      const { status } = error.response
      
      // 如果是401未授权，尝试刷新token
      if (status === 401) {
        try {
          // 尝试刷新令牌
          const refreshResult = await authStore.refreshTokens()
          
          if (refreshResult.success) {
            // 重新发起请求
            const originalRequest = error.config
            originalRequest.headers['Authorization'] = `Bearer ${authStore.token}`
            return request(originalRequest)
          } else {
            // 刷新失败，退出登录
            authStore.logout()
            window.location.href = '/login'
            return Promise.reject(new Error('登录已过期，请重新登录'))
          }
        } catch (refreshError) {
          // 刷新失败，退出登录
          authStore.logout()
          window.location.href = '/login'
          return Promise.reject(new Error('登录已过期，请重新登录'))
        }
      }
      
      // 如果是403禁止访问，提示用户
      if (status === 403) {
        return Promise.reject(new Error('没有权限访问此资源'))
      }
      
      // 如果后端返回了错误信息，使用后端的错误信息
      if (error.response.data) {
        if (typeof error.response.data === 'string') {
          return Promise.reject(new Error(error.response.data))
        }
        
        if (error.response.data.message) {
          return Promise.reject(new Error(error.response.data.message))
        }
      }
    }
    
    return Promise.reject(error)
  }
)

export default request 