using RecyclingApi.Domain.Entities.Products;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Domain.Entities.Data.Repositories;

/// <summary>
/// 产品仓储接口
/// 定义产品数据访问操作
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// 获取分页产品列表
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="keyword">关键词</param>
    /// <param name="categoryId">分类ID</param>
    /// <param name="orderBy">排序方式</param>
    /// <returns>分页结果</returns>
    Task<PagedResult<Product>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, int? categoryId, bool orderBy);

    /// <summary>
    /// 获取所有产品分类
    /// 包含每个分类下的产品列表
    /// </summary>
    /// <returns>产品分类列表</returns>
    Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();

    /// <summary>
    /// 根据ID获取产品
    /// 包含关联的分类和处理步骤信息
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品实体，如果不存在则返回null</returns>
    Task<Product?> GetByIdAsync(int id);

    /// <summary>
    /// 根据分类ID获取该分类下的所有产品
    /// 包含关联的分类和处理步骤信息
    /// </summary>
    /// <param name="categoryId">分类ID</param>
    /// <returns>产品列表</returns>
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);

    /// <summary>
    /// 创建新产品
    /// </summary>
    /// <param name="product">产品实体</param>
    /// <returns>创建的产品实体</returns>
    Task<Product> CreateAsync(Product product);

    /// <summary>
    /// 更新产品信息
    /// </summary>
    /// <param name="product">产品实体</param>
    /// <returns>更新后的产品实体</returns>
    Task<Product> UpdateAsync(Product product);

    /// <summary>
    /// 删除产品
    /// 同时删除相关的处理步骤记录
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>异步操作任务</returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// 检查产品是否存在
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>如果存在返回true，否则返回false</returns>
    Task<bool> ExistsAsync(int id);
}