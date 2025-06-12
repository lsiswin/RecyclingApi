<template>
  <div class="forgot-password-container">
    <div class="forgot-password-content">
      <div class="forgot-password-box">
        <div class="header">
          <div class="header-logo">
            <el-icon class="header-icon"><Lock /></el-icon>
          </div>
          <h3>忘记密码</h3>
          <p>请输入您的邮箱地址，我们将发送重置密码链接</p>
        </div>
        
        <el-form
          ref="forgotPasswordFormRef"
          :model="forgotPasswordForm"
          :rules="forgotPasswordRules"
          class="forgot-password-form"
          @submit.prevent="handleForgotPassword"
        >
          <el-form-item prop="email">
            <div class="input-wrapper">
              <label class="input-label">邮箱地址</label>
              <el-input
                v-model="forgotPasswordForm.email"
                placeholder="请输入您的邮箱地址"
                size="large"
                prefix-icon="Message"
                class="form-input"
                @keyup.enter="handleForgotPassword"
              />
            </div>
          </el-form-item>
          
          <el-form-item>
            <el-button
              type="primary"
              size="large"
              :loading="authStore.loading"
              @click="handleForgotPassword"
              class="submit-btn"
            >
              <span v-if="!authStore.loading">发送重置链接</span>
              <span v-else>发送中...</span>
            </el-button>
          </el-form-item>
        </el-form>
        
        <div class="footer">
          <p>记起密码了？ <router-link to="/login" class="footer-link">返回登录</router-link></p>
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

const router = useRouter()
const authStore = useAuthStore()
const forgotPasswordFormRef = ref<FormInstance>()

const forgotPasswordForm = reactive({
  email: ''
})

const forgotPasswordRules: FormRules = {
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur' }
  ]
}

const handleForgotPassword = async () => {
  if (!forgotPasswordFormRef.value) return
  
  await forgotPasswordFormRef.value.validate(async (valid) => {
    if (valid) {
      try {
        // const result = await authStore.forgotPassword(forgotPasswordForm.email)
        
        // if (result.success) {
        //   ElMessage.success('重置密码邮件已发送，请查收')
        //   router.push('/login')
        // } else {
        //   ElMessage.error(result.message || '发送失败')
        // }
      } catch (error) {
        ElMessage.error('发送过程中发生错误')
      }
    }
  })
}
</script>

<style scoped>
.forgot-password-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  position: relative;
  overflow: hidden;
}

.forgot-password-container::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="grain" width="100" height="100" patternUnits="userSpaceOnUse"><circle cx="25" cy="25" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="75" cy="75" r="1" fill="rgba(255,255,255,0.1)"/><circle cx="50" cy="10" r="0.5" fill="rgba(255,255,255,0.05)"/><circle cx="10" cy="50" r="0.5" fill="rgba(255,255,255,0.05)"/><circle cx="90" cy="30" r="0.5" fill="rgba(255,255,255,0.05)"/></pattern></defs><rect width="100" height="100" fill="url(%23grain)"/></svg>');
  opacity: 0.3;
}

.forgot-password-content {
  position: relative;
  z-index: 1;
  width: 100%;
  max-width: 400px;
}

.forgot-password-box {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 20px;
  padding: 40px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.2);
  backdrop-filter: blur(10px);
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

.header {
  text-align: center;
  margin-bottom: 30px;
}

.header-logo {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
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

.header h3 {
  font-size: 24px;
  font-weight: 700;
  color: #1a202c;
  margin: 0 0 10px 0;
}

.header p {
  font-size: 14px;
  color: #718096;
  margin: 0;
  line-height: 1.5;
}

.forgot-password-form {
  margin-bottom: 20px;
}

.input-wrapper {
  margin-bottom: 20px;
}

.input-label {
  display: block;
  font-size: 14px;
  font-weight: 600;
  color: #374151;
  margin-bottom: 8px;
}

.form-input {
  width: 100%;
}

.form-input :deep(.el-input__wrapper) {
  border-radius: 10px;
  border: 2px solid #e5e7eb;
  padding: 14px 16px;
  font-size: 14px;
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

.submit-btn {
  width: 100%;
  height: 48px;
  font-size: 16px;
  font-weight: 600;
  border-radius: 10px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.submit-btn::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: left 0.5s;
}

.submit-btn:hover::before {
  left: 100%;
}

.submit-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(102, 126, 234, 0.4);
}

.footer {
  text-align: center;
  color: #9ca3af;
  font-size: 14px;
}

.footer-link {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
  transition: color 0.3s ease;
}

.footer-link:hover {
  color: #5a67d8;
}

/* 响应式设计 */
@media (max-width: 768px) {
  .forgot-password-container {
    padding: 20px;
  }
  
  .forgot-password-box {
    padding: 30px 20px;
    border-radius: 16px;
  }
  
  .header h3 {
    font-size: 20px;
  }
  
  .header p {
    font-size: 12px;
  }
  
  .header-logo {
    width: 50px;
    height: 50px;
  }
  
  .header-icon {
    font-size: 24px;
  }
  
  .input-label {
    font-size: 12px;
  }
  
  .form-input :deep(.el-input__wrapper) {
    padding: 12px 14px;
    font-size: 13px;
  }
  
  .submit-btn {
    height: 42px;
    font-size: 14px;
  }
  
  .footer {
    font-size: 12px;
  }
}
</style> 