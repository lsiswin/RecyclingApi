using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecyclingApi.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// 产品DTO
    /// </summary>
    public class ProductDto
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
        /// 产品分类名称
        /// </summary>
        public string CategoryName { get; set; }

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
        public int Year { get; set; }

        /// <summary>
        /// 回收价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 配置规格
        /// </summary>
        public string Specs { get; set; }

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
        /// 详细配置
        /// </summary>
        public List<string> DetailSpecs { get; set; }

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
    }

    /// <summary>
    /// 创建或更新产品DTO
    /// </summary>
    public class CreateUpdateProductDto
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required(ErrorMessage = "产品名称不能为空")]
        [StringLength(100, ErrorMessage = "产品名称长度不能超过100个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        [Required(ErrorMessage = "产品型号不能为空")]
        [StringLength(100, ErrorMessage = "产品型号长度不能超过100个字符")]
        public string Model { get; set; }

        /// <summary>
        /// 产品分类ID
        /// </summary>
        [Required(ErrorMessage = "产品分类不能为空")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 品牌（lenovo:联想, dell:戴尔, hp:惠普, huawei:华为, apple:苹果, other:其他）
        /// </summary>
        [Required(ErrorMessage = "品牌不能为空")]
        public string Brand { get; set; }

        /// <summary>
        /// 设备状态（good:良好, fair:一般, poor:较差, broken:损坏）
        /// </summary>
        [Required(ErrorMessage = "设备状态不能为空")]
        public string Condition { get; set; }

        /// <summary>
        /// 生产年份
        /// </summary>
        [Required(ErrorMessage = "生产年份不能为空")]
        [Range(1990, 2100, ErrorMessage = "生产年份必须在1990-2100之间")]
        public int Year { get; set; }

        /// <summary>
        /// 回收价格
        /// </summary>
        [Required(ErrorMessage = "回收价格不能为空")]
        [Range(0, 1000000, ErrorMessage = "回收价格必须大于等于0")]
        public decimal Price { get; set; }

        /// <summary>
        /// 配置规格
        /// </summary>
        [Required(ErrorMessage = "配置规格不能为空")]
        [StringLength(200, ErrorMessage = "配置规格长度不能超过200个字符")]
        public string Specs { get; set; }

        /// <summary>
        /// 简短描述
        /// </summary>
        [Required(ErrorMessage = "简短描述不能为空")]
        [StringLength(500, ErrorMessage = "简短描述长度不能超过500个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 完整描述
        /// </summary>
        [Required(ErrorMessage = "完整描述不能为空")]
        public string FullDescription { get; set; }

        /// <summary>
        /// 产品图片URL
        /// </summary>
        [Required(ErrorMessage = "产品图片不能为空")]
        public string Image { get; set; }

        /// <summary>
        /// 详细配置
        /// </summary>
        public List<string> DetailSpecs { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 排序号（值越小越靠前）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime? PublishDate { get; set; }
    }

    /// <summary>
    /// 产品查询请求DTO
    /// </summary>
    public class ProductRequestDto : PagedRequestDto
    {
        /// <summary>
        /// 关键字（产品名称、型号、描述）
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 产品分类ID
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 最小价格
        /// </summary>
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// 最大价格
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string OrderBy { get; set; }
    }
} 