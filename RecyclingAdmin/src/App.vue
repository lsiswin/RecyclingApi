<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <div id="app" class="app-container">
    <router-view v-slot="{ Component }">
      <transition name="fade" mode="out-in">
        <component :is="Component" />
      </transition>
    </router-view>
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
  color: #303133;
  line-height: 1.6;
  font-size: 16px;
  background-color: #f0f2f5;
  height: 100%;
  overflow-x: hidden;
}

/* 滚动条样式优化 */
::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb {
  background: #c1c1c1;
  border-radius: 4px;
  transition: background 0.3s ease;
}

::-webkit-scrollbar-thumb:hover {
  background: #a8a8a8;
}

/* 链接样式 */
a {
  text-decoration: none;
  color: inherit;
  transition: color 0.3s ease;
}

a:hover {
  color: #409eff;
}

/* 列表样式 */
ul, ol {
  list-style: none;
}

/* 图片样式 */
img {
  max-width: 100%;
  height: auto;
  vertical-align: middle;
}

/* 表格样式 */
table {
  border-collapse: collapse;
  border-spacing: 0;
}

/* 输入框样式优化 */
input, textarea, select {
  font-family: inherit;
  font-size: inherit;
  line-height: inherit;
}

/* 按钮样式优化 */
button {
  font-family: inherit;
  font-size: inherit;
  line-height: inherit;
  cursor: pointer;
  border: none;
  outline: none;
  background: transparent;
}

/* 应用容器 */
.app-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: #f0f2f5;
}

#app {
  height: 100vh;
  width: 100vw;
  position: relative;
}

/* 页面过渡动画 */
.fade-enter-active,
.fade-leave-active {
  transition: all 0.3s ease;
}

.fade-enter-from {
  opacity: 0;
  transform: translateX(10px);
}

.fade-leave-to {
  opacity: 0;
  transform: translateX(-10px);
}

/* 通用工具类 */
.text-center {
  text-align: center;
}

.text-left {
  text-align: left;
}

.text-right {
  text-align: right;
}

.flex {
  display: flex;
}

.flex-center {
  display: flex;
  align-items: center;
  justify-content: center;
}

.flex-between {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.flex-column {
  display: flex;
  flex-direction: column;
}

.w-full {
  width: 100%;
}

.h-full {
  height: 100%;
}

.relative {
  position: relative;
}

.absolute {
  position: absolute;
}

.fixed {
  position: fixed;
}

/* 间距工具类 */
.m-0 { margin: 0; }
.m-1 { margin: 4px; }
.m-2 { margin: 8px; }
.m-3 { margin: 12px; }
.m-4 { margin: 16px; }
.m-5 { margin: 20px; }

.mt-0 { margin-top: 0; }
.mt-1 { margin-top: 4px; }
.mt-2 { margin-top: 8px; }
.mt-3 { margin-top: 12px; }
.mt-4 { margin-top: 16px; }
.mt-5 { margin-top: 20px; }

.mb-0 { margin-bottom: 0; }
.mb-1 { margin-bottom: 4px; }
.mb-2 { margin-bottom: 8px; }
.mb-3 { margin-bottom: 12px; }
.mb-4 { margin-bottom: 16px; }
.mb-5 { margin-bottom: 20px; }

.ml-0 { margin-left: 0; }
.ml-1 { margin-left: 4px; }
.ml-2 { margin-left: 8px; }
.ml-3 { margin-left: 12px; }
.ml-4 { margin-left: 16px; }
.ml-5 { margin-left: 20px; }

.mr-0 { margin-right: 0; }
.mr-1 { margin-right: 4px; }
.mr-2 { margin-right: 8px; }
.mr-3 { margin-right: 12px; }
.mr-4 { margin-right: 16px; }
.mr-5 { margin-right: 20px; }

.p-0 { padding: 0; }
.p-1 { padding: 4px; }
.p-2 { padding: 8px; }
.p-3 { padding: 12px; }
.p-4 { padding: 16px; }
.p-5 { padding: 20px; }

.pt-0 { padding-top: 0; }
.pt-1 { padding-top: 4px; }
.pt-2 { padding-top: 8px; }
.pt-3 { padding-top: 12px; }
.pt-4 { padding-top: 16px; }
.pt-5 { padding-top: 20px; }

.pb-0 { padding-bottom: 0; }
.pb-1 { padding-bottom: 4px; }
.pb-2 { padding-bottom: 8px; }
.pb-3 { padding-bottom: 12px; }
.pb-4 { padding-bottom: 16px; }
.pb-5 { padding-bottom: 20px; }

.pl-0 { padding-left: 0; }
.pl-1 { padding-left: 4px; }
.pl-2 { padding-left: 8px; }
.pl-3 { padding-left: 12px; }
.pl-4 { padding-left: 16px; }
.pl-5 { padding-left: 20px; }

.pr-0 { padding-right: 0; }
.pr-1 { padding-right: 4px; }
.pr-2 { padding-right: 8px; }
.pr-3 { padding-right: 12px; }
.pr-4 { padding-right: 16px; }
.pr-5 { padding-right: 20px; }

/* 文字颜色 */
.text-primary { color: #409eff; }
.text-success { color: #67c23a; }
.text-warning { color: #e6a23c; }
.text-danger { color: #f56c6c; }
.text-info { color: #909399; }
.text-muted { color: #c0c4cc; }

/* 背景颜色 */
.bg-primary { background-color: #409eff; }
.bg-success { background-color: #67c23a; }
.bg-warning { background-color: #e6a23c; }
.bg-danger { background-color: #f56c6c; }
.bg-info { background-color: #909399; }
.bg-light { background-color: #f8f9fa; }
.bg-white { background-color: #ffffff; }

/* 圆角 */
.rounded { border-radius: 4px; }
.rounded-sm { border-radius: 2px; }
.rounded-lg { border-radius: 8px; }
.rounded-xl { border-radius: 12px; }
.rounded-full { border-radius: 50%; }

/* 阴影 */
.shadow-sm {
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
}

.shadow {
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06);
}

.shadow-md {
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
}

.shadow-lg {
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}

.shadow-xl {
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
}

/* Element Plus 组件样式优化 */
.el-card {
  border-radius: 8px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  border: 1px solid #ebeef5;
  transition: all 0.3s ease;
}

.el-card:hover {
  box-shadow: 0 4px 20px 0 rgba(0, 0, 0, 0.15);
}

.el-button {
  border-radius: 6px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.el-button:hover {
  transform: translateY(-1px);
}

.el-input__wrapper {
  border-radius: 6px;
  transition: all 0.3s ease;
}

.el-input__wrapper:hover {
  box-shadow: 0 2px 8px rgba(64, 158, 255, 0.1);
}

.el-table {
  border-radius: 8px;
  overflow: hidden;
}

.el-table th {
  background-color: #fafafa;
  font-weight: 600;
}

.el-table tr:hover > td {
  background-color: #f8f9fa;
}

.el-dialog {
  border-radius: 12px;
  overflow: hidden;
}

.el-dialog__header {
  background-color: #fafafa;
  border-bottom: 1px solid #ebeef5;
}

.el-menu {
  border-radius: 8px;
}

.el-menu-item:hover {
  background-color: rgba(64, 158, 255, 0.1);
}

/* 自定义动画 */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes fadeInDown {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes fadeInLeft {
  from {
    opacity: 0;
    transform: translateX(-20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes fadeInRight {
  from {
    opacity: 0;
    transform: translateX(20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
}

@keyframes bounce {
  0%, 20%, 53%, 80%, 100% {
    transform: translateY(0);
  }
  40%, 43% {
    transform: translateY(-10px);
  }
  70% {
    transform: translateY(-5px);
  }
  90% {
    transform: translateY(-2px);
  }
}

/* 动画类 */
.animate-fadeInUp {
  animation: fadeInUp 0.6s ease-out;
}

.animate-fadeInDown {
  animation: fadeInDown 0.6s ease-out;
}

.animate-fadeInLeft {
  animation: fadeInLeft 0.6s ease-out;
}

.animate-fadeInRight {
  animation: fadeInRight 0.6s ease-out;
}

.animate-pulse {
  animation: pulse 2s infinite;
}

.animate-bounce {
  animation: bounce 1s infinite;
}

/* 响应式断点 - 参考RecyclingVue优化 */
@media (max-width: 480px) {
  html, body {
    font-size: 14px;
  }
  
  .app-container {
    padding: 0;
  }
}

@media (max-width: 768px) {
  html, body {
    font-size: 15px;
  }
}

/* 大屏幕优化 (1200px - 1920px) */
@media (min-width: 1200px) and (max-width: 1919px) {
  html, body {
    font-size: 16px;
  }
  
  .el-card {
    padding: 20px;
  }
  
  .el-button--large {
    padding: 12px 24px;
    font-size: 16px;
  }
  
  .el-input--large .el-input__wrapper {
    padding: 12px 16px;
    font-size: 16px;
  }
}

@media (min-width: 1440px) {
  html, body {
    font-size: 17px;
  }
}

/* 超高分辨率屏幕优化 (2K及以上) - 参考RecyclingVue */
@media (min-width: 1920px) {
  html, body {
    font-size: 18px;
    line-height: 1.8;
  }
  
  /* 容器和布局优化 */
  .el-container {
    max-width: none;
  }
  
  /* 卡片组件优化 */
  .el-card {
    padding: 30px;
    border-radius: 12px;
  }
  
  .el-card__header {
    padding: 24px 30px;
    font-size: 20px;
    font-weight: 600;
  }
  
  .el-card__body {
    padding: 30px;
  }
  
  /* 按钮组件优化 */
  .el-button--large {
    padding: 16px 32px;
    font-size: 18px;
    border-radius: 8px;
  }
  
  .el-button--default {
    padding: 14px 28px;
    font-size: 16px;
  }
  
  .el-button--small {
    padding: 10px 20px;
    font-size: 14px;
  }
  
  /* 输入框组件优化 */
  .el-input--large .el-input__wrapper {
    padding: 16px 20px;
    font-size: 18px;
    border-radius: 8px;
  }
  
  .el-input__wrapper {
    padding: 14px 18px;
    font-size: 16px;
  }
  
  /* 表格组件优化 */
  .el-table {
    font-size: 16px;
  }
  
  .el-table th {
    padding: 16px 0;
    font-size: 16px;
    font-weight: 600;
  }
  
  .el-table td {
    padding: 16px 0;
    font-size: 16px;
  }
  
  /* 对话框优化 */
  .el-dialog {
    border-radius: 16px;
  }
  
  .el-dialog__header {
    padding: 24px 30px;
    font-size: 20px;
  }
  
  .el-dialog__body {
    padding: 30px;
    font-size: 16px;
    line-height: 1.8;
  }
  
  /* 菜单组件优化 */
  .el-menu-item {
    height: 56px;
    line-height: 56px;
    font-size: 16px;
  }
  
  .el-sub-menu__title {
    height: 56px;
    line-height: 56px;
    font-size: 16px;
  }
  
  /* 表单组件优化 */
  .el-form-item {
    margin-bottom: 24px;
  }
  
  .el-form-item__label {
    font-size: 16px;
    line-height: 1.8;
  }
  
  /* 标签组件优化 */
  .el-tag {
    padding: 8px 16px;
    font-size: 14px;
    border-radius: 6px;
  }
  
  .el-tag--large {
    padding: 12px 20px;
    font-size: 16px;
  }
  
  /* 面包屑优化 */
  .el-breadcrumb {
    font-size: 16px;
  }
  
  .el-breadcrumb__item {
    font-size: 16px;
  }
  
  /* 分页组件优化 */
  .el-pagination {
    font-size: 16px;
  }
  
  .el-pagination .el-pager li {
    min-width: 40px;
    height: 40px;
    line-height: 40px;
    font-size: 16px;
  }
}

@media (min-width: 2560px) {
  html, body {
    font-size: 20px;
    line-height: 1.9;
  }
  
  /* 进一步优化超大屏幕 */
  .el-card {
    padding: 40px;
    border-radius: 16px;
  }
  
  .el-card__header {
    padding: 32px 40px;
    font-size: 24px;
  }
  
  .el-button--large {
    padding: 20px 40px;
    font-size: 20px;
    border-radius: 10px;
  }
  
  .el-input--large .el-input__wrapper {
    padding: 20px 24px;
    font-size: 20px;
    border-radius: 10px;
  }
  
  .el-menu-item {
    height: 64px;
    line-height: 64px;
    font-size: 18px;
  }
  
  .el-sub-menu__title {
    height: 64px;
    line-height: 64px;
    font-size: 18px;
  }
}

/* 打印样式 */
@media print {
  * {
    color: #000 !important;
    background: #fff !important;
    box-shadow: none !important;
  }
  
  .no-print {
    display: none !important;
  }
}

/* 高对比度模式支持 */
@media (prefers-contrast: more) {
  .el-card {
    border: 2px solid #000;
  }
  
  .el-button {
    border: 2px solid #000;
  }
}

/* 减少动画模式支持 */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* 深色模式支持 */
@media (prefers-color-scheme: dark) {
  html, body {
    background-color: #1a1a1a;
    color: #e0e0e0;
  }
  
  .app-container {
    background-color: #1a1a1a;
  }
}

/* Element Plus 消息提示样式优化 */
.el-message {
  z-index: 10000 !important;
  font-weight: 500;
  border-radius: 8px !important;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
}

.el-message--success {
  background-color: #f0f9ff !important;
  border-color: #67c23a !important;
  color: #67c23a !important;
}

.el-message--error {
  background-color: #fef2f2 !important;
  border-color: #f56c6c !important;
  color: #f56c6c !important;
}

.el-message--warning {
  background-color: #fefce8 !important;
  border-color: #e6a23c !important;
  color: #e6a23c !important;
}

/* 卡片样式增强 */
</style>
