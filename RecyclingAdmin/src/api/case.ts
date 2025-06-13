import request from '@/utils/request'

export interface CaseListQueryParams {
  pageindex: number
  pageSize: number
  keyword?: string
  category?: string
  DeviceType?: string
  Scale?: string  
  IsActive?: boolean
}

export interface CaseData {
  id?: number
  title: string
  category: string
  client: string
  date: string | Date
  image: string
  description: string
  fullDescription: string
  deviceType: string
  deviceCount: number
  duration: string
  scale: string
  tags: string[]
  projectDetails: string
  highlights: string[]
  isActive: boolean
  sort: number
  // 兼容旧属性
  Category?: string
  completionTime?: string | Date
  coverImage?: string
  summary?: string
  content?: string
  images?: string[]
  isPublished?: boolean
  sortOrder?: number
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