<template>
  <div class="login-page">
    <el-row class="login-container" justify="center">
      <el-col :xs="22" :sm="16" :md="12" :lg="8">
        <el-card class="login-card" shadow="hover">
          <div class="login-header">
            <div class="logo">
              <el-image :src="logoImage" class="logo-image" fit="cover" />
              <h2>IT设备回收</h2>
            </div>
            <p class="subtitle">登录您的账户继续访问</p>
          </div>
          
          <el-tabs v-model="activeTab" class="login-tabs">
            <el-tab-pane label="登录" name="login">
              <el-form
                ref="loginFormRef"
                :model="loginForm"
                :rules="loginRules"
                size="large"
                @submit.prevent="handleLogin"
              >
                <el-form-item prop="username">
                  <el-input
                    v-model="loginForm.username"
                    placeholder="用户名/邮箱/手机号"
                    prefix-icon="User"
                  />
                </el-form-item>

                <el-form-item prop="password">
                  <el-input
                    v-model="loginForm.password"
                    type="password"
                    placeholder="密码"
                    prefix-icon="Lock"
                    show-password
                  />
                </el-form-item>

                <div class="form-options">
                  <el-checkbox v-model="loginForm.remember">记住我</el-checkbox>
                  <el-link type="primary" @click="showForgotDialog">忘记密码？</el-link>
                </div>

                <el-button
                  type="primary"
                  size="large"
                  @click="handleLogin"
                  :loading="logging"
                  class="submit-btn"
                >
                  登录
                </el-button>
              </el-form>
            </el-tab-pane>
            
            <el-tab-pane label="注册" name="register">
              <el-form
                ref="registerFormRef"
                :model="registerForm"
                :rules="registerRules"
                size="large"
                @submit.prevent="handleRegister"
              >
                <el-form-item prop="username">
                  <el-input
                    v-model="registerForm.username"
                    placeholder="用户名"
                    prefix-icon="User"
                  />
                </el-form-item>

                <el-form-item prop="email">
                  <el-input
                    v-model="registerForm.email"
                    placeholder="邮箱地址"
                    prefix-icon="Message"
                  />
                </el-form-item>

                <el-form-item prop="phone">
                  <el-input
                    v-model="registerForm.phone"
                    placeholder="手机号码"
                    prefix-icon="Phone"
                  />
                </el-form-item>

                <el-form-item prop="password">
                  <el-input
                    v-model="registerForm.password"
                    type="password"
                    placeholder="密码"
                    prefix-icon="Lock"
                    show-password
                  />
                </el-form-item>

                <el-form-item prop="confirmPassword">
                  <el-input
                    v-model="registerForm.confirmPassword"
                    type="password"
                    placeholder="确认密码"
                    prefix-icon="Lock"
                    show-password
                  />
                </el-form-item>

                <el-form-item prop="agreement">
                  <el-checkbox v-model="registerForm.agreement">
                    我已阅读并同意
                    <el-link type="primary">《用户协议》</el-link>
                    和
                    <el-link type="primary">《隐私政策》</el-link>
                  </el-checkbox>
                </el-form-item>

                <el-button
                  type="primary"
                  size="large"
                  @click="handleRegister"
                  :loading="registering"
                  class="submit-btn"
                >
                  注册
                </el-button>
              </el-form>
            </el-tab-pane>
          </el-tabs>
          
          <div class="third-party-login">
            <div class="divider">
              <span>第三方账号登录</span>
            </div>
            <div class="third-party-icons">
              <el-button circle type="success">
                <el-icon><ChatDotRound /></el-icon>
              </el-button>
              <el-button circle type="primary">
                <el-icon><Connection /></el-icon>
              </el-button>
              <el-button circle type="warning">
                <el-icon><User /></el-icon>
              </el-button>
            </div>
          </div>
          
          <div class="footer-action">
            <el-button text type="primary" @click="$router.push('/')">返回首页</el-button>
          </div>
        </el-card>
      </el-col>
    </el-row>
    
    <!-- 忘记密码对话框 -->
    <el-dialog
      v-model="forgotDialogVisible"
      title="找回密码"
      width="30%"
      align-center
    >
      <el-form
        ref="forgotFormRef"
        :model="forgotForm"
        :rules="forgotRules"
        size="large"
      >
        <el-form-item prop="email">
          <el-input
            v-model="forgotForm.email"
            placeholder="请输入注册邮箱"
            prefix-icon="Message"
          />
        </el-form-item>

        <el-form-item prop="code">
          <div class="code-input-group">
            <el-input
              v-model="forgotForm.code"
              placeholder="验证码"
              prefix-icon="Key"
            />
            <el-button 
              type="primary" 
              :disabled="codeCountdown > 0" 
              @click="sendVerificationCode"
            >
              {{ codeCountdown > 0 ? `${codeCountdown}s后重发` : '获取验证码' }}
            </el-button>
          </div>
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="forgotDialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleResetPassword" :loading="resetting">
            提交
          </el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { useAuthStore } from '@/store/modules/authStore'
import { User, Lock, Message, Key, Phone, ChatDotRound, Connection } from '@element-plus/icons-vue'
import { sendVerificationCodeApi } from '@/api/sendVerificationCode'
// 导入图片
import logoImage from '@/assets/index.jpeg'

const router = useRouter()
const authStore = useAuthStore()
const activeTab = ref('login')
const loginFormRef = ref()
const registerFormRef = ref()
const forgotFormRef = ref()

const logging = ref(false)
const registering = ref(false)
const resetting = ref(false)
const forgotDialogVisible = ref(false)
const codeCountdown = ref(0)

// 登录表单
const loginForm = reactive({
  username: '',
  password: '',
  remember: false
})

// 注册表单
const registerForm = reactive({
  username: '',
  email: '',
  phone: '', // 后端接口使用phoneNumber，发送时需要转换
  password: '',
  confirmPassword: '',
  agreement: false
})

// 忘记密码表单
const forgotForm = reactive({
  email: '',
  code: ''
})

// 验证规则
const loginRules = {
  username: [
    { required: true, message: '请输入用户名或邮箱', trigger: 'blur' }
  ],
  password: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 6, message: '密码长度不能少于6位', trigger: 'blur' }
  ]
}

const registerRules = {
  username: [
    { required: true, message: '请输入用户名', trigger: 'blur' },
    { min: 3, max: 20, message: '用户名长度在3到20个字符', trigger: 'blur' }
  ],
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
  ],
  phone: [
    { required: true, message: '请输入手机号码', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码', trigger: 'blur' }
  ],
  password: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 6, message: '密码长度不能少于6位', trigger: 'blur' }
  ],
  confirmPassword: [
    { required: true, message: '请确认密码', trigger: 'blur' },
    {
      validator: (rule, value, callback) => {
        if (value !== registerForm.password) {
          callback(new Error('两次输入密码不一致'))
        } else {
          callback()
        }
      },
      trigger: 'blur'
    }
  ],
  agreement: [
    {
      validator: (rule, value, callback) => {
        if (!value) {
          callback(new Error('请阅读并同意用户协议'))
        } else {
          callback()
        }
      },
      trigger: 'change'
    }
  ]
}

const forgotRules = {
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
  ],
  code: [
    { required: true, message: '请输入验证码', trigger: 'blur' },
    { min: 6, max: 6, message: '验证码为6位数字', trigger: 'blur' }
  ]
}

// 检查用户是否已登录
onMounted(async () => {
  const isAuthenticated = await authStore.checkAuth()
  if (isAuthenticated) {
    router.push('/')
  }
})

// 处理登录
const handleLogin = async () => {
  try {
    await loginFormRef.value.validate()
    logging.value = true
    
    // 调用登录API
    const result = await authStore.login({
      username: loginForm.username,
      password: loginForm.password,
      remember: loginForm.remember
    })
    
    logging.value = false
    
    if (result.success) {
      ElMessage.success(result.message || '登录成功')
      router.push('/')
    } else {
      ElMessage.error(result.message || '登录失败，请检查用户名和密码')
    }
  } catch (error) {
    logging.value = false
    console.error('登录错误:', error)
  }
}

// 处理注册
const handleRegister = async () => {
  try {
    await registerFormRef.value.validate()
    registering.value = true
    
    // 调用注册API
    const result = await authStore.register({
      username: registerForm.username,
      email: registerForm.email,
      phoneNumber: registerForm.phone,
      password: registerForm.password
    })
    
    registering.value = false
    
    if (result.success) {
      ElMessage.success(result.message || '注册成功，请登录')
      activeTab.value = 'login'
      
      // 清空表单
      registerForm.username = ''
      registerForm.email = ''
      registerForm.phone = ''
      registerForm.password = ''
      registerForm.confirmPassword = ''
      registerForm.agreement = false
    } else {
      ElMessage.error(result.message || '注册失败，请检查输入信息')
    }
  } catch (error) {
    registering.value = false
    console.error('注册错误:', error)
  }
}

// 显示忘记密码对话框
const showForgotDialog = () => {
  forgotDialogVisible.value = true
}

// 发送验证码
const sendVerificationCode = async () => {
  if (!forgotForm.email) {
    ElMessage.warning('请先输入邮箱地址')
    return
  }
  
  try {
    const result = await sendVerificationCodeApi.sendToEmail(forgotForm.email, 'resetPassword')
    
    if (result.success) {
      ElMessage.success(result.message || '验证码已发送至您的邮箱')
      
      // 启动倒计时
      codeCountdown.value = 60
      const timer = setInterval(() => {
        codeCountdown.value--
        if (codeCountdown.value <= 0) {
          clearInterval(timer)
        }
      }, 1000)
    } else {
      ElMessage.error(result.message || '验证码发送失败')
    }
  } catch (error) {
    console.error('发送验证码错误:', error)
    ElMessage.error('验证码发送失败，请稍后重试')
  }
}

// 处理重置密码
const handleResetPassword = async () => {
  try {
    await forgotFormRef.value.validate()
    resetting.value = true
    
    // 先验证验证码
    const verifyResult = await sendVerificationCodeApi.verify({
      email: forgotForm.email,
      code: forgotForm.code,
      type: 'resetPassword'
    })
    
    if (!verifyResult.success) {
      resetting.value = false
      ElMessage.error(verifyResult.message || '验证码验证失败')
      return
    }
    
    // 调用重置密码API
    const result = await authStore.forgotPassword(forgotForm.email)
    
    resetting.value = false
    
    if (result.success) {
      ElMessage.success(result.message || '密码重置链接已发送至您的邮箱')
      forgotDialogVisible.value = false
    } else {
      ElMessage.error(result.message || '密码重置失败')
    }
  } catch (error) {
    resetting.value = false
    console.error('重置密码错误:', error)
  }
}
</script>

<style scoped lang="scss">
.login-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 0;
}

.login-container {
  width: 100%;
}

.login-card {
  padding: 20px;
  
  .login-header {
    text-align: center;
    margin-bottom: 30px;
    
    .logo {
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: 16px;
      
      .logo-image {
        width: 50px;
        height: 50px;
        border-radius: 8px;
        margin-right: 10px;
      }
      
      h2 {
        font-size: 24px;
        font-weight: 600;
        color: #303133;
        margin: 0;
      }
    }
    
    .subtitle {
      font-size: 14px;
      color: #606266;
    }
  }
  
  .login-tabs {
    margin-bottom: 20px;
  }
  
  .form-options {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
  }
  
  .submit-btn {
    width: 100%;
    margin-bottom: 20px;
  }
  
  .third-party-login {
    margin-bottom: 20px;
    
    .divider {
      display: flex;
      align-items: center;
      margin: 20px 0;
      
      &:before,
      &:after {
        content: '';
        flex: 1;
        height: 1px;
        background-color: #dcdfe6;
      }
      
      span {
        padding: 0 16px;
        font-size: 14px;
        color: #909399;
      }
    }
    
    .third-party-icons {
      display: flex;
      justify-content: center;
      gap: 20px;
    }
  }
  
  .footer-action {
    text-align: center;
  }
}

.code-input-group {
  display: flex;
  gap: 10px;
  
  .el-input {
    flex: 1;
  }
  
  .el-button {
    flex-shrink: 0;
  }
}

@media (max-width: 768px) {
  .login-page {
    padding: 20px 0;
  }
  
  .login-card {
    .login-header {
      .logo {
        h2 {
          font-size: 20px;
        }
      }
    }
  }
}
</style> 