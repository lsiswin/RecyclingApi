<template>
  <div class="resume-management">
    <el-card>
      <template #header>
        <div class="header-container">
          <span class="header-title">简历管理</span>
          <div class="header-actions">
            <el-button type="primary" @click="refreshData">刷新数据</el-button>
            <el-button type="success" @click="showUploadDialog">上传简历</el-button>
          </div>
        </div>
      </template>

      <!-- 筛选表单 -->
      <el-form :model="searchForm" inline class="filter-form">
        <el-form-item label="标题">
          <el-input v-model="searchForm.title" placeholder="请输入简历标题" clearable />
        </el-form-item>
        <el-form-item label="用户名">
          <el-input v-model="searchForm.userName" placeholder="请输入用户名" clearable />
        </el-form-item>
        <el-form-item label="技能关键词">
          <el-input v-model="searchForm.skills" placeholder="请输入技能关键词" clearable />
        </el-form-item>
        <el-form-item label="上传日期">
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
        <el-form-item label="状态">
          <el-select style="width: 100px;" v-model="searchForm.isActive" placeholder="请选择状态" clearable>
            <el-option :label="'激活'" :value="true" />
            <el-option :label="'未激活'" :value="false" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="search">搜索</el-button>
          <el-button @click="resetSearch">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 数据表格 -->
      <el-table :data="resumes" border stripe v-loading="loading" style="width: 100%">
        <el-table-column align="center" prop="id" label="ID" width="60" />
        <el-table-column align="center" prop="title" label="标题" width="120" />
        <el-table-column align="center" prop="userName" label="用户名" width="80" />
        <el-table-column align="center" prop="fileType" label="文件类型" width="100" />
        <el-table-column align="center" prop="skills" label="技能" width="191" :show-overflow-tooltip="true" />
        <el-table-column align="center" prop="uploadDate" label="上传日期" width="120">
          <template #default="scope">
            {{ formatDate(scope.row.uploadDate) }}
          </template>
        </el-table-column>
        <el-table-column align="center" prop="isActive" label="状态" width="80">
          <template #default="scope">
            <el-tag :type="scope.row.isActive ? 'success' : 'info'">
              {{ scope.row.isActive ? '激活' : '未激活' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column align="center" label="操作" width="280">
          <template #default="scope">
            <el-button type="primary" size="small" @click="viewResume(scope.row)">查看</el-button>
            <el-button type="success" size="small" @click="downloadResume(scope.row)">下载</el-button>
            <el-button type="warning" size="small" @click="editResume(scope.row)">编辑</el-button>
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
        v-model="viewDialogVisible"
        title="简历详情"
        width="70%"
        :before-close="handleDialogClose"
      >
        <template v-if="currentResume">
          <el-descriptions title="基本信息" :column="2" border>
            <el-descriptions-item label="标题">{{ currentResume.title }}</el-descriptions-item>
            <el-descriptions-item label="用户">{{ currentResume.userName }}</el-descriptions-item>
            <el-descriptions-item label="文件名">{{ currentResume.fileName }}</el-descriptions-item>
            <el-descriptions-item label="文件类型">{{ currentResume.fileType }}</el-descriptions-item>
            <el-descriptions-item label="文件大小">{{ formatFileSize(currentResume.fileSize) }}</el-descriptions-item>
            <el-descriptions-item label="上传日期">{{ formatDate(currentResume.uploadDate) }}</el-descriptions-item>
            <el-descriptions-item label="状态">
              <el-tag :type="currentResume.isActive ? 'success' : 'info'">
                {{ currentResume.isActive ? '激活' : '未激活' }}
              </el-tag>
            </el-descriptions-item>
          </el-descriptions>

          <div class="resume-section">
            <h3>技能</h3>
            <div class="resume-content">{{ currentResume.skills || '未填写' }}</div>
          </div>

          <div class="resume-section">
            <h3>教育经历</h3>
            <div class="resume-content">{{ currentResume.education || '未填写' }}</div>
          </div>

          <div class="resume-section">
            <h3>工作经验</h3>
            <div class="resume-content">{{ currentResume.workExperience || '未填写' }}</div>
          </div>

          <div class="resume-section">
            <h3>附加信息</h3>
            <div class="resume-content">{{ currentResume.additionalInfo || '未填写' }}</div>
          </div>

          <div class="file-section">
            <h3>简历文件</h3>
            <div class="file-action">
              <el-link :href="currentResume.fileUrl" target="_blank" type="primary">在线查看</el-link>
              <el-button type="primary" size="small" @click="downloadCurrentResume">下载文件</el-button>
            </div>
          </div>
        </template>
      </el-dialog>

      <!-- 编辑简历对话框 -->
      <el-dialog
        v-model="editDialogVisible"
        :title="isNewResume ? '上传简历' : '编辑简历'"
        width="60%"
        :before-close="handleDialogClose"
      >
        <el-form 
          :model="resumeForm" 
          :rules="resumeRules" 
          ref="resumeFormRef" 
          label-width="100px"
        >
          <el-form-item label="标题" prop="title">
            <el-input v-model="resumeForm.title" placeholder="请输入简历标题" />
          </el-form-item>

          <el-form-item label="简历文件" prop="file" v-if="isNewResume || resumeForm.updateFile">
            <el-upload
              class="resume-upload"
              :auto-upload="false"
              :limit="1"
              :on-change="handleFileChange"
              :on-remove="handleFileRemove"
              action="#"
            >
              <template #trigger>
                <el-button type="primary">选择文件</el-button>
              </template>
              <template #tip>
                <div class="el-upload__tip">支持PDF、Word、Excel等格式，文件大小不超过10MB</div>
              </template>
            </el-upload>
          </el-form-item>

          <el-form-item label="更新文件" v-if="!isNewResume">
            <el-switch v-model="resumeForm.updateFile" />
          </el-form-item>

          <el-form-item label="技能">
            <el-input 
              v-model="resumeForm.skills" 
              type="textarea" 
              :rows="3" 
              placeholder="请输入技能描述"
            />
          </el-form-item>

          <el-form-item label="教育经历">
            <el-input 
              v-model="resumeForm.education" 
              type="textarea" 
              :rows="3" 
              placeholder="请输入教育经历"
            />
          </el-form-item>

          <el-form-item label="工作经验">
            <el-input 
              v-model="resumeForm.workExperience" 
              type="textarea" 
              :rows="3" 
              placeholder="请输入工作经验"
            />
          </el-form-item>

          <el-form-item label="附加信息">
            <el-input 
              v-model="resumeForm.additionalInfo" 
              type="textarea" 
              :rows="3" 
              placeholder="请输入附加信息"
            />
          </el-form-item>

          <el-form-item label="状态">
            <el-switch v-model="resumeForm.isActive" />
          </el-form-item>
        </el-form>
        <template #footer>
          <span class="dialog-footer">
            <el-button @click="editDialogVisible = false">取消</el-button>
            <el-button type="primary" @click="submitResumeForm">确认</el-button>
          </span>
        </template>
      </el-dialog>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import type { FormInstance, FormRules } from 'element-plus';
import dayjs from 'dayjs';
import { 
  getResumes, 
  getResumeById, 
  createResume, 
  updateResume, 
  deleteResume, 
  changeResumeStatus,
  type ResumeDTO,
  type ResumeFilterDTO,
  type CreateResumeDTO,
  type UpdateResumeDTO
} from '@/api/recruitment';

// 定义简历类型
interface Resume {
  id: number;
  title: string;
  fileUrl: string;
  fileName: string;
  fileSize: number;
  fileType: string;
  skills: string;
  education: string;
  workExperience: string;
  additionalInfo: string;
  uploadDate: string;
  lastUpdatedDate: string;
  isActive: boolean;
  userId: string;
  userName: string;
}

// 数据加载状态
const loading = ref(false);

// 简历数据
const resumes = ref<Resume[]>([]);
const total = ref(0);
const currentResume = ref<Resume | null>(null);

// 对话框控制
const viewDialogVisible = ref(false);
const editDialogVisible = ref(false);
const isNewResume = ref(false);

// 表单引用
const resumeFormRef = ref<FormInstance>();

// 搜索表单
const searchForm = reactive({
  title: '',
  userName: '',
  skills: '',
  dateRange: [] as string[],
  isActive: null as boolean | null,
  pageNumber: 1,
  pageSize: 10
});

// 简历表单
const resumeForm = reactive({
  id: 0,
  title: '',
  skills: '',
  education: '',
  workExperience: '',
  additionalInfo: '',
  isActive: true,
  updateFile: false,
  file: null as File | null,
  fileName: ''
});

// 表单校验规则
const resumeRules = reactive<FormRules>({
  title: [
    { required: true, message: '请输入简历标题', trigger: 'blur' },
    { min: 2, max: 100, message: '长度在 2 到 100 个字符', trigger: 'blur' }
  ],
  file: [
    { 
      required: true, 
      message: '请上传简历文件', 
      trigger: 'change',
      validator: (rule, value, callback) => {
        if (isNewResume.value && !resumeForm.file) {
          callback(new Error('请上传简历文件'));
        } else if (resumeForm.updateFile && !resumeForm.file) {
          callback(new Error('请上传简历文件'));
        } else {
          callback();
        }
      }
    }
  ]
});

// 格式化日期
const formatDate = (dateStr: string) => {
  return dayjs(dateStr).format('YYYY-MM-DD');
};

// 格式化文件大小
const formatFileSize = (size: number) => {
  if (size < 1024) {
    return size + ' B';
  } else if (size < 1024 * 1024) {
    return (size / 1024).toFixed(2) + ' KB';
  } else if (size < 1024 * 1024 * 1024) {
    return (size / (1024 * 1024)).toFixed(2) + ' MB';
  } else {
    return (size / (1024 * 1024 * 1024)).toFixed(2) + ' GB';
  }
};

// 初始化数据
onMounted(() => {
  fetchResumes();
});

// 获取简历列表
const fetchResumes = async () => {
  loading.value = true;
  try {
    // 构建API请求参数
    const params: ResumeFilterDTO = {
      title: searchForm.title || undefined,
      userName: searchForm.userName || undefined,
      skills: searchForm.skills || undefined,
      uploadDateFrom: searchForm.dateRange[0] || undefined,
      uploadDateTo: searchForm.dateRange[1] || undefined,
      isActive: searchForm.isActive === null ? undefined : searchForm.isActive,
      pageindex: searchForm.pageNumber,
      pageSize: searchForm.pageSize
    };

    // 发送API请求
    const response = await getResumes(params);
    
    // 处理响应数据
    if (response.data && response.data.success && response.data.data) {
      const responseData = response.data.data;
      resumes.value = responseData.items;
      total.value = responseData.totalCount;
    } else {
      ElMessage.error(response.data?.message || '获取简历列表失败');
    }
  } catch (error) {
    console.error('获取简历列表失败', error);
    ElMessage.error('获取简历列表失败');
  } finally {
    loading.value = false;
  }
};

// 搜索
const search = () => {
  searchForm.pageNumber = 1;
  fetchResumes();
};

// 重置搜索
const resetSearch = () => {
  searchForm.title = '';
  searchForm.userName = '';
  searchForm.skills = '';
  searchForm.dateRange = [];
  searchForm.isActive = null;
  searchForm.pageNumber = 1;
  fetchResumes();
};

// 刷新数据
const refreshData = () => {
  fetchResumes();
};

// 分页处理
const handleSizeChange = (size: number) => {
  searchForm.pageSize = size;
  fetchResumes();
};

const handleCurrentChange = (page: number) => {
  searchForm.pageNumber = page;
  fetchResumes();
};

// 查看简历
const viewResume = (row: Resume) => {
  currentResume.value = { ...row };
  viewDialogVisible.value = true;
};

// 编辑简历
const editResume = (row: Resume) => {
  isNewResume.value = false;
  resumeForm.id = row.id;
  resumeForm.title = row.title;
  resumeForm.skills = row.skills || '';
  resumeForm.education = row.education || '';
  resumeForm.workExperience = row.workExperience || '';
  resumeForm.additionalInfo = row.additionalInfo || '';
  resumeForm.isActive = row.isActive;
  resumeForm.updateFile = false;
  resumeForm.file = null;
  resumeForm.fileName = '';
  
  editDialogVisible.value = true;
};

// 显示上传对话框
const showUploadDialog = () => {
  isNewResume.value = true;
  resumeForm.id = 0;
  resumeForm.title = '';
  resumeForm.skills = '';
  resumeForm.education = '';
  resumeForm.workExperience = '';
  resumeForm.additionalInfo = '';
  resumeForm.isActive = true;
  resumeForm.updateFile = true;
  resumeForm.file = null;
  resumeForm.fileName = '';
  
  editDialogVisible.value = true;
};

// 文件上传处理
const handleFileChange = (file: any) => {
  resumeForm.file = file.raw;
  resumeForm.fileName = file.name;
};

const handleFileRemove = () => {
  resumeForm.file = null;
  resumeForm.fileName = '';
};

// 提交表单
const submitResumeForm = async () => {
  if (!resumeFormRef.value) return;
  
  await resumeFormRef.value.validate(async (valid) => {
    if (!valid) return;
    
    try {
      // 准备表单数据
      if (isNewResume.value) {
        // 创建新简历
        if (resumeForm.file) {
          // 转换文件为Base64
          const reader = new FileReader();
          reader.readAsDataURL(resumeForm.file);
          reader.onload = async (e) => {
            // 从base64字符串中移除data:application/pdf;base64,这样的前缀
            const base64String = (e.target?.result as string).split(',')[1];
            
            const data: CreateResumeDTO = {
              title: resumeForm.title,
              fileName: resumeForm.fileName,
              fileContent: base64String,
              fileType: resumeForm.file?.type || '',
              skills: resumeForm.skills,
              education: resumeForm.education,
              workExperience: resumeForm.workExperience,
              additionalInfo: resumeForm.additionalInfo,
              isActive: resumeForm.isActive
            };
            
            const response = await createResume(data);
            if (response.data && response.data.success) {
              ElMessage.success('上传简历成功');
              editDialogVisible.value = false;
              fetchResumes();
            } else {
              ElMessage.error(response.data?.message || '上传简历失败');
            }
          };
        }
      } else {
        // 更新简历
        const data: UpdateResumeDTO = {
          id: resumeForm.id,
          title: resumeForm.title,
          skills: resumeForm.skills,
          education: resumeForm.education,
          workExperience: resumeForm.workExperience,
          additionalInfo: resumeForm.additionalInfo,
          isActive: resumeForm.isActive
        };
        
        if (resumeForm.updateFile && resumeForm.file) {
          // 转换文件为Base64
          const reader = new FileReader();
          reader.readAsDataURL(resumeForm.file);
          reader.onload = async (e) => {
            // 从base64字符串中移除data:application/pdf;base64,这样的前缀
            const base64String = (e.target?.result as string).split(',')[1];
            
            data.fileContent = base64String;
            data.fileName = resumeForm.fileName;
            data.fileType = resumeForm.file?.type || '';
            
            const response = await updateResume(resumeForm.id, data);
            if (response.data && response.data.success) {
              ElMessage.success('更新简历成功');
              editDialogVisible.value = false;
              fetchResumes();
            } else {
              ElMessage.error(response.data?.message || '更新简历失败');
            }
          };
        } else {
          // 不更新文件
          const response = await updateResume(resumeForm.id, data);
          if (response.data && response.data.success) {
            ElMessage.success('更新简历成功');
            editDialogVisible.value = false;
            fetchResumes();
          } else {
            ElMessage.error(response.data?.message || '更新简历失败');
          }
        }
      }
    } catch (error) {
      console.error('保存简历失败', error);
      ElMessage.error('保存简历失败');
    }
  });
};

// 删除简历
const handleDelete = (row: Resume) => {
  ElMessageBox.confirm('确认删除该简历?', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      const response = await deleteResume(row.id);
      
      if (response.data && response.data.success) {
        ElMessage.success('删除成功');
        fetchResumes();
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

// 下载简历
const downloadResume = (row: Resume) => {
  if (!row || !row.fileUrl) return;
  
  // 创建下载链接并点击
  const link = document.createElement('a');
  link.href = row.fileUrl;
  link.target = '_blank';
  link.download = `${row.title}-${row.fileName}`;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

// 下载当前查看的简历
const downloadCurrentResume = () => {
  if (!currentResume.value || !currentResume.value.fileUrl) return;
  
  const link = document.createElement('a');
  link.href = currentResume.value.fileUrl;
  link.target = '_blank';
  link.download = `${currentResume.value.title}-${currentResume.value.fileName}`;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

// 关闭对话框
const handleDialogClose = () => {
  viewDialogVisible.value = false;
  editDialogVisible.value = false;
  currentResume.value = null;
};
</script>

<style scoped>
.resume-management {
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
.file-section {
  margin-top: 20px;
  padding: 15px;
  border: 1px solid #ebeef5;
  border-radius: 4px;
}

.resume-content {
  padding: 10px;
  background-color: #f8f9fa;
  border-radius: 4px;
  white-space: pre-wrap;
}

.file-action {
  display: flex;
  gap: 10px;
  margin-top: 10px;
}

.resume-upload {
  width: 100%;
}
</style> 