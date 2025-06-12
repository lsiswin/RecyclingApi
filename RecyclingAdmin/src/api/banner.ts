import request from '@/utils/request'

export interface BannerListQueryParams {
  pageindex: number
  pageSize: number
  Keyword: string
  IsActive: boolean
}

export interface BannerData {
  id?: number
  title: string
  description: string
  imageUrl: string
  linkUrl: string
  sort: number
  isActive: boolean
}

/**
 * 获取轮播图列表
 */
export function getBannerList(params: BannerListQueryParams) {
  return request({
    url: '/api/banner/',
    method: 'get',
    params
  })
}

/**
 * 获取轮播图详情
 */
export function getBannerById(id: number) {
  return request({
    url: `/api/banner/${id}`,
    method: 'get'
  })
}

/**
 * 创建轮播图
 */
export function createBanner(data: BannerData) {
  return request({
    url: '/api/banner',
    method: 'post',
    data
  })
}

/**
 * 更新轮播图
 */
export function updateBanner(id: number, data: BannerData) {
  return request({
    url: `/api/banner/${id}`,
    method: 'put',
    data
  })
}

/**
 * 删除轮播图
 */
export function deleteBanner(id: number) {
  return request({
    url: `/api/banner/${id}`,
    method: 'delete'
  })
}

/**
 * 切换轮播图状态
 */
export function toggleBannerStatus(id: number) {
  return request({
    url: `/api/banner/${id}/toggle-status`,
    method: 'patch'
  })
} 