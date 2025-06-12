<template>
  <header class="app-header">
    <div class="header-container">
      <!-- Logo区域 -->
      <div class="logo-section">
        <router-link to="/" class="logo-link">
          <img :src="logoImage" alt="IT设备回收" class="logo-img" />
          <span class="logo-text">IT设备回收</span>
        </router-link>
      </div>
      
      <!-- 导航菜单 - 使用ElementPlus组件 -->
      <el-menu 
        class="nav-menu"
        mode="horizontal" 
        :default-active="activeIndex"
        @select="handleMenuSelect">
        <el-menu-item index="/">首页</el-menu-item>
        <el-menu-item index="/products">产品类型</el-menu-item>
        <el-menu-item index="/cases">案例展示</el-menu-item>
        <el-menu-item index="/about">关于我们</el-menu-item>
        <el-menu-item index="/contact">联系我们</el-menu-item>
        <el-menu-item index="/jobs">加入我们</el-menu-item>
      </el-menu>

      <!-- 用户操作区域 -->
      <div class="user-actions">
        <template v-if="!isLoggedIn">
          <el-button type="primary" size="small" @click="$router.push('/login')">登录</el-button>
          <el-button type="success" size="small" @click="$router.push('/register')">注册</el-button>
        </template>
        
        <el-dropdown v-else @command="handleCommand">
          <span class="el-dropdown-link">
            <el-avatar :size="32" :src="avatar">{{ userInitials }}</el-avatar>
            <span class="username-text">{{ displayName }}</span>
            <el-icon class="el-icon--right"><arrow-down /></el-icon>
          </span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item command="profile">个人中心</el-dropdown-item>
              <el-dropdown-item command="orders">我的订单</el-dropdown-item>
              <el-dropdown-item command="logout" divided>退出登录</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div>

      <!-- 移动端菜单按钮 -->
      <div class="mobile-menu-btn" @click="toggleMobileMenu">
        <el-icon size="24"><Menu /></el-icon>
      </div>
    </div>

    <!-- 移动端菜单 -->
    <el-drawer 
      v-model="showMobileMenu" 
      title="菜单" 
      direction="rtl"
      size="70%">
      <el-menu :default-active="activeIndex" @select="handleMobileMenuSelect">
        <el-menu-item index="/">首页</el-menu-item>
        <el-menu-item index="/products">产品类型</el-menu-item>
        <el-menu-item index="/cases">案例展示</el-menu-item>
        <el-menu-item index="/about">关于我们</el-menu-item>
        <el-menu-item index="/contact">联系我们</el-menu-item>
        <el-menu-item index="/jobs">加入我们</el-menu-item>
      </el-menu>
      <div class="mobile-actions">
        <el-button v-if="!isLoggedIn" type="primary" @click="goToLogin">登录</el-button>
        <el-button v-if="!isLoggedIn" type="success" @click="goToRegister">注册</el-button>
        <el-button v-else type="danger" @click="handleLogout">退出登录</el-button>
      </div>
    </el-drawer>
  </header>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/store/modules/authStore'
import { ElMessage } from 'element-plus'
import { Menu, ArrowDown } from '@element-plus/icons-vue'
// 导入图片
import logoImage from '@/assets/index.jpeg'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const showMobileMenu = ref(false)
const activeIndex = computed(() => route.path)

const isLoggedIn = computed(() => authStore.isLoggedIn)
const username = computed(() => authStore.user?.username || '用户')
const displayName = computed(() => authStore.realName || authStore.user?.username || '用户')
const avatar = computed(() => authStore.avatar)
const userInitials = computed(() => {
  const name = displayName.value
  if (!name || name === '用户') return 'U'
  return name.charAt(0).toUpperCase()
})

const toggleMobileMenu = () => {
  showMobileMenu.value = !showMobileMenu.value
}

const handleMenuSelect = (path) => {
  router.push(path)
}

const handleMobileMenuSelect = (path) => {
  router.push(path)
  showMobileMenu.value = false
}

const handleCommand = (command) => {
  if (command === 'logout') {
    handleLogout()
  } else if (command === 'profile') {
    // 跳转到个人中心
    router.push('/profile')
  } else if (command === 'orders') {
    // 跳转到订单页面
    router.push('/orders')
  }
}

const handleLogout = async () => {
  try {
    await authStore.logout()
    ElMessage.success('退出登录成功')
    if (route.meta.requiresAuth) {
      router.push('/login')
    }
  } catch (error) {
    console.error('登出失败:', error)
    ElMessage.error('退出登录失败，请稍后重试')
  }
}

const goToLogin = () => {
  router.push('/login')
  showMobileMenu.value = false
}

const goToRegister = () => {
  router.push('/register')
  showMobileMenu.value = false
}
</script>

<style scoped lang="scss">
.app-header {
  width: 100%;
  background: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  position: relative;
  z-index: 1000;

  .header-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 5px;
    height: 70px;
    display: flex;
    align-items: center;
    justify-content: space-between;
  }

  .logo-section {
    flex-shrink: 0;
    margin-right:10px;

    .logo-link {
      display: flex;
      align-items: center;
      text-decoration: none;
      color: #333;

      .logo-img {
        width: 40px;
        height: 40px;
        border-radius: 8px;
        margin-right: 10px;
        object-fit: cover;
      }

      .logo-text {
        font-size: 18px;
        font-weight: 700;
        color: #2c3e50;
        white-space: nowrap;
      }
    }
  }

  .nav-menu {
    flex: 1;
    justify-content: center;
    border-bottom: none;
  }

  .user-actions {
    display: flex;
    align-items: center;
    gap: 12px;
    margin-left: 20px;
    
    .el-dropdown-link {
      cursor: pointer;
      display: flex;
      align-items: center;
      gap: 8px;
      color: #409EFF;
      
      .username-text {
        margin: 0 4px;
      }
    }
  }

  .mobile-menu-btn {
    display: none;
    cursor: pointer;
  }

  .mobile-actions {
    display: flex;
    flex-direction: column;
    gap: 10px;
    margin-top: 10px;
    padding: 0 10px;
  }
}

// 大屏幕优化
@media (min-width: 1201px) {
  .app-header {
    .header-container {
      max-width: 2880px;
      padding: 0 40px;
      height: 80px;
    }
    
    .logo-section {
      .logo-link {
        .logo-img {
          width: 45px;
          height: 45px;
        }
        
        .logo-text {
          font-size: 20px;
        }
      }
    }
    
    .nav-menu {
      :deep(.el-menu-item) {
        padding: 0 25px;
        font-size: 16px;
      }
    }
  }
}

// 超高分辨率屏幕优化 (2K及以上)
@media (min-width: 1920px) {
  .app-header {
    .header-container {
      max-width: 1800px;
      padding: 0 60px;
      height: 85px;
    }
    
    .logo-section {
      .logo-link {
        .logo-img {
          width: 50px;
          height: 50px;
        }
        
        .logo-text {
          font-size: 22px;
        }
      }
    }
    
    .nav-menu {
      :deep(.el-menu-item) {
        padding: 0 30px;
        font-size: 17px;
      }
    }
    
    .user-actions {
      gap: 16px;
      
      .el-button {
        padding: 10px 20px;
      }
    }
  }
}

// 平板电脑
@media (max-width: 992px) {
  .app-header {
    .header-container {
      padding: 0 15px;
    }
    
    .nav-menu {
      :deep(.el-menu-item) {
        padding: 0 15px;
      }
    }
  }
}

// 移动设备
@media (max-width: 768px) {
  .app-header {
    .header-container {
      height: 60px;
      padding: 0 15px;
    }

    .logo-section {
      .logo-link {
        .logo-text {
          font-size: 16px;
        }
      }
    }

    .nav-menu {
      display: none; // 在移动设备上隐藏导航菜单
    }

    .user-actions {
      display: none; // 在移动设备上隐藏用户操作区
    }

    .mobile-menu-btn {
      display: block;
    }
  }
}
</style> 