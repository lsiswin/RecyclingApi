using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.Services.Product;

namespace RecyclingApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        /// <summary>
        /// 获取产品分类分页列表
        /// </summary>
        [HttpGet]
        public async Task<ApiResponse<PagedResponseDto<ProductCategoryDto>>> GetList([FromQuery] ProductCategoryRequestDto requestDto)
        {
            var result = await _productCategoryService.GetPagedListAsync(requestDto);
            return new ApiResponse<PagedResponseDto<ProductCategoryDto>>(result);
        }

        /// <summary>
        /// 获取所有启用的产品分类
        /// </summary>
        [HttpGet("active")]
        public async Task<ApiResponse<List<ProductCategoryDto>>> GetAllActive()
        {
            var categories = await _productCategoryService.GetAllActiveAsync();
            return new ApiResponse<List<ProductCategoryDto>>(categories);
        }

        /// <summary>
        /// 根据ID获取产品分类
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ApiResponse<ProductCategoryDto>> GetById(int id)
        {
            var category = await _productCategoryService.GetByIdAsync(id);
            if (category == null)
                return new ApiResponse<ProductCategoryDto>($"未找到ID为{id}的产品分类");

            return new ApiResponse<ProductCategoryDto>(category);
        }

        /// <summary>
        /// 创建产品分类
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductCategoryDto>> Create([FromBody] CreateUpdateProductCategoryDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<ProductCategoryDto>(ModelState.ToString());

            var result = await _productCategoryService.CreateAsync(input);
            return new ApiResponse<ProductCategoryDto>(result);
        }

        /// <summary>
        /// 更新产品分类
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductCategoryDto>> Update(int id, [FromBody] CreateUpdateProductCategoryDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<ProductCategoryDto>(ModelState.ToString());

            try
            {
                var result = await _productCategoryService.UpdateAsync(id, input);
                return new ApiResponse<ProductCategoryDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<ProductCategoryDto>(ex.Message);
            }
        }

        /// <summary>
        /// 删除产品分类
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var result = await _productCategoryService.DeleteAsync(id);
            if (!result)
                return new ApiResponse<string>($"未找到ID为{id}的产品分类或该分类下存在产品，无法删除");

            return new ApiResponse<string>("删除成功");
        }

        /// <summary>
        /// 切换产品分类状态
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<ProductCategoryDto>> ToggleStatus(int id)
        {
            try
            {
                var result = await _productCategoryService.ToggleStatusAsync(id);
                return new ApiResponse<ProductCategoryDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<ProductCategoryDto>(ex.Message);
            }
        }
    }
} 