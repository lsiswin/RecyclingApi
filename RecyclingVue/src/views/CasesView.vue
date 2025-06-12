<template>
  <div class="cases-page">
    <!-- 页面头部 -->
    <el-row class="page-header" justify="center">
      <el-col :span="22">
        <h1>案例展示</h1>
        <p class="subtitle">专业IT设备回收服务案例，见证我们的专业实力</p>
      </el-col>
    </el-row>

    <!-- 筛选区域 -->
    <el-row class="filter-section" justify="center">
      <el-col :span="22">
        <el-card shadow="never" class="filter-card">
          <el-row :gutter="20" align="middle">
            <el-col :xs="24" :sm="8" :md="6">
              <el-select v-model="filterForm.category" placeholder="选择案例类型" clearable @change="handleFilter">
                <el-option label="全部类型" value="" />
                <el-option label="企业回收" value="enterprise" />
                <el-option label="学校回收" value="school" />
                <el-option label="政府机构" value="government" />
                <el-option label="医院回收" value="hospital" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="8" :md="6">
              <el-select v-model="filterForm.deviceType" placeholder="设备类型" clearable @change="handleFilter">
                <el-option label="全部设备" value="" />
                <el-option label="台式电脑" value="desktop" />
                <el-option label="笔记本电脑" value="laptop" />
                <el-option label="服务器" value="server" />
                <el-option label="网络设备" value="network" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="8" :md="6">
              <el-select v-model="filterForm.scale" placeholder="回收规模" clearable @change="handleFilter">
                <el-option label="全部规模" value="" />
                <el-option label="小规模(1-50台)" value="small" />
                <el-option label="中规模(51-200台)" value="medium" />
                <el-option label="大规模(200台以上)" value="large" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="24" :md="6">
              <el-button type="primary" @click="resetFilter">重置筛选</el-button>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>

    <!-- 案例列表 -->
    <el-row class="cases-section" justify="center">
      <el-col :span="22">
        <el-row :gutter="20">
          <el-col 
            v-for="caseItem in filteredCases" 
            :key="caseItem.id" 
            :xs="24" 
            :sm="12" 
            :md="8"
            :lg="6"
            :xl="4"
          >
            <el-card shadow="hover" class="case-card" @click="viewCaseDetail(caseItem)">
              <div class="case-image">
                <el-image :src="caseItem.image" fit="cover" />
                <div class="case-overlay">
                  <el-tag :type="getCategoryType(caseItem.category)">{{ getCategoryName(caseItem.category) }}</el-tag>
                </div>
              </div>
              
              <div class="case-content">
                <h3>{{ caseItem.title }}</h3>
                <p class="case-description">{{ caseItem.description }}</p>
                
                <div class="case-info">
                  <div class="info-item">
                    <span class="label">客户：</span>
                    <span class="value">{{ caseItem.client }}</span>
                  </div>
                  <div class="info-item">
                    <span class="label">设备数量：</span>
                    <span class="value">{{ caseItem.deviceCount }}台</span>
                  </div>
                  <div class="info-item">
                    <span class="label">回收时间：</span>
                    <span class="value">{{ caseItem.date }}</span>
                  </div>
                </div>
                
                <div class="case-tags">
                  <el-tag 
                    v-for="tag in caseItem.tags" 
                    :key="tag" 
                    size="small" 
                    class="tag-item"
                  >
                    {{ tag }}
                  </el-tag>
                </div>
              </div>
              
              <div class="case-footer">
                <el-button type="primary" plain size="small">查看详情</el-button>
                <div class="case-stats">
                  <el-icon><View /></el-icon>
                  <span>{{ caseItem.views }}</span>
                </div>
              </div>
            </el-card>
          </el-col>
        </el-row>
        
        <!-- 分页 -->
        <div class="pagination-section">
          <el-pagination
            v-model:current-page="currentPage"
            v-model:page-size="pageSize"
            :page-sizes="[12, 18, 24, 30]"
            :total="totalCases"
            layout="total, sizes, prev, pager, next, jumper"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          />
        </div>
      </el-col>
    </el-row>

    <!-- 案例详情对话框 -->
    <el-dialog v-model="showCaseDetail" title="案例详情" width="80%" top="5vh">
      <div v-if="selectedCase" class="case-detail">
        <el-row :gutter="30">
          <el-col :span="24" :md="12">
            <el-image :src="selectedCase.image" fit="cover" class="detail-image" />
          </el-col>
          <el-col :span="24" :md="12">
            <div class="detail-info">
              <h2>{{ selectedCase.title }}</h2>
              <p class="detail-description">{{ selectedCase.fullDescription }}</p>
              
              <div class="detail-stats">
                <div class="stat-item">
                  <strong>客户名称：</strong>{{ selectedCase.client }}
                </div>
                <div class="stat-item">
                  <strong>项目类型：</strong>{{ getCategoryName(selectedCase.category) }}
                </div>
                <div class="stat-item">
                  <strong>设备数量：</strong>{{ selectedCase.deviceCount }}台
                </div>
                <div class="stat-item">
                  <strong>回收时间：</strong>{{ selectedCase.date }}
                </div>
                <div class="stat-item">
                  <strong>项目周期：</strong>{{ selectedCase.duration }}
                </div>
                <div class="stat-item">
                  <strong>服务满意度：</strong>
                  <el-rate v-model="selectedCase.rating" disabled show-score />
                </div>
              </div>
            </div>
          </el-col>
        </el-row>
        
        <div class="detail-content">
          <h3>项目详情</h3>
          <p>{{ selectedCase.projectDetails }}</p>
          
          <h3>服务亮点</h3>
          <ul class="highlight-list">
            <li v-for="highlight in selectedCase.highlights" :key="highlight">{{ highlight }}</li>
          </ul>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { View } from '@element-plus/icons-vue'
// 导入图片
import caseImage from '@/assets/index.jpeg'

const currentPage = ref(1)
const pageSize = ref(12)
const showCaseDetail = ref(false)
const selectedCase = ref(null)

const filterForm = ref({
  category: '',
  deviceType: '',
  scale: ''
})

// 模拟案例数据
const allCases = ref([
  {
    id: 1,
    title: '某大型科技公司IT设备更新回收',
    description: '为某知名科技公司提供全面的IT设备回收服务，包括1000+台电脑设备的安全回收处理',
    fullDescription: '该项目是我们承接的大型企业IT设备回收项目之一，客户因办公设备更新换代，需要对旧设备进行环保回收处理。',
    client: '某知名科技公司',
    category: 'enterprise',
    deviceType: 'desktop',
    deviceCount: 1200,
    date: '2024-01-15',
    duration: '7天',
    scale: 'large',
    image: caseImage,
    tags: ['数据安全', '环保处理', '批量回收'],
    views: 1580,
    rating: 5,
    projectDetails: '项目涉及台式机800台、笔记本电脑300台、服务器100台的回收处理。我们提供了专业的数据销毁服务，确保企业信息安全，同时按照环保标准进行设备拆解和材料回收。',
    highlights: [
      '专业数据销毁，零信息泄露风险',
      '7天内完成1200台设备回收',
      '100%环保处理，获得环保认证',
      '为客户节省处理成本30万元'
    ]
  },
  {
    id: 2,
    title: '某重点大学实验室设备回收',
    description: '协助某重点大学处理实验室淘汰的计算机设备，确保学术数据安全销毁',
    fullDescription: '该项目为某重点大学计算机学院实验室设备更新项目，涉及多个实验室的设备回收。',
    client: '某重点大学',
    category: 'school',
    deviceType: 'laptop',
    deviceCount: 300,
    date: '2024-02-20',
    duration: '5天',
    scale: 'medium',
    image: caseImage,
    tags: ['学术数据保护', '实验室设备', '教育机构'],
    views: 890,
    rating: 4.8,
    projectDetails: '项目包括实验室台式机200台、笔记本100台的回收。特别注意学术研究数据的安全处理，采用军用级数据销毁标准。',
    highlights: [
      '军用级数据销毁标准',
      '配合学校假期时间安排',
      '提供设备处理证明文件',
      '部分设备捐赠给贫困地区学校'
    ]
  },
  {
    id: 3,
    title: '政府机关办公设备统一回收',
    description: '为某市政府机关提供办公设备回收服务，严格按照政府采购和处置规定执行',
    fullDescription: '该项目严格按照政府资产处置相关规定执行，确保合规性和透明度。',
    client: '某市政府机关',
    category: 'government',
    deviceType: 'desktop',
    deviceCount: 500,
    date: '2024-03-10',
    duration: '10天',
    scale: 'large',
    image: caseImage,
    tags: ['政府采购', '合规处理', '资产处置'],
    views: 1200,
    rating: 4.9,
    projectDetails: '严格按照政府资产处置流程，提供完整的处置文件和证明。所有设备均进行登记造册，确保处置过程透明可追溯。',
    highlights: [
      '严格遵循政府资产处置规定',
      '提供完整的处置证明文件',
      '全程可追溯的处置流程',
      '获得政府部门高度认可'
    ]
  },
  {
    id: 4,
    title: '某三甲医院信息系统设备更新',
    description: '为某三甲医院提供信息系统设备回收，确保患者隐私数据安全',
    fullDescription: '医院信息系统设备回收项目，特别注重患者隐私数据的安全处理。',
    client: '某三甲医院',
    category: 'hospital',
    deviceType: 'server',
    deviceCount: 150,
    date: '2024-04-05',
    duration: '3天',
    scale: 'medium',
    image: caseImage,
    tags: ['医疗数据安全', '服务器回收', '隐私保护'],
    views: 750,
    rating: 5,
    projectDetails: '项目涉及医院信息系统服务器、工作站等设备的回收。采用最高级别的数据销毁标准，确保患者隐私数据安全。',
    highlights: [
      '最高级别数据销毁标准',
      '24小时不间断作业',
      '零患者数据泄露风险',
      '获得医院信息安全部门认证'
    ]
  },
  {
    id: 5,
    title: '某制造企业生产线设备回收',
    description: '协助某制造企业处理生产线淘汰的工控设备和办公电脑',
    fullDescription: '制造企业生产线升级改造项目，涉及工控设备和办公设备的综合回收。',
    client: '某制造企业',
    category: 'enterprise',
    deviceType: 'network',
    deviceCount: 80,
    date: '2024-05-12',
    duration: '4天',
    scale: 'small',
    image: caseImage,
    tags: ['工控设备', '生产线改造', '工业回收'],
    views: 620,
    rating: 4.7,
    projectDetails: '项目包括工控机、网络设备、办公电脑等多种设备类型。需要在不影响生产的情况下完成回收作业。',
    highlights: [
      '不影响正常生产作业',
      '多种设备类型专业处理',
      '提供临时设备支持',
      '获得企业安全部门认可'
    ]
  },
  {
    id: 6,
    title: '某金融机构数据中心设备回收',
    description: '为某银行数据中心提供服务器设备回收，采用银行级数据安全标准',
    fullDescription: '金融机构数据中心设备回收项目，安全要求极高。',
    client: '某银行',
    category: 'enterprise',
    deviceType: 'server',
    deviceCount: 200,
    date: '2024-06-08',
    duration: '5天',
    scale: 'medium',
    image: caseImage,
    tags: ['金融数据安全', '数据中心', '银行级标准'],
    views: 980,
    rating: 5,
    projectDetails: '严格按照银行业数据安全标准执行，所有操作人员均通过银行安全审查。采用物理销毁+软件擦除双重保障。',
    highlights: [
      '银行级数据安全标准',
      '双重数据销毁保障',
      '通过银行安全审查',
      '零金融数据泄露风险'
    ]
  }
])

const totalCases = computed(() => {
  let filtered = allCases.value

  if (filterForm.value.category) {
    filtered = filtered.filter(item => item.category === filterForm.value.category)
  }
  
  if (filterForm.value.deviceType) {
    filtered = filtered.filter(item => item.deviceType === filterForm.value.deviceType)
  }
  
  if (filterForm.value.scale) {
    filtered = filtered.filter(item => item.scale === filterForm.value.scale)
  }

  return filtered.length
})

const filteredCases = computed(() => {
  let filtered = allCases.value

  if (filterForm.value.category) {
    filtered = filtered.filter(item => item.category === filterForm.value.category)
  }
  
  if (filterForm.value.deviceType) {
    filtered = filtered.filter(item => item.deviceType === filterForm.value.deviceType)
  }
  
  if (filterForm.value.scale) {
    filtered = filtered.filter(item => item.scale === filterForm.value.scale)
  }

  // 分页处理
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filtered.slice(start, end)
})

const getCategoryName = (category) => {
  const categoryMap = {
    enterprise: '企业回收',
    school: '学校回收',
    government: '政府机构',
    hospital: '医院回收'
  }
  return categoryMap[category] || category
}

const getCategoryType = (category) => {
  const typeMap = {
    enterprise: 'primary',
    school: 'success',
    government: 'warning',
    hospital: 'danger'
  }
  return typeMap[category] || 'info'
}

const handleFilter = () => {
  currentPage.value = 1
}

const resetFilter = () => {
  filterForm.value = {
    category: '',
    deviceType: '',
    scale: ''
  }
  currentPage.value = 1
}

const handleSizeChange = (val) => {
  pageSize.value = val
  currentPage.value = 1
}

const handleCurrentChange = (val) => {
  currentPage.value = val
}

const viewCaseDetail = (caseItem) => {
  selectedCase.value = caseItem
  showCaseDetail.value = true
  
  // 增加浏览量
  caseItem.views += 1
}

onMounted(() => {
  console.log('Cases page mounted')
})
</script>

<style scoped lang="scss">
.cases-page {
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

.filter-section {
  margin-bottom: 40px;
  
  .filter-card {
    padding: 20px;
  }
}

.cases-section {
  margin-bottom: 40px;
  
  .case-card {
    height: 100%;
    margin-bottom: 20px;
    cursor: pointer;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .case-image {
      position: relative;
      height: 160px;
      overflow: hidden;
      
      .el-image {
        width: 100%;
        height: 100%;
      }
      
      .case-overlay {
        position: absolute;
        top: 10px;
        right: 10px;
      }
    }
    
    .case-content {
      padding: 16px;
      
      h3 {
        font-size: 15px;
        font-weight: 600;
        color: #303133;
        margin-bottom: 8px;
        line-height: 1.4;
        display: -webkit-box;
        -webkit-line-clamp: 1;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }
      
      .case-description {
        color: #606266;
        font-size: 12px;
        line-height: 1.5;
        margin-bottom: 12px;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }
      
      .case-info {
        margin-bottom: 12px;
        
        .info-item {
          display: flex;
          justify-content: space-between;
          margin-bottom: 4px;
          font-size: 11px;
          
          .label {
            color: #909399;
          }
          
          .value {
            color: #303133;
            font-weight: 500;
          }
        }
      }
      
      .case-tags {
        display: flex;
        flex-wrap: wrap;
        gap: 6px;
        
        .tag-item {
          font-size: 11px;
        }
      }
    }
    
    .case-footer {
      padding: 0 16px 16px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      
      .el-button {
        font-size: 12px;
        padding: 6px 12px;
      }
      
      .case-stats {
        display: flex;
        align-items: center;
        gap: 4px;
        color: #909399;
        font-size: 11px;
      }
    }
  }
}

.pagination-section {
  display: flex;
  justify-content: center;
  margin-top: 40px;
}

.case-detail {
  .detail-image {
    width: 100%;
    height: 300px;
    border-radius: 8px;
  }
  
  .detail-info {
    h2 {
      font-size: 24px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 16px;
    }
    
    .detail-description {
      color: #606266;
      line-height: 1.6;
      margin-bottom: 24px;
    }
    
    .detail-stats {
      .stat-item {
        margin-bottom: 12px;
        
        strong {
          color: #303133;
          margin-right: 8px;
        }
      }
    }
  }
  
  .detail-content {
    margin-top: 30px;
    
    h3 {
      font-size: 18px;
      font-weight: 600;
      color: #303133;
      margin: 24px 0 12px;
    }
    
    p {
      color: #606266;
      line-height: 1.6;
      margin-bottom: 16px;
    }
    
    .highlight-list {
      list-style: none;
      padding: 0;
      
      li {
        position: relative;
        padding-left: 20px;
        margin-bottom: 8px;
        color: #606266;
        
        &::before {
          content: '✓';
          position: absolute;
          left: 0;
          color: #67C23A;
          font-weight: bold;
        }
      }
    }
  }
}

@media (max-width: 768px) {
  .page-header {
    h1 {
      font-size: 28px;
    }
  }
  
  .filter-section {
    .filter-card {
      padding: 15px;
    }
  }
  
  .cases-section {
    .case-card {
      .case-image {
        height: 140px;
      }
      
      .case-content {
        padding: 12px;
        
        h3 {
          font-size: 14px;
        }
        
        .case-description {
          font-size: 11px;
        }
        
        .case-info .info-item {
          font-size: 10px;
        }
        
        .case-tags .tag-item {
          font-size: 10px;
        }
      }
      
      .case-footer {
        padding: 0 12px 12px;
        
        .el-button {
          font-size: 11px;
          padding: 4px 8px;
        }
        
        .case-stats {
          font-size: 10px;
        }
      }
    }
  }
  
  .case-detail {
    .detail-image {
      height: 200px;
      margin-bottom: 20px;
    }
  }
}

@media (min-width: 1920px) {
  .cases-section {
    .case-card {
      .case-image {
        height: 180px;
      }
      
      .case-content {
        padding: 18px;
        
        h3 {
          font-size: 16px;
        }
        
        .case-description {
          font-size: 13px;
        }
        
        .case-info .info-item {
          font-size: 12px;
        }
        
        .case-tags .tag-item {
          font-size: 12px;
        }
      }
      
      .case-footer {
        padding: 0 18px 18px;
        
        .el-button {
          font-size: 13px;
          padding: 8px 14px;
        }
        
        .case-stats {
          font-size: 12px;
        }
      }
    }
  }
}
</style> 