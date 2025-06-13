import request from '@/utils/request';
import type { AxiosPromise } from 'axios';
import type { ApiResponse, PagedResponse } from './recruitment';

// ==================== 应聘者相关接口 ====================

// 应聘者DTO
export interface JobApplicationDTO {
  id: number;
  name: string;
  email: string;
  phone: string;
  resumeUrl: string;
  coverLetter: string;
  jobPositionId: number;
  jobPositionTitle: string;
  status: number;
  appliedDate: string;
  lastUpdatedDate: string;
  notes: string;
  userId: string;
}

// 应聘者筛选条件DTO
export interface JobApplicationFilterDTO {
  name?: string;
  email?: string;
  jobPositionId?: number | null;
  status?: number | null;
  appliedDateFrom?: string;
  appliedDateTo?: string;
  pageindex?: number;
  pageSize?: number;
}

// 分页应聘者DTO
export interface PagedJobApplicationDTO extends PagedResponse<JobApplicationDTO> {}

// 创建应聘者DTO
export interface CreateJobApplicationDTO {
  name: string;
  email: string;
  phone: string;
  resumeId?: number;
  resumeUrl?: string;
  coverLetter?: string;
  jobPositionId: number;
}

// 更新应聘者状态DTO
export interface UpdateJobApplicationStatusDTO {
  id: number;
  status: number;
  notes?: string;
}

// 更新应聘者备注DTO
export interface UpdateJobApplicationNotesDTO {
  id: number;
  notes: string;
}

// 获取应聘者列表（分页）
export function getJobApplications(params: JobApplicationFilterDTO): AxiosPromise<ApiResponse<PagedJobApplicationDTO>> {
  return request({
    url: '/api/JobApplication',
    method: 'get',
    params
  });
}

// 根据ID获取应聘者
export function getJobApplicationById(id: number): AxiosPromise<ApiResponse<JobApplicationDTO>> {
  return request({
    url: `/api/JobApplication/${id}`,
    method: 'get'
  });
}

// 根据职位ID获取应聘者列表
export function getJobApplicationsByJobPositionId(
  jobPositionId: number,
  params?: {
    pageNumber?: number;
    pageSize?: number;
    status?: number | null;
  }
): AxiosPromise<ApiResponse<PagedJobApplicationDTO>> {
  return request({
    url: `/api/JobApplication/position/${jobPositionId}`,
    method: 'get',
    params
  });
}

// 创建应聘者
export function createJobApplication(data: CreateJobApplicationDTO): AxiosPromise<ApiResponse<number>> {
  return request({
    url: '/api/JobApplication',
    method: 'post',
    data
  });
}

// 更新应聘者状态
export function updateJobApplicationStatus(id: number, data: UpdateJobApplicationStatusDTO): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/JobApplication/${id}/status`,
    method: 'put',
    data
  });
}

// 更新应聘者备注
export function updateJobApplicationNotes(id: number, notes: string): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/JobApplication/${id}/notes`,
    method: 'put',
    data: { id, notes }
  });
}

// 删除应聘者
export function deleteJobApplication(id: number): AxiosPromise<ApiResponse<boolean>> {
  return request({
    url: `/api/JobApplication/${id}`,
    method: 'delete'
  });
} 