<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Monitor, Phone, Lock, Box, Trophy, Van } from '@element-plus/icons-vue'
// 导入图片
import heroImage from '@/assets/index.jpeg'

const router = useRouter()


// 轮播图数据
const bannerItems = ref([
  {
    id: 1,
    title: '专业IT设备回收服务',
    description: '环保、安全、高效的企业级IT设备回收解决方案',
    image: heroImage
  },
  {
    id: 2,
    title: '数据安全保障',
    description: '专业数据擦除技术，保障企业信息安全',
    image: heroImage
  },
  {
    id: 3,
    title: '高价回收各类IT设备',
    description: '让闲置资产创造最大价值',
    image: heroImage
  }
])

// 产品分类
const productCategories = ref([
  {
    id: 1,
    name: '电脑设备',
    description: '回收各类电脑设备，包括台式机、笔记本、服务器等',
    productCount: 100,
    icon: Monitor
  },
  {
    id: 2,
    name: '移动设备',
    description: '回收各类移动设备，包括手机、平板等',
    productCount: 50,
    icon: Phone
  }
])

// 服务特色
const features = ref([
  {
    id: 1,
    title: '数据安全',
    description: '专业数据销毁服务，确保企业信息安全',
    icon: Lock
  },
  {
    id: 2,
    title: '环保回收',
    description: '符合环保标准的回收处理流程',
    icon: Box
  },
  {
    id: 3,
    title: '价值最大化',
    description: '专业评估，确保设备残值最大化',
    icon: Trophy
  },
  {
    id: 4,
    title: '上门服务',
    description: '提供便捷的上门回收服务',
    icon: Van
  }
])

// 回收流程
const processSteps = ref([
  {
    id: 1,
    title: '设备评估',
    description: '专业技术人员对设备进行详细评估'
  },
  {
    id: 2,
    title: '报价确认',
    description: '提供透明的回收报价方案'
  },
  {
    id: 3,
    title: '上门回收',
    description: '安排专业团队上门回收设备'
  },
  {
    id: 4,
    title: '数据销毁',
    description: '安全彻底的数据销毁处理'
  },
  {
    id: 5,
    title: '环保处理',
    description: '按照环保标准进行设备处理'
  }
])

const navigateToProducts = () => {
  console.log('导航到产品页面')
  router.push('/products')
}

const navigateToContact = () => {
  console.log('导航到联系页面')
  router.push('/contact')
}

const navigateToCategory = (categoryId) => {
  console.log('导航到分类页面:', categoryId)
  router.push(`/products?category=${categoryId}`)
}

onMounted(() => {
  console.log('HomeView mounted successfully')
  console.log('bannerItems:', bannerItems.value)
  console.log('features:', features.value)
})
</script>

<template>
  <div class="home-page">
    <!-- 轮播图区域 -->
    <div class="banner-section">
      <el-carousel :interval="5000" type="card" height="500px">
        <el-carousel-item v-for="item in bannerItems" :key="item.id">
          <el-image :src="item.image" fit="cover" class="banner-image" />
          <div class="banner-content">
            <h2>{{ item.title }}</h2>
            <p>{{ item.description }}</p>
            <el-button type="primary" size="large" @click="navigateToProducts">查看服务</el-button>
          </div>
        </el-carousel-item>
      </el-carousel>
    </div>

    <!-- 服务特色 -->
    <el-row class="section features-section" justify="center">
      <el-col :span="20" :lg="18">
        <div class="section-title">
          <h2>为什么选择我们</h2>
          <p>专业IT设备回收服务商，提供一站式回收解决方案</p>
        </div>
        <el-row :gutter="20">
          <el-col v-for="feature in features" :key="feature.id" :xs="24" :sm="12" :md="6">
            <el-card shadow="hover" class="feature-card">
              <div class="feature-icon">
                <el-icon :size="40"><component :is="feature.icon" /></el-icon>
              </div>
              <h3>{{ feature.title }}</h3>
              <p>{{ feature.description }}</p>
            </el-card>
          </el-col>
        </el-row>
      </el-col>
    </el-row>

    <!-- 回收设备类型 -->
    <el-row class="section product-section" justify="center">
      <el-col :span="20" :lg="18">
        <div class="section-title">
          <h2>回收设备类型</h2>
          <p>我们回收各类IT设备，变废为宝，创造价值</p>
        </div>
        <el-row :gutter="30">
          <el-col v-for="category in productCategories" :key="category.id" :xs="24" :sm="12">
            <el-card shadow="hover" class="product-card" @click="navigateToCategory(category.id)">
              <div class="product-card-content">
                <div class="product-info">
                  <h3>{{ category.name }}</h3>
                  <p>{{ category.description }}</p>
                  <el-tag size="small" type="success" round>{{ category.productCount }}+ 设备</el-tag>
                </div>
                <div class="product-icon">
                  <el-icon :size="60"><Monitor v-if="category.id === 1" /><Phone v-else /></el-icon>
                </div>
              </div>
              <div class="card-footer">
                <el-button type="primary" plain>查看详情</el-button>
              </div>
            </el-card>
          </el-col>
        </el-row>
      </el-col>
    </el-row>

    <!-- 回收流程 -->
    <el-row class="section process-section" justify="center">
      <el-col :span="20" :lg="18">
        <div class="section-title">
          <h2>回收流程</h2>
          <p>专业、高效、透明的服务流程</p>
        </div>
        <el-steps :active="processSteps.length" finish-status="success">
          <el-step v-for="step in processSteps" :key="step.id" :title="step.title" :description="step.description" />
        </el-steps>
        <div class="process-action">
          <el-button type="success" size="large" @click="navigateToContact">立即咨询</el-button>
        </div>
      </el-col>
    </el-row>

    <!-- 价值评估 -->
    <el-row class="section calculator-section" justify="center">
      <el-col :span="20" :lg="18">
        <el-card shadow="hover" class="calculator-card">
          <el-row :gutter="30">
            <el-col :xs="24" :md="12">
              <div class="calculator-content">
                <h2>设备价值评估</h2>
                <p>您的闲置IT设备价值几何？我们提供专业评估服务，让您了解设备残值，获取最大回报。</p>
                <div class="calculator-actions">
                  <el-button type="primary" size="large" @click="navigateToContact">免费评估</el-button>
                  <el-button size="large" @click="navigateToContact">在线咨询</el-button>
                </div>
              </div>
            </el-col>
            <el-col :xs="24" :md="12">
              <div class="calculator-image">
                <el-image :src="heroImage" fit="cover" />
              </div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>

    <!-- 合作伙伴 -->
    <el-row class="section partners-section" justify="center">
      <el-col :span="20" :lg="18">
        <div class="section-title">
          <h2>合作伙伴</h2>
          <p>值得众多知名企业信赖的回收服务商</p>
        </div>
        <div class="partners-logo">
          <div v-for="i in 6" :key="i" class="partner-item">
            <el-image :src="heroImage" fit="contain" />
          </div>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<style scoped lang="scss">
.home-page {
  width: 100%;
}

// 测试区域样式
.test-section {
  padding: 40px 20px;
  background-color: #e6f7ff;
  text-align: center;
  border: 2px solid #1890ff;
  margin: 20px;
  border-radius: 8px;
  
  h1 {
    color: #1890ff;
    font-size: 24px;
    margin-bottom: 16px;
  }
  
  p {
    color: #666;
    font-size: 16px;
  }
}

// 轮播图区域
.banner-section {
  padding: 20px 0 40px;
  background-color: #f5f7fa;
  
  :deep(.el-carousel__item) {
    border-radius: 12px;
    overflow: hidden;
    
    .banner-image {
      width: 100%;
      height: 100%;
    }
    
    .banner-content {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      text-align: center;
      background: rgba(0, 0, 0, 0.5);
      color: white;
      padding: 0 30px;
      
      h2 {
        font-size: 32px;
        margin-bottom: 16px;
        font-weight: 600;
      }
      
      p {
        font-size: 18px;
        margin-bottom: 24px;
        max-width: 600px;
      }
    }
  }
}

// 通用部分样式
.section {
  padding: 60px 0;
  
  .section-title {
    text-align: center;
    margin-bottom: 40px;
    
    h2 {
      font-size: 30px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 12px;
    }
    
    p {
      font-size: 16px;
      color: #606266;
    }
  }
}

// 服务特色
.features-section {
  background-color: white;
  
  .feature-card {
    height: 100%;
    padding: 10px;
    text-align: center;
    transition: transform 0.3s;
    margin-bottom: 20px;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .feature-icon {
      color: #409EFF;
      margin-bottom: 20px;
    }
    
    h3 {
      font-size: 18px;
      margin-bottom: 10px;
      color: #303133;
    }
    
    p {
      color: #606266;
      line-height: 1.6;
    }
  }
}

// 产品分类
.product-section {
  background-color: #f5f7fa;
  
  .product-card {
    height: 100%;
    margin-bottom: 20px;
    cursor: pointer;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .product-card-content {
      display: flex;
      align-items: center;
      padding: 10px;
      
      .product-info {
        flex: 1;
        
        h3 {
          font-size: 20px;
          margin-bottom: 10px;
          color: #303133;
        }
        
        p {
          color: #606266;
          line-height: 1.6;
          margin-bottom: 16px;
        }
      }
      
      .product-icon {
        color: #409EFF;
        margin-left: 20px;
      }
    }
    
    .card-footer {
      padding-top: 15px;
      border-top: 1px solid #ebeef5;
      text-align: center;
    }
  }
}

// 回收流程
.process-section {
  background-color: white;
  
  :deep(.el-step__title) {
    font-size: 16px;
    font-weight: 500;
  }
  
  :deep(.el-step__description) {
    font-size: 14px;
  }
  
  .process-action {
    text-align: center;
    margin-top: 40px;
  }
}

// 价值评估
.calculator-section {
  background-color: #f5f7fa;
  
  .calculator-card {
    .calculator-content {
      padding: 20px;
      height: 100%;
      display: flex;
      flex-direction: column;
      justify-content: center;
      
      h2 {
        font-size: 28px;
        font-weight: 600;
        color: #303133;
        margin-bottom: 16px;
      }
      
      p {
        color: #606266;
        line-height: 1.8;
        margin-bottom: 24px;
      }
      
      .calculator-actions {
        display: flex;
        gap: 16px;
      }
    }
    
    .calculator-image {
      height: 300px;
      overflow: hidden;
      border-radius: 6px;
      
      .el-image {
        width: 100%;
        height: 100%;
      }
    }
  }
}

// 合作伙伴
.partners-section {
  background-color: white;
  
  .partners-logo {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 30px;
    
    .partner-item {
      width: 150px;
      height: 80px;
      padding: 10px;
      background-color: #f5f7fa;
      border-radius: 8px;
      display: flex;
      align-items: center;
      justify-content: center;
      
      .el-image {
        width: 120px;
        height: 60px;
      }
    }
  }
}

// 响应式调整
@media (max-width: 768px) {
  .banner-section {
    :deep(.el-carousel) {
      height: 400px !important;
    }
    
    :deep(.el-carousel__item .banner-content) {
      h2 {
        font-size: 24px;
      }
      
      p {
        font-size: 16px;
      }
    }
  }
  
  .section {
    padding: 40px 0;
    
    .section-title {
      h2 {
        font-size: 24px;
      }
    }
  }
  
  .process-section {
    :deep(.el-steps) {
      padding: 0 10px;
    }
  }
  
  .calculator-section {
    .calculator-image {
      margin-top: 20px;
    }
  }
  
  .partners-section {
    .partner-item {
      width: 120px;
      height: 70px;
    }
  }
}

// 大屏幕优化 (1200px - 1920px)
@media (min-width: 1200px) and (max-width: 1919px) {
  .banner-section {
    :deep(.el-carousel) {
      height: 550px !important;
    }
    
    :deep(.el-carousel__item .banner-content) {
      h2 {
        font-size: 36px;
      }
      
      p {
        font-size: 20px;
      }
    }
  }
  
  .section {
    padding: 80px 0;
    
    .section-title {
      h2 {
        font-size: 32px;
      }
      
      p {
        font-size: 18px;
      }
    }
  }
}

// 超高分辨率屏幕优化 (2K及以上)
@media (min-width: 1920px) {
  .banner-section {
    padding: 30px 0 60px;
    
    :deep(.el-carousel) {
      height: 600px !important;
    }
    
    :deep(.el-carousel__item .banner-content) {
      h2 {
        font-size: 40px;
        margin-bottom: 20px;
      }
      
      p {
        font-size: 22px;
        margin-bottom: 30px;
      }
      
      .el-button {
        padding: 15px 30px;
        font-size: 16px;
      }
    }
  }
  
  .section {
    padding: 100px 0;
    
    .section-title {
      margin-bottom: 60px;
      
      h2 {
        font-size: 36px;
        margin-bottom: 16px;
      }
      
      p {
        font-size: 20px;
      }
    }
  }
  
  .features-section {
    .feature-card {
      padding: 20px;
      
      .feature-icon {
        margin-bottom: 25px;
        
        .el-icon {
          font-size: 48px;
        }
      }
      
      h3 {
        font-size: 20px;
        margin-bottom: 15px;
      }
      
      p {
        font-size: 16px;
        line-height: 1.8;
      }
    }
  }
  
  .product-section {
    .product-card {
      .product-card-content {
        padding: 20px;
        
        .product-info {
          h3 {
            font-size: 22px;
            margin-bottom: 15px;
          }
          
          p {
            font-size: 16px;
            margin-bottom: 20px;
          }
        }
        
        .product-icon {
          .el-icon {
            font-size: 70px;
          }
        }
      }
    }
  }
  
  .calculator-section {
    .calculator-card {
      .calculator-content {
        padding: 40px;
        
        h2 {
          font-size: 32px;
          margin-bottom: 20px;
        }
        
        p {
          font-size: 18px;
          margin-bottom: 30px;
        }
        
        .calculator-actions {
          gap: 20px;
          
          .el-button {
            padding: 15px 30px;
            font-size: 16px;
          }
        }
      }
      
      .calculator-image {
        height: 350px;
      }
    }
  }
  
  .partners-section {
    .partner-item {
      width: 180px;
      height: 100px;
      
      .el-image {
        width: 150px;
        height: 80px;
      }
    }
  }
}
</style>

