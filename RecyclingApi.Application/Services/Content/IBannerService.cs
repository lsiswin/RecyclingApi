using System.Collections.Generic;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ContentDTOs;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 轮播图服务接口
    /// 提供网站首页轮播图的CRUD操作和状态管理
    /// </summary>
    public interface IBannerService
    {
        /// <summary>
        /// 获取轮播图分页列表
        /// </summary>
        /// <param name="requestDto">查询条件，包含分页、排序和筛选参数</param>
        /// <returns>轮播图分页结果</returns>
        Task<PagedResponseDto<BannerDto>> GetPagedListAsync(BannerRequestDto requestDto);

        /// <summary>
        /// 获取所有轮播图列表
        /// </summary>
        /// <returns>轮播图列表</returns>
        Task<List<BannerDto>> GetAllAsync();

        /// <summary>
        /// 获取所有激活的轮播图列表
        /// </summary>
        /// <returns>激活的轮播图列表</returns>
        Task<List<BannerDto>> GetActiveAsync();

        /// <summary>
        /// 根据ID获取轮播图
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <returns>轮播图DTO，如果不存在则返回null</returns>
        Task<BannerDto> GetByIdAsync(int id);

        /// <summary>
        /// 创建轮播图
        /// </summary>
        /// <param name="input">创建轮播图DTO，包含标题、图片地址等信息</param>
        /// <returns>创建后的轮播图DTO</returns>
        Task<BannerDto> CreateAsync(CreateUpdateBannerDto input);

        /// <summary>
        /// 更新轮播图
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <param name="input">更新轮播图DTO，包含更新的字段信息</param>
        /// <returns>更新后的轮播图DTO</returns>
        /// <exception cref="System.Exception">当找不到指定ID的轮播图时抛出异常</exception>
        Task<BannerDto> UpdateAsync(int id, CreateUpdateBannerDto input);

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <returns>如果成功删除返回true，否则返回false</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 切换轮播图状态（启用/禁用）
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <returns>更新后的轮播图DTO</returns>
        /// <exception cref="System.Exception">当找不到指定ID的轮播图时抛出异常</exception>
        Task<BannerDto> ToggleStatusAsync(int id);
    }
} 