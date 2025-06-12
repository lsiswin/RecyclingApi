<template>
  <div class="register-container">
    <div class="register-content">
      <!-- 左侧装饰区域 -->
      <div class="register-decoration">
        <div class="decoration-content">
          <div class="decoration-logo">
            <div class="logo-icon-wrapper">
              <el-icon class="decoration-icon"><Recycle /></el-icon>
            </div>
            <div class="logo-text">
              <h1 class="decoration-title">IT设备回收</h1>
              <h2 class="decoration-subtitle">用户注册</h2>
            </div>
          </div>
          <p class="decoration-desc">加入我们的环保事业<br/>共建绿色未来</p>
          
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

      <!-- 右侧注册表单 -->
      <div class="register-form-section">
        <div class="register-box">
          <div class="register-header">
            <div class="header-logo">
              <el-icon class="header-icon"><UserFilled /></el-icon>
            </div>
            <h3>创建账户</h3>
            <p>请填写您的信息以创建新账户</p>
          </div>
          
          <el-form
            ref="registerFormRef"
            :model="registerForm"
            :rules="registerRules"
            class="register-form"
            @submit.prevent="handleRegister"
          >
            <el-form-item prop="realName">
              <div class="input-wrapper">
                <label class="input-label">真实姓名</label>
                <el-input
                  v-model="registerForm.realName"
                  placeholder="请输入真实姓名"
                  size="large"
                  prefix-icon="User"
                  class="form-input"
                />
              </div>
            </el-form-item>

            <el-form-item prop="username">
              <div class="input-wrapper">
                <label class="input-label">用户名</label>
                <el-input
                  v-model="registerForm.username"
                  placeholder="请输入用户名"
                  size="large"
                  prefix-icon="Avatar"
                  class="form-input"
                />
              </div>
            </el-form-item>

            <el-form-item prop="email">
              <div class="input-wrapper">
                <label class="input-label">邮箱地址</label>
                <el-input
                  v-model="registerForm.email"
                  placeholder="请输入邮箱地址"
                  size="large"
                  prefix-icon="Message"
                  class="form-input"
                />
              </div>
            </el-form-item>

            <el-form-item prop="userType">
              <div class="input-wrapper">
                <label class="input-label">用户类型</label>
                <el-select
                  v-model="registerForm.userType"
                  placeholder="请选择用户类型"
                  size="large"
                  class="form-input"
                  style="width: 100%"
                >
                  <el-option label="普通用户" value="Customer" />
                  <el-option label="员工" value="Staff" />
                </el-select>
              </div>
            </el-form-item>
            
            <el-form-item prop="password">
              <div class="input-wrapper">
                <label class="input-label">密码</label>
                <el-input
                  v-model="registerForm.password"
                  type="password"
                  placeholder="请输入密码"
                  size="large"
                  prefix-icon="Lock"
                  show-password
                  class="form-input"
                />
              </div>
            </el-form-item>

            <el-form-item prop="confirmPassword">
              <div class="input-wrapper">
                <label class="input-label">确认密码</label>
                <el-input
                  v-model="registerForm.confirmPassword"
                  type="password"
                  placeholder="请再次输入密码"
                  size="large"
                  prefix-icon="Lock"
                  show-password
                  class="form-input"
                  @keyup.enter="handleRegister"
                />
              </div>
            </el-form-item>
            
            <el-form-item>
              <el-button
                type="primary"
                size="large"
                :loading="authStore.loading"
                @click="handleRegister"
                class="register-btn"
              >
                <span v-if="!authStore.loading">创建账户</span>
                <span v-else>注册中...</span>
              </el-button>
            </el-form-item>
          </el-form>
          
          <div class="register-footer">
            <p>已有账户？ <router-link to="/login" class="footer-link">立即登录</router-link></p>
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
import {  Headset } from '@element-plus/icons-vue'

const router = useRouter()
const authStore = useAuthStore()
const registerFormRef = ref<FormInstance>()

// 功能特性数据
const features = ref([
  {
    icon: Headset,
    title: '安全保障',
    description: '数据安全加密保护'
  },
  {
    icon: Headset,
    title: '环保奖励',
    description: '回收积分兑换礼品'
  },
  {
    icon: Headset,
    title: '专业服务',
    description: '7x24小时客服支持'
  }
])

// 统计数据
const stats = ref([
  { number: '50,000+', label: '注册用户' },
  { number: '1,000+', label: '合作企业' },
  { number: '99.8%', label: '满意度' }
])

const registerForm = reactive({
  realName: '',
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
  userType: 'Customer' as 'Customer' | 'Staff'
})

// 验证确认密码
const validateConfirmPassword = (rule: any, value: any, callback: any) => {
  if (value === '') {
    callback(new Error('请再次输入密码'))
  } else if (value !== registerForm.password) {
    callback(new Error('两次输入密码不一致'))
  } else {
    callback()
  }
}

const registerRules: FormRules = {
  realName: [
    { required: true, message: '请输入真实姓名', trigger: 'blur' },
    { min: 2, max: 20, message: '姓名长度在 2 到 20 个字符', trigger: 'blur' }
  ],
  username: [
    { required: true, message: '请输入用户名', trigger: 'blur' },
    { min: 3, max: 20, message: '用户名长度在 3 到 20 个字符', trigger: 'blur' },
    { pattern: /^[a-zA-Z0-9_]+$/, message: '用户名只能包含字母、数字和下划线', trigger: 'blur' }
  ],
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur' }
  ],
  userType: [
    { required: true, message: '请选择用户类型', trigger: 'change' }
  ],
  password: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 6, max: 20, message: '密码长度在 6 到 20 个字符', trigger: 'blur' },
    { pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/, message: '密码必须包含大小写字母和数字', trigger: 'blur' }
  ],
  confirmPassword: [
    { required: true, message: '请确认密码', trigger: 'blur' },
    { validator: validateConfirmPassword, trigger: 'blur' }
  ]
}

const handleRegister = async () => {
  if (!registerFormRef.value) return
  
  await registerFormRef.value.validate(async (valid) => {
    if (valid) {
      try {
        const result = await authStore.register({
          realName: registerForm.realName,
          username: registerForm.username,
          email: registerForm.email,
          password: registerForm.password,
          confirmPassword: registerForm.password, // 添加确认密码字段
          phoneNumber: '', // 添加必需的电话号码字段
          userType: registerForm.userType
        })
        
        if (result.success) {
          ElMessage.success('注册成功！请登录您的账户')
          router.push('/login')
        } else {
          ElMessage.error(result.message || '注册失败')
        }
      } catch (error) {
        ElMessage.error('注册过程中发生错误')
      }
    }
  })
}
</script>

<style scoped>
/* 复用登录页面的样式，只修改必要的部分 */
.register-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  position: relative;
  overflow: hidden;
}

.register-container::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="grain" width="100" height="100" patternUnits="userSpaceOnUse"><circle cx="25" cy="25" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="75" cy="75" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="50" cy="10" r="0.5" fill="rgba(255,255,255,0.05)"/><circle cx="10" cy="50" r="0.5" fill="rgba(255,255,255,0.05)"/><circle cx="90" cy="30" r="0.5" fill="rgba(255,255,255,0.05)"/></pattern></defs><rect width="100" height="100" fill="url(%23grain)"/></svg>');
  opacity: 0.3;
}

.register-content {
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
.register-decoration {
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

/* 右侧注册表单 */
.register-form-section {
  flex: 1;
  padding: 20px 30px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  background: white;
  min-height: 100%;
  overflow-y: auto;
}

.register-box {
  max-width: 380px;
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

.register-header {
  text-align: center;
  margin-bottom: 20px;
}

.header-logo {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 12px;
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
  transition: all 0.3s ease;
}

.header-logo:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 35px rgba(102, 126, 234, 0.4);
}

.header-icon {
  font-size: 24px;
  color: white;
}

.register-header h3 {
  font-size: 18px;
  font-weight: 700;
  color: #1a202c;
  margin: 0 0 5px 0;
}

.register-header p {
  font-size: 11px;
  color: #718096;
  margin: 0;
}

.register-form {
  margin-bottom: 15px;
}

.input-wrapper {
  margin-bottom: 15px;
}

.input-label {
  display: block;
  font-size: 11px;
  font-weight: 600;
  color: #374151;
  margin-bottom: 4px;
}

.form-input {
  width: 100%;
}

.form-input :deep(.el-input__wrapper) {
  border-radius: 6px;
  border: 2px solid #e5e7eb;
  padding: 10px 12px;
  font-size: 12px;
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

.form-input :deep(.el-select .el-input__wrapper) {
  border-radius: 6px;
  border: 2px solid #e5e7eb;
  padding: 10px 12px;
  font-size: 12px;
  transition: all 0.3s ease;
}

.register-btn {
  width: 100%;
  height: 38px;
  font-size: 12px;
  font-weight: 600;
  border-radius: 6px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.register-btn::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: left 0.5s;
}

.register-btn:hover::before {
  left: 100%;
}

.register-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(102, 126, 234, 0.4);
}

.register-footer {
  text-align: center;
  color: #9ca3af;
  font-size: 10px;
}

.footer-links {
  margin-top: 5px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 4px;
}

.footer-link {
  color: #6b7280;
  text-decoration: none;
  transition: color 0.3s ease;
  font-size: 9px;
}

.footer-link:hover {
  color: #374151;
}

.divider {
  color: #d1d5db;
  font-size: 9px;
}

/* 响应式设计 */
@media (max-width: 768px) {
  .register-content {
    flex-direction: column;
    height: auto;
  }
  
  .register-decoration {
    flex: none;
    padding: 25px 20px;
    min-height: 300px;
  }
  
  .register-form-section {
    padding: 25px 20px;
    min-height: auto;
  }
  
  .register-box {
    max-width: 280px;
  }
}
</style> 