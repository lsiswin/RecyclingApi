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
            <el-select v-model="searchForm.categoryId" placeholder="选择分类" clearable>
              <el-option
                v-for="item in categoryOptions"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
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
        <el-table-column prop="title" label="案例标题" min-width="180" show-overflow-tooltip />
        <el-table-column prop="categoryName" label="所属分类" width="150" />
        <el-table-column prop="client" label="客户名称" width="150" show-overflow-tooltip />
        <el-table-column label="封面图" width="120">
          <template #default="scope">
            <el-image
              v-if="scope.row.coverImage"
              :src="scope.row.coverImage"
              fit="cover"
              style="width: 80px; height: 50px"
              :preview-src-list="[scope.row.coverImage]"
              preview-teleported
            />
            <span v-else>无封面</span>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="创建时间" width="180" />
        <el-table-column label="状态" width="100">
          <template #default="scope">
            <el-tag :type="scope.row.isPublished ? 'success' : 'info'">
              {{ scope.row.isPublished ? '已发布' : '草稿' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="scope">
            <el-button size="small" @click="editCase(scope.row)">编辑</el-button>
            <el-button 
              size="small" 
              :type="scope.row.isPublished ? 'warning' : 'success'"
              @click="togglePublishStatus(scope.row)"
            >
              {{ scope.row.isPublished ? '取消发布' : '发布' }}
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
        <el-form-item label="所属分类" prop="categoryId">
          <el-select v-model="caseForm.categoryId" placeholder="选择所属分类">
            <el-option
              v-for="item in categoryOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="客户名称" prop="client">
          <el-input v-model="caseForm.client" placeholder="请输入客户名称" />
        </el-form-item>
        <el-form-item label="完成时间" prop="completionTime">
          <el-date-picker
            v-model="caseForm.completionTime"
            type="date"
            placeholder="选择完成日期"
            format="YYYY-MM-DD"
          />
        </el-form-item>
        <el-form-item label="封面图" prop="coverImage">
          <el-upload
            class="cover-uploader"
            action="#"
            :show-file-list="false"
            :auto-upload="false"
            :on-change="handleCoverChange"
          >
            <img v-if="caseForm.coverImage" :src="caseForm.coverImage" class="cover-preview" />
            <el-icon v-else class="uploader-icon"><Plus /></el-icon>
          </el-upload>
        </el-form-item>
        <el-form-item label="案例简介" prop="summary">
          <el-input
            v-model="caseForm.summary"
            type="textarea"
            :rows="3"
            placeholder="请输入案例简介"
          />
        </el-form-item>
        <el-form-item label="案例详情" prop="content">
          <el-input
            v-model="caseForm.content"
            type="textarea"
            :rows="5"
            placeholder="请输入案例详情"
          />
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
        <el-form-item label="发布状态">
          <el-switch
            v-model="caseForm.isPublished"
            :active-text="caseForm.isPublished ? '已发布' : '草稿'"
          />
        </el-form-item>
        <el-form-item label="排序" prop="sortOrder">
          <el-input-number v-model="caseForm.sortOrder" :min="0" :max="999" />
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
import { Plus } from '@element-plus/icons-vue'
import { getCaseList, getCaseById, createCase, updateCase, deleteCase, toggleCaseStatus } from '@/api/case'
import { getAllActiveCategories } from '@/api/category'

// 类型定义
interface Case {
  id: number
  title: string
  categoryId: number
  categoryName: string
  client: string
  completionTime: string | Date
  coverImage: string
  summary: string
  content: string
  images: string[]
  isPublished: boolean
  sortOrder: number
  createTime: string
}

interface CaseForm {
  id?: number
  title: string
  categoryId: number | null
  client: string
  completionTime: string | Date
  coverImage: string
  summary: string
  content: string
  images: string[]
  isPublished: boolean
  sortOrder: number
}

interface Category {
  id: number
  name: string
}

interface SearchForm {
  title: string
  categoryId: number | null
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
  categoryId: null
})

// 案例表单
const caseFormRef = ref<any>(null)
const dialogVisible = ref(false)
const isEdit = ref(false)
const caseForm = reactive<CaseForm>({
  title: '',
  categoryId: null,
  client: '',
  completionTime: '',
  coverImage: '',
  summary: '',
  content: '',
  images: [],
  isPublished: false,
  sortOrder: 0
})

// 分类选项
const categoryOptions = ref<Category[]>([])

// 表单验证规则
const caseRules = {
  title: [{ required: true, message: '请输入案例标题', trigger: 'blur' }],
  categoryId: [{ required: true, message: '请选择所属分类', trigger: 'change' }],
  client: [{ required: true, message: '请输入客户名称', trigger: 'blur' }],
  coverImage: [{ required: true, message: '请上传封面图', trigger: 'change' }],
  summary: [{ required: true, message: '请输入案例简介', trigger: 'blur' }]
}

// 初始化加载
onMounted(() => {
  loadCases()
  loadCategories()
})

// 加载分类
const loadCategories = async () => {
  try {
    const res = await getAllActiveCategories()
    categoryOptions.value = res.data || []
  } catch (error) {
    console.error('获取分类失败', error)
    ElMessage.error('获取分类列表失败')
  }
}

// 加载案例列表
const loadCases = async () => {
  loading.value = true
  try {
    const res = await getCaseList({
      page: page.value,
      pageSize: pageSize.value,
      title: searchForm.title,
      categoryId: searchForm.categoryId
    })
    
    if (res.data) {
      caseList.value = res.data.items || []
      total.value = res.data.totalCount || 0
    }
  } catch (error) {
    console.error('获取案例列表失败', error)
    ElMessage.error('获取案例列表失败')
  } finally {
    loading.value = false
  }
}

// 重置搜索
const resetSearch = () => {
  searchForm.title = ''
  searchForm.categoryId = null
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
    categoryId: null,
    client: '',
    completionTime: '',
    coverImage: '',
    summary: '',
    content: '',
    images: [],
    isPublished: false,
    sortOrder: 0
  })
  dialogVisible.value = true
}

// 编辑案例
const editCase = async (row: Case) => {
  isEdit.value = true
  try {
    const res = await getCaseById(row.id)
    if (res.data) {
      Object.assign(caseForm, res.data)
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
    `确定要${row.isPublished ? '取消发布' : '发布'}该案例吗？`,
    '提示',
    {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    }
  ).then(async () => {
    try {
      await toggleCaseStatus(row.id)
      row.isPublished = !row.isPublished
      ElMessage.success(`${row.isPublished ? '发布' : '取消发布'}成功`)
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

// 图片上传处理
interface UploadFile {
  raw: File
}

const handleCoverChange = (file: UploadFile) => {
  const reader = new FileReader()
  reader.readAsDataURL(file.raw)
  reader.onload = () => {
    if (reader.result) {
      caseForm.coverImage = reader.result as string
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
      caseForm.images.push(reader.result as string)
    }
  }
}

const handleImageRemove = (file: any, fileList: any[]) => {
  const index = caseForm.images.indexOf(file.url)
  if (index !== -1) {
    caseForm.images.splice(index, 1)
  }
}

// 提交表单
const submitCaseForm = async () => {
  if (!caseFormRef.value) return

  try {
    await caseFormRef.value.validate()
    
    if (isEdit.value && caseForm.id) {
      // 更新案例
      await updateCase(caseForm.id, caseForm)
      ElMessage.success('更新成功')
    } else {
      // 创建案例
      await createCase(caseForm)
      ElMessage.success('创建成功')
    }
    
    dialogVisible.value = false
    loadCases()
  } catch (error) {
    console.error('表单验证失败', error)
    ElMessage.error('请填写必填项')
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
</style>