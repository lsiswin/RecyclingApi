<template>
  <div class="contact-page">
    <!-- 页面头部 -->
    <el-row class="page-header" justify="center">
      <el-col :span="20" :lg="16">
        <h1>联系我们</h1>
        <p class="subtitle">我们随时为您提供专业的IT设备回收服务咨询</p>
      </el-col>
    </el-row>

    <!-- 在线咨询表单 - 移到最上面 -->
    <el-row class="contact-form-section" justify="center">
      <el-col :span="20" :lg="16">
        <el-card shadow="hover">
          <template #header>
            <div class="form-header">
              <h2>在线咨询</h2>
              <p>请填写以下信息，我们将尽快与您联系</p>
              <div class="consultation-options">
                <el-button type="primary" @click="openChat" size="large">
                  <el-icon><Message /></el-icon>
                  立即在线咨询
                </el-button>
                <span class="or-divider">或</span>
                <span class="form-option">填写表单咨询</span>
              </div>
            </div>
          </template>
          
          <el-form 
            ref="contactFormRef" 
            :model="contactForm" 
            :rules="contactRules" 
            label-width="100px"
            size="large"
          >
            <el-row :gutter="20">
              <el-col :span="24" :md="12">
                <el-form-item label="姓名" prop="name">
                  <el-input v-model="contactForm.name" placeholder="请输入您的姓名" />
                </el-form-item>
              </el-col>
              
              <el-col :span="24" :md="12">
                <el-form-item label="公司名称" prop="company">
                  <el-input v-model="contactForm.company" placeholder="请输入公司名称" />
                </el-form-item>
              </el-col>
            </el-row>
            
            <el-row :gutter="20">
              <el-col :span="24" :md="12">
                <el-form-item label="联系电话" prop="phone">
                  <el-input v-model="contactForm.phone" placeholder="请输入联系电话" />
                </el-form-item>
              </el-col>
              
              <el-col :span="24" :md="12">
                <el-form-item label="邮箱" prop="email">
                  <el-input v-model="contactForm.email" placeholder="请输入邮箱地址" />
                </el-form-item>
              </el-col>
            </el-row>
            
            <el-form-item label="设备类型" prop="deviceType">
              <el-select v-model="contactForm.deviceType" placeholder="请选择设备类型" style="width: 100%">
                <el-option label="台式电脑" value="desktop" />
                <el-option label="笔记本电脑" value="laptop" />
                <el-option label="服务器" value="server" />
                <el-option label="网络设备" value="network" />
                <el-option label="打印设备" value="printer" />
                <el-option label="其他设备" value="other" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="设备数量" prop="quantity">
              <el-input-number v-model="contactForm.quantity" :min="1" :max="10000" />
              <span class="unit">台</span>
            </el-form-item>
            
            <el-form-item label="详细需求" prop="message">
              <el-input 
                v-model="contactForm.message" 
                type="textarea" 
                :rows="5" 
                placeholder="请详细描述您的回收需求，包括设备型号、使用年限、期望回收时间等"
              />
            </el-form-item>
            
            <el-form-item>
              <el-button type="primary" @click="submitForm" :loading="submitting">
                提交咨询
              </el-button>
              <el-button @click="resetForm">重置</el-button>
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
    </el-row>

    <!-- 联系卡片 -->
    <el-row class="contact-cards" justify="center" :gutter="30">
      <el-col :xs="24" :sm="12" :lg="6">
        <el-card shadow="hover" class="contact-card">
          <div class="card-icon">
            <el-icon :size="36"><Phone /></el-icon>
          </div>
          <h3>电话咨询</h3>
          <p>400-123-4567</p>
          <p>010-12345678</p>
          <div class="time">工作时间：9:00-18:00</div>
        </el-card>
      </el-col>
      
      <el-col :xs="24" :sm="12" :lg="6">
        <el-card shadow="hover" class="contact-card">
          <div class="card-icon">
            <el-icon :size="36"><Message /></el-icon>
          </div>
          <h3>邮箱联系</h3>
          <p>info@recycling.com</p>
          <p>service@recycling.com</p>
          <div class="time">24小时内回复</div>
        </el-card>
      </el-col>
      
      <el-col :xs="24" :sm="12" :lg="6">
        <el-card shadow="hover" class="contact-card">
          <div class="card-icon">
            <el-icon :size="36"><Location /></el-icon>
          </div>
          <h3>公司地址</h3>
          <p>北京市朝阳区科技园区</p>
          <p>创新大厦A座15层</p>
          <div class="time">欢迎预约参观</div>
        </el-card>
      </el-col>
      
      <el-col :xs="24" :sm="12" :lg="6">
        <el-card shadow="hover" class="contact-card">
          <div class="card-icon">
            <el-icon :size="36"><Clock /></el-icon>
          </div>
          <h3>服务时间</h3>
          <p>周一至周五：9:00-18:00</p>
          <p>周六：9:00-17:00</p>
          <div class="time">节假日请提前预约</div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import { ref, reactive, inject } from 'vue'
import { ElMessage } from 'element-plus'
import { Phone, Message, Location, Clock } from '@element-plus/icons-vue'
// 导入图片
import contactImage from '@/assets/index.jpeg'

const contactFormRef = ref()
const submitting = ref(false)

// 获取全局聊天组件的引用
const globalChat = inject('globalChat', null)

const contactForm = reactive({
  name: '',
  company: '',
  phone: '',
  email: '',
  deviceType: '',
  quantity: 1,
  message: ''
})

const contactRules = {
  name: [
    { required: true, message: '请输入姓名', trigger: 'blur' }
  ],
  company: [
    { required: true, message: '请输入公司名称', trigger: 'blur' }
  ],
  phone: [
    { required: true, message: '请输入联系电话', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码', trigger: 'blur' }
  ],
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
  ],
  deviceType: [
    { required: true, message: '请选择设备类型', trigger: 'change' }
  ],
  message: [
    { required: true, message: '请描述您的回收需求', trigger: 'blur' }
  ]
}

const submitForm = async () => {
  try {
    await contactFormRef.value.validate()
    submitting.value = true
    
    // 模拟提交
    setTimeout(() => {
      submitting.value = false
      ElMessage.success('提交成功！我们将在24小时内与您联系')
      
      // 重置表单
      contactFormRef.value.resetFields()
    }, 2000)
    
  } catch (error) {
    ElMessage.error('请完善表单信息')
  }
}

const resetForm = () => {
  contactFormRef.value.resetFields()
}

// 打开在线聊天
const openChat = () => {
  // 触发全局聊天组件的展开方法
  const chatWidget = document.querySelector('.global-chat-widget')
  if (chatWidget) {
    const chatButton = chatWidget.querySelector('.chat-button')
    if (chatButton) {
      chatButton.click()
    }
  }
}
</script>

<style scoped lang="scss">
.contact-page {
  padding: 40px 0;
  background-color: #f5f7fa;
}

.page-header {
  text-align: center;
  margin-bottom: 40px;
  
  h1 {
    font-size: 36px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 16px;
  }
  
  .subtitle {
    font-size: 16px;
    color: #606266;
    max-width: 800px;
    margin: 0 auto;
  }
}

.contact-cards {
  margin-bottom: 40px;
  
  .contact-card {
    height: 100%;
    text-align: center;
    padding: 30px 20px;
    margin-bottom: 20px;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .card-icon {
      color: #409EFF;
      margin-bottom: 20px;
    }
    
    h3 {
      font-size: 18px;
      font-weight: 600;
      margin-bottom: 16px;
      color: #303133;
    }
    
    p {
      margin-bottom: 8px;
      color: #606266;
    }
    
    .time {
      margin-top: 16px;
      padding: 6px 12px;
      background-color: #f0f2f5;
      color: #909399;
      border-radius: 15px;
      display: inline-block;
      font-size: 12px;
    }
  }
}

.contact-form-section {
  margin-bottom: 40px;
  
  .form-header {
    text-align: center;
    
    h2 {
      font-size: 24px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 8px;
    }
    
    p {
      color: #606266;
      font-size: 14px;
      margin-bottom: 20px;
    }
    
    .consultation-options {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 16px;
      margin-top: 20px;
      padding: 20px;
      background: #f8f9fa;
      border-radius: 8px;
      
      .el-button {
        padding: 12px 24px;
        font-size: 16px;
        border-radius: 25px;
        
        .el-icon {
          margin-right: 8px;
        }
      }
      
      .or-divider {
        color: #909399;
        font-size: 14px;
        font-weight: 500;
        position: relative;
        
        &::before,
        &::after {
          content: '';
          position: absolute;
          top: 50%;
          width: 20px;
          height: 1px;
          background: #e4e7ed;
        }
        
        &::before {
          left: -30px;
        }
        
        &::after {
          right: -30px;
        }
      }
      
      .form-option {
        color: #606266;
        font-size: 14px;
      }
    }
  }
  
  .unit {
    margin-left: 10px;
    color: #606266;
  }
}

@media (max-width: 768px) {
  .page-header {
    h1 {
      font-size: 28px;
    }
  }
}
</style> 