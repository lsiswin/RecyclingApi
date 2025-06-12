// API配置
export const API_CONFIG = {
  // 基础URL - 可以通过环境变量覆盖
  BASE_URL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5279/api',
  
  // 请求超时时间
  TIMEOUT: 10000,
  
  // 默认请求头
  DEFAULT_HEADERS: {
    'Content-Type': 'application/json'
  }
}

// 环境配置
export const ENV_CONFIG = {
  // 应用标题
  APP_TITLE: import.meta.env.VITE_APP_TITLE || 'IT设备回收后台管理系统',
  
  // 应用版本
  APP_VERSION: import.meta.env.VITE_APP_VERSION || '1.0.0',
  
  // 是否为开发模式
}