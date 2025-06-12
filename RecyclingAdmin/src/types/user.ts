/**
 * API响应基础接口
 */
export interface ApiResponse<T = any> {
  success: boolean
  message: string
  data?: T
}

/**
 * 用户类型
 */
export interface User {
  id: string
  username: string
  realName?: string
  email: string
  phoneNumber?: string
  userType: number
  avatar?: string
  department?: string
  position?: string
  roles?: string[]
  isActive: boolean
  createdAt: string
  lastLoginAt?: string
}

/**
 * 用户角色类型
 */
export enum UserType {
  Customer = 0,
  Staff = 1,
  Admin = 2
}

/**
 * 登录请求参数
 */
export interface LoginRequest {
  username: string
  password: string
  rememberMe?: boolean
}

/**
 * 登录响应结果
 */
export interface LoginResponse {
  accessToken: string
  refreshToken: string
  tokenType: string
  expiresIn: number
  user: User
}

export interface RegisterRequest {
  username: string
  password: string
  email: string
  realName: string
  userType?: 'Customer' | 'Staff'
}

export interface RegisterResponse {
  success: boolean
  message: string
}

export interface RefreshTokenRequest {
  refreshToken: string
}

export interface RefreshTokenResponse {
  success: boolean
  message: string
  token?: string
  refreshToken?: string
}

export interface ForgotPasswordRequest {
  email: string
}

export interface ForgotPasswordResponse {
  success: boolean
  message: string
}

export interface ResetPasswordRequest {
  token: string
  newPassword: string
  confirmPassword: string
}

export interface ResetPasswordResponse {
  success: boolean
  message: string
}

export interface VerifyTokenResponse {
  success: boolean
  message: string
  user?: User
} 