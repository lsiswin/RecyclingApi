<template>
  <div class="jobs-page">
    <!-- 页面头部 -->
    <el-row class="page-header" justify="center">
      <el-col :span="20" :lg="16">
        <h1>加入我们</h1>
        <p class="subtitle">与我们一起创造绿色未来，共建可持续发展的IT回收事业</p>
      </el-col>
    </el-row>

    <!-- 公司文化 -->
    <el-row class="culture-section" justify="center">
      <el-col :span="20" :lg="16">
        <el-card shadow="hover" class="culture-card">
          <h2>为什么选择我们</h2>
          <el-row :gutter="30">
            <el-col :xs="24" :sm="8">
              <div class="culture-item">
                <el-icon :size="48" class="culture-icon"><Trophy /></el-icon>
                <h3>行业领先</h3>
                <p>专业的IT设备回收服务，行业内领先的技术和经验</p>
              </div>
            </el-col>
            <el-col :xs="24" :sm="8">
              <div class="culture-item">
                <el-icon :size="48" class="culture-icon"><User /></el-icon>
                <h3>团队协作</h3>
                <p>开放包容的工作环境，重视团队合作和个人成长</p>
              </div>
            </el-col>
            <el-col :xs="24" :sm="8">
              <div class="culture-item">
                <el-icon :size="48" class="culture-icon"><Star /></el-icon>
                <h3>发展前景</h3>
                <p>广阔的职业发展空间，完善的培训和晋升体系</p>
              </div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>

    <!-- 招聘职位 -->
    <el-row class="positions-section" justify="center">
      <el-col :span="20" :lg="16">
        <h2 class="section-title">招聘职位</h2>
        
        <!-- 职位筛选 -->
        <div class="position-filter">
          <el-select v-model="filterDepartment" placeholder="选择部门" clearable @change="filterPositions">
            <el-option label="全部部门" value="" />
            <el-option label="技术部" value="tech" />
            <el-option label="销售部" value="sales" />
            <el-option label="运营部" value="operations" />
            <el-option label="人事部" value="hr" />
          </el-select>
          
          <el-select v-model="filterLocation" placeholder="工作地点" clearable @change="filterPositions">
            <el-option label="全部地点" value="" />
            <el-option label="北京" value="beijing" />
            <el-option label="上海" value="shanghai" />
            <el-option label="深圳" value="shenzhen" />
            <el-option label="广州" value="guangzhou" />
          </el-select>
        </div>

        <el-row :gutter="30">
          <el-col 
            v-for="position in filteredPositions" 
            :key="position.id" 
            :xs="24" 
            :md="12"
          >
            <el-card shadow="hover" class="position-card" @click="viewPositionDetail(position)">
              <div class="position-header">
                <h3>{{ position.title }}</h3>
                <el-tag :type="getDepartmentType(position.department)">{{ getDepartmentName(position.department) }}</el-tag>
              </div>
              
              <div class="position-info">
                <div class="info-item">
                  <el-icon><Location /></el-icon>
                  <span>{{ position.location }}</span>
                </div>
                <div class="info-item">
                  <el-icon><Money /></el-icon>
                  <span>{{ position.salary }}</span>
                </div>
                <div class="info-item">
                  <el-icon><Calendar /></el-icon>
                  <span>{{ position.experience }}</span>
                </div>
              </div>
              
              <p class="position-description">{{ position.description }}</p>
              
              <div class="position-footer">
                <span class="publish-date">发布时间：{{ position.publishDate }}</span>
                <el-button type="primary" size="small">立即申请</el-button>
              </div>
            </el-card>
          </el-col>
        </el-row>
      </el-col>
    </el-row>

    <!-- 简历投递 -->
    <el-row class="resume-section" justify="center">
      <el-col :span="20" :lg="16">
        <h2 class="section-title">投递简历</h2>
        <el-card shadow="hover" class="resume-card">
          <el-form 
            ref="resumeFormRef" 
            :model="resumeForm" 
            :rules="resumeRules" 
            label-width="100px"
            size="large"
          >
            <el-row :gutter="30">
              <el-col :xs="24" :md="12">
                <el-form-item label="姓名" prop="name">
                  <el-input v-model="resumeForm.name" placeholder="请输入您的姓名" />
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="性别" prop="gender">
                  <el-radio-group v-model="resumeForm.gender">
                    <el-radio value="male">男</el-radio>
                    <el-radio value="female">女</el-radio>
                  </el-radio-group>
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="手机号码" prop="phone">
                  <el-input v-model="resumeForm.phone" placeholder="请输入手机号码" />
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="邮箱地址" prop="email">
                  <el-input v-model="resumeForm.email" placeholder="请输入邮箱地址" />
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="应聘职位" prop="position">
                  <el-select v-model="resumeForm.position" placeholder="选择应聘职位" style="width: 100%;">
                    <el-option 
                      v-for="position in allPositions" 
                      :key="position.id" 
                      :label="position.title" 
                      :value="position.title" 
                    />
                  </el-select>
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="工作经验" prop="experience">
                  <el-select v-model="resumeForm.experience" placeholder="选择工作经验" style="width: 100%;">
                    <el-option label="应届毕业生" value="fresh" />
                    <el-option label="1-3年" value="1-3" />
                    <el-option label="3-5年" value="3-5" />
                    <el-option label="5-10年" value="5-10" />
                    <el-option label="10年以上" value="10+" />
                  </el-select>
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="期望薪资" prop="expectedSalary">
                  <el-input v-model="resumeForm.expectedSalary" placeholder="如：8-12K" />
                </el-form-item>
              </el-col>
              
              <el-col :xs="24" :md="12">
                <el-form-item label="学历" prop="education">
                  <el-select v-model="resumeForm.education" placeholder="选择学历" style="width: 100%;">
                    <el-option label="高中及以下" value="high-school" />
                    <el-option label="大专" value="college" />
                    <el-option label="本科" value="bachelor" />
                    <el-option label="硕士" value="master" />
                    <el-option label="博士" value="phd" />
                  </el-select>
                </el-form-item>
              </el-col>
              
              <el-col :span="24">
                <el-form-item label="个人简介" prop="introduction">
                  <el-input 
                    v-model="resumeForm.introduction" 
                    type="textarea" 
                    :rows="4" 
                    placeholder="请简要介绍您的工作经历、技能特长等"
                  />
                </el-form-item>
              </el-col>
              
              <el-col :span="24">
                <el-form-item label="简历附件" prop="resume">
                  <el-upload
                    ref="uploadRef"
                    :auto-upload="false"
                    :limit="1"
                    accept=".pdf,.doc,.docx"
                    :on-change="handleFileChange"
                    :on-remove="handleFileRemove"
                  >
                    <el-button type="primary" plain>
                      <el-icon><Upload /></el-icon>
                      上传简历
                    </el-button>
                    <template #tip>
                      <div class="el-upload__tip">
                        支持PDF、Word格式，文件大小不超过5MB
                      </div>
                    </template>
                  </el-upload>
                </el-form-item>
              </el-col>
            </el-row>
            
            <el-form-item>
              <el-button type="primary" @click="submitResume" :loading="submitting" size="large">
                提交简历
              </el-button>
              <el-button @click="resetResumeForm" size="large">重置</el-button>
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
    </el-row>

    <!-- 职位详情对话框 -->
    <el-dialog v-model="showPositionDetail" title="职位详情" width="80%" top="5vh">
      <div v-if="selectedPosition" class="position-detail">
        <div class="detail-header">
          <h2>{{ selectedPosition.title }}</h2>
          <div class="detail-tags">
            <el-tag :type="getDepartmentType(selectedPosition.department)">{{ getDepartmentName(selectedPosition.department) }}</el-tag>
            <el-tag type="info">{{ selectedPosition.location }}</el-tag>
            <el-tag type="warning">{{ selectedPosition.salary }}</el-tag>
          </div>
        </div>
        
        <div class="detail-content">
          <h3>职位描述</h3>
          <p>{{ selectedPosition.fullDescription }}</p>
          
          <h3>岗位职责</h3>
          <ul class="responsibility-list">
            <li v-for="responsibility in selectedPosition.responsibilities" :key="responsibility">{{ responsibility }}</li>
          </ul>
          
          <h3>任职要求</h3>
          <ul class="requirement-list">
            <li v-for="requirement in selectedPosition.requirements" :key="requirement">{{ requirement }}</li>
          </ul>
          
          <h3>福利待遇</h3>
          <ul class="benefit-list">
            <li v-for="benefit in selectedPosition.benefits" :key="benefit">{{ benefit }}</li>
          </ul>
        </div>
        
        <div class="detail-footer">
          <el-button type="primary" size="large" @click="applyPosition">立即申请</el-button>
          <el-button size="large">收藏职位</el-button>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { Trophy, User, Star, Location, Money, Calendar, Upload } from '@element-plus/icons-vue'

const resumeFormRef = ref()
const uploadRef = ref()
const submitting = ref(false)
const showPositionDetail = ref(false)
const selectedPosition = ref(null)
const filterDepartment = ref('')
const filterLocation = ref('')

const resumeForm = ref({
  name: '',
  gender: '',
  phone: '',
  email: '',
  position: '',
  experience: '',
  expectedSalary: '',
  education: '',
  introduction: '',
  resume: null
})

const resumeRules = {
  name: [
    { required: true, message: '请输入姓名', trigger: 'blur' }
  ],
  gender: [
    { required: true, message: '请选择性别', trigger: 'change' }
  ],
  phone: [
    { required: true, message: '请输入手机号码', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码', trigger: 'blur' }
  ],
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
  ],
  position: [
    { required: true, message: '请选择应聘职位', trigger: 'change' }
  ],
  experience: [
    { required: true, message: '请选择工作经验', trigger: 'change' }
  ],
  education: [
    { required: true, message: '请选择学历', trigger: 'change' }
  ],
  introduction: [
    { required: true, message: '请填写个人简介', trigger: 'blur' },
    { min: 50, message: '个人简介至少50个字符', trigger: 'blur' }
  ]
}

// 模拟职位数据
const allPositions = ref([
  {
    id: 1,
    title: '前端开发工程师',
    department: 'tech',
    location: 'beijing',
    salary: '12-20K',
    experience: '3-5年经验',
    description: '负责公司回收管理系统前端开发，使用Vue.js等现代前端技术',
    fullDescription: '我们正在寻找一位有经验的前端开发工程师，负责开发和维护我们的IT设备回收管理系统。您将使用最新的前端技术栈，包括Vue.js、TypeScript等，为用户提供优秀的交互体验。',
    responsibilities: [
      '负责前端页面开发和维护',
      '与UI设计师协作，实现高质量的用户界面',
      '优化前端性能，提升用户体验',
      '参与技术方案讨论和代码评审',
      '协助解决线上问题和bug修复'
    ],
    requirements: [
      '本科及以上学历，计算机相关专业',
      '3年以上前端开发经验',
      '熟练掌握Vue.js、JavaScript、HTML、CSS',
      '了解TypeScript、Webpack等工具',
      '有良好的代码规范和团队协作能力'
    ],
    benefits: [
      '五险一金，带薪年假',
      '弹性工作时间',
      '技术培训和学习机会',
      '年终奖金和股权激励',
      '免费午餐和下午茶'
    ],
    publishDate: '2024-01-15'
  },
  {
    id: 2,
    title: '销售经理',
    department: 'sales',
    location: 'shanghai',
    salary: '10-15K+提成',
    experience: '3-5年经验',
    description: '负责企业客户开发和维护，推广IT设备回收服务',
    fullDescription: '我们需要一位经验丰富的销售经理，负责开发和维护企业客户，推广我们的IT设备回收服务。您将与各类企业建立合作关系，为客户提供专业的回收解决方案。',
    responsibilities: [
      '开发新客户，维护老客户关系',
      '制定销售策略和计划',
      '参与商务谈判和合同签署',
      '收集市场信息和竞争对手动态',
      '完成销售目标和业绩指标'
    ],
    requirements: [
      '大专及以上学历，市场营销相关专业优先',
      '3年以上B2B销售经验',
      '具备良好的沟通和谈判能力',
      '熟悉企业采购流程',
      '有环保或IT行业经验者优先'
    ],
    benefits: [
      '高额销售提成',
      '五险一金',
      '销售奖励和旅游',
      '职业发展培训',
      '灵活的工作安排'
    ],
    publishDate: '2024-01-20'
  },
  {
    id: 3,
    title: '运营专员',
    department: 'operations',
    location: 'shenzhen',
    salary: '8-12K',
    experience: '1-3年经验',
    description: '负责回收流程管理和客户服务，确保服务质量',
    fullDescription: '我们正在招聘运营专员，负责IT设备回收流程的管理和优化，以及客户服务工作。您将确保回收服务的高质量执行，提升客户满意度。',
    responsibilities: [
      '管理设备回收流程',
      '协调物流和仓储工作',
      '处理客户咨询和投诉',
      '监控服务质量和客户满意度',
      '制作运营报告和数据分析'
    ],
    requirements: [
      '大专及以上学历',
      '1-3年运营或客服经验',
      '熟练使用Office办公软件',
      '具备良好的沟通和协调能力',
      '细心负责，抗压能力强'
    ],
    benefits: [
      '完善的培训体系',
      '五险一金',
      '年终奖金',
      '团队建设活动',
      '职业发展机会'
    ],
    publishDate: '2024-02-01'
  },
  {
    id: 4,
    title: 'Java后端工程师',
    department: 'tech',
    location: 'beijing',
    salary: '15-25K',
    experience: '3-5年经验',
    description: '负责后端系统开发和维护，使用Spring Boot等技术栈',
    fullDescription: '我们正在寻找一位经验丰富的Java后端工程师，负责开发和维护我们的核心业务系统。您将使用Spring Boot、MySQL等技术，构建稳定高效的后端服务。',
    responsibilities: [
      '负责后端API开发和维护',
      '设计和优化数据库结构',
      '参与系统架构设计',
      '解决性能问题和技术难题',
      '编写技术文档和单元测试'
    ],
    requirements: [
      '本科及以上学历，计算机相关专业',
      '3年以上Java开发经验',
      '熟练掌握Spring Boot、MyBatis等框架',
      '熟悉MySQL、Redis等数据库',
      '有微服务架构经验者优先'
    ],
    benefits: [
      '具有竞争力的薪资',
      '股权激励计划',
      '技术大会和培训',
      '弹性工作制',
      '完善的福利待遇'
    ],
    publishDate: '2024-02-10'
  },
  {
    id: 5,
    title: '人事专员',
    department: 'hr',
    location: 'guangzhou',
    salary: '6-10K',
    experience: '1-3年经验',
    description: '负责招聘、培训和员工关系管理',
    fullDescription: '我们需要一位人事专员，负责公司的招聘、培训和员工关系管理工作。您将帮助公司吸引和留住优秀人才，营造良好的工作氛围。',
    responsibilities: [
      '负责招聘流程管理',
      '组织员工培训和发展',
      '处理员工关系和劳动纠纷',
      '制定和执行HR政策',
      '维护人事档案和数据'
    ],
    requirements: [
      '本科及以上学历，人力资源相关专业',
      '1-3年HR工作经验',
      '熟悉劳动法律法规',
      '具备良好的沟通和协调能力',
      '有招聘经验者优先'
    ],
    benefits: [
      '稳定的工作环境',
      '五险一金',
      '职业发展培训',
      '年终奖金',
      '员工活动和福利'
    ],
    publishDate: '2024-02-15'
  }
])

const filteredPositions = computed(() => {
  let filtered = allPositions.value

  if (filterDepartment.value) {
    filtered = filtered.filter(position => position.department === filterDepartment.value)
  }

  if (filterLocation.value) {
    filtered = filtered.filter(position => position.location === filterLocation.value)
  }

  return filtered
})

const getDepartmentName = (department) => {
  const departmentMap = {
    tech: '技术部',
    sales: '销售部',
    operations: '运营部',
    hr: '人事部'
  }
  return departmentMap[department] || department
}

const getDepartmentType = (department) => {
  const typeMap = {
    tech: 'primary',
    sales: 'success',
    operations: 'warning',
    hr: 'danger'
  }
  return typeMap[department] || 'info'
}

const filterPositions = () => {
  // 筛选逻辑已在computed中处理
}

const viewPositionDetail = (position) => {
  selectedPosition.value = position
  showPositionDetail.value = true
}

const applyPosition = () => {
  showPositionDetail.value = false
  // 滚动到简历投递区域
  document.querySelector('.resume-section').scrollIntoView({ behavior: 'smooth' })
  // 自动填充职位
  resumeForm.value.position = selectedPosition.value.title
}

const handleFileChange = (file) => {
  const isValidType = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'].includes(file.raw.type)
  const isValidSize = file.raw.size / 1024 / 1024 < 5

  if (!isValidType) {
    ElMessage.error('只支持PDF、Word格式的文件')
    return false
  }

  if (!isValidSize) {
    ElMessage.error('文件大小不能超过5MB')
    return false
  }

  resumeForm.value.resume = file.raw
  return true
}

const handleFileRemove = () => {
  resumeForm.value.resume = null
}

const submitResume = async () => {
  try {
    await resumeFormRef.value.validate()
    
    if (!resumeForm.value.resume) {
      ElMessage.error('请上传简历文件')
      return
    }

    submitting.value = true
    
    // 模拟提交
    setTimeout(() => {
      submitting.value = false
      ElMessage.success('简历提交成功！我们会在3个工作日内与您联系')
      
      // 重置表单
      resetResumeForm()
    }, 2000)
    
  } catch (error) {
    ElMessage.error('请完善简历信息')
  }
}

const resetResumeForm = () => {
  resumeFormRef.value.resetFields()
  resumeForm.value.resume = null
  uploadRef.value.clearFiles()
}

onMounted(() => {
  console.log('Jobs page mounted')
})
</script>

<style scoped lang="scss">
.jobs-page {
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

.culture-section {
  margin-bottom: 60px;
  
  .culture-card {
    padding: 40px 20px;
    text-align: center;
    
    h2 {
      font-size: 28px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 40px;
    }
    
    .culture-item {
      text-align: center;
      padding: 20px;
      
      .culture-icon {
        color: #409EFF;
        margin-bottom: 20px;
      }
      
      h3 {
        font-size: 20px;
        font-weight: 600;
        color: #303133;
        margin-bottom: 12px;
      }
      
      p {
        color: #606266;
        line-height: 1.6;
      }
    }
  }
}

.positions-section {
  margin-bottom: 60px;
  
  .section-title {
    font-size: 28px;
    font-weight: 600;
    color: #303133;
    text-align: center;
    margin-bottom: 40px;
  }
  
  .position-filter {
    display: flex;
    gap: 20px;
    margin-bottom: 30px;
    justify-content: center;
    
    .el-select {
      width: 150px;
    }
  }
  
  .position-card {
    height: 100%;
    margin-bottom: 30px;
    cursor: pointer;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .position-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
      
      h3 {
        font-size: 18px;
        font-weight: 600;
        color: #303133;
        margin: 0;
      }
    }
    
    .position-info {
      margin-bottom: 16px;
      
      .info-item {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 8px;
        color: #606266;
        font-size: 14px;
        
        .el-icon {
          color: #909399;
        }
      }
    }
    
    .position-description {
      color: #606266;
      line-height: 1.6;
      margin-bottom: 20px;
    }
    
    .position-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      
      .publish-date {
        color: #909399;
        font-size: 12px;
      }
    }
  }
}

.resume-section {
  margin-bottom: 40px;
  
  .section-title {
    font-size: 28px;
    font-weight: 600;
    color: #303133;
    text-align: center;
    margin-bottom: 40px;
  }
  
  .resume-card {
    padding: 40px;
  }
}

.position-detail {
  .detail-header {
    margin-bottom: 30px;
    
    h2 {
      font-size: 24px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 16px;
    }
    
    .detail-tags {
      display: flex;
      gap: 10px;
      flex-wrap: wrap;
    }
  }
  
  .detail-content {
    h3 {
      font-size: 18px;
      font-weight: 600;
      color: #303133;
      margin: 24px 0 12px;
    }
    
    p {
      color: #606266;
      line-height: 1.6;
      margin-bottom: 16px;
    }
    
    .responsibility-list,
    .requirement-list,
    .benefit-list {
      list-style: none;
      padding: 0;
      margin-bottom: 20px;
      
      li {
        position: relative;
        padding-left: 20px;
        margin-bottom: 8px;
        color: #606266;
        line-height: 1.6;
        
        &::before {
          content: '•';
          position: absolute;
          left: 0;
          color: #409EFF;
          font-weight: bold;
        }
      }
    }
  }
  
  .detail-footer {
    margin-top: 30px;
    text-align: center;
    
    .el-button {
      margin: 0 10px;
    }
  }
}

@media (max-width: 768px) {
  .page-header {
    h1 {
      font-size: 28px;
    }
  }
  
  .culture-section {
    .culture-card {
      padding: 30px 15px;
      
      h2 {
        font-size: 24px;
      }
    }
  }
  
  .position-filter {
    flex-direction: column;
    align-items: center;
    
    .el-select {
      width: 200px;
    }
  }
  
  .resume-section {
    .resume-card {
      padding: 20px;
    }
  }
  
  .position-detail {
    .detail-footer {
      .el-button {
        display: block;
        width: 100%;
        margin: 10px 0;
      }
    }
  }
}
</style> 