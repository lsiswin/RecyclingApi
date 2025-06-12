using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.Common.Exceptions;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Application.Services.Product;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Web.Controllers;

/// <summary>
/// 产品管理控制器
/// 提供产品的增删改查、分类管理、价值评估等功能
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    /// <summary>
    /// 构造函数，注入产品服务
    /// </summary>
    /// <param name="productService">产品服务接口</param>
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// 分页查询产品列表
    /// </summary>
    /// <param name="request">查询请求参数</param>
    /// <returns>分页产品列表</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse<PagedResult<ProductDto>>> GetProducts([FromQuery] ProductRequestDto request)
    {
        var result = await _productService.GetPagedListAsync(request);
        return new ApiResponse<PagedResult<ProductDto>>(result, "获取产品列表成功");
    }

    /// <summary>
    /// 获取所有产品分类
    /// </summary>
    /// <returns>产品分类列表</returns>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductCategoryDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse<IEnumerable<ProductCategoryDto>>> GetCategories()
    {
        var categories = await _productService.GetAllCategoriesAsync();
        return new ApiResponse<IEnumerable<ProductCategoryDto>>(categories, "获取产品分类成功");
    }

    /// <summary>
    /// 根据ID获取产品详细信息
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品详细信息</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ApiResponse<ProductDto>> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return new ApiResponse<ProductDto>(product, "获取产品详情成功");
        }
        catch (NotFoundException)
        {
            return new ApiResponse<ProductDto>("产品未找到");
        }
    }

    /// <summary>
    /// 根据分类ID获取该分类下的所有产品
    /// </summary>
    /// <param name="categoryId">分类ID</param>
    /// <returns>产品列表</returns>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        return new ApiResponse<IEnumerable<ProductDto>>(products, "获取分类产品成功");
    }

    /// <summary>
    /// 创建新产品
    /// </summary>
    /// <param name="createProductDto">创建产品的数据传输对象</param>
    /// <returns>创建的产品信息</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse<ProductDto>> CreateProduct([FromBody] CreateUpdateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
            return new ApiResponse<ProductDto>("请求参数无效");

        var product = await _productService.CreateAsync(createProductDto);
        return new ApiResponse<ProductDto>(product, "创建产品成功");
    }

    /// <summary>
    /// 更新产品信息
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <param name="updateProductDto">更新产品的数据传输对象</param>
    /// <returns>更新后的产品信息</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ApiResponse<ProductDto>> UpdateProduct(int id, [FromBody] CreateUpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
            return new ApiResponse<ProductDto>("请求参数无效");

        try
        {
            var product = await _productService.UpdateAsync(id, updateProductDto);
            return new ApiResponse<ProductDto>(product, "更新产品成功");
        }
        catch (NotFoundException)
        {
            return new ApiResponse<ProductDto>("产品未找到");
        }
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ApiResponse<bool>> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteAsync(id);
            return new ApiResponse<bool>(true, "删除产品成功");
        }
        catch (NotFoundException)
        {
            return new ApiResponse<bool>("产品未找到");
        }
    }

    /// <summary>
    /// 获取产品的估值
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品估值</returns>
    [HttpGet("{id}/estimate")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ApiResponse<decimal>> GetEstimatedValue(int id)
    {
        try
        {
            var estimatedValue = await _productService.CalculateEstimatedValueAsync(id);
            return new ApiResponse<decimal>(estimatedValue, "获取产品估值成功");
        }
        catch (NotFoundException)
        {
            return new ApiResponse<decimal>("产品未找到");
        }
    }

    /// <summary>
    /// 获取案例列表
    /// </summary>
    /// <param name="request">查询请求参数</param>
    /// <returns>分页案例列表</returns>
    [HttpGet("cases")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CaseListDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse<PagedResult<CaseListDto>>> GetCases([FromQuery] CaseQueryRequest request)
    {
        var result = await _productService.GetCasesPagedAsync(request);
        return new ApiResponse<PagedResult<CaseListDto>>(result, "获取案例列表成功");
    }

    /// <summary>
    /// 根据ID获取案例详细信息
    /// </summary>
    /// <param name="id">案例ID</param>
    /// <returns>案例详细信息</returns>
    [HttpGet("cases/{id}")]
    [ProducesResponseType(typeof(ApiResponse<CaseDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ApiResponse<CaseDetailDto>> GetCase(int id)
    {
        try
        {
            var caseDetail = await _productService.GetCaseByIdAsync(id);
            return new ApiResponse<CaseDetailDto>(caseDetail, "获取案例详情成功");
        }
        catch (NotFoundException)
        {
            return new ApiResponse<CaseDetailDto>("案例未找到");
        }
    }
}
