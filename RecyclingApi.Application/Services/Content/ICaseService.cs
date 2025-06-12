using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;

namespace RecyclingApi.Application.Services.Content
{
    /// <summary>
    /// 案例服务接口
    /// </summary>
    public interface ICaseService
    {
        /// <summary>
        /// 获取案例分页列表
        /// </summary>
        /// <param name="requestDto">查询条件</param>
        /// <returns>案例分页结果</returns>
        Task<PagedResponseDto<CaseDto>> GetPagedListAsync(CaseRequestDto requestDto);

        /// <summary>
        /// 根据ID获取案例
        /// </summary>
        /// <param name="id">案例ID</param>
        /// <returns>案例DTO</returns>
        Task<CaseDto> GetByIdAsync(int id);

        /// <summary>
        /// 创建案例
        /// </summary>
        /// <param name="input">创建案例DTO</param>
        /// <returns>创建后的案例DTO</returns>
        Task<CaseDto> CreateAsync(CreateUpdateCaseDto input);

        /// <summary>
        /// 更新案例
        /// </summary>
        /// <param name="id">案例ID</param>
        /// <param name="input">更新案例DTO</param>
        /// <returns>更新后的案例DTO</returns>
        Task<CaseDto> UpdateAsync(int id, CreateUpdateCaseDto input);

        /// <summary>
        /// 删除案例
        /// </summary>
        /// <param name="id">案例ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 切换案例状态
        /// </summary>
        /// <param name="id">案例ID</param>
        /// <returns>更新后的案例DTO</returns>
        Task<CaseDto> ToggleStatusAsync(int id);

        /// <summary>
        /// 增加案例浏览次数
        /// </summary>
        /// <param name="id">案例ID</param>
        /// <returns>更新后的浏览次数</returns>
        Task<int> IncrementViewsAsync(int id);
    }
} 