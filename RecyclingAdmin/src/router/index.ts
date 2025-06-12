import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: () => import('@/views/Login.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('@/views/Register.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/forgot-password',
      name: 'ForgotPassword',
      component: () => import('@/views/ForgotPassword.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/',
      component: () => import('@/layouts/MainLayout.vue'),
      meta: { requiresAuth: true },
      children: [
        {
          path: '',
          name: 'Dashboard',
          component: () => import('@/views/Dashboard.vue'),
          meta: { title: '仪表板', roles: ['admin'] }
        },
        {
          path: '/content',
          name: 'ContentManagement',
          meta: { title: '内容管理', roles: ['admin'] },
          children: [
            {
              path: 'banners',
              name: 'BannerManagement',
              component: () => import('@/views/content/BannerManagement.vue'),
              meta: { title: '首页轮播图管理', roles: ['admin'] }
            },
            {
              path: 'company',
              name: 'CompanyInfo',
              component: () => import('@/views/content/CompanyInfo.vue'),
              meta: { title: '公司信息编辑', roles: ['admin'] }
            },
            {
              path: 'categories',
              name: 'CategoryManagement',
              component: () => import('@/views/content/CategoryManagement.vue'),
              meta: { title: '产品分类管理', roles: ['admin'] }
            },
            {
              path: 'cases',
              name: 'CaseManagement',
              component: () => import('@/views/content/CaseManagement.vue'),
              meta: { title: '案例内容管理', roles: ['admin'] }
            }
          ]
        },
        {
          path: '/recruitment',
          name: 'RecruitmentManagement',
          meta: { title: '招聘管理', roles: ['admin'] },
          children: [
            {
              path: 'jobs',
              name: 'JobManagement',
              component: () => import('@/views/recruitment/JobManagement.vue'),
              meta: { title: '岗位发布/下架', roles: ['admin'] }
            },
            {
              path: 'resumes',
              name: 'ResumeManagement',
              component: () => import('@/views/recruitment/ResumeManagement.vue'),
              meta: { title: '简历管理', roles: ['admin'] }
            },
            {
              path: 'applicants',
              name: 'ApplicantTracking',
              component: () => import('@/views/recruitment/ApplicantTracking.vue'),
              meta: { title: '应聘者状态跟踪', roles: ['admin'] }
            }
          ]
        },
        {
          path: '/consultation',
          name: 'ConsultationManagement',
          meta: { title: '咨询管理' },
          children: [
            {
              path: 'workbench',
              name: 'CustomerServiceWorkbench',
              component: () => import('@/views/consultation/CustomerServiceWorkbench.vue'),
              meta: { title: '客服工作台', roles: ['staff'] }
            },
            {
              path: 'accounts',
              name: 'CustomerServiceAccounts',
              component: () => import('@/views/consultation/CustomerServiceAccounts.vue'),
              meta: { title: '客服账号管理', roles: ['admin'] }
            },
            {
              path: 'monitoring',
              name: 'ConversationMonitoring',
              component: () => import('@/views/consultation/ConversationMonitoring.vue'),
              meta: { title: '对话监控', roles: ['admin'] }
            },
            {
              path: 'statistics',
              name: 'DataStatistics',
              component: () => import('@/views/consultation/DataStatistics.vue'),
              meta: { title: '数据统计', roles: ['admin'] }
            }
          ]
        },
        {
          path: '/forms',
          name: 'FormManagement',
          meta: { title: '表单管理', roles: ['admin'] },
          children: [
            {
              path: 'contacts',
              name: 'ContactForms',
              component: () => import('@/views/forms/ContactForms.vue'),
              meta: { title: '联系表单查看', roles: ['admin'] }
            },
            {
              path: 'export',
              name: 'ExportFunction',
              component: () => import('@/views/forms/ExportFunction.vue'),
              meta: { title: '导出功能', roles: ['admin'] }
            }
          ]
        },
        {
          path: '/users',
          name: 'UserManagement',
          component: () => import('@/views/UserManagement.vue'),
          meta: { title: '用户管理', roles: ['admin'] }
        }
      ]
    }
  ]
})

// 路由守卫
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  console.log('路由守卫:', {
    to: to.path,
    isAuthenticated: authStore.isAuthenticated,
    userType: authStore.user?.userType
  })
  
  // 检查是否需要认证
  if (to.meta.requiresAuth !== false && !authStore.isAuthenticated) {
    console.log('需要认证但未登录，跳转到登录页')
    next('/login')
    return
  }
  
  // 已登录用户访问登录页或注册页重定向到首页
  if ((to.path === '/login' || to.path === '/register') && authStore.isAuthenticated) {
    console.log('已登录用户访问登录页，重定向')
    // 根据用户角色重定向到不同页面
    if (authStore.user?.userType === 1) {
      next('/consultation/workbench')
    } else {
      next('/')
    }
    return
  }
  
  // 检查角色权限
  if (to.meta.roles && Array.isArray(to.meta.roles)) {
    const userType = authStore.user?.userType
    
    if (!userType) {
      console.log('需要角色权限但用户类型为空')
      next('/login')
      return
    }
    
    // 角色映射：将用户类型转换为路由中使用的角色名
    const roleMap: Record<string, string> = {
      2: 'admin',
      1: 'staff',
      0: 'customer'
    }
    
    const userRole = roleMap[userType]
    console.log('角色检查:', { userType, userRole, requiredRoles: to.meta.roles })
    
    if (!userRole || !to.meta.roles.includes(userRole)) {
      console.log('权限不足，重定向')
      // 无权限访问，重定向到合适的页面
      if (userType === 1) {
        next('/consultation/workbench')
      } else {
        next('/')
      }
      return
    }
  }
  
  console.log('路由守卫通过')
  next()
})

export default router 