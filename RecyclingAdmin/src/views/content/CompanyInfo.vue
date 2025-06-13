<template>
  <div class="company-info-management">
    <el-tabs v-model="activeTab">
      <el-tab-pane label="公司基本信息" name="basic">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>公司基本信息</span>
            </div>
          </template>
          <el-form :model="companyForm" :rules="companyRules" ref="companyFormRef" label-width="120px">
            <el-form-item label="公司名称" prop="name">
              <el-input v-model="companyForm.name" placeholder="请输入公司名称" />
            </el-form-item>
            <el-form-item label="公司Logo" prop="logoUrl">
              <el-upload
                class="avatar-uploader"
                action="#"
                :show-file-list="false"
                :auto-upload="false"
                :on-change="handleLogoChange"
              >
                <img v-if="companyForm.logoUrl" :src="companyForm.logoUrl" class="avatar" />
                <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
              </el-upload>
            </el-form-item>
            <el-form-item label="公司封面图" prop="coverImageUrl">
              <el-upload
                class="avatar-uploader"
                action="#"
                :show-file-list="false"
                :auto-upload="false"
                :on-change="handleCoverChange"
              >
                <img v-if="companyForm.coverImageUrl" :src="companyForm.coverImageUrl" class="cover-image" />
                <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
              </el-upload>
            </el-form-item>
            <el-form-item label="成立时间" prop="establishDate">
              <el-date-picker
                v-model="companyForm.establishDate"
                type="date"
                placeholder="选择成立日期"
                format="YYYY-MM-DD"
              />
            </el-form-item>
            <el-form-item label="联系电话" prop="phone">
              <el-input v-model="companyForm.phone" placeholder="请输入联系电话" />
            </el-form-item>
            <el-form-item label="联系邮箱" prop="email">
              <el-input v-model="companyForm.email" placeholder="请输入联系邮箱" />
            </el-form-item>
            <el-form-item label="公司地址" prop="address">
              <el-input v-model="companyForm.address" placeholder="请输入公司地址" />
            </el-form-item>
            <el-form-item label="公司简介" prop="introduction">
              <el-input
                v-model="companyForm.introduction"
                type="textarea"
                :rows="4"
                placeholder="请输入公司简介"
              />
            </el-form-item>
            <el-form-item label="企业文化/愿景" prop="vision">
              <el-input
                v-model="companyForm.vision"
                type="textarea"
                :rows="3"
                placeholder="请输入企业文化/愿景"
              />
            </el-form-item>
            <el-form-item label="企业使命" prop="mission">
              <el-input
                v-model="companyForm.mission"
                type="textarea"
                :rows="3"
                placeholder="请输入企业使命"
              />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitCompanyForm">保存信息</el-button>
            </el-form-item>
          </el-form>
        </el-card>
      </el-tab-pane>

      <el-tab-pane label="企业优势" name="advantages">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>企业优势</span>
              <el-button type="primary" @click="addAdvantage">
                <el-icon><Plus /></el-icon>
                添加优势
              </el-button>
            </div>
          </template>

          <el-table :data="companyForm.advantages" style="width: 100%">
            <el-table-column prop="title" label="标题" width="180" />
            <el-table-column prop="description" label="描述" show-overflow-tooltip />
            <el-table-column label="图标" width="100">
              <template #default="scope">
                <el-image
                  v-if="scope.row.iconUrl"
                  :src="scope.row.iconUrl"
                  style="width: 40px; height: 40px"
                />
                <el-icon v-else><Picture /></el-icon>
              </template>
            </el-table-column>
            <el-table-column label="操作" width="150">
              <template #default="scope">
                <el-button size="small" @click="editAdvantage(scope.$index, scope.row)">编辑</el-button>
                <el-button
                  size="small"
                  type="danger"
                  @click="removeAdvantage(scope.$index)"
                >删除</el-button>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-tab-pane>

      <el-tab-pane label="发展历程" name="milestones">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>发展历程</span>
              <el-button type="primary" @click="addMilestone">
                <el-icon><Plus /></el-icon>
                添加历程
              </el-button>
            </div>
          </template>

          <el-table :data="companyForm.milestones" style="width: 100%">
            <el-table-column prop="year" label="年份" width="120" />
            <el-table-column prop="title" label="标题" width="180" />
            <el-table-column prop="description" label="描述" show-overflow-tooltip />
            <el-table-column label="操作" width="150">
              <template #default="scope">
                <el-button size="small" @click="editMilestone(scope.$index, scope.row)">编辑</el-button>
                <el-button
                  size="small"
                  type="danger"
                  @click="removeMilestone(scope.$index)"
                >删除</el-button>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-tab-pane>

      <el-tab-pane label="企业证书" name="certifications">
    <el-card>
      <template #header>
            <div class="card-header">
              <span>企业证书</span>
              <el-button type="primary" @click="addCertification">
                <el-icon><Plus /></el-icon>
                添加证书
              </el-button>
            </div>
          </template>

          <el-row :gutter="20">
            <el-col v-for="(cert, index) in companyForm.certifications" :key="index" :span="6">
              <el-card class="cert-card">
                <el-image :src="cert" fit="cover" class="cert-image" />
                <div class="cert-actions">
                  <el-button size="small" type="danger" @click="removeCertification(index)">
                    <el-icon><Delete /></el-icon>
                  </el-button>
                </div>
              </el-card>
            </el-col>
          </el-row>
        </el-card>
      </el-tab-pane>
    </el-tabs>

    <!-- 优势编辑对话框 -->
    <el-dialog
      v-model="advantageDialogVisible"
      :title="isEditAdvantage ? '编辑企业优势' : '添加企业优势'"
      width="500px"
    >
      <el-form :model="currentAdvantage" label-width="80px">
        <el-form-item label="标题" required>
          <el-input v-model="currentAdvantage.title" placeholder="请输入标题" />
        </el-form-item>
        <el-form-item label="描述" required>
          <el-input
            v-model="currentAdvantage.description"
            type="textarea"
            :rows="3"
            placeholder="请输入描述"
          />
        </el-form-item>
        <el-form-item label="图标">
          <el-upload
            class="avatar-uploader"
            action="#"
            :show-file-list="false"
            :auto-upload="false"
            :on-change="handleAdvantageIconChange"
          >
            <img v-if="currentAdvantage.iconUrl" :src="currentAdvantage.iconUrl" class="avatar" />
            <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
          </el-upload>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="advantageDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitAdvantage">确定</el-button>
      </template>
    </el-dialog>

    <!-- 发展历程编辑对话框 -->
    <el-dialog
      v-model="milestoneDialogVisible"
      :title="isEditMilestone ? '编辑发展历程' : '添加发展历程'"
      width="500px"
    >
      <el-form :model="currentMilestone" label-width="80px">
        <el-form-item label="年份" required>
          <el-input v-model="currentMilestone.year" placeholder="请输入年份，如：2020" />
        </el-form-item>
        <el-form-item label="标题" required>
          <el-input v-model="currentMilestone.title" placeholder="请输入标题" />
        </el-form-item>
        <el-form-item label="描述" required>
          <el-input
            v-model="currentMilestone.description"
            type="textarea"
            :rows="3"
            placeholder="请输入描述"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="milestoneDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitMilestone">确定</el-button>
      </template>
    </el-dialog>

    <!-- 企业证书上传对话框 -->
    <el-dialog
      v-model="certDialogVisible"
      title="上传企业证书"
      width="500px"
    >
      <el-upload
        class="cert-uploader"
        action="#"
        :show-file-list="false"
        :auto-upload="false"
        :on-change="handleCertChange"
      >
        <img v-if="currentCertUrl" :src="currentCertUrl" class="cert-preview" />
        <div v-else class="cert-uploader-placeholder">
          <el-icon><Plus /></el-icon>
          <div class="el-upload__text">点击上传证书图片</div>
        </div>
      </el-upload>
      <template #footer>
        <el-button @click="certDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitCertification">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Delete, Picture } from '@element-plus/icons-vue'
import { getCompanyProfile, updateCompanyProfile } from '@/api/company'

// 类型定义
interface Advantage {
  title: string
  description: string
  iconUrl: string
}

interface Milestone {
  year: string
  title: string
  description: string
}

interface CompanyForm {
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

// 表单数据
const companyFormRef = ref<any>(null)
const companyForm = reactive<CompanyForm>({
  name: '',
  logoUrl: '',
  coverImageUrl: '',
  establishDate: '',
  phone: '',
  email: '',
  address: '',
  introduction: '',
  vision: '',
  mission: '',
  advantages: [],
  milestones: [],
  certifications: []
})

// 表单验证规则
const companyRules = {
  name: [{ required: true, message: '请输入公司名称', trigger: 'blur' }],
  logoUrl: [{ required: true, message: '请上传公司Logo', trigger: 'change' }],
  phone: [{ required: true, message: '请输入联系电话', trigger: 'blur' }],
  email: [{ required: true, message: '请输入联系邮箱', trigger: 'blur' }],
  introduction: [{ required: true, message: '请输入公司简介', trigger: 'blur' }]
}

// 当前活动标签
const activeTab = ref('basic')

// 企业优势相关
const advantageDialogVisible = ref(false)
const isEditAdvantage = ref(false)
const currentAdvantage = reactive<Advantage>({
  title: '',
  description: '',
  iconUrl: ''
})
const currentAdvantageIndex = ref(-1)

// 发展历程相关
const milestoneDialogVisible = ref(false)
const isEditMilestone = ref(false)
const currentMilestone = reactive<Milestone>({
  year: '',
  title: '',
  description: ''
})
const currentMilestoneIndex = ref(-1)

// 企业证书相关
const certDialogVisible = ref(false)
const currentCertUrl = ref('')

// 初始化加载公司信息
onMounted(async () => {
  try {
    const response = await getCompanyProfile()
    if (response.data.data) {
      const data = response.data.data
      console.log(data);      
      companyForm.name = data.name
      companyForm.logoUrl = data.logoUrl
      companyForm.coverImageUrl = data.coverImageUrl
      companyForm.establishDate = data.establishDate ? new Date(data.establishDate) : ''
      companyForm.phone = data.phone
      companyForm.email = data.email
      companyForm.address = data.address
      companyForm.introduction = data.introduction
      companyForm.vision = data.vision
      companyForm.mission = data.mission
      companyForm.advantages = data.advantages || []
      companyForm.milestones = data.milestones || []
      companyForm.certifications = data.certifications || []
    }
  } catch (error) {
    console.error('获取公司信息失败', error)
    ElMessage.error('获取公司信息失败')
  }
})

// 提交公司基本信息表单
const submitCompanyForm = async () => {
  if (!companyFormRef.value) return
  
  try {
    await companyFormRef.value.validate()
    const formData = {
      name: companyForm.name,
      logoUrl: companyForm.logoUrl,
      coverImageUrl: companyForm.coverImageUrl,
      establishDate: companyForm.establishDate,
      phone: companyForm.phone,
      email: companyForm.email,
      address: companyForm.address,
      introduction: companyForm.introduction,
      vision: companyForm.vision,
      mission: companyForm.mission,
      advantages: companyForm.advantages,
      milestones: companyForm.milestones,
      certifications: companyForm.certifications
    }
    
    await updateCompanyProfile(formData)
    ElMessage.success('公司信息保存成功')
  } catch (error) {
    console.error('保存公司信息失败', error)
    ElMessage.error('保存公司信息失败，请检查表单')
  }
}

// 图片上传处理类型定义
interface UploadFile {
  raw: File
}

// 图片上传处理函数
const handleLogoChange = (file: UploadFile) => {
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      companyForm.logoUrl = reader.result as string
    }
  }
}

const handleCoverChange = (file: UploadFile) => {
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      companyForm.coverImageUrl = reader.result as string
    }
  }
}

const handleAdvantageIconChange = (file: UploadFile) => {
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      currentAdvantage.iconUrl = reader.result as string
    }
  }
}

const handleCertChange = (file: UploadFile) => {
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      currentCertUrl.value = reader.result as string
    }
  }
}

// 企业优势操作
const addAdvantage = () => {
  isEditAdvantage.value = false
  Object.assign(currentAdvantage, {
    title: '',
    description: '',
    iconUrl: ''
  })
  advantageDialogVisible.value = true
}

const editAdvantage = (index: number, advantage: Advantage) => {
  isEditAdvantage.value = true
  currentAdvantageIndex.value = index
  Object.assign(currentAdvantage, advantage)
  advantageDialogVisible.value = true
}

const removeAdvantage = (index: number) => {
  ElMessageBox.confirm('确定要删除这个企业优势吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(() => {
    companyForm.advantages.splice(index, 1)
    ElMessage.success('删除成功')
  })
}

const submitAdvantage = () => {
  if (!currentAdvantage.title || !currentAdvantage.description) {
    ElMessage.warning('请填写完整信息')
    return
  }
  
  if (isEditAdvantage.value && currentAdvantageIndex.value !== -1) {
    companyForm.advantages[currentAdvantageIndex.value] = { ...currentAdvantage }
  } else {
    companyForm.advantages.push({ ...currentAdvantage })
  }
  
  advantageDialogVisible.value = false
  ElMessage.success(isEditAdvantage.value ? '编辑成功' : '添加成功')
}

// 发展历程操作
const addMilestone = () => {
  isEditMilestone.value = false
  Object.assign(currentMilestone, {
    year: '',
    title: '',
    description: ''
  })
  milestoneDialogVisible.value = true
}

const editMilestone = (index: number, milestone: Milestone) => {
  isEditMilestone.value = true
  currentMilestoneIndex.value = index
  Object.assign(currentMilestone, milestone)
  milestoneDialogVisible.value = true
}

const removeMilestone = (index: number) => {
  ElMessageBox.confirm('确定要删除这个发展历程吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(() => {
    companyForm.milestones.splice(index, 1)
    ElMessage.success('删除成功')
  })
}

const submitMilestone = () => {
  if (!currentMilestone.year || !currentMilestone.title || !currentMilestone.description) {
    ElMessage.warning('请填写完整信息')
    return
  }
  
  if (isEditMilestone.value && currentMilestoneIndex.value !== -1) {
    companyForm.milestones[currentMilestoneIndex.value] = { ...currentMilestone }
  } else {
    companyForm.milestones.push({ ...currentMilestone })
  }
  
  milestoneDialogVisible.value = false
  ElMessage.success(isEditMilestone.value ? '编辑成功' : '添加成功')
}

// 企业证书操作
const addCertification = () => {
  currentCertUrl.value = ''
  certDialogVisible.value = true
}

const removeCertification = (index: number) => {
  ElMessageBox.confirm('确定要删除这个企业证书吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(() => {
    companyForm.certifications.splice(index, 1)
    ElMessage.success('删除成功')
  })
}

const submitCertification = () => {
  if (!currentCertUrl.value) {
    ElMessage.warning('请上传证书图片')
    return
  }
  
  companyForm.certifications.push(currentCertUrl.value)
  certDialogVisible.value = false
  ElMessage.success('添加成功')
}
</script>

<style scoped>
.company-info-management {
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.avatar-uploader {
  width: 178px;
  height: 178px;
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: border-color 0.3s;
}

.avatar-uploader:hover {
  border-color: #409EFF;
}

.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178px;
  height: 178px;
  line-height: 178px;
  text-align: center;
}

.avatar {
  width: 178px;
  height: 178px;
  display: block;
  object-fit: cover;
}

.cover-image {
  width: 300px;
  height: 178px;
  display: block;
  object-fit: cover;
}

.cert-card {
  margin-bottom: 20px;
  position: relative;
}

.cert-image {
  width: 100%;
  height: 150px;
  object-fit: cover;
}

.cert-actions {
  position: absolute;
  top: 5px;
  right: 5px;
}

.cert-uploader {
  width: 100%;
  height: 300px;
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: border-color 0.3s;
}

.cert-uploader:hover {
  border-color: #409EFF;
}

.cert-uploader-placeholder {
  height: 300px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #8c939d;
}

.cert-uploader-placeholder .el-icon {
  font-size: 40px;
  margin-bottom: 10px;
}

.cert-preview {
  width: 100%;
  height: 300px;
  object-fit: contain;
}
</style> 