<template>
  <div class="pagination-container">
    <el-pagination
      v-model:current-page="currentPage"
      v-model:page-size="pageSize"
      :page-sizes="[10, 20, 30, 50]"
      :total="total"
      layout="total, sizes, prev, pager, next, jumper"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
    />
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watchEffect } from 'vue'

const props = defineProps({
  page: {
    type: Number,
    default: 1
  },
  limit: {
    type: Number,
    default: 10
  },
  total: {
    type: Number,
    required: true
  },
  pageSizes: {
    type: Array as () => number[],
    default: () => [10, 20, 30, 50]
  },
  layout: {
    type: String,
    default: 'total, sizes, prev, pager, next, jumper'
  },
  background: {
    type: Boolean,
    default: true
  },
  autoScroll: {
    type: Boolean,
    default: true
  },
  hidden: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:page', 'update:limit', 'pagination'])

// 内部状态
const currentPage = ref(props.page)
const pageSize = ref(props.limit)

// 监听外部props变化，同步到内部状态
watchEffect(() => {
  currentPage.value = props.page
  pageSize.value = props.limit
})

// 计算属性：是否隐藏分页
const showPagination = computed(() => {
  return !props.hidden && props.total > 0
})

// 处理每页条数改变
const handleSizeChange = (val: number) => {
  pageSize.value = val
  emit('update:limit', val)
  emit('pagination', { page: currentPage.value, limit: val })
}

// 处理当前页改变
const handleCurrentChange = (val: number) => {
  currentPage.value = val
  emit('update:page', val)
  emit('pagination', { page: val, limit: pageSize.value })
  
  // 自动滚动到顶部
  if (props.autoScroll) {
    scrollToTop()
  }
}

// 滚动到顶部
const scrollToTop = () => {
  const el = document.querySelector('.el-main') || window
  el.scrollTo({ top: 0, behavior: 'smooth' })
}
</script>

<style scoped>
.pagination-container {
  margin-top: 15px;
  padding: 10px 0;
  text-align: right;
}
</style> 