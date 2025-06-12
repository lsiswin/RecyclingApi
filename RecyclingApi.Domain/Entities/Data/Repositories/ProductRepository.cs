using Microsoft.EntityFrameworkCore;
using RecyclingApi.Domain.Entities.Products;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Domain.Entities.Data.Repositories;

/// <summary>
/// 产品仓储实现类
/// 实现产品数据访问操作
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// 构造函数，注入数据库上下文
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取分页产品列表
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="keyword">关键词</param>
    /// <param name="categoryId">分类ID</param>
    /// <param name="orderBy">排序方式</param>
    /// <returns>分页结果</returns>
    public async Task<PagedResult<Product>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, int? categoryId, bool orderBy)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        // 应用筛选条件
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(p =>
                p.Name.Contains(keyword) ||
                p.Description.Contains(keyword) ||
                p.Model.Contains(keyword) ||
                p.Brand.Contains(keyword));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // 应用排序
        query = ApplySorting(query, orderBy);

        // 获取总记录数
        var totalCount = await query.CountAsync();

        // 执行分页查询
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 构造分页结果
        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            Page = pageIndex,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
    }

    /// <summary>
    /// 应用排序
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="orderBy">排序方式</param>
    /// <returns>应用排序后的查询</returns>
    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, bool orderBy)
    {
        return orderBy switch
        {
            true => query.OrderBy(p => p.Id),
            false => query.OrderByDescending(p => p.Id),
        };
    }

    /// <summary>
    /// 获取所有产品分类
    /// 使用Include加载关联的产品数据，AsNoTracking提高查询性能
    /// </summary>
    /// <returns>产品分类列表</returns>
    public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
    {
        return await _context.ProductCategories
            .Include(c => c.Products)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 根据ID获取产品
    /// 包含关联的分类和处理步骤信息
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品实体，如果不存在则返回null</returns>
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProcessSteps)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// 根据分类ID获取该分类下的所有产品
    /// 包含关联的分类和处理步骤信息，使用AsNoTracking提高查询性能
    /// </summary>
    /// <param name="categoryId">分类ID</param>
    /// <returns>产品列表</returns>
    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProcessSteps)
            .Where(p => p.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 创建新产品
    /// 添加到数据库并保存更改
    /// </summary>
    /// <param name="product">产品实体</param>
    /// <returns>创建的产品实体</returns>
    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// 更新产品信息
    /// 标记实体为已修改状态并保存更改
    /// </summary>
    /// <param name="product">产品实体</param>
    /// <returns>更新后的产品实体</returns>
    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// 删除产品
    /// 先查找产品，如果存在则删除并保存更改
    /// 由于配置了级联删除，相关的处理步骤也会被自动删除
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>异步操作任务</returns>
    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 检查产品是否存在
    /// 使用Any方法进行高效的存在性检查
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>如果存在返回true，否则返回false</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}