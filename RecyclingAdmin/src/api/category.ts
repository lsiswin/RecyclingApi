import request from '@/utils/request'

export interface CategoryListQueryParams {
  page: number
  pageSize: number
  keyword?: string
}

export interface CategoryData {
  id?: number
  name: string
  description: string
  coverImage: string
  sortOrder: number
  isActive: boolean
}

/**
 * 获取分类分页列表
 */
export function getCategoryList(params: CategoryListQueryParams) {
  return request({
    url: '/api/productcategory',
    method: 'get',
    params
  })
}

/**
 * 获取所有启用的分类
 */
export function getAllActiveCategories() {
  return request({
    url: '/api/productcategory/active',
    method: 'get'
  })
}

/**
 * 获取分类详情
 */
export function getCategoryById(id: number) {
  return request({
    url: `/api/productcategory/${id}`,
    method: 'get'
  })
}

/**
 * 创建分类
 */
export function createCategory(data: CategoryData) {
  return request({
    url: '/api/productcategory',
    method: 'post',
    data
  })
}

/**
 * 更新分类
 */
export function updateCategory(id: number, data: CategoryData) {
  return request({
    url: `/api/productcategory/${id}`,
    method: 'put',
    data
  })
}

/**
 * 删除分类
 */
export function deleteCategory(id: number) {
  return request({
    url: `/api/productcategory/${id}`,
    method: 'delete'
  })
}

/**
 * 切换分类状态
 */
export function toggleCategoryStatus(id: number) {
  return request({
    url: `/api/productcategory/${id}/toggle-status`,
    method: 'patch'
  })
} 