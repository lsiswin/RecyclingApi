import axios from 'axios'

/**
 * HTTP客户端配置
 * 基于axios创建的HTTP客户端，包含请求和响应拦截器
 */

// 创建axios实例
const httpClient = axios.create({
  // API基础URL，从环境变量获取或使用默认值
  baseURL:'http://localhost:5279/api',
  // 请求超时时间（毫秒）
  timeout: 1000,
  // 默认请求头
  headers: {
    'Content-Type': 'application/json',
  },
})

/**
 * 请求拦截器
 * 在发送请求之前进行处理
 */
httpClient.interceptors.request.use(
  (config) => {
    // 在发送请求之前做些什么
    console.log('发送请求:', config.method?.toUpperCase(), config.url)
    
    // 添加认证token
    const token = localStorage.getItem('auth_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    
    return config
  },
  (error) => {
    // 对请求错误做些什么
    console.error('请求错误:', error)
    return Promise.reject(error)
  }
)

/**
 * 响应拦截器
 * 在接收响应之后进行处理
 */
httpClient.interceptors.response.use(
  (response) => {
    // 2xx 范围内的状态码都会触发该函数
    console.log('收到响应:', response.status, response.config.url)
    
    // 直接返回响应数据
    return response.data
  },
  (error) => {
    // 超出 2xx 范围的状态码都会触发该函数
    console.error('响应错误:', error)
    
    // 处理不同类型的错误
    if (error.response) {
      // 服务器返回了错误状态码
      const { status, data } = error.response
      
      switch (status) {
        case 401:
          // 未授权，可能需要重新登录
          console.error('未授权访问，请重新登录')
          // 清除本地认证信息
          localStorage.removeItem('auth_token')
          localStorage.removeItem('refresh_token')
          // 可以在这里进行路由跳转到登录页
          window.location.href = '/login'
          break
        case 403:
          // 禁止访问
          console.error('禁止访问该资源')
          break
        case 404:
          // 资源未找到
          console.error('请求的资源未找到')
          break
        case 500:
          // 服务器内部错误
          console.error('服务器内部错误')
          break
        default:
          console.error(`请求失败: ${status}`, data?.message || error.message)
      }
      
      // 返回格式化的错误信息
      return Promise.reject({
        status,
        message: data?.message || error.message,
        data: data
      })
    } else if (error.request) {
      // 请求已发出但没有收到响应
      console.error('网络错误，请检查网络连接')
      return Promise.reject({
        status: 0,
        message: '网络错误，请检查网络连接',
        data: null
      })
    } else {
      // 其他错误
      console.error('请求配置错误:', error.message)
      return Promise.reject({
        status: -1,
        message: error.message,
        data: null
      })
    }
  }
)

export default httpClient 