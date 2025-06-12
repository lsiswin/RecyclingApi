import { defineStore } from 'pinia'
import { ProductApi } from '@/api/productApi'

/**
 * 产品状态管理Store
 * 管理产品分类、产品列表等状态
 */
export const useProductStore = defineStore('product', {
  state: () => ({
    categories: [],
    currentProducts: [],
    loading: false,
    error: null
  }),

  getters: {
    serverCategories: (state) => state.categories.filter(c => c.type === 'server'),
    networkCategories: (state) => state.categories.filter(c => c.type === 'network'),
    storageCategories: (state) => state.categories.filter(c => c.type === 'storage')
  },

  actions: {
    /**
     * 加载产品分类
     */
    async loadCategories() {
      this.loading = true
      try {
        const response = await ProductApi.getCategories()
        this.categories = response.data
        this.error = null
      } catch (err) {
        this.error = err.message
        console.error('加载产品分类失败:', err)
      } finally {
        this.loading = false
      }
    },

    /**
     * 根据ID加载产品详情
     * @param {number} id - 产品ID
     * @returns {Object} 产品详情
     */
    async loadProductById(id) {
      this.loading = true
      try {
        const response = await ProductApi.getProductById(id)
        const product = response.data
        const index = this.currentProducts.findIndex(p => p.id === id)
        if (index > -1) {
          this.currentProducts[index] = product
        } else {
          this.currentProducts.push(product)
        }
        this.error = null
        return product
      } catch (err) {
        this.error = err.message
        console.error('加载产品详情失败:', err)
        throw err
      } finally {
        this.loading = false
      }
    },

    /**
     * 清除错误信息
     */
    clearError() {
      this.error = null
    }
  }
}) 