<template>
  <div class="login-container">
    <div class="login-content">
      <!-- 左侧装饰区域 -->
      <div class="login-decoration">
        <div class="decoration-content">
          <div class="decoration-logo">
            <div class="logo-icon-wrapper">
              <el-icon class="decoration-icon"><Recycle /></el-icon>
            </div>
            <div class="logo-text">
              <h1 class="decoration-title">IT设备回收</h1>
              <h2 class="decoration-subtitle">后台管理系统</h2>
            </div>
          </div>
          <p class="decoration-desc">专业的IT设备回收管理平台<br/>安全 · 高效 · 环保</p>
          
          <div class="decoration-features">
            <div class="feature-item" v-for="(feature, index) in features" :key="index">
              <div class="feature-icon">
                <el-icon><component :is="feature.icon" /></el-icon>
              </div>
              <div class="feature-content">
                <h4>{{ feature.title }}</h4>
                <p>{{ feature.description }}</p>
              </div>
            </div>
          </div>
          
          <div class="decoration-stats">
            <div class="stat-item" v-for="(stat, index) in stats" :key="index">
              <div class="stat-number">{{ stat.number }}</div>
              <div class="stat-label">{{ stat.label }}</div>
            </div>
          </div>
        </div>
        
        <!-- 装饰性背景元素 -->
        <div class="decoration-bg-elements">
          <div class="bg-circle bg-circle-1"></div>
          <div class="bg-circle bg-circle-2"></div>
          <div class="bg-circle bg-circle-3"></div>
        </div>
      </div>

      <!-- 右侧登录表单 -->
      <div class="login-form-section">
        <div class="login-box">
          <div class="login-header">
            <div class="header-logo">
              <el-icon class="header-icon"><Lock /></el-icon>
            </div>
            <h3>欢迎登录</h3>
            <p>请输入您的账号信息以访问管理系统</p>
          </div>
          
          <el-form
            ref="loginFormRef"
            :model="loginForm"
            :rules="loginRules"
            class="login-form"
            @submit.prevent="handleLogin"
          >
            <el-form-item prop="username">
              <div class="input-wrapper">
                <label class="input-label">用户名</label>
                <el-input
                  v-model="loginForm.username"
                  placeholder="请输入用户名"
                  size="large"
                  prefix-icon="User"
                  class="form-input"
                />
              </div>
            </el-form-item>
            
            <el-form-item prop="password">
              <div class="input-wrapper">
                <label class="input-label">密码</label>
                <el-input
                  v-model="loginForm.password"
                  type="password"
                  placeholder="请输入密码"
                  size="large"
                  prefix-icon="Lock"
                  show-password
                  class="form-input"
                  @keyup.enter="handleLogin"
                />
              </div>
            </el-form-item>
            
            <el-form-item>
              <el-button
                type="primary"
                size="large"
                :loading="authStore.loading"
                @click="handleLogin"
                class="login-btn"
              >
                <span v-if="!authStore.loading">登录系统</span>
                <span v-else>登录中...</span>
              </el-button>
            </el-form-item>
            
            <div class="forgot-password-link">
              <router-link to="/forgot-password" class="forgot-link">忘记密码？</router-link>
            </div>
          </el-form>
          
                  
          <div class="login-footer">
            <p>还没有账户？ <router-link to="/register" class="footer-link">立即注册</router-link></p>
            <div class="footer-links">
              <a href="#" class="footer-link">隐私政策</a>
              <span class="divider">|</span>
              <a href="#" class="footer-link">服务条款</a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import { Monitor, UserFilled, ChatDotRound, Service } from '@element-plus/icons-vue'

const router = useRouter()
const authStore = useAuthStore()
const loginFormRef = ref<FormInstance>()

// 功能特性数据
const features = ref([
  {
    icon: Monitor,
    title: '设备管理',
    description: '全生命周期管理'
  },
  {
    icon: UserFilled,
    title: '用户管理',
    description: '多角色权限控制'
  },
  {
    icon: ChatDotRound,
    title: '客服系统',
    description: '实时在线服务'
  }
])

// 统计数据
const stats = ref([
  { number: '10,000+', label: '设备回收' },
  { number: '500+', label: '企业客户' },
  { number: '99.9%', label: '数据安全' }
])

const loginForm = reactive({
  username: '',
  password: ''
})

const loginRules: FormRules = {
  username: [
    { required: true, message: '请输入用户名', trigger: 'blur' }
  ],
  password: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 6, message: '密码长度不能少于6位', trigger: 'blur' }
  ]
}

// 复制到剪贴板功能
const copyToClipboard = async (text: string) => {
  try {
    await navigator.clipboard.writeText(text)
    ElMessage.success('已复制到剪贴板')
  } catch (err) {
    ElMessage.warning('复制失败，请手动复制')
  }
}

const handleLogin = async () => {
  if (!loginFormRef.value) return
  
  await loginFormRef.value.validate(async (valid) => {
    if (valid) {
      try {
        console.log('开始登录:', loginForm.username)
        const result = await authStore.login(loginForm.username, loginForm.password)
        console.log('登录结果:', result)
        
        if (result?.message) {
          // 显示成功消息
          ElMessage({
            message: result.message || '登录成功',
            type: 'success',
            duration: 2000
          })
          
          console.log('登录成功，当前用户:', authStore.user)
          console.log('认证状态:', authStore.isAuthenticated)
          
          // 等待一小段时间确保状态更新
          await new Promise(resolve => setTimeout(resolve, 300))
          
          // 根据用户角色跳转到不同页面
          const userType = authStore.user?.userType
          console.log('用户类型:', userType)
          
          try {
            if (userType === 1) {
              console.log('跳转到客服工作台')
              await router.push('/consultation/workbench')
              console.log('跳转完成')
            } else {
              console.log('跳转到仪表板')
              await router.push('/')
              console.log('跳转完成')
            }
          } catch (routerError) {
            console.error('路由跳转错误:', routerError)
          }
        } else {
          ElMessage({
            message: result.message || '登录失败',
            type: 'error',
            duration: 3000
          })
        }
      } catch (error) {
        console.error('登录错误:', error)
        ElMessage({
          message: '登录过程中发生错误',
          type: 'error',
          duration: 3000
        })
      }
    }
  })
}
</script>

<style scoped>
.login-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  position: relative;
  overflow: hidden;
}

.login-container::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="grain" width="100" height="100" patternUnits="userSpaceOnUse"><circle cx="25" cy="25" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="75" cy="75" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="50" cy="10" r="0.5" fill="rgba(255,255,255,0.05)"/><circle cx="10" cy="50" r="0.5" fill="rgba(255,255,255,0.05)"/><circle cx="90" cy="30" r="0.5" fill="rgba(255,255,255,0.05)"/></pattern></defs><rect width="100" height="100" fill="url(%23grain)"/></svg>');
  opacity: 0.3;
}

.login-content {
  display: flex;
  width: 100%;
  height: 100vh;
  background: rgba(255, 255, 255, 0.95);
  border-radius: 0;
  box-shadow: none;
  backdrop-filter: blur(10px);
  overflow: hidden;
  position: relative;
  z-index: 1;
  animation: slideInUp 0.8s ease-out;
}

@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* 左侧装饰区域 */
.login-decoration {
  flex: 1.2;
  background: linear-gradient(135deg, #1e3a8a 0%, #3730a3 50%, #581c87 100%);
  padding: 35px 30px;
  color: white;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: center;
  min-height: 100%;
  overflow: hidden;
}

.decoration-content {
  position: relative;
  z-index: 2;
}

.decoration-logo {
  display: flex;
  align-items: center;
  margin-bottom: 25px;
  animation: fadeInLeft 1s ease-out 0.2s both;
}

.logo-icon-wrapper {
  width: 45px;
  height: 45px;
  background: rgba(255, 208, 75, 0.2);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 14px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 208, 75, 0.3);
}

.decoration-icon {
  font-size: 24px;
  color: #ffd04b;
}

.logo-text {
  flex: 1;
}

.decoration-title {
  font-size: 24px;
  font-weight: 700;
  margin: 0 0 5px 0;
  line-height: 1.2;
  background: linear-gradient(135deg, #ffffff 0%, #ffd04b 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.decoration-subtitle {
  font-size: 14px;
  font-weight: 400;
  margin: 0;
  opacity: 0.9;
}

.decoration-desc {
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 30px;
  opacity: 0.9;
  animation: fadeInLeft 1s ease-out 0.4s both;
}

.decoration-features {
  margin-bottom: 30px;
}

.feature-item {
  display: flex;
  align-items: center;
  margin-bottom: 16px;
  padding: 14px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 8px;
  backdrop-filter: blur(5px);
  transition: all 0.3s ease;
  border: 1px solid rgba(255, 255, 255, 0.1);
  animation: fadeInLeft 1s ease-out calc(0.6s + var(--delay, 0s)) both;
}

.feature-item:nth-child(1) { --delay: 0s; }
.feature-item:nth-child(2) { --delay: 0.1s; }
.feature-item:nth-child(3) { --delay: 0.2s; }

.feature-item:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateX(8px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.2);
}

.feature-icon {
  width: 36px;
  height: 36px;
  background: rgba(255, 208, 75, 0.2);
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 14px;
  color: #ffd04b;
  font-size: 18px;
  border: 1px solid rgba(255, 208, 75, 0.3);
  transition: all 0.3s ease;
}

.feature-item:hover .feature-icon {
  background: rgba(255, 208, 75, 0.3);
  transform: scale(1.1);
}

.feature-content h4 {
  font-size: 13px;
  font-weight: 600;
  margin: 0 0 3px 0;
}

.feature-content p {
  font-size: 11px;
  margin: 0;
  opacity: 0.8;
}

.decoration-stats {
  display: flex;
  justify-content: space-between;
  gap: 16px;
  animation: fadeInUp 1s ease-out 1s both;
}

.stat-item {
  text-align: center;
  flex: 1;
  padding: 14px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 8px;
  backdrop-filter: blur(5px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  transition: all 0.3s ease;
}

.stat-item:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-3px);
}

.stat-number {
  font-size: 18px;
  font-weight: 700;
  color: #ffd04b;
  margin-bottom: 3px;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.stat-label {
  font-size: 11px;
  opacity: 0.8;
}

.decoration-bg-elements {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 1;
  overflow: hidden;
}

.bg-circle {
  position: absolute;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.05);
  animation: float 20s infinite linear;
}

.bg-circle-1 {
  width: 200px;
  height: 200px;
  top: 10%;
  left: 10%;
  animation-delay: 0s;
}

.bg-circle-2 {
  width: 150px;
  height: 150px;
  top: 30%;
  right: 15%;
  animation-delay: -7s;
}

.bg-circle-3 {
  width: 100px;
  height: 100px;
  bottom: 20%;
  left: 30%;
  animation-delay: -14s;
}

@keyframes float {
  0%, 100% {
    transform: translateY(0px) rotate(0deg);
    opacity: 0.3;
  }
  50% {
    transform: translateY(-20px) rotate(180deg);
    opacity: 0.1;
  }
}

@keyframes fadeInLeft {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* 右侧登录表单 */
.login-form-section {
  flex: 1;
  padding: 35px 30px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  background: white;
  min-height: 100%;
}

.login-box {
  max-width: 320px;
  margin: 0 auto;
  width: 100%;
  animation: fadeInRight 1s ease-out 0.3s both;
}

@keyframes fadeInRight {
  from {
    opacity: 0;
    transform: translateX(30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.login-header {
  text-align: center;
  margin-bottom: 25px;
}

.header-logo {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 14px;
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
  transition: all 0.3s ease;
}

.header-logo:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 35px rgba(102, 126, 234, 0.4);
}

.header-icon {
  font-size: 28px;
  color: white;
}

.login-header h3 {
  font-size: 20px;
  font-weight: 700;
  color: #1a202c;
  margin: 0 0 6px 0;
}

.login-header p {
  font-size: 12px;
  color: #718096;
  margin: 0;
}

.login-form {
  margin-bottom: 20px;
}

.input-wrapper {
  margin-bottom: 20px;
}

.input-label {
  display: block;
  font-size: 12px;
  font-weight: 600;
  color: #374151;
  margin-bottom: 5px;
}

.form-input {
  width: 100%;
}

.form-input :deep(.el-input__wrapper) {
  border-radius: 8px;
  border: 2px solid #e5e7eb;
  padding: 12px 14px;
  font-size: 13px;
  transition: all 0.3s ease;
}

.form-input :deep(.el-input__wrapper:hover) {
  border-color: #667eea;
  box-shadow: 0 0 0 2px rgba(102, 126, 234, 0.1);
}

.form-input :deep(.el-input__wrapper.is-focus) {
  border-color: #667eea;
  box-shadow: 0 0 0 2px rgba(102, 126, 234, 0.2);
}

.login-btn {
  width: 100%;
  height: 42px;
  font-size: 13px;
  font-weight: 600;
  border-radius: 8px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.login-btn::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: left 0.5s;
}

.login-btn:hover::before {
  left: 100%;
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(102, 126, 234, 0.4);
}

.login-tips {
  margin-bottom: 20px;
}

.tips-card {
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  transition: all 0.3s ease;
}

.tips-card:hover {
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
}

.tips-card :deep(.el-card__header) {
  background: #f8fafc;
  border-bottom: 1px solid #e5e7eb;
  padding: 10px 14px;
}

.tips-header {
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 11px;
  font-weight: 600;
  color: #374151;
}

.tips-content {
  padding: 0;
}

.account-group {
  padding: 10px 0;
}

.account-group:not(:last-child) {
  border-bottom: 1px solid #f3f4f6;
}

.account-title {
  font-size: 11px;
  font-weight: 600;
  color: #374151;
  margin-bottom: 6px;
  display: flex;
  align-items: center;
  gap: 5px;
}

.role-icon {
  font-size: 12px;
}

.admin-icon {
  color: #ef4444;
}

.staff-icon {
  color: #10b981;
}

.account-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 5px;
  font-size: 10px;
}

.account-label {
  color: #6b7280;
}

.account-info {
  font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
  background: #f3f4f6;
  padding: 3px 6px;
  border-radius: 3px;
  color: #374151;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  font-size: 10px;
}

.account-info:hover {
  background: #e5e7eb;
  transform: scale(1.05);
}

.account-info:active {
  transform: scale(0.95);
}

.login-footer {
  text-align: center;
  color: #9ca3af;
  font-size: 11px;
}

.footer-links {
  margin-top: 6px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 5px;
}

.footer-link {
  color: #6b7280;
  text-decoration: none;
  transition: color 0.3s ease;
  font-size: 10px;
}

.footer-link:hover {
  color: #374151;
}

.divider {
  color: #d1d5db;
  font-size: 10px;
}

/* 响应式设计 - 参考RecyclingVue */
@media (max-width: 768px) {
  .login-container {
    padding: 0;
  }
  
  .login-content {
    flex-direction: column;
    width: 100%;
    height: 100vh;
    border-radius: 0;
  }
  
  .login-decoration {
    flex: none;
    padding: 25px 20px;
    min-height: 300px;
  }
  
  .login-decoration {
    flex: none;
    padding: 25px 20px;
    min-height: 300px;
  }
  
  .decoration-title {
    font-size: 18px;
  }
  
  .decoration-subtitle {
    font-size: 12px;
  }
  
  .decoration-desc {
    font-size: 12px;
    margin-bottom: 20px;
  }
  
  .decoration-features {
    margin-bottom: 20px;
  }
  
  .feature-item {
    padding: 10px;
    margin-bottom: 12px;
  }
  
  .feature-icon {
    width: 30px;
    height: 30px;
    font-size: 14px;
  }
  
  .feature-content h4 {
    font-size: 11px;
  }
  
  .feature-content p {
    font-size: 9px;
  }
  
  .decoration-stats {
    gap: 10px;
  }
  
  .stat-item {
    padding: 10px;
  }
  
  .stat-number {
    font-size: 14px;
  }
  
  .stat-label {
    font-size: 9px;
  }
  
  .login-form-section {
    padding: 25px 20px;
    min-height: auto;
  }
  
  .login-box {
    max-width: 280px;
  }
  
  .login-header {
    margin-bottom: 20px;
  }
  
  .login-header h3 {
    font-size: 16px;
  }
  
  .login-header p {
    font-size: 10px;
  }
  
  .header-logo {
    width: 50px;
    height: 50px;
  }
  
  .header-icon {
    font-size: 22px;
  }
  
  .input-label {
    font-size: 13px;
  }
  
  .form-input :deep(.el-input__wrapper) {
    padding: 10px 12px;
    font-size: 11px;
  }
  
  .login-btn {
    height: 36px;
    font-size: 11px;
  }
  
  .tips-card :deep(.el-card__header) {
    padding: 8px 12px;
  }
  
  .tips-header {
    font-size: 9px;
  }
  
  .account-title {
    font-size: 9px;
  }
  
  .account-item {
    font-size: 8px;
  }
  
  .account-info {
    font-size: 8px;
    padding: 2px 4px;
  }
  
  .login-footer {
    font-size: 9px;
  }
  
  .footer-link {
    font-size: 8px;
  }
}

/* 大屏幕优化 (1200px - 1920px) */
@media (min-width: 1200px) and (max-width: 1919px) {
  .login-container {
    padding: 0;
  }
  
  .login-content {
    width: 100%;
    height: 100vh;
  }
  
  .login-decoration {
    padding: 45px 40px;
  }
  
  .decoration-title {
    font-size: 28px;
  }
  
  .decoration-subtitle {
    font-size: 16px;
  }
  
  .decoration-desc {
    font-size: 16px;
    margin-bottom: 35px;
  }
  
  .logo-icon-wrapper {
    width: 55px;
    height: 55px;
  }
  
  .decoration-icon {
    font-size: 28px;
  }
  
  .decoration-features {
    margin-bottom: 35px;
  }
  
  .feature-item {
    padding: 18px;
    margin-bottom: 18px;
  }
  
  .feature-icon {
    width: 42px;
    height: 42px;
    font-size: 22px;
  }
  
  .feature-content h4 {
    font-size: 15px;
  }
  
  .feature-content p {
    font-size: 13px;
  }
  
  .stat-item {
    padding: 18px;
  }
  
  .stat-number {
    font-size: 22px;
  }
  
  .stat-label {
    font-size: 13px;
  }
  
  .login-form-section {
    padding: 45px 40px;
  }
  
  .login-box {
    max-width: 380px;
  }
  
  .login-header {
    margin-bottom: 30px;
  }
  
  .login-header h3 {
    font-size: 24px;
  }
  
  .login-header p {
    font-size: 14px;
  }
  
  .header-logo {
    width: 75px;
    height: 75px;
  }
  
  .header-icon {
    font-size: 34px;
  }
  
  .input-label {
    font-size: 13px;
  }
  
  .form-input :deep(.el-input__wrapper) {
    padding: 15px 18px;
    font-size: 15px;
  }
  
  .login-btn {
    height: 50px;
    font-size: 15px;
  }
  
  .tips-card :deep(.el-card__header) {
    padding: 14px 18px;
  }
  
  .tips-header {
    font-size: 13px;
  }
  
  .account-title {
    font-size: 13px;
  }
  
  .account-item {
    font-size: 12px;
  }
  
  .account-info {
    font-size: 12px;
  }
  
  .login-footer {
    font-size: 13px;
  }
  
  .footer-link {
    font-size: 12px;
  }
}

/* 超高分辨率屏幕优化 (2K及以上) */
@media (min-width: 1920px) {
  .login-container {
    padding: 0;
  }
  
  .login-content {
    width: 100%;
    height: 100vh;
    border-radius: 0;
  }
  
  .login-decoration {
    padding: 55px 50px;
  }
  
  .decoration-title {
    font-size: 32px;
  }
  
  .decoration-subtitle {
    font-size: 18px;
  }
  
  .decoration-desc {
    font-size: 18px;
    margin-bottom: 40px;
  }
  
  .logo-icon-wrapper {
    width: 65px;
    height: 65px;
  }
  
  .decoration-icon {
    font-size: 32px;
  }
  
  .decoration-features {
    margin-bottom: 40px;
  }
  
  .feature-item {
    padding: 20px;
    margin-bottom: 20px;
    border-radius: 12px;
  }
  
  .feature-icon {
    width: 48px;
    height: 48px;
    font-size: 26px;
    margin-right: 18px;
  }
  
  .feature-content h4 {
    font-size: 16px;
  }
  
  .feature-content p {
    font-size: 14px;
  }
  
  .stat-item {
    padding: 20px;
  }
  
  .stat-number {
    font-size: 26px;
  }
  
  .stat-label {
    font-size: 14px;
  }
  
  .login-form-section {
    padding: 55px 50px;
  }
  
  .login-box {
    max-width: 420px;
  }
  
  .login-header {
    margin-bottom: 35px;
  }
  
  .login-header h3 {
    font-size: 28px;
  }
  
  .login-header p {
    font-size: 16px;
  }
  
  .header-logo {
    width: 85px;
    height: 85px;
    border-radius: 20px;
    margin-bottom: 18px;
  }
  
  .header-icon {
    font-size: 40px;
  }
  
  .input-label {
    font-size: 13px;
    margin-bottom: 7px;
  }
  
  .form-input :deep(.el-input__wrapper) {
    padding: 16px 22px;
    font-size: 16px;
    border-radius: 10px;
  }
  
  .login-btn {
    height: 54px;
    font-size: 16px;
    border-radius: 10px;
  }
  
  .tips-card {
    border-radius: 12px;
  }
  
  .tips-card :deep(.el-card__header) {
    padding: 16px 20px;
  }
  
  .tips-header {
    font-size: 15px;
  }
  
  .account-title {
    font-size: 15px;
  }
  
  .account-item {
    font-size: 14px;
  }
  
  .account-info {
    font-size: 14px;
  }
  
  .login-footer {
    font-size: 15px;
  }
  
  .footer-link {
    font-size: 14px;
  }
}

@media (min-width: 2560px) {
  .login-container {
    padding: 0;
  }
  
  .login-content {
    width: 100%;
    height: 100vh;
  }
  
  .login-decoration {
    padding: 65px 60px;
  }
  
  .decoration-title {
    font-size: 36px;
  }
  
  .decoration-subtitle {
    font-size: 20px;
  }
  
  .decoration-desc {
    font-size: 20px;
    margin-bottom: 45px;
  }
  
  .logo-icon-wrapper {
    width: 75px;
    height: 75px;
  }
  
  .decoration-icon {
    font-size: 36px;
  }
  
  .decoration-features {
    margin-bottom: 45px;
  }
  
  .feature-item {
    padding: 24px;
    margin-bottom: 24px;
  }
  
  .feature-icon {
    width: 54px;
    height: 54px;
    font-size: 30px;
  }
  
  .feature-content h4 {
    font-size: 18px;
  }
  
  .feature-content p {
    font-size: 16px;
  }
  
  .stat-item {
    padding: 24px;
  }
  
  .stat-number {
    font-size: 30px;
  }
  
  .stat-label {
    font-size: 16px;
  }
  
  .login-form-section {
    padding: 65px 60px;
  }
  
  .login-box {
    max-width: 450px;
  }
  
  .login-header h3 {
    font-size: 32px;
  }
  
  .login-header p {
    font-size: 18px;
  }
  
  .header-logo {
    width: 95px;
    height: 95px;
  }
  
  .header-icon {
    font-size: 45px;
  }
  
  .form-input :deep(.el-input__wrapper) {
    padding: 18px 26px;
    font-size: 18px;
  }
  
  .login-btn {
    height: 58px;
    font-size: 18px;
  }
  
  .tips-header {
    font-size: 15px;
  }
  
  .account-title {
    font-size: 15px;
  }
  
  .account-item {
    font-size: 14px;
  }
  
  .account-info {
    font-size: 14px;
  }
  
  .login-footer {
    font-size: 15px;
  }
  
  .footer-link {
    font-size: 14px;
  }
}
</style>

