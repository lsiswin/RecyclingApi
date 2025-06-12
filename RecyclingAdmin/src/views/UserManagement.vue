<template>
  <div class="user-management">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>用户管理</span>
          <el-button type="primary" @click="showAddDialog">
            <el-icon><Plus /></el-icon>
            添加用户
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <div class="search-bar">
        <el-row :gutter="20">
          <el-col :span="6">
            <el-input
              v-model="searchForm.keyword"
              placeholder="搜索用户名或姓名"
              clearable
              @clear="handleSearch"
              @keyup.enter="handleSearch"
            >
              <template #prefix>
                <el-icon><Search /></el-icon>
              </template>
            </el-input>
          </el-col>
          <el-col :span="4">
            <el-select v-model="searchForm.userType" placeholder="用户类型" clearable>
              <el-option label="管理员" value="Admin" />
              <el-option label="客服员工" value="Staff" />
              <el-option label="客户" value="Customer" />
            </el-select>
          </el-col>
          <el-col :span="4">
            <el-select v-model="searchForm.status" placeholder="状态" clearable>
              <el-option label="启用" :value="true" />
              <el-option label="禁用" :value="false" />
            </el-select>
          </el-col>
          <el-col :span="4">
            <el-button type="primary" @click="handleSearch">搜索</el-button>
            <el-button @click="resetSearch">重置</el-button>
          </el-col>
        </el-row>
      </div>

      <!-- 统计卡片 -->
      <el-row :gutter="20" class="stats-row">
        <el-col :span="6">
          <div class="stat-card admin">
            <div class="stat-icon">
              <el-icon><UserFilled /></el-icon>
            </div>
            <div class="stat-info">
              <h3>{{ stats.adminCount }}</h3>
              <p>管理员</p>
            </div>
          </div>
        </el-col>
        <el-col :span="6">
          <div class="stat-card staff">
            <div class="stat-icon">
              <el-icon><Service /></el-icon>
            </div>
            <div class="stat-info">
              <h3>{{ stats.staffCount }}</h3>
              <p>客服员工</p>
            </div>
          </div>
        </el-col>
        <el-col :span="6">
          <div class="stat-card customer">
            <div class="stat-icon">
              <el-icon><User /></el-icon>
            </div>
            <div class="stat-info">
              <h3>{{ stats.customerCount }}</h3>
              <p>客户</p>
            </div>
          </div>
        </el-col>
        <el-col :span="6">
          <div class="stat-card total">
            <div class="stat-icon">
              <el-icon><Users /></el-icon>
            </div>
            <div class="stat-info">
              <h3>{{ stats.totalCount }}</h3>
              <p>总用户数</p>
            </div>
          </div>
        </el-col>
      </el-row>

      <!-- 用户表格 -->
      <el-table :data="filteredUsers" style="width: 100%">
        <el-table-column label="头像" width="80">
          <template #default="scope">
            <el-avatar :size="40">
              <el-icon><User /></el-icon>
            </el-avatar>
          </template>
        </el-table-column>
        <el-table-column prop="realName" label="姓名" />
        <el-table-column prop="username" label="用户名" />
        <el-table-column prop="email" label="邮箱" />
        <el-table-column label="用户类型" width="100">
          <template #default="scope">
            <el-tag :type="getUserTypeTagType(scope.row.userType)">
              {{ getUserTypeText(scope.row.userType) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="状态" width="80">
          <template #default="scope">
            <el-tag :type="scope.row.isActive ? 'success' : 'danger'">
              {{ scope.row.isActive ? '启用' : '禁用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="创建时间" width="120" />
        <el-table-column label="操作" width="200">
          <template #default="scope">
            <el-button size="small" @click="editUser(scope.row)">编辑</el-button>
            <el-button
              size="small"
              :type="scope.row.isActive ? 'warning' : 'success'"
              @click="toggleUserStatus(scope.row)"
            >
              {{ scope.row.isActive ? '禁用' : '启用' }}
            </el-button>
            <el-button size="small" type="danger" @click="deleteUser(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- 添加/编辑用户对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑用户' : '添加用户'"
      width="500px"
    >
      <el-form :model="form" :rules="rules" ref="formRef" label-width="100px">
        <el-form-item label="姓名" prop="realName">
          <el-input v-model="form.realName" placeholder="请输入真实姓名" />
        </el-form-item>
        <el-form-item label="用户名" prop="username">
          <el-input v-model="form.username" placeholder="请输入用户名" :disabled="isEdit" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="form.email" placeholder="请输入邮箱地址" />
        </el-form-item>
        <el-form-item label="用户类型" prop="userType">
          <el-select v-model="form.userType" placeholder="请选择用户类型">
            <el-option label="管理员" value="Admin" />
            <el-option label="客服员工" value="Staff" />
            <el-option label="客户" value="Customer" />
          </el-select>
        </el-form-item>
        <el-form-item label="密码" prop="password" v-if="!isEdit">
          <el-input v-model="form.password" type="password" placeholder="请输入密码" />
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
import { ref, reactive, computed, onMounted } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import type { User } from '@/types/user'

const users = ref<User[]>([])
const dialogVisible = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()

const searchForm = reactive({
  keyword: '',
  userType: '',
  status: null as boolean | null
})

const form = reactive({
  id: '',
  realName: '',
  username: '',
  email: '',
  userType: 0,
  password: '',
  isActive: true
})

const rules: FormRules = {
  realName: [{ required: true, message: '请输入真实姓名', trigger: 'blur' }],
  username: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur' }
  ],
  userType: [{ required: true, message: '请选择用户类型', trigger: 'change' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }]
}

// 统计数据
const stats = computed(() => {
  const adminCount = users.value.filter(u => u.userType === 2).length
  const staffCount = users.value.filter(u => u.userType === 1).length
  const customerCount = users.value.filter(u => u.userType === 0).length
  const totalCount = users.value.length

  return { adminCount, staffCount, customerCount, totalCount }
})

// 过滤用户
const filteredUsers = computed(() => {
  return users.value.filter(user => {
    const matchKeyword = !searchForm.keyword || 
      user.realName?.includes(searchForm.keyword) || 
      user.username.includes(searchForm.keyword)
    
    const matchType = !searchForm.userType || user.userType === Number(searchForm.userType)
    const matchStatus = searchForm.status === null || user.isActive === searchForm.status

    return matchKeyword && matchType && matchStatus
  })
})

const loadUsers = () => {
  // 模拟数据
  users.value = [
    {
      id: '1',
      username: 'admin',
      realName: '系统管理员',
      userType: 2,
      email: 'admin@recycling.com',
      isActive: true,
      createdAt: '2025-01-01'
    },
    {
      id: '2',
      username: 'staff',
      realName: '客服员工',
      userType: 1,
      email: 'staff@recycling.com',
      isActive: true,
      createdAt: '2025-01-02'
    },
    {
      id: '3',
      username: 'customer1',
      realName: '张三',
      userType: 0,
      email: 'zhangsan@example.com',
      isActive: true,
      createdAt: '2025-01-03'
    },
    {
      id: '4',
      username: 'customer2',
      realName: '李四',
      userType: 0,
      email: 'lisi@example.com',
      isActive: false,
      createdAt: '2025-01-04'
    }
  ]
}

const getUserTypeText = (type: string) => {
  const typeMap = {
    'Admin': '管理员',
    'Staff': '客服员工',
    'Customer': '客户'
  }
  return typeMap[type as keyof typeof typeMap] || type
}

const getUserTypeTagType = (type: string) => {
  const typeMap = {
    'Admin': 'danger',
    'Staff': 'warning',
    'Customer': 'info'
  }
  return typeMap[type as keyof typeof typeMap] || 'info'
}

const showAddDialog = () => {
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const editUser = (user: User) => {
  isEdit.value = true
  Object.assign(form, user)
  dialogVisible.value = true
}

const resetForm = () => {
  Object.assign(form, {
    id: '',
    realName: '',
    username: '',
    email: '',
    userType: 'Customer',
    password: '',
    isActive: true
  })
}

const submitForm = async () => {
  if (!formRef.value) return
  
  await formRef.value.validate((valid) => {
    if (valid) {
      if (isEdit.value) {
        // 更新用户
        const index = users.value.findIndex(u => u.id === form.id)
        if (index !== -1) {
          users.value[index] = { 
            ...form, 
            createdAt: users.value[index].createdAt 
          }
        }
        ElMessage.success('用户更新成功')
      } else {
        // 添加用户
        const newUser: User = {
          ...form,
          id: Date.now().toString(),
          createdAt: new Date().toISOString().split('T')[0]
        }
        users.value.unshift(newUser)
        ElMessage.success('用户添加成功')
      }
      dialogVisible.value = false
    }
  })
}

const toggleUserStatus = (user: User) => {
  user.isActive = !user.isActive
  ElMessage.success(`用户已${user.isActive ? '启用' : '禁用'}`)
}

const deleteUser = (user: User) => {
  ElMessageBox.confirm('确定要删除这个用户吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(() => {
    const index = users.value.findIndex(u => u.id === user.id)
    if (index !== -1) {
      users.value.splice(index, 1)
      ElMessage.success('删除成功')
    }
  })
}

const handleSearch = () => {
  // 搜索逻辑已在computed中处理
}

const resetSearch = () => {
  Object.assign(searchForm, {
    keyword: '',
    userType: '',
    status: null
  })
}

onMounted(() => {
  loadUsers()
})
</script>

<style scoped>
.user-management {
  padding: 0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.search-bar {
  margin-bottom: 20px;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
}

.stats-row {
  margin-bottom: 20px;
}

.stat-card {
  display: flex;
  align-items: center;
  padding: 20px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.stat-icon {
  width: 50px;
  height: 50px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 16px;
  font-size: 20px;
  color: white;
}

.stat-card.admin .stat-icon {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.stat-card.staff .stat-icon {
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
}

.stat-card.customer .stat-icon {
  background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
}

.stat-card.total .stat-icon {
  background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
}

.stat-info h3 {
  margin: 0 0 4px 0;
  font-size: 24px;
  font-weight: 600;
  color: #303133;
}

.stat-info p {
  margin: 0;
  font-size: 14px;
  color: #909399;
}
</style> 