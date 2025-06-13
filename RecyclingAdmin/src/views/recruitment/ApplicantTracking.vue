<template>
  <div class="applicant-tracking">
    <el-card>
      <template #header>
        <div class="header-container">
          <span class="header-title">应聘者状态跟踪</span>
          <div class="header-actions">
            <el-button type="primary" @click="refreshData">刷新数据</el-button>
          </div>
        </div>
      </template>

      <!-- 筛选表单 -->
      <el-form :model="searchForm" inline class="filter-form">
        <el-form-item label="姓名">
          <el-input v-model="searchForm.name" placeholder="请输入姓名" clearable />
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="searchForm.email" placeholder="请输入邮箱" clearable />
        </el-form-item>
        <el-form-item label="职位">
          <el-select style="width: 120px" v-model="searchForm.jobPositionId" placeholder="请选择职位" clearable>
            <el-option 
              v-for="position in positions" 
              :key="position.id" 
              :label="position.title" 
              :value="position.id" 
            />
          </el-select>
        </el-form-item>
        <el-form-item label="状态">
          <el-select style="width: 120px" v-model="searchForm.status" placeholder="请选择状态" clearable>
            <el-option v-for="(name, value) in statusOptions" :key="value" :label="name" :value="Number(value)" />
          </el-select>
        </el-form-item>
        <el-form-item label="申请日期">
          <el-date-picker
            v-model="searchForm.dateRange"
            type="daterange"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
            clearable
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="search">搜索</el-button>
          <el-button @click="resetSearch">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 数据表格 -->
      <el-table :data="applications" border stripe v-loading="loading" style="width: 100%">
        <el-table-column align="center" prop="id" label="ID" width="60" />
        <el-table-column align="center" prop="name" label="姓名" width="80" />
        <el-table-column align="center" prop="email" label="邮箱" width="180" />
        <el-table-column align="center" prop="phone" label="电话" width="120" />
        <el-table-column align="center" prop="jobPositionTitle" label="应聘职位" width="130" />
        <el-table-column align="center" prop="statusName" label="状态" width="100">
          <template #default="scope">
            <el-tag :type="getStatusTagType(scope.row.status)">{{ scope.row.statusName }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column align="center" prop="appliedDate" label="申请日期" width="120">
          <template #default="scope">
            {{ formatDate(scope.row.appliedDate) }}
          </template>
        </el-table-column>
        <el-table-column align="center" label="操作" width="240">
          <template #default="scope">
            <el-button type="primary" size="small" @click="viewApplication(scope.row)">查看</el-button>
            <el-button type="success" size="small" @click="updateStatus(scope.row)">更新状态</el-button>
            <el-button type="danger" size="small" @click="handleDelete(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <div class="pagination-container">
        <el-pagination
          background
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
          :page-size="searchForm.pageSize"
          :page-sizes="[10, 20, 50, 100]"
          :current-page="searchForm.pageNumber"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>

      <!-- 查看简历对话框 -->
      <el-dialog
        v-model="dialogVisible"
        title="应聘详情"
        width="70%"
        :before-close="handleDialogClose"
      >
        <template v-if="currentApplication">
          <el-descriptions title="基本信息" :column="2" border>
            <el-descriptions-item label="姓名">{{ currentApplication.name }}</el-descriptions-item>
            <el-descriptions-item label="邮箱">{{ currentApplication.email }}</el-descriptions-item>
            <el-descriptions-item label="电话">{{ currentApplication.phone }}</el-descriptions-item>
            <el-descriptions-item label="应聘职位">{{ currentApplication.jobPositionTitle }}</el-descriptions-item>
            <el-descriptions-item label="申请状态">
              <el-tag :type="getStatusTagType(currentApplication.status)">{{ currentApplication.statusName }}</el-tag>
            </el-descriptions-item>
            <el-descriptions-item label="申请日期">{{ formatDate(currentApplication.appliedDate) }}</el-descriptions-item>
          </el-descriptions>

          <div class="resume-section" v-if="currentApplication.resumeUrl">
            <h3>简历文件</h3>
            <div class="resume-action">
              <el-link :href="currentApplication.resumeUrl" target="_blank" type="primary">查看简历</el-link>
              <el-button type="primary" size="small" @click="downloadResume">下载简历</el-button>
            </div>
          </div>

          <div class="cover-letter-section" v-if="currentApplication.coverLetter">
            <h3>求职信</h3>
            <div class="cover-letter-content">{{ currentApplication.coverLetter }}</div>
          </div>

          <div class="notes-section">
            <h3>备注</h3>
            <el-input
              v-model="currentApplication.notes"
              type="textarea"
              :rows="4"
              placeholder="添加备注..."
            />
          </div>
        </template>
        <template #footer>
          <span class="dialog-footer">
            <el-button @click="dialogVisible = false">取消</el-button>
            <el-button type="primary" @click="saveNotes">保存备注</el-button>
          </span>
        </template>
      </el-dialog>

      <!-- 更新状态对话框 -->
      <el-dialog
        v-model="statusDialogVisible"
        title="更新申请状态"
        width="30%"
      >
        <template v-if="currentApplication">
          <el-form :model="statusForm" label-width="100px">
            <el-form-item label="当前状态">
              <el-tag :type="getStatusTagType(currentApplication.status)">{{ currentApplication.statusName }}</el-tag>
            </el-form-item>
            <el-form-item label="更新为">
              <el-select v-model="statusForm.status" placeholder="请选择状态">
                <el-option v-for="(name, value) in statusOptions" :key="value" :label="name" :value="Number(value)" />
              </el-select>
            </el-form-item>
            <el-form-item label="备注">
              <el-input
                v-model="statusForm.notes"
                type="textarea"
                :rows="3"
                placeholder="添加状态变更备注..."
              />
            </el-form-item>
          </el-form>
        </template>
        <template #footer>
          <span class="dialog-footer">
            <el-button @click="statusDialogVisible = false">取消</el-button>
            <el-button type="primary" @click="confirmStatusUpdate">确认更新</el-button>
          </span>
        </template>
      </el-dialog>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import dayjs from 'dayjs';
import { 
  getAllJobPositions, 
  type JobPositionDTO 
} from '@/api/recruitment';
import {
  getJobApplications,
  getJobApplicationById,
  updateJobApplicationStatus,
  updateJobApplicationNotes,
  deleteJobApplication,
  type JobApplicationDTO,
  type JobApplicationFilterDTO,
  type UpdateJobApplicationStatusDTO
} from '@/api/applicant';

// 定义职位类型
interface Position {
  id: number;
  title: string;
  department: string;
}

// 定义应聘者类型
interface JobApplication {
  id: number;
  name: string;
  email: string;
  phone: string;
  resumeUrl: string;
  coverLetter: string;
  jobPositionId: number;
  jobPositionTitle: string;
  status: number;
  statusName: string;
  appliedDate: string;
  lastUpdatedDate: string;
  notes: string;
  userId: string;
}

// 应聘者状态枚举
const statusOptions = {
  0: '新申请',
  1: '简历筛选中',
  2: '面试阶段',
  3: '已通知',
  4: '已录用',
  5: '已拒绝'
};

// 数据加载状态
const loading = ref(false);

// 应聘者数据
const applications = ref<JobApplication[]>([]);
const positions = ref<Position[]>([]);
const total = ref(0);
const currentApplication = ref<JobApplication | null>(null);

// 对话框控制
const dialogVisible = ref(false);
const statusDialogVisible = ref(false);

// 搜索表单
const searchForm = reactive({
  name: '',
  email: '',
  jobPositionId: null as number | null,
  status: null as number | null,
  dateRange: [] as string[],
  pageNumber: 1,
  pageSize: 10
});

// 状态更新表单
const statusForm = reactive({
  id: 0,
  status: 0,
  notes: ''
});

// 获取状态标签类型
const getStatusTagType = (status: number) => {
  const map: Record<number, string> = {
    0: 'info',
    1: 'warning',
    2: 'primary',
    3: 'success',
    4: 'success',
    5: 'danger'
  };
  return map[status] || 'info';
};

// 格式化日期
const formatDate = (dateStr: string) => {
  return dayjs(dateStr).format('YYYY-MM-DD');
};

// 初始化数据
onMounted(() => {
  fetchPositions();
  fetchApplications();
});

// 获取职位列表
const fetchPositions = async () => {
  try {
    const response = await getAllJobPositions(false);
    if (response.data && response.data.success) {
      positions.value = response.data.data.map((item: JobPositionDTO) => ({
        id: item.id,
        title: item.title,
        department: item.department
      }));
    } else {
      ElMessage.error(response.data?.message || '获取职位列表失败');
    }
  } catch (error) {
    console.error('获取职位列表失败', error);
    ElMessage.error('获取职位列表失败');
  }
};

// 获取应聘者列表
const fetchApplications = async () => {
  loading.value = true;
  try {
    const params: JobApplicationFilterDTO = {
      name: searchForm.name,
      email: searchForm.email,
      jobPositionId: searchForm.jobPositionId,
      status: searchForm.status === null ? undefined : searchForm.status,
      appliedDateFrom: searchForm.dateRange[0] || undefined,
      appliedDateTo: searchForm.dateRange[1] || undefined,
      pageindex: searchForm.pageNumber,
      pageSize: searchForm.pageSize
    };

    const response = await getJobApplications(params);
    console.log("response",response);
    
    if (response.data && response.data.success && response.data.data) {
      console.log("response.data.data",response.data.data);
      
      const responseData = response.data.data;
      // 确保返回数据格式一致
      applications.value = responseData.items.map((app: JobApplicationDTO) => {
        // 确保状态名称正确
        const statusName = statusOptions[app.status as keyof typeof statusOptions] || '未知状态';
        
        return {
          ...app,
          statusName
        };
      });
      total.value = responseData.totalCount;
    } else {
      ElMessage.error(response.data?.message || '获取应聘者列表失败');
    }
  } catch (error) {
    console.error('获取应聘者列表失败', error);
    ElMessage.error('获取应聘者列表失败');
  } finally {
    loading.value = false;
  }
};

// 搜索
const search = () => {
  searchForm.pageNumber = 1;
  fetchApplications();
};

// 重置搜索
const resetSearch = () => {
  searchForm.name = '';
  searchForm.email = '';
  searchForm.jobPositionId = null;
  searchForm.status = null;
  searchForm.dateRange = [];
  searchForm.pageNumber = 1;
  fetchApplications();
};

// 刷新数据
const refreshData = () => {
  fetchApplications();
};

// 分页处理
const handleSizeChange = (size: number) => {
  searchForm.pageSize = size;
  fetchApplications();
};

const handleCurrentChange = (page: number) => {
  searchForm.pageNumber = page;
  fetchApplications();
};

// 查看应聘者详情
const viewApplication = (row: JobApplication) => {
  currentApplication.value = { ...row };
  dialogVisible.value = true;
};

// 更新应聘者状态
const updateStatus = (row: JobApplication) => {
  currentApplication.value = { ...row };
  statusForm.id = row.id;
  statusForm.status = row.status;
  statusForm.notes = '';
  statusDialogVisible.value = true;
};

// 确认更新状态
const confirmStatusUpdate = async () => {
  try {
    const data: UpdateJobApplicationStatusDTO = {
      id: statusForm.id,
      status: statusForm.status,
      notes: statusForm.notes
    };
    
    const response = await updateJobApplicationStatus(statusForm.id, data);

    if (response.data && response.data.success) {
      ElMessage.success('更新状态成功');
      statusDialogVisible.value = false;
      fetchApplications();
    } else {
      ElMessage.error(response.data?.message || '更新状态失败');
    }
  } catch (error) {
    console.error('更新状态失败', error);
    ElMessage.error('更新状态失败');
  }
};

// 删除应聘者
const handleDelete = (row: JobApplication) => {
  ElMessageBox.confirm('确认删除该应聘者记录?', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      const response = await deleteJobApplication(row.id);
      
      if (response.data && response.data.success) {
        ElMessage.success('删除成功');
        fetchApplications();
      } else {
        ElMessage.error(response.data?.message || '删除失败');
      }
    } catch (error) {
      console.error('删除失败', error);
      ElMessage.error('删除失败');
    }
  }).catch(() => {
    // 取消删除
  });
};

// 保存备注
const saveNotes = async () => {
  if (!currentApplication.value) return;
  
  try {
    const response = await updateJobApplicationNotes(
      currentApplication.value.id, 
      currentApplication.value.notes
    );

    if (response.data && response.data.success) {
      ElMessage.success('保存备注成功');
      dialogVisible.value = false;
      fetchApplications();
    } else {
      ElMessage.error(response.data?.message || '保存备注失败');
    }
  } catch (error) {
    console.error('保存备注失败', error);
    ElMessage.error('保存备注失败');
  }
};

// 下载简历
const downloadResume = () => {
  if (!currentApplication.value || !currentApplication.value.resumeUrl) return;
  
  // 创建下载链接并点击
  const link = document.createElement('a');
  link.href = currentApplication.value.resumeUrl;
  link.target = '_blank';
  link.download = `${currentApplication.value.name}-简历`;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

// 关闭对话框
const handleDialogClose = () => {
  dialogVisible.value = false;
  currentApplication.value = null;
};
</script>

<style scoped>
.applicant-tracking {
  padding: 0;
}

.header-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-title {
  font-size: 18px;
  font-weight: bold;
}

.filter-form {
  margin-bottom: 20px;
  padding: 15px;
  background-color: #f8f9fa;
  border-radius: 4px;
}

.pagination-container {
  margin-top: 20px;
  display: flex;
  justify-content: center;
}

.resume-section,
.cover-letter-section,
.notes-section {
  margin-top: 20px;
  padding: 15px;
  border: 1px solid #ebeef5;
  border-radius: 4px;
}

.resume-action {
  display: flex;
  gap: 10px;
  margin-top: 10px;
}

.cover-letter-content {
  padding: 10px;
  background-color: #f8f9fa;
  border-radius: 4px;
  white-space: pre-wrap;
}
</style> 