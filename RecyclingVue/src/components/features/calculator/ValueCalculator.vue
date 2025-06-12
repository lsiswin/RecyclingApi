<template>
  <div class="value-calculator">
    <el-card class="calculator-card">
      <template #header>
        <h3>设备价值评估</h3>
      </template>
      
      <el-form :model="form" label-width="120px">
        <el-form-item label="设备类型">
          <el-select v-model="form.type" placeholder="请选择设备类型">
            <el-option label="服务器" value="server" />
            <el-option label="台式机" value="desktop" />
            <el-option label="笔记本" value="laptop" />
            <el-option label="网络设备" value="network" />
            <el-option label="存储设备" value="storage" />
          </el-select>
        </el-form-item>
        
        <el-form-item label="品牌">
          <el-input v-model="form.brand" placeholder="请输入品牌" />
        </el-form-item>
        
        <el-form-item label="型号">
          <el-input v-model="form.model" placeholder="请输入型号" />
        </el-form-item>
        
        <el-form-item label="生产年份">
          <el-date-picker
            v-model="form.year"
            type="year"
            placeholder="选择年份"
            format="YYYY"
            value-format="YYYY"
          />
        </el-form-item>
        
        <el-form-item label="设备状况">
          <el-radio-group v-model="form.condition">
            <el-radio label="excellent">优秀</el-radio>
            <el-radio label="good">良好</el-radio>
            <el-radio label="fair">一般</el-radio>
            <el-radio label="poor">较差</el-radio>
          </el-radio-group>
        </el-form-item>
        
        <el-form-item>
          <AppButton @click="calculateValue" :loading="calculating">
            计算估值
          </AppButton>
        </el-form-item>
      </el-form>
      
      <div v-if="estimatedValue !== null" class="result">
        <el-alert
          :title="`估算价值: ¥${estimatedValue}`"
          type="success"
          :closable="false"
          show-icon
        />
        <p class="result-note">
          *此为初步估算，实际价值需要专业评估师现场评估确定
        </p>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import AppButton from '@/components/common/AppButton.vue'

const form = reactive({
  type: '',
  brand: '',
  model: '',
  year: '',
  condition: 'good'
})

const calculating = ref(false)
const estimatedValue = ref(null)

const calculateValue = async () => {
  calculating.value = true
  
  // 模拟计算过程
  await new Promise(resolve => setTimeout(resolve, 1000))
  
  // 简单的估值算法
  let baseValue = 1000
  
  // 根据设备类型调整基础价值
  const typeMultiplier = {
    server: 3.0,
    desktop: 1.0,
    laptop: 1.5,
    network: 2.0,
    storage: 2.5
  }
  
  baseValue *= typeMultiplier[form.type] || 1.0
  
  // 根据年份计算折旧
  const currentYear = new Date().getFullYear()
  const age = currentYear - parseInt(form.year)
  const ageDepreciation = Math.max(0, 1 - (age * 0.15))
  
  // 根据状况调整
  const conditionMultiplier = {
    excellent: 1.0,
    good: 0.8,
    fair: 0.6,
    poor: 0.4
  }
  
  const finalValue = Math.round(
    baseValue * ageDepreciation * conditionMultiplier[form.condition]
  )
  
  estimatedValue.value = Math.max(finalValue, 50)
  calculating.value = false
}
</script>

<style scoped lang="scss">
.value-calculator {
  max-width: 600px;
  margin: 0 auto;

  .calculator-card {
    .el-card__header {
      text-align: center;
      
      h3 {
        margin: 0;
        color: #333;
      }
    }
  }

  .result {
    margin-top: 2rem;
    text-align: center;

    .result-note {
      margin-top: 1rem;
      font-size: 0.9rem;
      color: #666;
      font-style: italic;
    }
  }
}
</style> 