// 公司相关接口类型定义
import { ApiResponse } from "./user"
declare module '@/api/company' {
  interface CompanyProfileData {
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
    advantages: Array<{
      title: string
      description: string
      iconUrl: string
    }>
    milestones: Array<{
      year: string
      title: string
      description: string
    }>
    certifications: string[]
  }

  export function getCompanyProfile()
  export function updateCompanyProfile(data: CompanyProfileData): Promise<ApiResponse<any>>
  
  export function getTeamMemberTypes(): Promise<ApiResponse<any>>
  export function getTeamMemberTypeById(id: number): Promise<ApiResponse<any>>
  export function createTeamMemberType(data: any): Promise<ApiResponse<any>>
  export function updateTeamMemberType(id: number, data: any): Promise<ApiResponse<any>>
  export function deleteTeamMemberType(id: number): Promise<ApiResponse<any>>
  
  export function getTeamMembers(typeId?: number): Promise<ApiResponse<any>>
  export function getTeamMemberById(id: number): Promise<ApiResponse<any>>
  export function createTeamMember(data: any): Promise<ApiResponse<any>>
  export function updateTeamMember(id: number, data: any): Promise<ApiResponse<any>>
  export function deleteTeamMember(id: number): Promise<ApiResponse<any>>
  
  export function getAllEmployeesSimple(): Promise<ApiResponse<any>>
} 