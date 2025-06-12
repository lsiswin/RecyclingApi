using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ProductDTOs;

namespace RecyclingApi.Application.Services.Product
{
    /// <summary>
    /// 产品分类服务接口
    /// </summary>
    public interface IProductCategoryService
    {
        /// <summary>
        /// 获取产品分类分页列表
        /// </summary>
        /// <param name="requestDto">查询条件</param>
        /// <returns>产品分类分页结果</returns>
        Task<PagedResponseDto<ProductCategoryDto>> GetPagedListAsync(ProductCategoryRequestDto requestDto);

        /// <summary>
        /// 获取所有启用的产品分类
        /// </summary>
        /// <returns>产品分类列表</returns>
        Task<List<ProductCategoryDto>> GetAllActiveAsync();

        /// <summary>
        /// 根据ID获取产品分类
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <returns>产品分类DTO</returns>
        Task<ProductCategoryDto> GetByIdAsync(int id);

        /// <summary>
        /// 创建产品分类
        /// </summary>
        /// <param name="input">创建产品分类DTO</param>
        /// <returns>创建后的产品分类DTO</returns>
        Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input);

        /// <summary>
        /// 更新产品分类
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <param name="input">更新产品分类DTO</param>
        /// <returns>更新后的产品分类DTO</returns>
        Task<ProductCategoryDto> UpdateAsync(int id, CreateUpdateProductCategoryDto input);

        /// <summary>
        /// 删除产品分类
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 切换产品分类状态
        /// </summary>
        /// <param name="id">产品分类ID</param>
        /// <returns>更新后的产品分类DTO</returns>
        Task<ProductCategoryDto> ToggleStatusAsync(int id);
    }
} 