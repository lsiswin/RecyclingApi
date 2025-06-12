<template>
  <div class="register-page">
    <el-row class="register-container" justify="center">
      <el-col :xs="22" :sm="16" :md="12" :lg="8">
        <el-card class="register-card" shadow="hover">
          <div class="register-header">
            <div class="logo">
              <el-image :src="logoImage" class="logo-image" fit="cover" />
              <h2>IT设备回收</h2>
            </div>
            <p class="subtitle">创建您的账户，开始设备回收之旅</p>
          </div>
          
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
              立即注册
            </el-button>
          </el-form>
          
          <div class="third-party-login">
            <div class="divider">
              <span>第三方账号注册</span>
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
            <p>已有账号？ <el-link type="primary" @click="navigateToLogin">立即登录</el-link></p>
            <el-button text type="primary" @click="$router.push('/')">返回首页</el-button>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { User, Lock, Message, Phone, ChatDotRound, Connection } from '@element-plus/icons-vue'
// 导入图片
import logoImage from '@/assets/index.jpeg'

const router = useRouter()
const registerFormRef = ref()
const registering = ref(false)

// 注册表单
const registerForm = reactive({
  username: '',
  email: '',
  phone: '',
  password: '',
  confirmPassword: '',
  agreement: false
})

// 验证规则
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

// 处理注册
const handleRegister = async () => {
  try {
    await registerFormRef.value.validate()
    registering.value = true
    
    // 模拟注册请求
    setTimeout(() => {
      registering.value = false
      ElMessage.success('注册成功，请登录')
      router.push('/login')
    }, 1500)
  } catch (error) {
    console.error(error)
  }
}

// 导航到登录页
const navigateToLogin = () => {
  router.push('/login')
}

// 页面加载时重定向到登录页面
onMounted(() => {
  ElMessage.info('注册功能已集成到登录页面，正在为您跳转...')
  setTimeout(() => {
    router.push('/login')
  }, 2000)
})
</script>

<style scoped lang="scss">
.register-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 0;
}

.register-container {
  width: 100%;
}

.register-card {
  padding: 20px;
  
  .register-header {
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
    
    p {
      margin-bottom: 16px;
      color: #606266;
    }
  }
}

@media (max-width: 768px) {
  .register-page {
    padding: 20px 0;
  }
  
  .register-card {
    .register-header {
      .logo {
        h2 {
          font-size: 20px;
        }
      }
    }
  }
}
</style> 