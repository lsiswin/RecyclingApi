using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyclingApi.Application.Common.Responses;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Application.Services.Content;

namespace RecyclingApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyMemberController : ControllerBase
    {
        private readonly ICompanyInfoService _companyInfoService;

        public CompanyMemberController(ICompanyInfoService companyInfoService)
        {
            _companyInfoService = companyInfoService;
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <returns>公司信息</returns>
        [HttpGet]
        public async Task<ApiResponse<CompanyInfoDto>> GetCompanyProfile()
        {
            var result = await _companyInfoService.GetCompanyInfoAsync();
            return new ApiResponse<CompanyInfoDto>(result);
        }

        /// <summary>
        /// 创建或更新公司信息
        /// </summary>
        /// <param name="dto">公司信息DTO</param>
        /// <returns>更新后的公司信息</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<CompanyInfoDto>> CreateOrUpdateCompanyProfile(
            [FromBody] UpdateCompanyInfoDto dto)
        {
            var result = await _companyInfoService.UpdateCompanyInfoAsync(dto);
            return new ApiResponse<CompanyInfoDto>(result, "公司信息更新成功");
        }
        


        
    }
} 