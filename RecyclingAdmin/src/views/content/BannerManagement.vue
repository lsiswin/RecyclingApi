<template>
  <div class="banner-management">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>首页轮播图管理</span>
          <el-button type="primary" @click="showAddDialog">
            <el-icon><Plus /></el-icon>
            添加轮播图
          </el-button>
        </div>
      </template>

      <!-- 添加筛选条件区域 -->
      <div class="filter-container">
        <el-form :inline="true" :model="queryParams" class="filter-form">
          <el-form-item label="标题">
            <el-input v-model="queryParams.Keyword" placeholder="请输入标题关键词" clearable @clear="handleFilter" />
          </el-form-item>
          <el-form-item label="状态">
            <el-select 
              v-model="queryParams.IsActive" 
              placeholder="请选择" 
              clearable 
              @change="handleFilter"
              style="width: 90px"
            >
              <el-option key="all" label="全部" :value="null" />
              <el-option key="active" label="启用" :value="true" />
              <el-option key="inactive" label="禁用" :value="false" />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="handleFilter">查询</el-button>
            <el-button @click="resetFilter">重置</el-button>
          </el-form-item>
        </el-form>
      </div>

      <el-table :data="banners" style="width: 100%">
        <el-table-column align="center" prop="id" label="ID" width="80" />
        <el-table-column label="预览图" align="center" width="120">
          <template #default="scope">
            <el-image
              :src="scope.row.imageUrl"
              :preview-src-list="[scope.row.imageUrl]"
              style="width: 80px; height: 45px"
              fit="cover"
            />
          </template>
        </el-table-column>
        <el-table-column prop="title" label="标题" align="center"/>
        <el-table-column prop="description" label="描述" show-overflow-tooltip align="center"/>
        <el-table-column prop="linkUrl" label="链接地址" show-overflow-tooltip align="center"/>
        <el-table-column label="状态" width="100" align="center">
          <template #default="scope">
            <el-tag :type="scope.row.isActive ? 'success' : 'danger'">
              {{ scope.row.isActive ? '启用' : '禁用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="sort" label="排序" width="80" align="center"/>
        <el-table-column label="操作" width="200" align="center">
          <template #default="scope">
            <el-button size="small" @click="editBanner(scope.row)">编辑</el-button>
            <el-button
              size="small"
              :type="scope.row.isActive ? 'warning' : 'success'"
              @click="toggleStatus(scope.row)"
            >
              {{ scope.row.isActive ? '禁用' : '启用' }}
            </el-button>
            <el-button size="small" type="danger" @click="deleteBanner(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 添加分页组件 -->
      <pagination 
        v-model:page="queryParams.pageindex" 
        v-model:limit="queryParams.pageSize"
        :total="total"
        @pagination="loadBanners"
      />
    </el-card>

    <!-- 添加/编辑对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑轮播图' : '添加轮播图'"
      width="600px"
    >
      <el-form :model="form" :rules="rules" ref="formRef" label-width="100px">
        <el-form-item label="标题" prop="title">
          <el-input v-model="form.title" placeholder="请输入轮播图标题" />
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input
            v-model="form.description"
            type="textarea"
            :rows="3"
            placeholder="请输入轮播图描述"
          />
        </el-form-item>
        <el-form-item label="图片" prop="imageUrl">
          <el-upload
            class="banner-uploader"
            action="#"
            :show-file-list="false"
            :before-upload="beforeUpload"
            :http-request="uploadFile"
          >
            <img v-if="form.imageUrl" :src="form.imageUrl" class="banner-image" />
            <el-icon v-else class="banner-uploader-icon"><Plus /></el-icon>
          </el-upload>
        </el-form-item>
        <el-form-item label="链接地址" prop="linkUrl">
          <el-input v-model="form.linkUrl" placeholder="请输入跳转链接地址" />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number v-model="form.sort" :min="0" :max="999" />
        </el-form-item>
        <el-form-item label="状态" prop="isActive">
          <el-switch v-model="form.isActive" />
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
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import { getBannerList, getBannerById, createBanner, updateBanner, deleteBanner as removeBanner, toggleBannerStatus } from '@/api/banner'
import Pagination from '@/components/Pagination/index.vue'

interface Banner {
  id: number
  title: string
  description: string
  imageUrl: string
  linkUrl: string
  sort: number
  isActive: boolean
  createdAt: string
}

const banners = ref<Banner[]>([])
const dialogVisible = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()
const loading = ref(false)
const total = ref(0)

// 查询参数对象
const queryParams = reactive({
  pageindex: 1,
  pageSize: 10,
  Keyword: '',
  IsActive: null as boolean | null
})

const form = reactive({
  id: 0,
  title: '',
  description: '',
  imageUrl: '',
  linkUrl: '',
  sort: 0,
  isActive: true
})

const rules: FormRules = {
  title: [{ required: true, message: '请输入标题', trigger: 'blur' }],
  description: [{ required: true, message: '请输入描述', trigger: 'blur' }],
  imageUrl: [{ required: true, message: '请上传图片', trigger: 'change' }]
}

// 查询方法
const handleFilter = () => {
  queryParams.pageindex = 1
  loadBanners()
}

// 重置筛选条件
const resetFilter = () => {
  queryParams.Keyword = ''
  queryParams.IsActive = null
  handleFilter()
}

const loadBanners = async () => {
  loading.value = true
  try {
    const res = await getBannerList(queryParams)
    console.log(res);
    
    if (res.data) {
      banners.value = res.data.data.items || []
      total.value = res.data.data.totalCount || 0
    }
  } catch (error) {
    console.error('获取轮播图列表失败', error)
    ElMessage.error('获取轮播图列表失败')
  } finally {
    loading.value = false
  }
}

const showAddDialog = () => {
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const editBanner = async (banner: Banner) => {
  isEdit.value = true
  try {
    const res = await getBannerById(banner.id)
    if (res.data.data) {
      Object.assign(form, res.data.data)
    }
    dialogVisible.value = true
  } catch (error) {
    console.error('获取轮播图详情失败', error)
    ElMessage.error('获取轮播图详情失败')
  }
}

const resetForm = () => {
  Object.assign(form, {
    id: 0,
    title: '',
    description: '',
    imageUrl: '',
    linkUrl: '',
    sort: 0,
    isActive: true
  })
}

const submitForm = async () => {
  if (!formRef.value) return
  
  await formRef.value.validate(async (valid) => {
    if (valid) {
      try {
        if (isEdit.value) {
          // 更新轮播图
          await updateBanner(form.id, form)
          ElMessage.success('轮播图更新成功')
        } else {
          // 添加轮播图
          await createBanner(form)
          ElMessage.success('轮播图添加成功')
        }
        dialogVisible.value = false
        loadBanners()
      } catch (error) {
        console.error('保存轮播图失败', error)
        ElMessage.error('操作失败')
      }
    }
  })
}

const toggleStatus = async (banner: Banner) => {
  try {
    await toggleBannerStatus(banner.id)
    banner.isActive = !banner.isActive
    ElMessage.success(`轮播图已${banner.isActive ? '启用' : '禁用'}`)
  } catch (error) {
    console.error('更新状态失败', error)
    ElMessage.error('操作失败')
  }
}

const deleteBanner = (banner: Banner) => {
  ElMessageBox.confirm('确定要删除这个轮播图吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      await removeBanner(banner.id)
      ElMessage.success('删除成功')
      loadBanners()
    } catch (error) {
      console.error('删除失败', error)
      ElMessage.error('删除失败')
    }
  })
}

const beforeUpload = (file: File) => {
  const isJPG = file.type === 'image/jpeg' || file.type === 'image/png'
  const isLt2M = file.size / 1024 / 1024 < 2

  if (!isJPG) {
    ElMessage.error('上传图片只能是 JPG/PNG 格式!')
  }
  if (!isLt2M) {
    ElMessage.error('上传图片大小不能超过 2MB!')
  }
  return isJPG && isLt2M
}

const uploadFile = (options: any) => {
  // 模拟文件上传
  const file = options.file
  const reader = new FileReader()
  reader.onload = (e) => {
    form.imageUrl = e.target?.result as string
  }
  reader.readAsDataURL(file)
}

onMounted(() => {
  loadBanners()
})
</script>

<style scoped>
.banner-management {
  padding: 0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.filter-container {
  margin-bottom: 20px;
}

.filter-form {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
}

.banner-uploader {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: border-color 0.3s;
}

.banner-uploader:hover {
  border-color: #409eff;
}

.banner-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 200px;
  height: 100px;
  text-align: center;
  line-height: 100px;
}

.banner-image {
  width: 200px;
  height: 100px;
  display: block;
  object-fit: cover;
}
</style> 