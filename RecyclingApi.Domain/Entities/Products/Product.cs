using System;
using System.Collections.Generic;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Domain.Entities.Products
{
    /// <summary>
    /// 产品实体
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 产品分类ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType Type { get; set; }

        /// <summary>
        /// 品牌（lenovo:联想, dell:戴尔, hp:惠普, huawei:华为, apple:苹果, other:其他）
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 设备状态（good:良好, fair:一般, poor:较差, broken:损坏）
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 生产年份
        /// </summary>
        public int ManufactureYear { get; set; }

        /// <summary>
        /// 预估回收价值
        /// </summary>
        public decimal EstimatedValue { get; set; }

        /// <summary>
        /// 回收价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 配置规格
        /// </summary>
        public string Specs { get; set; }

        /// <summary>
        /// 详细配置规格
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 简短描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 完整描述
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// 产品图片URL
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 详细配置（以分号分隔）
        /// </summary>
        public string DetailSpecs { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 产品分类（导航属性）
        /// </summary>
        public ProductCategory Category { get; set; }

        /// <summary>
        /// 处理步骤（导航属性）
        /// </summary>
        public ICollection<ProcessStep> ProcessSteps { get; set; }
    }
}