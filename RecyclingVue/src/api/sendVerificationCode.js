import httpClient from './httpClient'

/**
 * 发送验证码API接口
 */
export const sendVerificationCodeApi = {
  /**
   * 发送验证码到邮箱
   * @param {string} email - 接收验证码的邮箱
   * @param {string} type - 验证码类型，如 'register', 'resetPassword'
   * @returns {Promise<Object>} 发送结果
   */
  async sendToEmail(email, type = 'default') {
    try {
      const response = await httpClient.post('/auth/send-verification-code', {
        email,
        type
      })
      return response
    } catch (error) {
      throw error
    }
  },

  /**
   * 发送验证码到手机
   * @param {string} phoneNumber - 接收验证码的手机号
   * @param {string} type - 验证码类型，如 'register', 'resetPassword'
   * @returns {Promise<Object>} 发送结果
   */
  async sendToPhone(phoneNumber, type = 'default') {
    try {
      const response = await httpClient.post('/auth/send-verification-code-sms', {
        phoneNumber,
        type
      })
      return response
    } catch (error) {
      throw error
    }
  },
  
  /**
   * 验证验证码
   * @param {Object} data - 验证数据
   * @param {string} data.email - 邮箱
   * @param {string} data.code - 验证码
   * @param {string} data.type - 验证码类型
   * @returns {Promise<Object>} 验证结果
   */
  async verify(data) {
    try {
      const response = await httpClient.post('/auth/verify-code', data)
      return response
    } catch (error) {
      throw error
    }
  }
} 