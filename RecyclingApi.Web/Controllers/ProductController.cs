using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.Services.Products;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 获取产品分页列表
        /// </summary>
        [HttpGet]
        public async Task<ApiResponse<PagedResponseDto<ProductDto>>> GetList([FromQuery] ProductRequestDto requestDto)
        {
            var result = await _productService.GetPagedListAsync(requestDto);
            return new ApiResponse<PagedResponseDto<ProductDto>>(result);
        }

        /// <summary>
        /// 根据ID获取产品
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ApiResponse<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return new ApiResponse<ProductDto>($"未找到ID为{id}的产品");

            return new ApiResponse<ProductDto>(product);
        }

        /// <summary>
        /// 创建产品
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductDto>> Create([FromBody] CreateUpdateProductDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<ProductDto>(ModelState.ToString());

            var result = await _productService.CreateAsync(input);
            return new ApiResponse<ProductDto>(result);
        }

        /// <summary>
        /// 更新产品
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductDto>> Update(int id, [FromBody] CreateUpdateProductDto input)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<ProductDto>(ModelState.ToString());

            try
            {
                var result = await _productService.UpdateAsync(id, input);
                return new ApiResponse<ProductDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<ProductDto>(ex.Message);
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
                return new ApiResponse<string>($"未找到ID为{id}的产品");

            return new ApiResponse<string>("删除成功");
        }

        /// <summary>
        /// 切换产品状态
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductDto>> ToggleStatus(int id)
        {
            try
            {
                var result = await _productService.ToggleStatusAsync(id);
                return new ApiResponse<ProductDto>(result);
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<ProductDto>(ex.Message);
            }
        }
    }
} 