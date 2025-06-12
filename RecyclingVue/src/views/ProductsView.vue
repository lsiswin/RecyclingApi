<template>
  <div class="products-page">
    <!-- 页面头部 -->
    <el-row class="page-header" justify="center">
      <el-col :span="22">
        <h1>产品类型</h1>
        <p class="subtitle">我们回收各类IT设备，为您的闲置资产创造最大价值</p>
      </el-col>
    </el-row>

    <!-- 筛选区域 -->
    <el-row class="filter-section" justify="center">
      <el-col :span="22">
        <el-card shadow="never" class="filter-card">
          <el-row :gutter="20" align="middle">
            <el-col :xs="24" :sm="6" :md="4">
              <el-select v-model="filterForm.category" placeholder="设备类型" clearable @change="handleFilter">
                <el-option label="全部类型" value="" />
                <el-option label="台式电脑" value="desktop" />
                <el-option label="笔记本电脑" value="laptop" />
                <el-option label="服务器" value="server" />
                <el-option label="网络设备" value="network" />
                <el-option label="打印设备" value="printer" />
                <el-option label="移动设备" value="mobile" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="6" :md="4">
              <el-select v-model="filterForm.brand" placeholder="品牌" clearable @change="handleFilter">
                <el-option label="全部品牌" value="" />
                <el-option label="联想" value="lenovo" />
                <el-option label="戴尔" value="dell" />
                <el-option label="惠普" value="hp" />
                <el-option label="华为" value="huawei" />
                <el-option label="苹果" value="apple" />
                <el-option label="其他" value="other" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="6" :md="4">
              <el-select v-model="filterForm.condition" placeholder="设备状态" clearable @change="handleFilter">
                <el-option label="全部状态" value="" />
                <el-option label="正常使用" value="good" />
                <el-option label="轻微损坏" value="fair" />
                <el-option label="严重损坏" value="poor" />
                <el-option label="无法开机" value="broken" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="6" :md="4">
              <el-select v-model="filterForm.priceRange" placeholder="价格范围" clearable @change="handleFilter">
                <el-option label="全部价格" value="" />
                <el-option label="100-500元" value="100-500" />
                <el-option label="500-1000元" value="500-1000" />
                <el-option label="1000-3000元" value="1000-3000" />
                <el-option label="3000元以上" value="3000+" />
              </el-select>
            </el-col>
            
            <el-col :xs="24" :sm="12" :md="4">
              <el-input 
                v-model="filterForm.keyword" 
                placeholder="搜索产品型号" 
                clearable 
                @input="handleSearch"
              >
                <template #prefix>
                  <el-icon><Search /></el-icon>
                </template>
              </el-input>
            </el-col>
            
            <el-col :xs="24" :sm="12" :md="4">
              <el-button type="primary" @click="resetFilter">重置筛选</el-button>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>

    <!-- 产品统计 -->
    <el-row class="stats-section" justify="center">
      <el-col :span="22">
        <div class="stats-info">
          <span>共找到 <strong>{{ totalProducts }}</strong> 个产品</span>
          <div class="sort-options">
            <span>排序：</span>
            <el-select v-model="sortBy" @change="handleSort" style="width: 120px;">
              <el-option label="默认排序" value="default" />
              <el-option label="价格升序" value="price-asc" />
              <el-option label="价格降序" value="price-desc" />
              <el-option label="最新发布" value="date-desc" />
            </el-select>
          </div>
        </div>
      </el-col>
    </el-row>

    <!-- 产品列表 -->
    <el-row class="products-section" justify="center">
      <el-col :span="22">
        <el-row :gutter="20">
          <el-col 
            v-for="product in filteredProducts" 
            :key="product.id" 
            :xs="24" 
            :sm="12" 
            :md="8" 
            :lg="6"
            :xl="4"
          >
            <el-card shadow="hover" class="product-card" @click="viewProductDetail(product)">
              <div class="product-image">
                <el-image :src="product.image" fit="cover" />
                <div class="product-overlay">
                  <el-tag :type="getConditionType(product.condition)">{{ getConditionName(product.condition) }}</el-tag>
                </div>
              </div>
              
              <div class="product-content">
                <h3>{{ product.name }}</h3>
                <p class="product-model">{{ product.model }}</p>
                <p class="product-description">{{ product.description }}</p>
                
                <div class="product-info">
                  <div class="info-item">
                    <span class="label">品牌：</span>
                    <span class="value">{{ getBrandName(product.brand) }}</span>
                  </div>
                  <div class="info-item">
                    <span class="label">年份：</span>
                    <span class="value">{{ product.year }}年</span>
                  </div>
                  <div class="info-item">
                    <span class="label">配置：</span>
                    <span class="value">{{ product.specs }}</span>
                  </div>
                </div>
                
                <div class="product-price">
                  <span class="price-label">回收价格：</span>
                  <span class="price-value">¥{{ product.price }}</span>
                </div>
              </div>
              
              <div class="product-footer">
                <el-button type="primary" size="small">立即回收</el-button>
                <el-button type="success" plain size="small">获取报价</el-button>
              </div>
            </el-card>
          </el-col>
        </el-row>
        
        <!-- 分页 -->
        <div class="pagination-section">
          <el-pagination
            v-model:current-page="currentPage"
            v-model:page-size="pageSize"
            :page-sizes="[10, 20, 30, 40]"
            :total="totalProducts"
            layout="total, sizes, prev, pager, next, jumper"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          />
        </div>
      </el-col>
    </el-row>

    <!-- 产品详情对话框 -->
    <el-dialog v-model="showProductDetail" title="产品详情" width="80%" top="5vh">
      <div v-if="selectedProduct" class="product-detail">
        <el-row :gutter="30">
          <el-col :span="24" :md="12">
            <el-image :src="selectedProduct.image" fit="cover" class="detail-image" />
          </el-col>
          <el-col :span="24" :md="12">
            <div class="detail-info">
              <h2>{{ selectedProduct.name }}</h2>
              <p class="detail-model">型号：{{ selectedProduct.model }}</p>
              <p class="detail-description">{{ selectedProduct.fullDescription }}</p>
              
              <div class="detail-specs">
                <h3>详细配置</h3>
                <ul class="specs-list">
                  <li v-for="spec in selectedProduct.detailSpecs" :key="spec">{{ spec }}</li>
                </ul>
              </div>
              
              <div class="detail-price">
                <div class="price-info">
                  <span class="current-price">¥{{ selectedProduct.price }}</span>
                  <span class="price-note">（参考价格，实际价格以评估为准）</span>
                </div>
              </div>
              
              <div class="detail-actions">
                <el-button type="primary" size="large">立即回收</el-button>
                <el-button type="success" size="large">获取详细报价</el-button>
              </div>
            </div>
          </el-col>
        </el-row>
        
        <div class="detail-notes">
          <h3>回收说明</h3>
          <ul class="notes-list">
            <li>价格仅供参考，实际回收价格需要根据设备具体状况评估</li>
            <li>设备需要能正常开机，系统运行稳定</li>
            <li>外观无严重损坏，屏幕无裂痕</li>
            <li>提供原装配件可获得更高回收价格</li>
            <li>我们提供免费上门评估服务</li>
          </ul>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { Search } from '@element-plus/icons-vue'
// 导入图片
import productImage from '@/assets/index.jpeg'

const currentPage = ref(1)
const pageSize = ref(10)
const showProductDetail = ref(false)
const selectedProduct = ref(null)
const sortBy = ref('default')

const filterForm = ref({
  category: '',
  brand: '',
  condition: '',
  priceRange: '',
  keyword: ''
})

// 模拟产品数据
const allProducts = ref([
  {
    id: 1,
    name: 'ThinkPad X1 Carbon',
    model: 'X1 Carbon Gen 9',
    category: 'laptop',
    brand: 'lenovo',
    condition: 'good',
    year: 2021,
    price: 4500,
    specs: 'i7-1165G7/16GB/512GB',
    description: '轻薄商务笔记本，性能优秀，外观良好',
    fullDescription: '联想ThinkPad X1 Carbon第九代，搭载Intel i7-1165G7处理器，16GB内存，512GB固态硬盘。外观保持良好，无明显磨损，键盘手感优秀，屏幕显示清晰。',
    image: productImage,
    detailSpecs: [
      '处理器：Intel Core i7-1165G7',
      '内存：16GB LPDDR4X',
      '存储：512GB PCIe SSD',
      '显示：14英寸 2K IPS屏幕',
      '重量：约1.13kg',
      '电池：57Wh锂电池'
    ],
    publishDate: '2024-01-15'
  },
  {
    id: 2,
    name: 'MacBook Pro',
    model: 'MacBook Pro 13" M1',
    category: 'laptop',
    brand: 'apple',
    condition: 'good',
    year: 2020,
    price: 6800,
    specs: 'M1/8GB/256GB',
    description: '苹果M1芯片笔记本，性能强劲，续航优秀',
    fullDescription: '苹果MacBook Pro 13英寸M1版本，搭载苹果自研M1芯片，8GB统一内存，256GB固态硬盘。外观精美，性能强劲，续航时间长。',
    image: productImage,
    detailSpecs: [
      '芯片：Apple M1',
      '内存：8GB统一内存',
      '存储：256GB SSD',
      '显示：13.3英寸 Retina显示屏',
      '重量：约1.4kg',
      '续航：最长20小时'
    ],
    publishDate: '2024-01-20'
  },
  {
    id: 3,
    name: 'Dell OptiPlex',
    model: 'OptiPlex 7090',
    category: 'desktop',
    brand: 'dell',
    condition: 'good',
    year: 2021,
    price: 2800,
    specs: 'i5-11500/8GB/256GB',
    description: '戴尔商用台式机，稳定可靠，适合办公使用',
    fullDescription: '戴尔OptiPlex 7090商用台式机，搭载Intel i5-11500处理器，8GB内存，256GB固态硬盘。机箱小巧，性能稳定，适合办公环境使用。',
    image: productImage,
    detailSpecs: [
      '处理器：Intel Core i5-11500',
      '内存：8GB DDR4',
      '存储：256GB PCIe SSD',
      '显卡：Intel UHD Graphics 750',
      '接口：USB 3.2、HDMI、DP',
      '尺寸：小型机箱设计'
    ],
    publishDate: '2024-02-01'
  },
  {
    id: 4,
    name: 'HP ProBook',
    model: 'ProBook 450 G8',
    category: 'laptop',
    brand: 'hp',
    condition: 'fair',
    year: 2021,
    price: 3200,
    specs: 'i5-1135G7/8GB/512GB',
    description: '惠普商务笔记本，性价比高，轻微使用痕迹',
    fullDescription: '惠普ProBook 450 G8商务笔记本，搭载Intel i5-1135G7处理器，8GB内存，512GB固态硬盘。有轻微使用痕迹，但功能正常。',
    image: productImage,
    detailSpecs: [
      '处理器：Intel Core i5-1135G7',
      '内存：8GB DDR4',
      '存储：512GB PCIe SSD',
      '显示：15.6英寸 FHD屏幕',
      '显卡：Intel Iris Xe Graphics',
      '重量：约1.74kg'
    ],
    publishDate: '2024-02-10'
  },
  {
    id: 5,
    name: 'Huawei MateBook',
    model: 'MateBook 14 2021',
    category: 'laptop',
    brand: 'huawei',
    condition: 'good',
    year: 2021,
    price: 4200,
    specs: 'i5-1135G7/16GB/512GB',
    description: '华为轻薄笔记本，屏幕素质优秀，触控体验佳',
    fullDescription: '华为MateBook 14 2021款，搭载Intel i5-1135G7处理器，16GB内存，512GB固态硬盘。2K触控屏幕，色彩还原度高。',
    image: productImage,
    detailSpecs: [
      '处理器：Intel Core i5-1135G7',
      '内存：16GB LPDDR4X',
      '存储：512GB PCIe SSD',
      '显示：14英寸 2K触控屏',
      '显卡：Intel Iris Xe Graphics',
      '重量：约1.49kg'
    ],
    publishDate: '2024-02-15'
  },
  {
    id: 6,
    name: 'Lenovo ThinkCentre',
    model: 'ThinkCentre M720q',
    category: 'desktop',
    brand: 'lenovo',
    condition: 'good',
    year: 2020,
    price: 1800,
    specs: 'i3-9100T/8GB/256GB',
    description: '联想迷你主机，体积小巧，性能够用',
    fullDescription: '联想ThinkCentre M720q迷你主机，搭载Intel i3-9100T处理器，8GB内存，256GB固态硬盘。体积小巧，适合空间有限的办公环境。',
    image: productImage,
    detailSpecs: [
      '处理器：Intel Core i3-9100T',
      '内存：8GB DDR4',
      '存储：256GB SATA SSD',
      '显卡：Intel UHD Graphics 630',
      '尺寸：179×183×34.5mm',
      '接口：USB 3.1、HDMI、DP'
    ],
    publishDate: '2024-02-20'
  },
  {
    id: 7,
    name: 'Dell PowerEdge',
    model: 'PowerEdge R740',
    category: 'server',
    brand: 'dell',
    condition: 'good',
    year: 2019,
    price: 15000,
    specs: 'Xeon Silver 4214/32GB/1TB',
    description: '戴尔机架式服务器，企业级性能，稳定可靠',
    fullDescription: '戴尔PowerEdge R740机架式服务器，搭载Intel Xeon Silver 4214处理器，32GB内存，1TB硬盘。企业级服务器，性能强劲，稳定性高。',
    image: productImage,
    detailSpecs: [
      '处理器：Intel Xeon Silver 4214',
      '内存：32GB DDR4 ECC',
      '存储：1TB SATA硬盘',
      '网络：双千兆网卡',
      '电源：冗余电源设计',
      '规格：2U机架式'
    ],
    publishDate: '2024-03-01'
  },
  {
    id: 8,
    name: 'HP LaserJet',
    model: 'LaserJet Pro M404dn',
    category: 'printer',
    brand: 'hp',
    condition: 'good',
    year: 2020,
    price: 800,
    specs: '黑白激光/双面打印',
    description: '惠普激光打印机，打印速度快，成本低',
    fullDescription: '惠普LaserJet Pro M404dn黑白激光打印机，支持双面打印，打印速度快，成本低廉。适合中小型办公室使用。',
    image: productImage,
    detailSpecs: [
      '类型：黑白激光打印机',
      '打印速度：38页/分钟',
      '分辨率：1200×1200 dpi',
      '内存：256MB',
      '接口：USB、网络',
      '功能：双面打印'
    ],
    publishDate: '2024-03-05'
  }
])

const totalProducts = computed(() => {
  let filtered = allProducts.value

  if (filterForm.value.category) {
    filtered = filtered.filter(item => item.category === filterForm.value.category)
  }
  
  if (filterForm.value.brand) {
    filtered = filtered.filter(item => item.brand === filterForm.value.brand)
  }
  
  if (filterForm.value.condition) {
    filtered = filtered.filter(item => item.condition === filterForm.value.condition)
  }
  
  if (filterForm.value.priceRange) {
    filtered = filtered.filter(item => {
      const price = item.price
      switch (filterForm.value.priceRange) {
        case '100-500':
          return price >= 100 && price <= 500
        case '500-1000':
          return price >= 500 && price <= 1000
        case '1000-3000':
          return price >= 1000 && price <= 3000
        case '3000+':
          return price >= 3000
        default:
          return true
      }
    })
  }
  
  if (filterForm.value.keyword) {
    const keyword = filterForm.value.keyword.toLowerCase()
    filtered = filtered.filter(item => 
      item.name.toLowerCase().includes(keyword) || 
      item.model.toLowerCase().includes(keyword) ||
      item.description.toLowerCase().includes(keyword)
    )
  }

  return filtered.length
})

const filteredProducts = computed(() => {
  let filtered = allProducts.value

  // 应用筛选条件
  if (filterForm.value.category) {
    filtered = filtered.filter(item => item.category === filterForm.value.category)
  }
  
  if (filterForm.value.brand) {
    filtered = filtered.filter(item => item.brand === filterForm.value.brand)
  }
  
  if (filterForm.value.condition) {
    filtered = filtered.filter(item => item.condition === filterForm.value.condition)
  }
  
  if (filterForm.value.priceRange) {
    filtered = filtered.filter(item => {
      const price = item.price
      switch (filterForm.value.priceRange) {
        case '100-500':
          return price >= 100 && price <= 500
        case '500-1000':
          return price >= 500 && price <= 1000
        case '1000-3000':
          return price >= 1000 && price <= 3000
        case '3000+':
          return price >= 3000
        default:
          return true
      }
    })
  }
  
  if (filterForm.value.keyword) {
    const keyword = filterForm.value.keyword.toLowerCase()
    filtered = filtered.filter(item => 
      item.name.toLowerCase().includes(keyword) || 
      item.model.toLowerCase().includes(keyword) ||
      item.description.toLowerCase().includes(keyword)
    )
  }

  // 应用排序
  if (sortBy.value === 'price-asc') {
    filtered.sort((a, b) => a.price - b.price)
  } else if (sortBy.value === 'price-desc') {
    filtered.sort((a, b) => b.price - a.price)
  } else if (sortBy.value === 'date-desc') {
    filtered.sort((a, b) => new Date(b.publishDate) - new Date(a.publishDate))
  }

  // 分页处理
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filtered.slice(start, end)
})

const getBrandName = (brand) => {
  const brandMap = {
    lenovo: '联想',
    dell: '戴尔',
    hp: '惠普',
    huawei: '华为',
    apple: '苹果',
    other: '其他'
  }
  return brandMap[brand] || brand
}

const getConditionName = (condition) => {
  const conditionMap = {
    good: '良好',
    fair: '一般',
    poor: '较差',
    broken: '损坏'
  }
  return conditionMap[condition] || condition
}

const getConditionType = (condition) => {
  const typeMap = {
    good: 'success',
    fair: 'warning',
    poor: 'danger',
    broken: 'info'
  }
  return typeMap[condition] || 'info'
}

const handleFilter = () => {
  currentPage.value = 1
}

const handleSearch = () => {
  currentPage.value = 1
}

const handleSort = () => {
  currentPage.value = 1
}

const resetFilter = () => {
  filterForm.value = {
    category: '',
    brand: '',
    condition: '',
    priceRange: '',
    keyword: ''
  }
  sortBy.value = 'default'
  currentPage.value = 1
}

const handleSizeChange = (val) => {
  pageSize.value = val
  currentPage.value = 1
}

const handleCurrentChange = (val) => {
  currentPage.value = val
}

const viewProductDetail = (product) => {
  selectedProduct.value = product
  showProductDetail.value = true
}

onMounted(() => {
  console.log('Products page mounted')
})
</script>

<style scoped lang="scss">
.products-page {
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
  margin-bottom: 30px;
  
  .filter-card {
    padding: 20px;
  }
}

.stats-section {
  margin-bottom: 30px;
  
  .stats-info {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 10px;
    
    .sort-options {
      display: flex;
      align-items: center;
      gap: 10px;
    }
  }
}

.products-section {
  margin-bottom: 40px;
  
  .product-card {
    height: 100%;
    margin-bottom: 20px;
    cursor: pointer;
    transition: transform 0.3s;
    
    &:hover {
      transform: translateY(-5px);
    }
    
    .product-image {
      position: relative;
      height: 160px;
      overflow: hidden;
      
      .el-image {
        width: 100%;
        height: 100%;
      }
      
      .product-overlay {
        position: absolute;
        top: 10px;
        right: 10px;
      }
    }
    
    .product-content {
      padding: 16px;
      
      h3 {
        font-size: 15px;
        font-weight: 600;
        color: #303133;
        margin-bottom: 6px;
        line-height: 1.4;
        display: -webkit-box;
        -webkit-line-clamp: 1;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }
      
      .product-model {
        color: #909399;
        font-size: 12px;
        margin-bottom: 6px;
      }
      
      .product-description {
        color: #606266;
        font-size: 12px;
        line-height: 1.4;
        margin-bottom: 10px;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }
      
      .product-info {
        margin-bottom: 10px;
        
        .info-item {
          display: flex;
          justify-content: space-between;
          margin-bottom: 3px;
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
      
      .product-price {
        text-align: center;
        padding: 8px 0;
        border-top: 1px solid #ebeef5;
        
        .price-label {
          color: #909399;
          font-size: 11px;
          margin-right: 6px;
        }
        
        .price-value {
          color: #f56c6c;
          font-size: 16px;
          font-weight: 600;
        }
      }
    }
    
    .product-footer {
      padding: 0 16px 16px;
      display: flex;
      gap: 8px;
      
      .el-button {
        flex: 1;
        font-size: 12px;
        padding: 6px 8px;
      }
    }
  }
}

.pagination-section {
  display: flex;
  justify-content: center;
  margin-top: 40px;
}

.product-detail {
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
      margin-bottom: 8px;
    }
    
    .detail-model {
      color: #909399;
      margin-bottom: 16px;
    }
    
    .detail-description {
      color: #606266;
      line-height: 1.6;
      margin-bottom: 24px;
    }
    
    .detail-specs {
      margin-bottom: 24px;
      
      h3 {
        font-size: 16px;
        font-weight: 600;
        color: #303133;
        margin-bottom: 12px;
      }
      
      .specs-list {
        list-style: none;
        padding: 0;
        
        li {
          padding: 4px 0;
          color: #606266;
          font-size: 14px;
        }
      }
    }
    
    .detail-price {
      margin-bottom: 24px;
      
      .price-info {
        .current-price {
          color: #f56c6c;
          font-size: 28px;
          font-weight: 600;
          margin-right: 10px;
        }
        
        .price-note {
          color: #909399;
          font-size: 12px;
        }
      }
    }
    
    .detail-actions {
      display: flex;
      gap: 16px;
    }
  }
  
  .detail-notes {
    margin-top: 30px;
    
    h3 {
      font-size: 16px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 12px;
    }
    
    .notes-list {
      list-style: none;
      padding: 0;
      
      li {
        position: relative;
        padding-left: 20px;
        margin-bottom: 8px;
        color: #606266;
        font-size: 14px;
        
        &::before {
          content: '•';
          position: absolute;
          left: 0;
          color: #409EFF;
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
  
  .stats-section {
    .stats-info {
      flex-direction: column;
      gap: 15px;
      align-items: flex-start;
    }
  }
  
  .products-section {
    .product-card {
      .product-image {
        height: 140px;
      }
      
      .product-content {
        padding: 12px;
        
        h3 {
          font-size: 14px;
        }
        
        .product-description {
          font-size: 11px;
        }
      }
      
      .product-footer {
        padding: 0 12px 12px;
        
        .el-button {
          font-size: 11px;
          padding: 4px 6px;
        }
      }
    }
  }
  
  .product-detail {
    .detail-image {
      height: 200px;
      margin-bottom: 20px;
    }
    
    .detail-actions {
      flex-direction: column;
    }
  }
}

@media (min-width: 1920px) {
  .products-section {
    .product-card {
      .product-image {
        height: 180px;
      }
      
      .product-content {
        padding: 18px;
        
        h3 {
          font-size: 16px;
        }
        
        .product-description {
          font-size: 13px;
        }
        
        .product-info .info-item {
          font-size: 12px;
        }
      }
      
      .product-footer {
        padding: 0 18px 18px;
        
        .el-button {
          font-size: 13px;
          padding: 8px 12px;
        }
      }
    }
  }
}
</style> 