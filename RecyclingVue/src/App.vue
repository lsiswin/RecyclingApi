<script setup>
import { RouterLink, RouterView } from 'vue-router'
import AppHeader from '@/components/layout/AppHeader.vue'
import AppFooter from '@/components/layout/AppFooter.vue'
import GlobalChatWidget from '@/components/GlobalChatWidget.vue'
</script>

<template>
  <div class="app-container">
    <AppHeader />
    <main class="main-content">
      <router-view v-slot="{ Component }">
        <transition name="fade" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>
    <AppFooter />
    
    <!-- 全局聊天组件 -->
    <GlobalChatWidget />
  </div>
</template>

<style lang="scss">
/* 导入Element Plus样式 */
@import 'element-plus/dist/index.css';

/* 全局样式重置和基础样式 */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

html, body {
  font-family: 'Helvetica Neue', Helvetica, 'PingFang SC', 'Hiragino Sans GB',
    'Microsoft YaHei', '微软雅黑', Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #333;
  line-height: 1.6;
  font-size: 16px;
  background-color: #f5f7fa;
}

a {
  text-decoration: none;
  color: inherit;
}

ul {
  list-style: none;
}

.app-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.main-content {
  flex: 1;
}

/* 页面过渡动画 */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* 页脚样式 */
.app-footer {
  background-color: #303133;
  color: #fff;
  padding: 60px 0 30px;
  
  .footer-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 20px;
  }
  
  .footer-columns {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    margin-bottom: 40px;
  }
  
  .footer-column {
    flex: 1;
    min-width: 200px;
    margin-bottom: 30px;
    
    h3 {
      font-size: 18px;
      font-weight: 600;
      margin-bottom: 20px;
      position: relative;
      padding-bottom: 10px;
      
      &:after {
        content: '';
        position: absolute;
        left: 0;
        bottom: 0;
        width: 40px;
        height: 2px;
        background-color: #409EFF;
      }
    }
    
    ul {
      li {
        margin-bottom: 12px;
        
        a {
          color: #a6a9ad;
          transition: color 0.3s;
          
          &:hover {
            color: #fff;
          }
        }
      }
    }
  }
  
  .footer-bottom {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    align-items: center;
    padding-top: 30px;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
    
    .copyright {
      color: #909399;
      font-size: 14px;
    }
    
    .footer-links {
      display: flex;
      gap: 20px;
      
      a {
        color: #909399;
        font-size: 14px;
        transition: color 0.3s;
        
        &:hover {
          color: #fff;
        }
      }
    }
  }
}

/* 响应式调整 */
@media (max-width: 768px) {
  .app-footer {
    padding: 40px 0 20px;
    
    .footer-columns {
      flex-direction: column;
      
      .footer-column {
        margin-bottom: 30px;
      }
    }
    
    .footer-bottom {
      flex-direction: column;
      gap: 15px;
      text-align: center;
      
      .footer-links {
        justify-content: center;
      }
    }
  }
}
</style>
