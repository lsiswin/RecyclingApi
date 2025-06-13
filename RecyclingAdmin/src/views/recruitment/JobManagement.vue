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

      <!-- 筛选条件区域 -->
      <div class="filter-container">
        <el-form :inline="true" :model="filterForm" class="filter-form">
          <el-form-item label="部门">
            <el-select style="width: 100px" v-model="filterForm.department" placeholder="选择部门" clearable @change="handleFilter">
              <el-option label="全部" value="" />
              <el-option v-for="dept in departments" :key="dept" :label="dept" :value="dept" />
            </el-select>
          </el-form-item>
          <el-form-item label="状态">
            <el-select style="width: 100px" v-model="filterForm.status" placeholder="选择状态" clearable @change="handleFilter">
              <el-option label="全部" value="" />
              <el-option label="上架中" value="active" />
              <el-option label="已下架" value="inactive" />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-checkbox v-model="filterForm.includeInactive" @change="handleFilter">
              包含未激活职位
            </el-checkbox>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="handleFilter">搜索</el-button>
            <el-button @click="resetFilter">重置</el-button>
          </el-form-item>
        </el-form>
      </div>

      <el-table :data="filteredJobs" style="width: 100%">
        <el-table-column align="center" prop="id" label="ID" width="80" />
        <el-table-column align="center" prop="title" label="职位名称" />
        <el-table-column align="center" prop="department" label="部门" />
        <el-table-column align="center" prop="location" label="工作地点" />
        <el-table-column align="center" prop="salaryRange" label="薪资范围" />
        <el-table-column align="center" label="状态" width="100">
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

      <!-- 分页组件 -->
      <div class="pagination-container">
        <el-pagination
          v-model:current-page="pagination.currentPage"
          v-model:page-size="pagination.pageSize"
          :page-sizes="[10, 20, 50]"
          layout="total, sizes, prev, pager, next, jumper"
          :total="pagination.total"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-card>

    <!-- 添加/编辑职位对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑职位' : '发布职位'"
      width="600px"
    >
      <el-form :model="form" :rules="rules" ref="formRef" label-width="100px">
        <el-form-item label="职位名称" prop="title" >
          <el-input v-model="form.title" placeholder="请输入职位名称" />
        </el-form-item>
        <el-form-item label="部门" prop="department">
          <el-select v-model="form.department" placeholder="请选择部门">
            <el-option v-for="dept in departments" :key="dept" :label="dept" :value="dept" />
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
import { ref, reactive, onMounted, computed } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import dayjs from 'dayjs'
import { 
  getJobPositions, 
  getAllJobPositions, 
  createJobPosition, 
  updateJobPosition, 
  deleteJobPosition, 
  changeJobPositionStatus,
  type JobPositionDTO,
  type CreateJobPositionDTO,
  type UpdateJobPositionDTO
} from '@/api/recruitment'

interface Job {
  id: number;
  title: string;
  department: string;
  location: string;
  salaryRange: string;
  description: string;
  requirements: string;
  status: 'active' | 'inactive';
  applicantCount: number;
  publishDate: string;
}

const jobs = ref<Job[]>([])
const dialogVisible = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()

// 筛选表单
const filterForm = reactive({
  department: '',
  status: '',
  includeInactive: false
})

// 分页数据
const pagination = reactive({
  currentPage: 1,
  pageSize: 10,
  total: 0
})

// 可选部门列表
const departments = ref(['技术部', '客服部', '市场部', '人事部'])

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

const loading = ref(false)

const loadJobs = async () => {
  try {
    // 设置加载状态
    loading.value = true;
    
    // 调用API获取职位列表
    const response = await getJobPositions();
    console.log("response",response);
    
    if (response.data && response.data.success && response.data.data) {
      // 映射后端数据到前端模型
      const responseData = response.data.data;
      jobs.value = responseData.items.map((item: JobPositionDTO) => ({
        id: item.id,
        title: item.title,
        department: item.department,
        location: item.location,
        salaryRange: `${item.salaryMin}-${item.salaryMax}`,
        description: item.description,
        requirements: item.requirements,
        status: item.isActive ? 'active' : 'inactive',
        applicantCount: item.applicationCount || 0,
        publishDate: formatDate(item.postedDate)
      }));
      
      pagination.total = responseData.totalCount;
    } else {
      // 处理API返回错误
      ElMessage.error(response.data?.message || '获取职位列表失败');
    }
  } catch (error) {
    console.error('加载职位列表失败', error);
    ElMessage.error('加载职位列表失败');
  } finally {
    loading.value = false;
  }
};

// 格式化日期
const formatDate = (dateStr: string) => {
  if (!dateStr) return '';
  return dayjs(dateStr).format('YYYY-MM-DD');
};

// 根据筛选条件过滤职位
const filteredJobs = computed(() => {
  let result = [...jobs.value]
  
  // 按部门筛选
  if (filterForm.department) {
    result = result.filter(job => job.department === filterForm.department)
  }
  
  // 按状态筛选
  if (filterForm.status) {
    result = result.filter(job => job.status === filterForm.status)
  }
  
  // 是否包含未激活职位
  if (!filterForm.includeInactive) {
    result = result.filter(job => job.status === 'active')
  }
  
  return result
})

const getStatusText = (status: string) => {
  return status === 'active' ? '上架中' : '已下架'
}

const getStatusTagType = (status: string) => {
  return status === 'active' ? 'success' : 'danger'
}

// 处理筛选
const handleFilter = () => {
  pagination.currentPage = 1
  loadJobs()
}

// 重置筛选条件
const resetFilter = () => {
  filterForm.department = ''
  filterForm.status = ''
  filterForm.includeInactive = false
  handleFilter()
}

// 处理分页大小变化
const handleSizeChange = (size: number) => {
  pagination.pageSize = size
  loadJobs()
}

// 处理页码变化
const handleCurrentChange = (page: number) => {
  pagination.currentPage = page
  loadJobs()
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
  if (!formRef.value) return;
  
  await formRef.value.validate(async (valid) => {
    if (valid) {
      try {
        // 解析薪资范围为最小值和最大值
        const salaryParts = form.salaryRange.split('-');
        const salaryMin = Number(salaryParts[0].replace(/[^\d]/g, ''));
        const salaryMax = Number(salaryParts[1]?.replace(/[^\d]/g, '') || salaryMin);
        
        const formData: CreateJobPositionDTO | UpdateJobPositionDTO = {
          id: isEdit.value ? form.id : 0,
          title: form.title,
          department: form.department,
          location: form.location,
          salaryMin,
          salaryMax,
          description: form.description,
          requirements: form.requirements,
          isActive: form.status === 'active'
        };
        
        let response;
        if (isEdit.value) {
          // 更新职位
          response = await updateJobPosition(form.id, formData as UpdateJobPositionDTO);
          if (response.data && response.data.success) {
            ElMessage.success('职位更新成功');
            dialogVisible.value = false;
            loadJobs(); // 重新加载职位列表
          } else {
            ElMessage.error(response.data?.message || '更新职位失败');
          }
        } else {
          // 添加职位
          response = await createJobPosition(formData as CreateJobPositionDTO);
          if (response.data && response.data.success) {
            ElMessage.success('职位发布成功');
            dialogVisible.value = false;
            loadJobs(); // 重新加载职位列表
          } else {
            ElMessage.error(response.data?.message || '发布职位失败');
          }
        }
      } catch (error) {
        console.error('保存职位失败', error);
        ElMessage.error('保存职位失败');
      }
    }
  });
};

const toggleJobStatus = async (job: Job) => {
  try {
    const response = await changeJobPositionStatus(job.id, job.status === 'inactive');
    
    if (response.data && response.data.success) {
      job.status = job.status === 'active' ? 'inactive' : 'active';
      ElMessage.success(`职位已${job.status === 'active' ? '上架' : '下架'}`);
    } else {
      ElMessage.error(response.data?.message || '更改状态失败');
    }
  } catch (error) {
    console.error('更改状态失败', error);
    ElMessage.error('更改状态失败');
  }
};

const deleteJob = (job: Job) => {
  ElMessageBox.confirm('确定要删除这个职位吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      const response = await deleteJobPosition(job.id);
      
      if (response.data && response.data.success) {
        const index = jobs.value.findIndex(j => j.id === job.id);
        if (index !== -1) {
          jobs.value.splice(index, 1);
          ElMessage.success('删除成功');
        }
      } else {
        ElMessage.error(response.data?.message || '删除失败');
      }
    } catch (error) {
      console.error('删除失败', error);
      ElMessage.error('删除失败');
    }
  }).catch(() => {
    // 用户取消删除
  });
};

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

.filter-container {
  margin-bottom: 20px;
}

.filter-form {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
}

.pagination-container {
  margin-top: 20px;
  text-align: right;
}
</style> 