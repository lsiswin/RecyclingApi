import request from '@/utils/request';
import type { AxiosPromise } from 'axios';

// 接口返回的通用响应格式
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  timestamp: string;
}

// 分页响应格式
export interface PagedResponse<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  items: T[];
}

// ==================== 职位相关接口 ====================

// 职位DTO
export interface JobPositionDTO {
  id: number;
  title: string;
  department: string;
  location: string;
  salaryMin: number;
  salaryMax: number;
  description: string;
  requirements: string;
  isActive: boolean;
  postedDate: string;
  lastUpdatedDate: string;
  applicationCount: number;
}

// 分页职位DTO
export interface PagedJobPositionsDTO extends PagedResponse<JobPositionDTO> {}

// 创建职位DTO
export interface CreateJobPositionDTO {
  title: string;
  department: string;
  location: string;
  salaryMin: number;
  salaryMax: number;
  description: string;
  requirements: string;
  isActive: boolean;
}

// 更新职位DTO
export interface UpdateJobPositionDTO extends CreateJobPositionDTO {
  id: number;
}

// 获取所有职位
export function getAllJobPositions(includeInactive: boolean = false): AxiosPromise<ApiResponse<JobPositionDTO[]>> {
  return request({
    url: '/api/JobPosition/all',
    method: 'get',
    params: { includeInactive }
  });
}

// 获取职位列表（分页）
export function getJobPositions(): AxiosPromise<ApiResponse<PagedJobPositionsDTO>> {
  return request({
    url: '/api/JobPosition',
    method: 'get'
  });
}

// 根据部门获取职位列表
export function getJobPositionsByDepartment(
  department: string,
  includeInactive: boolean = false
): AxiosPromise<ApiResponse<JobPositionDTO[]>> {
  return request({
    url: `/api/JobPosition/department/${department}`,
    method: 'get',
    params: { includeInactive }
  });
}

// 根据ID获取职位
export function getJobPositionById(id: number): AxiosPromise<ApiResponse<JobPositionDTO>> {
  return request({
    url: `/api/JobPosition/${id}`,
    method: 'get'
  });
}

// 创建职位
export function createJobPosition(data: CreateJobPositionDTO): AxiosPromise<ApiResponse<number>> {
  return request({
    url: '/api/JobPosition',
    method: 'post',
    data
  });
}

// 更新职位
export function updateJobPosition(id: number, data: UpdateJobPositionDTO): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/JobPosition/${id}`,
    method: 'put',
    data
  });
}

// 删除职位
export function deleteJobPosition(id: number): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/JobPosition/${id}`,
    method: 'delete'
  });
}

// 更改职位状态
export function changeJobPositionStatus(id: number, isActive: boolean): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/JobPosition/${id}/status`,
    method: 'patch',
    params: { isActive }
  });
}

// 获取职位的申请数量
export function getJobPositionApplicationCount(id: number): AxiosPromise<ApiResponse<number>> {
  return request({
    url: `/api/JobPosition/${id}/applications/count`,
    method: 'get'
  });
}

// ==================== 简历相关接口 ====================

// 简历DTO
export interface ResumeDTO {
  id: number;
  title: string;
  fileName: string;
  fileUrl: string;
  fileSize: number;
  fileType: string;
  skills: string;
  education: string;
  workExperience: string;
  additionalInfo: string;
  uploadDate: string;
  lastUpdatedDate: string;
  isActive: boolean;
  userId: string;
  userName: string;
}

// 简历筛选条件DTO
export interface ResumeFilterDTO {
  title?: string;
  userName?: string;
  skills?: string;
  uploadDateFrom?: string;
  uploadDateTo?: string;
  isActive?: boolean;
  pageindex?: number;
  pageSize?: number;
}

// 分页简历DTO
export interface PagedResumeDTO extends PagedResponse<ResumeDTO> {}

// 创建简历DTO
export interface CreateResumeDTO {
  title: string;
  fileName: string;
  fileContent: string;
  fileType: string;
  skills?: string;
  education?: string;
  workExperience?: string;
  additionalInfo?: string;
  isActive?: boolean;
}

// 更新简历DTO
export interface UpdateResumeDTO {
  id: number;
  title: string;
  skills?: string;
  education?: string;
  workExperience?: string;
  additionalInfo?: string;
  isActive?: boolean;
  fileContent?: string;
  fileName?: string;
  fileType?: string;
}

// 获取简历列表（分页）
export function getResumes(params: ResumeFilterDTO): AxiosPromise<ApiResponse<PagedResumeDTO>> {
  return request({
    url: '/api/Resume',
    method: 'get',
    params
  });
}

// 根据ID获取简历
export function getResumeById(id: number): AxiosPromise<ApiResponse<ResumeDTO>> {
  return request({
    url: `/api/Resume/${id}`,
    method: 'get'
  });
}

// 获取用户的所有简历
export function getMyResumes(): AxiosPromise<ApiResponse<ResumeDTO[]>> {
  return request({
    url: '/api/Resume/my',
    method: 'get'
  });
}

// 创建简历
export function createResume(data: CreateResumeDTO): AxiosPromise<ApiResponse<number>> {
  return request({
    url: '/api/Resume',
    method: 'post',
    data
  });
}

// 更新简历
export function updateResume(id: number, data: UpdateResumeDTO): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/Resume/${id}`,
    method: 'put',
    data
  });
}

// 删除简历
export function deleteResume(id: number): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/Resume/${id}`,
    method: 'delete'
  });
}

// 更改简历状态
export function changeResumeStatus(id: number, isActive: boolean): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/Resume/${id}/status`,
    method: 'patch',
    params: { isActive }
  });
} 