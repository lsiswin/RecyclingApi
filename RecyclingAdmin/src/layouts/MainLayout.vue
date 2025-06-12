<template>
  <el-container class="layout-container">
    <!-- 侧边栏 -->
    <el-aside :width="isCollapse ? '64px' : '250px'" class="sidebar">
      <div class="logo">
        <el-icon v-if="isCollapse" class="logo-icon"><Recycle /></el-icon>
        <template v-else>
          <el-icon class="logo-icon"><Recycle /></el-icon>
          <span class="logo-text">后台管理</span>
        </template>
      </div>
      
      <el-menu
        :default-active="$route.path"
        :collapse="isCollapse"
        :unique-opened="true"
        router
        class="sidebar-menu"
        background-color="#1e3a8a"
        text-color="#ffffff"
        active-text-color="#ffd04b"
      >


        <!-- 管理员菜单 -->
        <template v-if="isAdmin">
          <el-menu-item index="/">
            <el-icon><HomeFilled /></el-icon>
            <template #title>仪表板</template>
          </el-menu-item>

          <el-sub-menu index="/content">
            <template #title>
              <el-icon><Document /></el-icon>
              <span>内容管理</span>
            </template>
            <el-menu-item index="/content/banners">首页轮播图管理</el-menu-item>
            <el-menu-item index="/content/company">公司信息编辑</el-menu-item>
            <el-menu-item index="/content/categories">产品分类管理</el-menu-item>
            <el-menu-item index="/content/cases">案例内容管理</el-menu-item>
          </el-sub-menu>

          <el-sub-menu index="/recruitment">
            <template #title>
              <el-icon><UserFilled /></el-icon>
              <span>招聘管理</span>
            </template>
            <el-menu-item index="/recruitment/jobs">岗位发布/下架</el-menu-item>
            <el-menu-item index="/recruitment/resumes">简历管理</el-menu-item>
            <el-menu-item index="/recruitment/applicants">应聘者状态跟踪</el-menu-item>
          </el-sub-menu>

          <el-sub-menu index="/consultation">
            <template #title>
              <el-icon><ChatDotRound /></el-icon>
              <span>咨询管理</span>
            </template>
            <el-menu-item index="/consultation/accounts">客服账号管理</el-menu-item>
            <el-menu-item index="/consultation/monitoring">对话监控</el-menu-item>
            <el-menu-item index="/consultation/statistics">数据统计</el-menu-item>
          </el-sub-menu>

          <el-sub-menu index="/forms">
            <template #title>
              <el-icon><Files /></el-icon>
              <span>表单管理</span>
            </template>
            <el-menu-item index="/forms/contacts">联系表单查看</el-menu-item>
            <el-menu-item index="/forms/export">导出功能</el-menu-item>
          </el-sub-menu>

          <el-menu-item index="/users">
            <el-icon><User /></el-icon>
            <template #title>用户管理</template>
          </el-menu-item>
        </template>

        <!-- 员工菜单 -->
        <template v-if="isStaff">
          <el-menu-item index="/consultation/workbench">
            <el-icon><ChatDotRound /></el-icon>
            <template #title>客服工作台</template>
          </el-menu-item>
        </template>

        <!-- 临时菜单 - 确保总是有内容显示 -->
        <template v-if="!isAdmin && !isStaff">
          <el-menu-item index="/temp">
            <el-icon><Warning /></el-icon>
            <template #title>用户角色未识别</template>
          </el-menu-item>
        </template>
      </el-menu>
    </el-aside>

    <!-- 主内容区 -->
    <el-container>
      <!-- 顶部栏 -->
      <el-header class="header">
        <div class="header-left">
          <el-button
            type="text"
            @click="toggleCollapse"
            class="collapse-btn"
          >
            <el-icon><Expand v-if="isCollapse" /><Fold v-else /></el-icon>
          </el-button>
          <el-breadcrumb separator="/">
            <el-breadcrumb-item :to="{ path: isStaff ? '/consultation/workbench' : '/' }">
              {{ isStaff ? '工作台' : '首页' }}
            </el-breadcrumb-item>
            <el-breadcrumb-item v-if="$route.meta.title">{{ $route.meta.title }}</el-breadcrumb-item>
          </el-breadcrumb>
        </div>
        
        <div class="header-right">
          <div class="user-role-badge">
            <el-tag :type="isAdmin ? 'danger' : 'success'" size="small">
              {{ isAdmin ? '管理员' : '客服员工' }}
            </el-tag>
          </div>
          <el-dropdown @command="handleCommand">
            <span class="user-info">
              <el-avatar :size="32" class="user-avatar">
                <el-icon><User /></el-icon>
              </el-avatar>
              <span class="user-name">{{ authStore.user?.realName }}</span>
              <el-icon class="el-icon--right"><ArrowDown /></el-icon>
            </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item command="profile">个人信息</el-dropdown-item>
                <el-dropdown-item command="settings">设置</el-dropdown-item>
                <el-dropdown-item divided command="logout">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </el-header>

      <!-- 主内容 -->
      <el-main class="main-content">
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { ElMessage } from 'element-plus'

const router = useRouter()
const authStore = useAuthStore()
const isCollapse = ref(false)

// 计算用户角色
const isAdmin = computed(() => {
  console.log('计算isAdmin:', authStore.user?.userType, authStore.user?.userType === 2)
  return authStore.user?.userType === 2
})
const isStaff = computed(() => {
  console.log('计算isStaff:', authStore.user?.userType, authStore.user?.userType === 1)
  return authStore.user?.userType === 1
})

const toggleCollapse = () => {
  isCollapse.value = !isCollapse.value
}

const handleCommand = async (command: string) => {
  switch (command) {
    case 'profile':
      ElMessage.info('个人信息功能开发中')
      break
    case 'settings':
      ElMessage.info('设置功能开发中')
      break
    case 'logout':
      try {
        await authStore.logout()
        ElMessage.success('已退出登录')
        await router.push('/login')
      } catch (error) {
        console.error('登出错误:', error)
        ElMessage.error('登出失败')
      }
      break
  }
}

onMounted(async () => {
  console.log('MainLayout mounted, 初始化认证状态')
  await authStore.initAuth()
  console.log('认证状态初始化完成:', {
    isAuthenticated: authStore.isAuthenticated,
    user: authStore.user,
    userType: authStore.user?.userType
  })
})
</script>

<style scoped>
.layout-container {
  height: 100vh;
}

.sidebar {
  background: linear-gradient(180deg, #1e3a8a 0%, #3730a3 50%, #581c87 100%);
  transition: width 0.3s;
}

.logo {
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 18px;
  font-weight: bold;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.logo-icon {
  font-size: 24px;
  margin-right: 8px;
}

.logo-text {
  font-size: 18px;
  font-weight: 600;
}

.sidebar-menu {
  border: none;
  height: calc(100vh - 60px);
  overflow-y: auto;
}

.sidebar-menu:not(.el-menu--collapse) {
  width: 250px;
}

.header {
  background: white;
  border-bottom: 1px solid #e4e7ed;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px;
  height: 60px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
}

.collapse-btn {
  font-size: 18px;
  color: #606266;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.user-role-badge {
  margin-right: 8px;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  padding: 8px 12px;
  border-radius: 6px;
  transition: background-color 0.3s;
}

.user-info:hover {
  background-color: #f5f7fa;
}

.user-avatar {
  background-color: #409eff;
}

.user-name {
  font-size: 14px;
  color: #303133;
  font-weight: 500;
}

.main-content {
  background-color: #f0f2f5;
  padding: 20px;
  overflow-y: auto;
  height: calc(100vh - 60px);
}

/* 响应式优化 - 参考RecyclingVue */
@media (max-width: 768px) {
  .sidebar {
    position: fixed;
    z-index: 1001;
    height: 100vh;
  }
  
  .header {
    padding: 0 15px;
  }
  
  .main-content {
    padding: 15px;
  }
  
  .user-name {
    display: none;
  }
}

/* 大屏幕优化 (1200px - 1920px) */
@media (min-width: 1200px) and (max-width: 1919px) {
  .logo {
    height: 70px;
    font-size: 20px;
  }
  
  .logo-icon {
    font-size: 28px;
  }
  
  .header {
    height: 70px;
    padding: 0 30px;
  }
  
  .main-content {
    padding: 30px;
    height: calc(100vh - 70px);
  }
  
  .sidebar-menu {
    height: calc(100vh - 70px);
  }
  
  .user-name {
    font-size: 16px;
  }
  
  .collapse-btn {
    font-size: 20px;
  }
}

/* 超高分辨率屏幕优化 (2K及以上) */
@media (min-width: 1920px) {
  .logo {
    height: 80px;
    font-size: 22px;
  }
  
  .logo-icon {
    font-size: 32px;
    margin-right: 12px;
  }
  
  .logo-text {
    font-size: 22px;
  }
  
  .header {
    height: 80px;
    padding: 0 40px;
  }
  
  .header-left {
    gap: 24px;
  }
  
  .header-right {
    gap: 24px;
  }
  
  .main-content {
    padding: 40px;
    height: calc(100vh - 80px);
  }
  
  .sidebar-menu {
    height: calc(100vh - 80px);
  }
  
  .user-info {
    padding: 12px 16px;
    gap: 12px;
  }
  
  .user-avatar {
    width: 40px;
    height: 40px;
  }
  
  .user-name {
    font-size: 18px;
  }
  
  .collapse-btn {
    font-size: 24px;
  }
  
  .user-role-badge {
    margin-right: 12px;
  }
  
  /* 面包屑优化 */
  :deep(.el-breadcrumb) {
    font-size: 18px;
  }
  
  :deep(.el-breadcrumb__item) {
    font-size: 18px;
  }
  
  /* 菜单项优化 */
  :deep(.el-menu-item) {
    height: 56px;
    line-height: 56px;
    font-size: 16px;
    padding-left: 24px !important;
  }
  
  :deep(.el-sub-menu__title) {
    height: 56px;
    line-height: 56px;
    font-size: 16px;
    padding-left: 24px !important;
  }
  
  :deep(.el-menu-item .el-icon) {
    font-size: 18px;
    margin-right: 12px;
  }
  
  :deep(.el-sub-menu__title .el-icon) {
    font-size: 18px;
    margin-right: 12px;
  }
}

@media (min-width: 2560px) {
  .logo {
    height: 90px;
    font-size: 26px;
  }
  
  .logo-icon {
    font-size: 36px;
    margin-right: 16px;
  }
  
  .logo-text {
    font-size: 26px;
  }
  
  .header {
    height: 90px;
    padding: 0 50px;
  }
  
  .main-content {
    padding: 50px;
    height: calc(100vh - 90px);
  }
  
  .sidebar-menu {
    height: calc(100vh - 90px);
  }
  
  .user-avatar {
    width: 48px;
    height: 48px;
  }
  
  .user-name {
    font-size: 20px;
  }
  
  .collapse-btn {
    font-size: 28px;
  }
  
  /* 菜单项进一步优化 */
  :deep(.el-menu-item) {
    height: 64px;
    line-height: 64px;
    font-size: 18px;
    padding-left: 30px !important;
  }
  
  :deep(.el-sub-menu__title) {
    height: 64px;
    line-height: 64px;
    font-size: 18px;
    padding-left: 30px !important;
  }
  
  :deep(.el-menu-item .el-icon) {
    font-size: 20px;
    margin-right: 16px;
  }
  
  :deep(.el-sub-menu__title .el-icon) {
    font-size: 20px;
    margin-right: 16px;
  }
  
  :deep(.el-breadcrumb) {
    font-size: 20px;
  }
  
  :deep(.el-breadcrumb__item) {
    font-size: 20px;
  }
}

/* 侧边栏折叠状态优化 */
.sidebar.el-aside--collapse {
  :deep(.el-menu-item),
  :deep(.el-sub-menu__title) {
    padding-left: 20px !important;
  }
}

@media (min-width: 1920px) {
  .sidebar.el-aside--collapse {
    :deep(.el-menu-item),
    :deep(.el-sub-menu__title) {
      padding-left: 22px !important;
    }
  }
}

@media (min-width: 2560px) {
  .sidebar.el-aside--collapse {
    :deep(.el-menu-item),
    :deep(.el-sub-menu__title) {
      padding-left: 24px !important;
    }
  }
}
</style> 