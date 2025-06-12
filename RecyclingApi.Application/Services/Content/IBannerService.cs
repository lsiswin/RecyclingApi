using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.ContentDTOs;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 轮播图服务接口
    /// </summary>
    public interface IBannerService
    {
        /// <summary>
        /// 获取轮播图分页列表
        /// </summary>
        /// <param name="requestDto">查询条件</param>
        /// <returns>轮播图分页结果</returns>
        Task<PagedResponseDto<BannerDto>> GetPagedListAsync(BannerRequestDto requestDto);

        /// <summary>
        /// 根据ID获取轮播图
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <returns>轮播图DTO</returns>
        Task<BannerDto> GetByIdAsync(int id);

        /// <summary>
        /// 创建轮播图
        /// </summary>
        /// <param name="input">创建轮播图DTO</param>
        /// <returns>创建后的轮播图DTO</returns>
        Task<BannerDto> CreateAsync(CreateUpdateBannerDto input);

        /// <summary>
        /// 更新轮播图
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <param name="input">更新轮播图DTO</param>
        /// <returns>更新后的轮播图DTO</returns>
        Task<BannerDto> UpdateAsync(int id, CreateUpdateBannerDto input);

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 切换轮播图状态
        /// </summary>
        /// <param name="id">轮播图ID</param>
        /// <returns>更新后的轮播图DTO</returns>
        Task<BannerDto> ToggleStatusAsync(int id);
    }
} 