<template>
  <div class="about-page">
    <el-row class="page-header" justify="center">
      <el-col :span="20" :lg="16">
        <h1>关于我们</h1>
        <p class="subtitle">专业IT设备回收服务商，致力于为企业和个人提供安全、环保、高效的设备回收解决方案</p>
      </el-col>
    </el-row>

    <!-- 公司简介 -->
    <el-row class="section company-intro" justify="center">
      <el-col :span="20" :lg="16">
        <el-card shadow="hover">
          <template #header>
            <div class="section-title">
              <h2>公司简介</h2>
            </div>
          </template>
          <el-row :gutter="30">
            <el-col :span="24" :md="12">
              <div class="intro-text">
                <p>我们是一家专业的IT设备回收服务商，致力于为企业和个人提供安全、环保、高效的设备回收解决方案。自成立以来，我们已经成功回收了数万台设备，为客户创造了巨大的价值，同时为环保事业做出了重要贡献。</p>
                <p>我们拥有专业的技术团队和完善的服务体系，能够处理各种类型的IT设备回收需求。无论是企业批量设备更新，还是个人设备处理，我们都能提供最优质的服务。</p>
              </div>
            </el-col>
            <el-col :span="24" :md="12">
              <div class="intro-image">
                <el-image :src="companyImage" alt="公司介绍" fit="cover" />
              </div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>

    <!-- 核心优势 -->
    <el-row class="section advantages" justify="center">
      <el-col :span="20" :lg="16">
        <div class="section-title">
          <h2>核心优势</h2>
        </div>
        <el-row :gutter="20">
          <el-col v-for="advantage in advantages" :key="advantage.id"
                 :span="24" :sm="12" :lg="6">
            <el-card shadow="hover" class="advantage-card">
              <div class="advantage-icon">
                <el-icon :size="48">
                  <component :is="advantage.icon" />
                </el-icon>
              </div>
              <h3>{{ advantage.title }}</h3>
              <p>{{ advantage.description }}</p>
            </el-card>
          </el-col>
        </el-row>
      </el-col>
    </el-row>

    <!-- 发展历程 -->
    <el-row class="section timeline" justify="center">
      <el-col :span="20" :lg="16">
        <div class="section-title">
          <h2>发展历程</h2>
        </div>
        <el-timeline>
          <el-timeline-item
            v-for="item in timeline"
            :key="item.id"
            :timestamp="item.year"
            placement="top"
            :type="item.id % 2 === 0 ? 'primary' : 'success'"
          >
            <el-card shadow="hover">
              <h3>{{ item.title }}</h3>
              <p>{{ item.description }}</p>
            </el-card>
          </el-timeline-item>
        </el-timeline>
      </el-col>
    </el-row>

    <!-- 团队介绍 -->
    <el-row class="section team" justify="center">
      <el-col :span="20" :lg="16">
        <div class="section-title">
          <h2>团队介绍</h2>
        </div>
        <el-row :gutter="20">
          <el-col :span="24" :sm="12" :lg="8">
            <el-card shadow="hover" class="team-card">
              <div class="team-avatar">
                <el-avatar :size="100" :src="teamImage" />
              </div>
              <h3>张总</h3>
              <p class="position">首席执行官</p>
              <p class="description">拥有15年IT行业经验，致力于推动绿色科技发展。</p>
            </el-card>
          </el-col>
          <el-col :span="24" :sm="12" :lg="8">
            <el-card shadow="hover" class="team-card">
              <div class="team-avatar">
                <el-avatar :size="100" :src="teamImage" />
              </div>
              <h3>李总</h3>
              <p class="position">首席技术官</p>
              <p class="description">技术专家，负责公司技术发展方向和创新。</p>
            </el-card>
          </el-col>
          <el-col :span="24" :sm="12" :lg="8">
            <el-card shadow="hover" class="team-card">
              <div class="team-avatar">
                <el-avatar :size="100" :src="teamImage" />
              </div>
              <h3>王总</h3>
              <p class="position">运营总监</p>
              <p class="description">运营专家，确保公司高效运营和客户满意度。</p>
            </el-card>
          </el-col>
        </el-row>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import {Trophy, Timer, Money } from '@element-plus/icons-vue'
// 导入图片
import companyImage from '@/assets/index.jpeg'
import teamImage from '@/assets/index.jpeg'

// 核心优势数据
const advantages = [
  {
    id: 1,
    icon: Shield,
    title: '数据安全',
    description: '专业数据销毁服务，确保企业信息安全，符合国际安全标准'
  },
  {
    id: 2,
    icon: Trophy,
    title: '行业领先',
    description: '多年行业经验，服务过众多知名企业，获得客户一致好评'
  },
  {
    id: 3,
    icon: Timer,
    title: '快速响应',
    description: '24小时快速响应，专业团队上门服务，高效处理回收需求'
  },
  {
    id: 4,
    icon: Money,
    title: '价值最大化',
    description: '专业评估团队，确保设备残值最大化，为客户创造更多价值'
  }
]

// 发展历程数据
const timeline = [
  {
    id: 1,
    year: '2018',
    title: '公司成立',
    description: '在北京成立，专注于IT设备回收服务'
  },
  {
    id: 2,
    year: '2019',
    title: '业务扩展',
    description: '服务范围扩展至华北地区，建立专业回收团队'
  },
  {
    id: 3,
    year: '2020',
    title: '技术升级',
    description: '引入先进的数据销毁技术，获得相关资质认证'
  },
  {
    id: 4,
    year: '2021',
    title: '规模扩大',
    description: '成立多个分公司，服务网络覆盖全国主要城市'
  },
  {
    id: 5,
    year: '2022',
    title: '创新发展',
    description: '推出在线评估系统，提升服务效率和客户体验'
  },
  {
    id: 6,
    year: '2023',
    title: '行业领先',
    description: '成为行业标杆企业，累计回收设备超过10万台'
  }
]
</script>

<style scoped lang="scss">
.about-page {
  padding: 40px 0;
  background-color: #f5f7fa;
}

.page-header {
  text-align: center;
  margin-bottom: 40px;
  
  h1 {
    font-size: 36px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 16px;
  }
  
  .subtitle {
    font-size: 16px;
    color: #606266;
    max-width: 800px;
    margin: 0 auto;
  }
}

.section {
  margin-bottom: 60px;
  
  .section-title {
    text-align: center;
    margin-bottom: 30px;
    
    h2 {
      font-size: 28px;
      font-weight: 600;
      color: #303133;
      position: relative;
      display: inline-block;
      padding-bottom: 10px;
      
      &:after {
        content: '';
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 60px;
        height: 3px;
        background: linear-gradient(90deg, #409EFF, #67C23A);
      }
    }
  }
}

.company-intro {
  .intro-text {
    p {
      margin-bottom: 16px;
      line-height: 1.8;
      color: #606266;
      
      &:last-child {
        margin-bottom: 0;
      }
    }
  }
  
  .intro-image {
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
    margin-top: 20px;
    
    .el-image {
      width: 100%;
      height: 300px;
    }
  }
}

.advantages {
  .advantage-card {
    text-align: center;
    padding: 30px 20px;
    height: 100%;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .advantage-icon {
      color: #409EFF;
      margin-bottom: 20px;
    }
    
    h3 {
      font-size: 18px;
      margin-bottom: 16px;
      color: #303133;
    }
    
    p {
      color: #606266;
      line-height: 1.6;
    }
  }
}

.timeline {
  :deep(.el-timeline) {
    padding: 20px;
  }
  
  :deep(.el-timeline-item__node) {
    width: 16px;
    height: 16px;
  }
  
  :deep(.el-timeline-item__wrapper) {
    padding-left: 20px;
  }
  
  :deep(.el-timeline-item__timestamp) {
    font-size: 16px;
    font-weight: bold;
    color: #409EFF;
  }
  
  :deep(.el-card) {
    h3 {
      font-size: 18px;
      margin-bottom: 8px;
      color: #303133;
    }
    
    p {
      color: #606266;
      line-height: 1.6;
    }
  }
}

.team {
  .team-card {
    text-align: center;
    padding: 30px 20px;
    height: 100%;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .team-avatar {
      margin-bottom: 20px;
    }
    
    h3 {
      font-size: 18px;
      margin-bottom: 8px;
      color: #303133;
    }
    
    .position {
      color: #409EFF;
      font-weight: 500;
      margin-bottom: 16px;
    }
    
    .description {
      color: #606266;
      line-height: 1.6;
    }
  }
}

@media (max-width: 768px) {
  .page-header {
    h1 {
      font-size: 28px;
    }
  }
  
  .section {
    .section-title {
      h2 {
        font-size: 24px;
      }
    }
  }
  
  .team-card, .advantage-card {
    margin-bottom: 20px;
  }
}
</style>
