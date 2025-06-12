<template>
  <el-button
    :type="type"
    :size="size"
    :loading="loading"
    :disabled="disabled"
    @click="$emit('click')"
  >
    <slot></slot>
  </el-button>
</template>

<script setup>
defineProps({
  type: {
    type: String,
    default: 'primary'
  },
  size: {
    type: String,
    default: 'default'
  },
  loading: {
    type: Boolean,
    default: false
  },
  disabled: {
    type: Boolean,
    default: false
  }
})

defineEmits(['click'])
</script>

<style scoped lang="scss">
.el-button {
  font-weight: 500;
  border-radius: 4px;
  
  &.is-disabled {
    opacity: 0.6;
  }
}
</style> 