import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  define: {
    // 定义环境变量
    'import.meta.env.VITE_API_URL': JSON.stringify(process.env.VITE_API_URL || 'http://localhost:5279'),
    'import.meta.env.VITE_CHATHUB_URL': JSON.stringify(process.env.VITE_CHATHUB_URL || 'http://localhost:5279/chathub')
  }
})
