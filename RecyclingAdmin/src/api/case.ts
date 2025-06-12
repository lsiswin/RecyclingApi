import request from '@/utils/request'

export interface CaseListQueryParams {
  page: number
  pageSize: number
  title?: string
  categoryId?: number | null
}

export interface CaseData {
  id?: number
  title: string
  categoryId: number | null
  client: string
  completionTime: string | Date
  coverImage: string
  summary: string
  content: string
  images: string[]
  isPublished: boolean
  sortOrder: number
}

/**
 * 获取案例列表
 */
export function getCaseList(params: CaseListQueryParams) {
  return request({
    url: '/api/case',
    method: 'get',
    params
  })
}

/**
 * 获取案例详情
 */
export function getCaseById(id: number) {
  return request({
    url: `/api/case/${id}`,
    method: 'get'
  })
}

/**
 * 创建案例
 */
export function createCase(data: CaseData) {
  return request({
    url: '/api/case',
    method: 'post',
    data
  })
}

/**
 * 更新案例
 */
export function updateCase(id: number, data: CaseData) {
  return request({
    url: `/api/case/${id}`,
    method: 'put',
    data
  })
}

/**
 * 删除案例
 */
export function deleteCase(id: number) {
  return request({
    url: `/api/case/${id}`,
    method: 'delete'
  })
}

/**
 * 切换案例发布状态
 */
export function toggleCaseStatus(id: number) {
  return request({
    url: `/api/case/${id}/toggle-status`,
    method: 'patch'
  })
} 