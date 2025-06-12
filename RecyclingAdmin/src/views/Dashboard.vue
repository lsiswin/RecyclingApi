<template>
  <div class="dashboard">
    <el-row :gutter="20" class="stats-row">
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <div class="stat-icon users">
              <el-icon><User /></el-icon>
            </div>
            <div class="stat-info">
              <h3>156</h3>
              <p>总用户数</p>
            </div>
          </div>
        </el-card>
      </el-col>
      
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <div class="stat-icon consultations">
              <el-icon><ChatDotRound /></el-icon>
            </div>
            <div class="stat-info">
              <h3>28</h3>
              <p>今日咨询</p>
            </div>
          </div>
        </el-card>
      </el-col>
      
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <div class="stat-icon jobs">
              <el-icon><Briefcase /></el-icon>
            </div>
            <div class="stat-info">
              <h3>8</h3>
              <p>活跃职位</p>
            </div>
          </div>
        </el-card>
      </el-col>
      
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <div class="stat-icon resumes">
              <el-icon><Document /></el-icon>
            </div>
            <div class="stat-info">
              <h3>15</h3>
              <p>待处理简历</p>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="content-row">
      <el-col :span="16">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>功能模块</span>
            </div>
          </template>
          <el-row :gutter="20">
            <el-col :span="8" v-for="module in modules" :key="module.name">
              <div class="module-card" @click="navigateTo(module.path)">
                <div class="module-icon" :style="{ background: module.color }">
                  <el-icon><component :is="module.icon" /></el-icon>
                </div>
                <h4>{{ module.name }}</h4>
                <p>{{ module.description }}</p>
              </div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
      
      <el-col :span="8">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>系统活动</span>
            </div>
          </template>
          <el-timeline>
            <el-timeline-item
              v-for="activity in activities"
              :key="activity.id"
              :timestamp="activity.time"
              :type="activity.type"
            >
              {{ activity.content }}
            </el-timeline-item>
          </el-timeline>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="status-row">
      <el-col :span="24">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>系统状态监控</span>
              <el-tag type="success">系统正常</el-tag>
            </div>
          </template>
          <el-row :gutter="20">
            <el-col :span="6" v-for="service in services" :key="service.name">
              <div class="service-status">
                <div class="service-header">
                  <el-icon><component :is="service.icon" /></el-icon>
                  <span>{{ service.name }}</span>
                </div>
                <div class="service-indicator">
                  <el-tag :type="service.status === 'running' ? 'success' : 'danger'">
                    {{ service.status === 'running' ? '运行中' : '已停止' }}
                  </el-tag>
                </div>
              </div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const modules = ref([
  {
    name: '轮播图管理',
    description: '管理首页轮播图片和内容',
    icon: 'Picture',
    color: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
    path: '/content/banners'
  },
  {
    name: '公司信息',
    description: '编辑公司基本信息和介绍',
    icon: 'OfficeBuilding',
    color: 'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
    path: '/content/company'
  },
  {
    name: '产品分类',
    description: '管理回收产品分类信息',
    icon: 'Grid',
    color: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
    path: '/content/categories'
  },
  {
    name: '招聘管理',
    description: '管理职位发布和简历处理',
    icon: 'UserFilled',
    color: 'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)',
    path: '/recruitment/jobs'
  },
  {
    name: '咨询管理',
    description: '客服账号管理和对话监控',
    icon: 'ChatDotRound',
    color: 'linear-gradient(135deg, #fa709a 0%, #fee140 100%)',
    path: '/consultation/accounts'
  },
  {
    name: '表单管理',
    description: '联系表单查看和导出功能',
    icon: 'Files',
    color: 'linear-gradient(135deg, #a8edea 0%, #fed6e3 100%)',
    path: '/forms/contacts'
  }
])

const activities = ref([
  {
    id: 1,
    content: '用户 admin 登录系统',
    time: '2025-01-11 14:30',
    type: 'primary'
  },
  {
    id: 2,
    content: '新增轮播图：春节促销活动',
    time: '2025-01-11 14:25',
    type: 'success'
  },
  {
    id: 3,
    content: '发布新职位：前端开发工程师',
    time: '2025-01-11 14:20',
    type: 'info'
  },
  {
    id: 4,
    content: '处理客户咨询：设备回收流程',
    time: '2025-01-11 14:15',
    type: 'warning'
  }
])

const services = ref([
  {
    name: 'Web服务',
    status: 'running',
    icon: 'Monitor'
  },
  {
    name: '数据库',
    status: 'running',
    icon: 'Coin'
  },
  {
    name: '聊天服务',
    status: 'running',
    icon: 'ChatDotRound'
  },
  {
    name: '文件服务',
    status: 'running',
    icon: 'Folder'
  }
])

const navigateTo = (path: string) => {
  router.push(path)
}
</script>

<style scoped>
.dashboard {
  padding: 0;
}

.stats-row {
  margin-bottom: 20px;
}

.stat-card {
  height: 145px;
}

.stat-content {
  display: flex;
  align-items: center;
  height: 100%;
}

.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 16px;
  font-size: 24px;
  color: white;
}

.stat-icon.users {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.stat-icon.consultations {
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
}

.stat-icon.jobs {
  background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
}

.stat-icon.resumes {
  background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
}

.stat-info h3 {
  margin: 0 0 4px 0;
  font-size: 28px;
  font-weight: 600;
  color: #303133;
}

.stat-info p {
  margin: 0;
  font-size: 14px;
  color: #909399;
}

.content-row {
  margin-bottom: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.module-card {
  text-align: center;
  padding: 20px;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s;
  margin-bottom: 20px;
}

.module-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
}

.module-icon {
  width: 60px;
  height: 60px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 12px;
  font-size: 24px;
  color: white;
}

.module-card h4 {
  margin: 0 0 8px 0;
  font-size: 16px;
  color: #303133;
}

.module-card p {
  margin: 0;
  font-size: 12px;
  color: #909399;
  line-height: 1.4;
}

.status-row {
  margin-bottom: 20px;
}

.service-status {
  text-align: center;
  padding: 16px;
  border: 1px solid #e4e7ed;
  border-radius: 8px;
}

.service-header {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 12px;
  font-size: 14px;
  color: #303133;
}

.service-header .el-icon {
  margin-right: 8px;
  font-size: 18px;
}

.service-indicator {
  display: flex;
  justify-content: center;
}
</style> 