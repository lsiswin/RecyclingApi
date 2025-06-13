<template>
  <div class="case-management">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>案例内容管理</span>
          <el-button type="primary" @click="openCreateDialog">
            <el-icon><Plus /></el-icon>
            添加案例
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <div class="search-bar">
        <el-form :inline="true" :model="searchForm">
          <el-form-item label="案例标题">
            <el-input v-model="searchForm.title" placeholder="输入案例标题搜索" clearable />
          </el-form-item>
          <el-form-item label="案例分类">
            <el-select style="width: 100px" v-model="searchForm.category" placeholder="选择分类" clearable>
              <el-option label="企业回收" value="enterprise" />
              <el-option label="学校回收" value="school" />
              <el-option label="政府机构" value="government" />
              <el-option label="医院回收" value="hospital" />
            </el-select>
          </el-form-item>
          <el-form-item label="设备类型">
            <el-select style="width: 120px" v-model="searchForm.deviceType" placeholder="选择设备类型" clearable>
              <el-option label="台式电脑" value="desktop" />
              <el-option label="笔记本电脑" value="laptop" />
              <el-option label="服务器" value="server" />
              <el-option label="网络设备" value="network" />
            </el-select>
          </el-form-item>
          <el-form-item label="回收规模">
            <el-select style="width: 100px" v-model="searchForm.scale" placeholder="选择规模" clearable>
              <el-option label="小规模" value="small" />
              <el-option label="中规模" value="medium" />
              <el-option label="大规模" value="large" />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="loadCases">搜索</el-button>
            <el-button @click="resetSearch">重置</el-button>
          </el-form-item>
        </el-form>
      </div>

      <!-- 案例列表 -->
      <el-table
        v-loading="loading"
        :data="caseList"
        border
        style="width: 100%"
      >
        <el-table-column align="center" prop="title" label="案例标题" min-width="180" show-overflow-tooltip />
        <el-table-column align="center" label="所属分类" width="120">
          <template #default="scope">
            {{ getCategoryName(scope.row.category) }}
          </template>
        </el-table-column>
        <el-table-column align="center" prop="client" label="客户名称" width="150" show-overflow-tooltip />
        <el-table-column align="center" label="封面图" width="120">
          <template #default="scope">
            <el-image
              v-if="scope.row.image"
              :src="scope.row.image"
              fit="cover"
              style="width: 80px; height: 50px"
              :preview-src-list="[scope.row.image]"
              preview-teleported
            />
            <span v-else>无封面</span>
          </template>
        </el-table-column>
        <el-table-column align="center" prop="description" label="案例简介" min-width="250" show-overflow-tooltip />
        <el-table-column align="center" label="状态" width="100">
          <template #default="scope">
            <el-tag :type="scope.row.isActive ? 'success' : 'info'">
              {{ scope.row.isActive ? '已发布' : '草稿' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column align="center" label="操作" width="220" fixed="right">
          <template #default="scope">
            <el-button size="small" @click="editCase(scope.row)">编辑</el-button>
            <el-button 
              size="small" 
              :type="scope.row.isActive ? 'warning' : 'success'"
              @click="togglePublishStatus(scope.row)"
            >
              {{ scope.row.isActive ? '取消发布' : '发布' }}
            </el-button>
            <el-button
              size="small"
              type="danger"
              @click="handleDelete(scope.row)"
            >删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <div class="pagination-container">
        <el-pagination
          v-model:current-page="page"
          v-model:page-size="pageSize"
          :page-sizes="[10, 20, 30, 50]"
          background
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-card>

    <!-- 添加/编辑案例对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑案例' : '添加案例'"
      width="70%"
      destroy-on-close
    >
      <el-form
        ref="caseFormRef"
        :model="caseForm"
        :rules="caseRules"
        label-width="100px"
      >
        <el-form-item label="案例标题" prop="title">
          <el-input v-model="caseForm.title" placeholder="请输入案例标题" />
        </el-form-item>
        <el-form-item label="所属分类" prop="category">
          <el-select v-model="caseForm.category" placeholder="选择所属分类">
            <el-option label="企业回收" value="enterprise" />
            <el-option label="学校回收" value="school" />
            <el-option label="政府机构" value="government" />
            <el-option label="医院回收" value="hospital" />
          </el-select>
        </el-form-item>
        <el-form-item label="客户名称" prop="client">
          <el-input v-model="caseForm.client" placeholder="请输入客户名称" />
        </el-form-item>
        <el-form-item label="完成时间" prop="date">
          <el-date-picker
            v-model="caseForm.date"
            type="date"
            placeholder="选择完成日期"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
          />
        </el-form-item>
        <el-form-item label="封面图" prop="image">
          <el-upload
            class="cover-uploader"
            action="#"
            :show-file-list="false"
            :auto-upload="false"
            :on-change="handleCoverChange"
          >
            <img v-if="caseForm.image" :src="caseForm.image" class="cover-preview" />
            <el-icon v-else class="uploader-icon"><Plus /></el-icon>
          </el-upload>
        </el-form-item>
        <el-form-item label="案例简介" prop="description">
          <el-input
            v-model="caseForm.description"
            type="textarea"
            :rows="3"
            placeholder="请输入案例简介"
          />
        </el-form-item>
        <el-form-item label="案例详情" prop="fullDescription">
          <el-input
            v-model="caseForm.fullDescription"
            type="textarea"
            :rows="5"
            placeholder="请输入案例详情"
          />
        </el-form-item>
        <el-form-item label="设备类型" prop="deviceType">
          <el-select v-model="caseForm.deviceType" placeholder="选择设备类型">
            <el-option label="台式电脑" value="desktop" />
            <el-option label="笔记本电脑" value="laptop" />
            <el-option label="服务器" value="server" />
            <el-option label="网络设备" value="network" />
          </el-select>
        </el-form-item>
        <el-form-item label="设备数量" prop="deviceCount">
          <el-input-number v-model="caseForm.deviceCount" :min="1" :max="10000" />
        </el-form-item>
        <el-form-item label="项目周期" prop="duration">
          <el-input v-model="caseForm.duration" placeholder="例如：7天" />
        </el-form-item>
        <el-form-item label="回收规模" prop="scale">
          <el-select v-model="caseForm.scale" placeholder="选择回收规模">
            <el-option label="小规模" value="small" />
            <el-option label="中规模" value="medium" />
            <el-option label="大规模" value="large" />
          </el-select>
        </el-form-item>
        <el-form-item label="相关图片">
          <el-upload
            action="#"
            list-type="picture-card"
            :auto-upload="false"
            :on-change="handleImagesChange"
            :on-remove="handleImageRemove"
          >
            <el-icon><Plus /></el-icon>
          </el-upload>
        </el-form-item>
        <el-form-item label="标签" prop="tags">
          <el-select
            v-model="caseForm.tags"
            multiple
            filterable
            allow-create
            default-first-option
            placeholder="请输入标签，按回车键确认"
          >
            <el-option
              v-for="tag in defaultTags"
              :key="tag"
              :label="tag"
              :value="tag"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="项目详情" prop="projectDetails">
          <el-input
            v-model="caseForm.projectDetails"
            type="textarea"
            :rows="3"
            placeholder="请输入项目详情"
          />
        </el-form-item>
        <el-form-item label="服务亮点">
          <div v-for="(highlight, index) in caseForm.highlights" :key="index" class="highlight-item">
            <el-input v-model="caseForm.highlights[index]" placeholder="请输入服务亮点" />
            <el-button type="danger" circle @click="removeHighlight(index)">
              <el-icon><Close /></el-icon>
            </el-button>
          </div>
          <el-button type="primary" @click="addHighlight">添加亮点</el-button>
        </el-form-item>
        <el-form-item label="发布状态">
          <el-switch
            v-model="caseForm.isActive"
            :active-text="caseForm.isActive ? '已发布' : '草稿'"
          />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number v-model="caseForm.sort" :min="0" :max="999" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitCaseForm">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Close } from '@element-plus/icons-vue'
import { getCaseList, getCaseById, createCase, updateCase, deleteCase, toggleCaseStatus } from '@/api/case'
import dayjs from 'dayjs'

// 类型定义
interface Case {
  id: number
  title: string
  category: string
  client: string
  date: string | Date
  image: string
  description: string
  fullDescription: string
  deviceType: string
  deviceCount: number
  duration: string
  scale: string
  tags: string[]
  views: number
  rating: number
  projectDetails: string
  highlights: string[]
  isActive: boolean
  sort: number
  createdAt?: string
}

interface CaseForm {
  id?: number
  title: string
  category: string
  client: string
  date: string | Date
  image: string
  description: string
  fullDescription: string
  deviceType: string
  deviceCount: number
  duration: string
  scale: string
  tags: string[]
  projectDetails: string
  highlights: string[]
  isActive: boolean
  sort: number
}

interface SearchForm {
  title: string
  category: string
  deviceType: string
  scale: string
}

// 列表数据
const loading = ref(false)
const caseList = ref<Case[]>([])
const page = ref(1)
const pageSize = ref(10)
const total = ref(0)

// 搜索表单
const searchForm = reactive<SearchForm>({
  title: '',
  category: '',
  deviceType: '',
  scale: ''
})

// 案例表单
const caseFormRef = ref<any>(null)
const dialogVisible = ref(false)
const isEdit = ref(false)
const caseForm = reactive<CaseForm>({
  title: '',
  category: '',
  client: '',
  date: '',
  image: '',
  description: '',
  fullDescription: '',
  deviceType: '',
  deviceCount: 0,
  duration: '',
  scale: '',
  tags: [],
  projectDetails: '',
  highlights: [],
  isActive: false,
  sort: 0
})

// 默认标签列表
const defaultTags = [
  '数据安全', '环保处理', '批量回收', '服务器回收', '电脑回收',
  '网络设备', '企业服务', '学校服务', '政府项目', '医疗设备'
]

// 分类名称映射
const categoryMap = {
  'enterprise': '企业回收',
  'school': '学校回收',
  'government': '政府机构',
  'hospital': '医院回收'
}

// 获取分类名称
const getCategoryName = (category: string) => {
  return categoryMap[category as keyof typeof categoryMap] || category
}

// 表单验证规则
const caseRules = {
  title: [{ required: true, message: '请输入案例标题', trigger: 'blur' }],
  category: [{ required: true, message: '请选择所属分类', trigger: 'change' }],
  client: [{ required: true, message: '请输入客户名称', trigger: 'blur' }],
  date: [{ required: true, message: '请选择完成日期', trigger: 'change' }],
  image: [{ required: true, message: '请上传封面图', trigger: 'change' }],
  description: [{ required: true, message: '请输入案例简介', trigger: 'blur' }],
  fullDescription: [{ required: true, message: '请输入案例详情', trigger: 'blur' }],
  deviceType: [{ required: true, message: '请选择设备类型', trigger: 'change' }],
  deviceCount: [{ required: true, message: '请输入设备数量', trigger: 'blur' }],
  duration: [{ required: true, message: '请输入项目周期', trigger: 'blur' }],
  scale: [{ required: true, message: '请选择回收规模', trigger: 'change' }],
  projectDetails: [{ required: true, message: '请输入项目详情', trigger: 'blur' }]
}

// 初始化加载
onMounted(() => {
  loadCases()
})

// 加载案例列表
const loadCases = async () => {
  loading.value = true
  try {
    const res = await getCaseList({
      pageindex: page.value,
      pageSize: pageSize.value,
      keyword: searchForm.title,
      category: searchForm.category,
      DeviceType: searchForm.deviceType,
      Scale: searchForm.scale,
      IsActive: true
    })
    
    console.log('API响应:', res.data)
    
    if (res.data && res.data.data) {
      // 检查数据是否为数组，如果不是，尝试提取items
      const dataItems = Array.isArray(res.data.data) 
        ? res.data.data 
        : (res.data.data.items || [])
      
      // 确保返回的是数组
      caseList.value = Array.isArray(dataItems) ? dataItems : []
      total.value = res.data.totalCount || 0
      
      console.log('解析后的案例列表:', caseList.value)
    } else {
      caseList.value = []
      total.value = 0
      console.warn('返回数据格式不符合预期', res.data)
    }
  } catch (error) {
    console.error('获取案例列表失败', error)
    ElMessage.error('获取案例列表失败')
    caseList.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

// 重置搜索
const resetSearch = () => {
  searchForm.title = ''
  searchForm.category = ''
  searchForm.deviceType = ''
  searchForm.scale = ''
  loadCases()
}

// 分页处理
const handleSizeChange = (val: number) => {
  pageSize.value = val
  loadCases()
}

const handleCurrentChange = (val: number) => {
  page.value = val
  loadCases()
}

// 打开创建对话框
const openCreateDialog = () => {
  isEdit.value = false
  Object.assign(caseForm, {
    title: '',
    category: '',
    client: '',
    date: '',
    image: '',
    description: '',
    fullDescription: '',
    deviceType: '',
    deviceCount: 0,
    duration: '',
    scale: '',
    tags: [],
    projectDetails: '',
    highlights: [],
    isActive: false,
    sort: 0
  })
  dialogVisible.value = true
}

// 编辑案例
const editCase = async (row: Case) => {
  isEdit.value = true
  try {
    const res = await getCaseById(row.id)
    console.log('案例详情:', res.data)
    
    if (res.data) {
      // 深拷贝，防止直接修改原始数据
      const caseData = JSON.parse(JSON.stringify(res.data.data))
      
      // 确保日期格式正确
      if (caseData.date && typeof caseData.date === 'string') {
        caseData.date = dayjs(caseData.date).format('YYYY-MM-DD')
      }
      
      // 确保标签和亮点是数组
      caseData.tags = Array.isArray(caseData.tags) ? caseData.tags : []
      caseData.highlights = Array.isArray(caseData.highlights) ? caseData.highlights : []
      
      Object.assign(caseForm, caseData)
    }
    dialogVisible.value = true
  } catch (error) {
    console.error('获取案例详情失败', error)
    ElMessage.error('获取案例详情失败')
  }
}

// 切换发布状态
const togglePublishStatus = (row: Case) => {
  ElMessageBox.confirm(
    `确定要${row.isActive ? '取消发布' : '发布'}该案例吗？`,
    '提示',
    {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    }
  ).then(async () => {
    try {
      await toggleCaseStatus(row.id)
      row.isActive = !row.isActive
      ElMessage.success(`${row.isActive ? '发布' : '取消发布'}成功`)
    } catch (error) {
      console.error('更新状态失败', error)
      ElMessage.error('操作失败')
    }
  })
}

// 删除案例
const handleDelete = (row: Case) => {
  ElMessageBox.confirm('确定要删除该案例吗？删除后不可恢复！', '警告', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      await deleteCase(row.id)
      ElMessage.success('删除成功')
      loadCases()
    } catch (error) {
      console.error('删除失败', error)
      ElMessage.error('删除失败')
    }
  })
}

// 服务亮点操作
const addHighlight = () => {
  caseForm.highlights.push('')
}

const removeHighlight = (index: number) => {
  caseForm.highlights.splice(index, 1)
}

// 图片上传处理
interface UploadFile {
  raw: File
}

const handleCoverChange = (file: UploadFile) => {
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      caseForm.image = reader.result as string
    }
  }
}

interface UploadChangeEvent {
  file: UploadFile
}

const handleImagesChange = (uploadInfo: UploadChangeEvent) => {
  const file = uploadInfo.file
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      // 这里应该处理多图片上传，但表单中没有对应字段
      // 此处仅作示例
      console.log('上传图片:', reader.result)
    }
  }
}

const handleImageRemove = (file: any, fileList: any[]) => {
  // 这里应该处理图片移除，但表单中没有对应字段
  console.log('移除图片:', file, fileList)
}

// 提交表单
const submitCaseForm = async () => {
  if (!caseFormRef.value) return

  try {
    await caseFormRef.value.validate()
    
    // 准备要提交的数据
    const formData = {
      ...caseForm,
      // 确保日期格式正确
      date: typeof caseForm.date === 'string' ? caseForm.date : dayjs(caseForm.date).format('YYYY-MM-DD')
    }
    
    console.log('提交数据:', formData)
    
    if (isEdit.value && caseForm.id) {
      // 更新案例
      await updateCase(caseForm.id, formData)
      ElMessage.success('更新成功')
    } else {
      // 创建案例
      await createCase(formData)
      ElMessage.success('创建成功')
    }
    
    dialogVisible.value = false
    loadCases()
  } catch (error) {
    console.error('表单验证或提交失败', error)
    if (error instanceof Error) {
      ElMessage.error(`操作失败: ${error.message}`)
    } else {
      ElMessage.error('请填写必填项')
    }
  }
}
</script>

<style scoped>
.case-management {
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.search-bar {
  margin-bottom: 20px;
}

.pagination-container {
  margin-top: 20px;
  text-align: right;
}

.cover-uploader {
  width: 300px;
  height: 180px;
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: border-color 0.3s;
}

.cover-uploader:hover {
  border-color: #409EFF;
}

.uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 300px;
  height: 180px;
  line-height: 180px;
  text-align: center;
}

.cover-preview {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.highlight-item {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.highlight-item .el-button {
  margin-left: 10px;
}
</style>