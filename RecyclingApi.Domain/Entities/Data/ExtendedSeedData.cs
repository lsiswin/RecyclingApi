namespace RecyclingApi.Domain.Entities.Data;

/// <summary>
/// 扩展数据种子类
/// </summary>
public static class ExtendedSeedData
{
    /// <summary>
    /// 初始化所有数据
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public static void Initialize(ApplicationDbContext context)
    {
        // 初始化产品相关数据
        SeedData.Initialize(context);

        // 初始化内容相关数据
        ContentSeedData.Initialize(context);

        // 初始化HR相关数据
        HRSeedData.Initialize(context);
    }
} 