<template>
  <footer class="app-footer">
    <div class="footer-container">
      <div class="footer-columns">
        <div class="footer-column">
          <h3>关于我们</h3>
          <ul>
            <li><router-link to="/about">公司简介</router-link></li>
            <li><router-link to="/about#culture">企业文化</router-link></li>
            <li><router-link to="/about#timeline">发展历程</router-link></li>
            <li><router-link to="/about#team">团队介绍</router-link></li>
            <li><router-link to="/jobs">加入我们</router-link></li>
          </ul>
        </div>
        
        <div class="footer-column">
          <h3>回收业务</h3>
          <ul>
            <li><router-link to="/products?category=device">设备回收</router-link></li>
            <li><router-link to="/products?category=data">数据销毁</router-link></li>
            <li><router-link to="/products?category=asset">资产处置</router-link></li>
            <li><router-link to="/products?category=green">绿色认证</router-link></li>
          </ul>
        </div>
        
        <div class="footer-column">
          <h3>服务案例</h3>
          <ul>
            <li><router-link to="/cases?type=government">政府机构</router-link></li>
            <li><router-link to="/cases?type=education">教育行业</router-link></li>
            <li><router-link to="/cases?type=finance">金融行业</router-link></li>
            <li><router-link to="/cases?type=enterprise">企业客户</router-link></li>
          </ul>
        </div>
        
        <div class="footer-column">
          <h3>联系我们</h3>
          <ul>
            <li><router-link to="/contact">联系方式</router-link></li>
            <li><router-link to="/contact#consult">在线咨询</router-link></li>
            <li><a href="tel:400-123-4567">服务热线：400-123-4567</a></li>
            <li><a href="mailto:info@recycling.com">邮箱：info@recycling.com</a></li>
          </ul>
        </div>
      </div>
      
      <div class="footer-bottom">
        <div class="copyright">
          <p>© {{ currentYear }} IT设备回收服务 版权所有 ICP备案号：123456789</p>
        </div>
        <div class="footer-links">
          <router-link to="/privacy">隐私政策</router-link>
          <router-link to="/terms">用户协议</router-link>
          <router-link to="/sitemap">站点地图</router-link>
        </div>
      </div>
    </div>
  </footer>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const currentYear = ref(new Date().getFullYear());
</script>

<style lang="scss" scoped>
/* 页脚样式 */
.app-footer {
  background-color: #303133;
  color: #fff;
  padding: 60px 0 30px;
  
  .footer-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 20px;
  }
  
  .footer-columns {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    margin-bottom: 40px;
  }
  
  .footer-column {
    flex: 1;
    min-width: 200px;
    margin-bottom: 30px;
    
    h3 {
      font-size: 18px;
      font-weight: 600;
      margin-bottom: 20px;
      position: relative;
      padding-bottom: 10px;
      
      &:after {
        content: '';
        position: absolute;
        left: 0;
        bottom: 0;
        width: 40px;
        height: 2px;
        background-color: #409EFF;
      }
    }
    
    ul {
      li {
        margin-bottom: 12px;
        
        a {
          color: #a6a9ad;
          transition: color 0.3s;
          
          &:hover {
            color: #fff;
          }
        }
      }
    }
  }
  
  .footer-bottom {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    align-items: center;
    padding-top: 30px;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
    
    .copyright {
      color: #909399;
      font-size: 14px;
    }
    
    .footer-links {
      display: flex;
      gap: 20px;
      
      a {
        color: #909399;
        font-size: 14px;
        transition: color 0.3s;
        
        &:hover {
          color: #fff;
        }
      }
    }
  }
}

/* 响应式调整 */
@media (max-width: 768px) {
  .app-footer {
    padding: 40px 0 20px;
    
    .footer-columns {
      flex-direction: column;
      
      .footer-column {
        margin-bottom: 30px;
      }
    }
    
    .footer-bottom {
      flex-direction: column;
      gap: 15px;
      text-align: center;
      
      .footer-links {
        justify-content: center;
      }
    }
  }
}
</style> 