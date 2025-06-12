import request from '@/utils/request'

// 类型定义
export interface Advantage {
  title: string
  description: string
  iconUrl: string
}

export interface Milestone {
  year: string
  title: string
  description: string
}

export interface CompanyProfileData {
  name: string
  logoUrl: string
  coverImageUrl: string
  establishDate: string | Date
  phone: string
  email: string
  address: string
  introduction: string
  vision: string
  mission: string
  advantages: Advantage[]
  milestones: Milestone[]
  certifications: string[]
}

// 团队成员类型相关接口
interface TeamMemberType {
  id: number
  name: string
  description?: string
  sortOrder?: number
}

// 团队成员相关接口
interface TeamMember {
  id: number
  name: string
  title: string
  avatar?: string
  description?: string
  typeId: number
  typeName?: string
  employeeId?: number
  sortOrder?: number
}

// 简单员工信息
interface SimpleEmployee {
  id: number
  name: string
  department?: string
}

/**
 * 获取公司信息
 * @returns {Promise}
 */
export function getCompanyProfile() {
  return request({
    url: '/api/companymember',
    method: 'get'
  })
}

/**
 * 更新公司信息
 * @param {CompanyProfileData} data 公司信息数据
 * @returns {Promise}
 */
export function updateCompanyProfile(data: CompanyProfileData) {
  return request({
    url: '/api/companymember',
    method: 'post',
    data
  })
}

/**
 * 获取所有团队成员类型
 * @returns {Promise}
 */
export function getTeamMemberTypes() {
  return request({
    url: '/api/team/types',
    method: 'get'
  })
}

/**
 * 获取单个团队成员类型详情
 * @param {number} id 类型ID
 * @returns {Promise}
 */
export function getTeamMemberTypeById(id: number) {
  return request({
    url: `/api/team/types/${id}`,
    method: 'get'
  })
}

/**
 * 创建团队成员类型
 * @param {TeamMemberType} data 类型数据
 * @returns {Promise}
 */
export function createTeamMemberType(data: TeamMemberType) {
  return request({
    url: '/api/team/types',
    method: 'post',
    data
  })
}

/**
 * 更新团队成员类型
 * @param {number} id 类型ID
 * @param {TeamMemberType} data 类型数据
 * @returns {Promise}
 */
export function updateTeamMemberType(id: number, data: TeamMemberType) {
  return request({
    url: `/api/team/types/${id}`,
    method: 'put',
    data
  })
}

/**
 * 删除团队成员类型
 * @param {number} id 类型ID
 * @returns {Promise}
 */
export function deleteTeamMemberType(id: number) {
  return request({
    url: `/api/team/types/${id}`,
    method: 'delete'
  })
}

/**
 * 获取团队成员列表
 * @param {number} typeId 可选的类型ID筛选
 * @returns {Promise}
 */
export function getTeamMembers(typeId?: number) {
  return request({
    url: '/api/team/members',
    method: 'get',
    params: { typeId }
  })
}

/**
 * 获取单个团队成员详情
 * @param {number} id 成员ID
 * @returns {Promise}
 */
export function getTeamMemberById(id: number) {
  return request({
    url: `/api/team/members/${id}`,
    method: 'get'
  })
}

/**
 * 创建团队成员
 * @param {TeamMember} data 成员数据
 * @returns {Promise}
 */
export function createTeamMember(data: TeamMember) {
  return request({
    url: '/api/team/members',
    method: 'post',
    data
  })
}

/**
 * 更新团队成员
 * @param {number} id 成员ID
 * @param {TeamMember} data 成员数据
 * @returns {Promise}
 */
export function updateTeamMember(id: number, data: TeamMember) {
  return request({
    url: `/api/team/members/${id}`,
    method: 'put',
    data
  })
}

/**
 * 删除团队成员
 * @param {number} id 成员ID
 * @returns {Promise}
 */
export function deleteTeamMember(id: number) {
  return request({
    url: `/api/team/members/${id}`,
    method: 'delete'
  })
}

/**
 * 获取员工简略信息列表（用于选择关联）
 * @returns {Promise}
 */
export function getAllEmployeesSimple() {
  return request({
    url: '/api/team/employees',
    method: 'get'
  })
}
