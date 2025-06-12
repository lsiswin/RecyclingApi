using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Application.Services.Product;

/// <summary>
/// 产品服务接口
/// 定义产品相关的业务逻辑操作
/// </summary>
public interface IProductService
{
    /// <summary>
    /// 获取产品分页列表
    /// </summary>
    /// <param name="requestDto">查询条件</param>
    /// <returns>产品分页结果</returns>
    Task<PagedResult<ProductDto>> GetPagedListAsync(ProductRequestDto requestDto);

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品DTO</returns>
    Task<ProductDto> GetByIdAsync(int id);

    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="input">创建产品DTO</param>
    /// <returns>创建后的产品DTO</returns>
    Task<ProductDto> CreateAsync(CreateUpdateProductDto input);

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <param name="input">更新产品DTO</param>
    /// <returns>更新后的产品DTO</returns>
    Task<ProductDto> UpdateAsync(int id, CreateUpdateProductDto input);

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// 切换产品状态
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>更新后的产品DTO</returns>
    Task<ProductDto> ToggleStatusAsync(int id);

    /// <summary>
    /// 获取所有产品分类
    /// </summary>
    /// <returns>产品分类列表</returns>
    Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync();

    /// <summary>
    /// 根据ID获取产品详细信息
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品详细信息</returns>
    /// <exception cref="NotFoundException">当产品不存在时抛出</exception>
    Task<ProductDto> GetProductByIdAsync(int id);

    /// <summary>
    /// 根据分类ID获取该分类下的所有产品
    /// </summary>
    /// <param name="categoryId">分类ID</param>
    /// <returns>产品列表</returns>
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);

    /// <summary>
    /// 计算产品的估值
    /// 基于产品类型、年份、条件等因素计算回收价值
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品估值</returns>
    /// <exception cref="NotFoundException">当产品不存在时抛出</exception>
    Task<decimal> CalculateEstimatedValueAsync(int id);

    /// <summary>
    /// 分页查询案例列表
    /// </summary>
    /// <param name="request">查询请求参数</param>
    /// <returns>分页案例列表</returns>
    Task<PagedResult<CaseListDto>> GetCasesPagedAsync(CaseQueryRequest request);

    /// <summary>
    /// 根据ID获取案例详细信息
    /// </summary>
    /// <param name="id">案例ID</param>
    /// <returns>案例详细信息</returns>
    /// <exception cref="NotFoundException">当案例不存在时抛出</exception>
    Task<CaseDetailDto> GetCaseByIdAsync(int id);
}