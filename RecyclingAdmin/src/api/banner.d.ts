export interface BannerDto {
  id: number
  title: string
  description: string
  imageUrl: string
  linkUrl: string
  sort: number
  isActive: boolean
  createdAt: string
}

export interface BannerListQueryParams {
  pageindex: number
  pageSize: number
  Keyword: string
  IsActive: boolean | null
}

export interface PagedResponseDto<T> {
  items: T[]
  totalCount: number
  pageIndex: number
  pageSize: number
}

export interface ApiResponseDto<T> {
  success: boolean
  data: T
  message: string
  errors: string[]
}

export interface CreateUpdateBannerDto {
  title: string
  description: string
  imageUrl: string
  linkUrl: string
  sort: number
  isActive: boolean
} 