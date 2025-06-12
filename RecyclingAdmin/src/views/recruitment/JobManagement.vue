<template>
  <div class="job-management">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>岗位发布/下架</span>
          <el-button type="primary" @click="showAddDialog">
            <el-icon><Plus /></el-icon>
            发布职位
          </el-button>
        </div>
      </template>

      <el-table :data="jobs" style="width: 100%">
        <el-table-column prop="id" label="ID" width="80" />
        <el-table-column prop="title" label="职位名称" />
        <el-table-column prop="department" label="部门" />
        <el-table-column prop="location" label="工作地点" />
        <el-table-column prop="salaryRange" label="薪资范围" />
        <el-table-column label="状态" width="100">
          <template #default="scope">
            <el-tag :type="getStatusTagType(scope.row.status)">
              {{ getStatusText(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="applicantCount" label="应聘人数" width="100" />
        <el-table-column prop="publishDate" label="发布时间" width="120" />
        <el-table-column label="操作" width="200">
          <template #default="scope">
            <el-button size="small" @click="editJob(scope.row)">编辑</el-button>
            <el-button
              size="small"
              :type="scope.row.status === 'active' ? 'warning' : 'success'"
              @click="toggleJobStatus(scope.row)"
            >
              {{ scope.row.status === 'active' ? '下架' : '上架' }}
            </el-button>
            <el-button size="small" type="danger" @click="deleteJob(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- 添加/编辑职位对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑职位' : '发布职位'"
      width="600px"
    >
      <el-form :model="form" :rules="rules" ref="formRef" label-width="100px">
        <el-form-item label="职位名称" prop="title">
          <el-input v-model="form.title" placeholder="请输入职位名称" />
        </el-form-item>
        <el-form-item label="部门" prop="department">
          <el-select v-model="form.department" placeholder="请选择部门">
            <el-option label="技术部" value="技术部" />
            <el-option label="客服部" value="客服部" />
            <el-option label="市场部" value="市场部" />
            <el-option label="人事部" value="人事部" />
          </el-select>
        </el-form-item>
        <el-form-item label="工作地点" prop="location">
          <el-input v-model="form.location" placeholder="请输入工作地点" />
        </el-form-item>
        <el-form-item label="薪资范围" prop="salaryRange">
          <el-input v-model="form.salaryRange" placeholder="例如：8K-15K" />
        </el-form-item>
        <el-form-item label="职位描述" prop="description">
          <el-input
            v-model="form.description"
            type="textarea"
            :rows="4"
            placeholder="请输入职位描述"
          />
        </el-form-item>
        <el-form-item label="任职要求" prop="requirements">
          <el-input
            v-model="form.requirements"
            type="textarea"
            :rows="4"
            placeholder="请输入任职要求"
          />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio-group v-model="form.status">
            <el-radio label="active">上架</el-radio>
            <el-radio label="inactive">下架</el-radio>
          </el-radio-group>
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

interface Job {
  id: number
  title: string
  department: string
  location: string
  salaryRange: string
  description: string
  requirements: string
  status: 'active' | 'inactive'
  applicantCount: number
  publishDate: string
}

const jobs = ref<Job[]>([])
const dialogVisible = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()

const form = reactive({
  id: 0,
  title: '',
  department: '',
  location: '',
  salaryRange: '',
  description: '',
  requirements: '',
  status: 'active' as 'active' | 'inactive'
})

const rules: FormRules = {
  title: [{ required: true, message: '请输入职位名称', trigger: 'blur' }],
  department: [{ required: true, message: '请选择部门', trigger: 'change' }],
  location: [{ required: true, message: '请输入工作地点', trigger: 'blur' }],
  salaryRange: [{ required: true, message: '请输入薪资范围', trigger: 'blur' }],
  description: [{ required: true, message: '请输入职位描述', trigger: 'blur' }],
  requirements: [{ required: true, message: '请输入任职要求', trigger: 'blur' }]
}

const loadJobs = () => {
  // 模拟数据
  jobs.value = [
    {
      id: 1,
      title: '前端开发工程师',
      department: '技术部',
      location: '北京',
      salaryRange: '15K-25K',
      description: '负责前端页面开发和维护',
      requirements: '熟练掌握Vue.js、React等前端框架',
      status: 'active',
      applicantCount: 12,
      publishDate: '2025-01-11'
    },
    {
      id: 2,
      title: '客服专员',
      department: '客服部',
      location: '上海',
      salaryRange: '8K-12K',
      description: '负责客户咨询和问题解答',
      requirements: '良好的沟通能力，有客服经验优先',
      status: 'active',
      applicantCount: 8,
      publishDate: '2025-01-10'
    },
    {
      id: 3,
      title: '市场推广专员',
      department: '市场部',
      location: '广州',
      salaryRange: '10K-18K',
      description: '负责市场推广和品牌宣传',
      requirements: '有市场营销经验，熟悉数字营销',
      status: 'inactive',
      applicantCount: 5,
      publishDate: '2025-01-09'
    }
  ]
}

const getStatusText = (status: string) => {
  return status === 'active' ? '上架中' : '已下架'
}

const getStatusTagType = (status: string) => {
  return status === 'active' ? 'success' : 'danger'
}

const showAddDialog = () => {
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const editJob = (job: Job) => {
  isEdit.value = true
  Object.assign(form, job)
  dialogVisible.value = true
}

const resetForm = () => {
  Object.assign(form, {
    id: 0,
    title: '',
    department: '',
    location: '',
    salaryRange: '',
    description: '',
    requirements: '',
    status: 'active'
  })
}

const submitForm = async () => {
  if (!formRef.value) return
  
  await formRef.value.validate((valid) => {
    if (valid) {
      if (isEdit.value) {
        // 更新职位
        const index = jobs.value.findIndex(j => j.id === form.id)
        if (index !== -1) {
          jobs.value[index] = { 
            ...form, 
            applicantCount: jobs.value[index].applicantCount,
            publishDate: jobs.value[index].publishDate 
          }
        }
        ElMessage.success('职位更新成功')
      } else {
        // 添加职位
        const newJob: Job = {
          ...form,
          id: Date.now(),
          applicantCount: 0,
          publishDate: new Date().toISOString().split('T')[0]
        }
        jobs.value.unshift(newJob)
        ElMessage.success('职位发布成功')
      }
      dialogVisible.value = false
    }
  })
}

const toggleJobStatus = (job: Job) => {
  job.status = job.status === 'active' ? 'inactive' : 'active'
  ElMessage.success(`职位已${job.status === 'active' ? '上架' : '下架'}`)
}

const deleteJob = (job: Job) => {
  ElMessageBox.confirm('确定要删除这个职位吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(() => {
    const index = jobs.value.findIndex(j => j.id === job.id)
    if (index !== -1) {
      jobs.value.splice(index, 1)
      ElMessage.success('删除成功')
    }
  })
}

onMounted(() => {
  loadJobs()
})
</script>

<style scoped>
.job-management {
  padding: 0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style> 