import httpClient from './httpClient'

/**
 * 产品相关API接口
 * 提供产品的增删改查、分类管理、价值评估等功能
 */

/**
 * 产品API类
 * 封装所有与产品相关的HTTP请求
 */
export class ProductApi {
  
  /**
   * 获取所有产品分类
   * @returns {Promise<Object>} 返回包含分类列表的响应对象
   * @example
   * const response = await ProductApi.getCategories()
   * console.log(response.data) // 分类列表
   */
  static async getCategories() {
    try {
      const response = await httpClient.get('/products/categories')
      return response
    } catch (error) {
      console.error('获取产品分类失败:', error)
      throw error
    }
  }

  /**
   * 根据ID获取产品详细信息
   * @param {number} id - 产品ID
   * @returns {Promise<Object>} 返回包含产品详细信息的响应对象
   * @throws {Error} 当产品不存在时抛出404错误
   * @example
   * const response = await ProductApi.getProductById(1)
   * console.log(response.data) // 产品详细信息
   */
  static async getProductById(id) {
    try {
      const response = await httpClient.get(`/products/${id}`)
      return response
    } catch (error) {
      console.error(`获取产品详情失败 (ID: ${id}):`, error)
      throw error
    }
  }

  /**
   * 根据分类ID获取该分类下的所有产品
   * @param {number} categoryId - 分类ID
   * @returns {Promise<Object>} 返回包含产品列表的响应对象
   * @example
   * const response = await ProductApi.getProductsByCategory(1)
   * console.log(response.data) // 该分类下的产品列表
   */
  static async getProductsByCategory(categoryId) {
    try {
      const response = await httpClient.get(`/products/category/${categoryId}`)
      return response
    } catch (error) {
      console.error(`获取分类产品失败 (分类ID: ${categoryId}):`, error)
      throw error
    }
  }

  /**
   * 创建新产品
   * @param {Object} productData - 产品数据对象
   * @param {string} productData.name - 产品名称
   * @param {string} [productData.description] - 产品描述
   * @param {string} productData.type - 产品类型
   * @param {number} [productData.estimatedValue] - 估值
   * @param {string} [productData.specifications] - 规格说明
   * @param {string} [productData.brand] - 品牌
   * @param {string} [productData.model] - 型号
   * @param {number} productData.manufactureYear - 制造年份
   * @param {string} [productData.condition] - 设备条件
   * @param {number} productData.categoryId - 分类ID
   * @returns {Promise<Object>} 返回包含创建的产品信息的响应对象
   * @example
   * const productData = {
   *   name: 'iPhone 13',
   *   type: 'Mobile',
   *   categoryId: 2,
   *   manufactureYear: 2021
   * }
   * const response = await ProductApi.createProduct(productData)
   */
  static async createProduct(productData) {
    try {
      const response = await httpClient.post('/products', productData)
      return response
    } catch (error) {
      console.error('创建产品失败:', error)
      throw error
    }
  }

  /**
   * 更新产品信息
   * @param {number} id - 产品ID
   * @param {Object} productData - 要更新的产品数据（只需包含要更新的字段）
   * @returns {Promise<Object>} 返回包含更新后产品信息的响应对象
   * @throws {Error} 当产品不存在时抛出404错误
   * @example
   * const updateData = {
   *   name: '新的产品名称',
   *   condition: '良好'
   * }
   * const response = await ProductApi.updateProduct(1, updateData)
   */
  static async updateProduct(id, productData) {
    try {
      const response = await httpClient.put(`/products/${id}`, productData)
      return response
    } catch (error) {
      console.error(`更新产品失败 (ID: ${id}):`, error)
      throw error
    }
  }

  /**
   * 删除产品
   * @param {number} id - 产品ID
   * @returns {Promise<void>} 删除成功时返回空响应
   * @throws {Error} 当产品不存在时抛出404错误
   * @example
   * await ProductApi.deleteProduct(1)
   * console.log('产品删除成功')
   */
  static async deleteProduct(id) {
    try {
      await httpClient.delete(`/products/${id}`)
    } catch (error) {
      console.error(`删除产品失败 (ID: ${id}):`, error)
      throw error
    }
  }

  /**
   * 获取产品的估值
   * 基于产品类型、年份、条件等因素计算回收价值
   * @param {number} id - 产品ID
   * @returns {Promise<Object>} 返回包含产品估值的响应对象
   * @throws {Error} 当产品不存在时抛出404错误
   * @example
   * const response = await ProductApi.getEstimatedValue(1)
   * console.log(`产品估值: ¥${response.data}`)
   */
  static async getEstimatedValue(id) {
    try {
      const response = await httpClient.get(`/products/${id}/estimate`)
      return response
    } catch (error) {
      console.error(`获取产品估值失败 (ID: ${id}):`, error)
      throw error
    }
  }

  /**
   * 测试API连接
   * @returns {Promise<Object>} 返回API状态信息
   * @example
   * const response = await ProductApi.testConnection()
   * console.log(response.data) // API状态信息
   */
  static async testConnection() {
    try {
      const response = await httpClient.get('/test')
      return response
    } catch (error) {
      console.error('API连接测试失败:', error)
      throw error
    }
  }

  /**
   * 健康检查
   * @returns {Promise<Object>} 返回系统健康状态信息
   * @example
   * const response = await ProductApi.healthCheck()
   * console.log(response) // 系统健康状态
   */
  static async healthCheck() {
    try {
      const response = await httpClient.get('/test/health')
      return response
    } catch (error) {
      console.error('健康检查失败:', error)
      throw error
    }
  }
}

// 导出默认实例（保持向后兼容）
export default ProductApi 