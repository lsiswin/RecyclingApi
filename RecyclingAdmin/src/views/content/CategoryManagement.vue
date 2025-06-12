<template>
  <div class="category-management">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>案例分类管理</span>
          <el-button type="primary" @click="openCreateDialog">
            <el-icon><Plus /></el-icon>
            添加分类
          </el-button>
        </div>
      </template>

      <!-- 分类列表 -->
      <el-table
        v-loading="loading"
        :data="categories"
        border
        style="width: 100%"
      >
        <el-table-column prop="id" label="ID" width="80" />
        <el-table-column prop="name" label="分类名称" width="200" />
        <el-table-column prop="description" label="描述" min-width="180" show-overflow-tooltip />
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
        <el-table-column prop="casesCount" label="案例数量" width="100" align="center" />
        <el-table-column prop="sortOrder" label="排序" width="100" align="center" />
        <el-table-column label="状态" width="100">
          <template #default="scope">
            <el-tag :type="scope.row.isActive ? 'success' : 'info'">
              {{ scope.row.isActive ? '启用' : '禁用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="220" fixed="right">
          <template #default="scope">
            <el-button size="small" @click="editCategory(scope.row)">编辑</el-button>
            <el-button 
              size="small" 
              :type="scope.row.isActive ? 'warning' : 'success'"
              @click="toggleStatus(scope.row)"
            >
              {{ scope.row.isActive ? '禁用' : '启用' }}
            </el-button>
            <el-button
              size="small"
              type="danger"
              @click="handleDelete(scope.row)"
              :disabled="scope.row.casesCount > 0"
            >删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- 添加/编辑分类对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑分类' : '添加分类'"
      width="500px"
      destroy-on-close
    >
      <el-form
        ref="categoryFormRef"
        :model="categoryForm"
        :rules="categoryRules"
        label-width="100px"
      >
        <el-form-item label="分类名称" prop="name">
          <el-input v-model="categoryForm.name" placeholder="请输入分类名称" />
        </el-form-item>
        <el-form-item label="分类描述" prop="description">
          <el-input
            v-model="categoryForm.description"
            type="textarea"
            :rows="3"
            placeholder="请输入分类描述"
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
            <img v-if="categoryForm.coverImage" :src="categoryForm.coverImage" class="cover-preview" />
            <el-icon v-else class="uploader-icon"><Plus /></el-icon>
          </el-upload>
        </el-form-item>
        <el-form-item label="排序" prop="sortOrder">
          <el-input-number v-model="categoryForm.sortOrder" :min="0" :max="999" />
        </el-form-item>
        <el-form-item label="状态">
          <el-switch
            v-model="categoryForm.isActive"
            :active-text="categoryForm.isActive ? '启用' : '禁用'"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitForm">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus } from '@element-plus/icons-vue'
import { getCategoryList, createCategory, updateCategory, deleteCategory, toggleCategoryStatus } from '@/api/category'

// 类型定义
interface Category {
  id: number
  name: string
  description: string
  coverImage: string
  sortOrder: number
  isActive: boolean
  casesCount: number
}

interface CategoryForm {
  id?: number
  name: string
  description: string
  coverImage: string
  sortOrder: number
  isActive: boolean
}

// 列表数据
const loading = ref(false)
const categories = ref<Category[]>([])
const page = ref(1)
const pageSize = ref(10)
const total = ref(0)

// 表单相关
const categoryFormRef = ref<any>(null)
const dialogVisible = ref(false)
const isEdit = ref(false)
const categoryForm = reactive<CategoryForm>({
  name: '',
  description: '',
  coverImage: '',
  sortOrder: 0,
  isActive: true
})

// 表单验证规则
const categoryRules = {
  name: [
    { required: true, message: '请输入分类名称', trigger: 'blur' },
    { min: 2, max: 20, message: '长度在 2 到 20 个字符', trigger: 'blur' }
  ],
  description: [
    { required: true, message: '请输入分类描述', trigger: 'blur' }
  ],
  coverImage: [
    { required: true, message: '请上传封面图', trigger: 'change' }
  ]
}

// 初始化加载
onMounted(() => {
  loadCategories()
})

// 加载分类列表
const loadCategories = async () => {
  loading.value = true
  try {
    const res = await getCategoryList({
      page: page.value,
      pageSize: pageSize.value
    })
    
    if (res.data) {
      categories.value = res.data.items || []
      total.value = res.data.totalCount || 0
    }
  } catch (error) {
    console.error('获取分类列表失败', error)
    ElMessage.error('获取分类列表失败')
  } finally {
    loading.value = false
  }
}

// 打开创建对话框
const openCreateDialog = () => {
  isEdit.value = false
  Object.assign(categoryForm, {
    name: '',
    description: '',
    coverImage: '',
    sortOrder: 0,
    isActive: true
  })
  dialogVisible.value = true
}

// 编辑分类
const editCategory = (row: Category) => {
  isEdit.value = true
  Object.assign(categoryForm, {
    id: row.id,
    name: row.name,
    description: row.description,
    coverImage: row.coverImage,
    sortOrder: row.sortOrder,
    isActive: row.isActive
  })
  dialogVisible.value = true
}

// 切换状态
const toggleStatus = (row: Category) => {
  ElMessageBox.confirm(
    `确定要${row.isActive ? '禁用' : '启用'}该分类吗？`,
    '提示',
    {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    }
  ).then(async () => {
    try {
      await toggleCategoryStatus(row.id)
      row.isActive = !row.isActive
      ElMessage.success(`${row.isActive ? '启用' : '禁用'}成功`)
    } catch (error) {
      console.error('更新状态失败', error)
      ElMessage.error('操作失败')
    }
  })
}

// 删除分类
const handleDelete = (row: Category) => {
  if (row.casesCount > 0) {
    ElMessage.warning('该分类下有关联的案例，无法删除')
    return
  }
  
  ElMessageBox.confirm('确定要删除该分类吗？删除后不可恢复！', '警告', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      await deleteCategory(row.id)
      ElMessage.success('删除成功')
      loadCategories()
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
      categoryForm.coverImage = reader.result as string
    }
  }
}

// 提交表单
const submitForm = async () => {
  if (!categoryFormRef.value) return

  try {
    await categoryFormRef.value.validate()
    
    if (isEdit.value && categoryForm.id) {
      // 更新分类
      await updateCategory(categoryForm.id, categoryForm)
      ElMessage.success('更新成功')
    } else {
      // 创建分类
      await createCategory(categoryForm)
      ElMessage.success('创建成功')
    }
    
    dialogVisible.value = false
    loadCategories()
  } catch (error) {
    console.error('表单验证失败', error)
    ElMessage.error('请填写必填项')
  }
}
</script>

<style scoped>
.category-management {
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.cover-uploader {
  width: 200px;
  height: 120px;
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
  width: 200px;
  height: 120px;
  line-height: 120px;
  text-align: center;
}

.cover-preview {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style> 