import httpClient from './httpClient'

/**
 * 认证相关API服务
 */
export const AuthApi = {
  /**
   * 用户登录
   * @param {Object} loginData - 登录信息
   * @param {string} loginData.username - 用户名/邮箱/手机号
   * @param {string} loginData.password - 密码
   * @param {boolean} loginData.remember - 是否记住登录状态
   * @returns {Promise<Object>} 登录结果
   */
  async login(loginData) {
    try {
      const response = await httpClient.post('/auth/login', loginData)
      return response
    } catch (error) {
      console.error('登录失败:', error)
      throw error
    }
  },

  /**
   * 用户注册
   * @param {Object} registerData - 注册信息
   * @param {string} registerData.username - 用户名
   * @param {string} registerData.email - 邮箱
   * @param {string} registerData.password - 密码
   * @param {string} registerData.phoneNumber - 手机号
   * @returns {Promise<Object>} 注册结果
   */
  async register(registerData) {
    try {
      const response = await httpClient.post('/auth/register', registerData)
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 验证令牌有效性
   * @returns {Promise<Object>} 验证结果及用户信息
   */
  async verifyToken() {
    try {
      const response = await httpClient.get('/auth/verify')
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 刷新令牌
   * @param {string} refreshToken - 刷新令牌
   * @returns {Promise<Object>} 新的访问令牌
   */
  async refreshToken(refreshToken) {
    try {
      const response = await httpClient.post('/auth/refresh', { refreshToken })
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 用户登出
   * @returns {Promise<Object>} 登出结果
   */
  async logout() {
    try {
      const response = await httpClient.post('/auth/logout')
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 获取当前用户信息
   * @returns {Promise<Object>} 用户信息
   */
  async getCurrentUser() {
    try {
      const response = await httpClient.get('/auth/me')
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 忘记密码请求
   * @param {Object} data - 请求数据
   * @param {string} data.email - 用户邮箱
   * @returns {Promise<Object>} 处理结果
   */
  async forgotPassword(data) {
    try {
      const response = await httpClient.post('/auth/forgot-password', data)
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 重置密码
   * @param {Object} data - 重置密码数据
   * @param {string} data.token - 重置令牌
   * @param {string} data.email - 用户邮箱
   * @param {string} data.newPassword - 新密码
   * @returns {Promise<Object>} 重置结果
   */
  async resetPassword(data) {
    try {
      const response = await httpClient.post('/auth/reset-password', data)
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 发送验证码
   * @param {string} email - 用户邮箱
   * @returns {Promise<Object>} 发送结果
   */
  async sendVerificationCode(email) {
    try {
      const response = await httpClient.post('/auth/send-verification-code', { email })
      return response
    } catch (error) {
      throw error
    }
  }
} 