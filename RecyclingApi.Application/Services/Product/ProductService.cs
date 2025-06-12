using AutoMapper;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.Common.Exceptions;
using RecyclingApi.Domain.Enums;
using RecyclingApi.Domain.Entities.Data.Repositories;

namespace RecyclingApi.Application.Services.Product;

/// <summary>
/// 产品服务实现类
/// 实现产品相关的业务逻辑操作
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数，注入依赖项
    /// </summary>
    /// <param name="productRepository">产品仓储接口</param>
    /// <param name="mapper">对象映射器</param>
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// 获取产品分页列表
    /// </summary>
    /// <param name="requestDto">查询条件</param>
    /// <returns>产品分页结果</returns>
    public async Task<PagedResult<ProductDto>> GetPagedListAsync(ProductRequestDto requestDto)
    {
        var result = await _productRepository.GetPagedListAsync(
            requestDto.PageIndex,
            requestDto.PageSize,
            requestDto.Keyword,
            requestDto.CategoryId,
            requestDto.IsDesc);

        return new PagedResult<ProductDto>
        {
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages,
            Items = _mapper.Map<List<ProductDto>>(result.Items)
        };
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品DTO</returns>
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException($"产品ID {id} 未找到");
        }

        return _mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="input">创建产品DTO</param>
    /// <returns>创建后的产品DTO</returns>
    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var product = _mapper.Map<Domain.Entities.Products.Product>(input);
        product.CreatedAt = DateTime.UtcNow;
        product.IsActive = true;

        var createdProduct = await _productRepository.CreateAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <param name="input">更新产品DTO</param>
    /// <returns>更新后的产品DTO</returns>
    public async Task<ProductDto> UpdateAsync(int id, CreateUpdateProductDto input)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new NotFoundException($"产品ID {id} 未找到");
        }

        _mapper.Map(input, existingProduct);
        existingProduct.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>是否删除成功</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var exists = await _productRepository.ExistsAsync(id);
        if (!exists)
        {
            throw new NotFoundException($"产品ID {id} 未找到");
        }

        await _productRepository.DeleteAsync(id);
        return true;
    }

    /// <summary>
    /// 切换产品状态
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>更新后的产品DTO</returns>
    public async Task<ProductDto> ToggleStatusAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException($"产品ID {id} 未找到");
        }

        product.IsActive = !product.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(product);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    /// <summary>
    /// 获取所有产品分类
    /// 包含每个分类下的产品数量统计
    /// </summary>
    /// <returns>产品分类列表</returns>
    public async Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _productRepository.GetAllCategoriesAsync();
        return _mapper.Map<IEnumerable<ProductCategoryDto>>(categories);
    }

    /// <summary>
    /// 根据ID获取产品详细信息
    /// 包含产品的所有属性、分类信息和处理步骤
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品详细信息</returns>
    /// <exception cref="NotFoundException">当产品不存在时抛出</exception>
    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException($"产品ID {id} 未找到");
        }

        return _mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 根据分类ID获取该分类下的所有产品
    /// </summary>
    /// <param name="categoryId">分类ID</param>
    /// <returns>产品列表</returns>
    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _productRepository.GetByCategoryIdAsync(categoryId);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    /// <summary>
    /// 计算产品的估值
    /// 基于产品类型、制造年份、条件等因素计算回收价值
    /// 使用折旧算法和条件系数来确定最终估值
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品估值</returns>
    /// <exception cref="NotFoundException">当产品不存在时抛出</exception>
    public async Task<decimal> CalculateEstimatedValueAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException($"产品ID {id} 未找到");
        }

        // 基础价值（根据产品类型设定）
        decimal baseValue = GetBaseValueByType(product.Type);

        // 年份折旧系数（每年折旧10%，最低保留20%价值）
        int currentYear = DateTime.Now.Year;
        int age = currentYear - product.ManufactureYear;
        decimal depreciationRate = Math.Max(0.2m, 1 - age * 0.1m);

        // 条件系数
        decimal conditionMultiplier = GetConditionMultiplier(product.Condition);

        // 品牌系数
        decimal brandMultiplier = GetBrandMultiplier(product.Brand);

        // 最终估值计算
        decimal estimatedValue = baseValue * depreciationRate * conditionMultiplier * brandMultiplier;

        return Math.Round(estimatedValue, 2);
    }

    /// <summary>
    /// 根据产品类型获取基础价值
    /// </summary>
    /// <param name="productType">产品类型</param>
    /// <returns>基础价值</returns>
    private static decimal GetBaseValueByType(ProductType productType)
    {
        return productType switch
        {
            ProductType.Desktop => 3000m,
            ProductType.Laptop => 5000m,
            ProductType.Mobile => 4000m,
            ProductType.Tablet => 2500m,
            ProductType.Server => 15000m,
            ProductType.NetworkEquipment => 3500m,
            ProductType.Monitor => 1500m,
            ProductType.Printer => 1000m,
            _ => 1000m
        };
    }

    /// <summary>
    /// 根据设备条件获取条件系数
    /// </summary>
    /// <param name="condition">设备条件</param>
    /// <returns>条件系数</returns>
    private static decimal GetConditionMultiplier(string? condition)
    {
        return condition?.ToLower() switch
        {
            "优秀" or "excellent" => 1.0m,
            "良好" or "good" => 0.8m,
            "一般" or "fair" => 0.6m,
            "较差" or "poor" => 0.4m,
            "损坏" or "damaged" => 0.2m,
            _ => 0.7m // 默认值
        };
    }

    /// <summary>
    /// 根据品牌获取品牌系数
    /// </summary>
    /// <param name="brand">品牌名称</param>
    /// <returns>品牌系数</returns>
    private static decimal GetBrandMultiplier(string? brand)
    {
        if (string.IsNullOrEmpty(brand))
            return 1.0m;

        return brand.ToLower() switch
        {
            "apple" => 1.3m,
            "dell" => 1.1m,
            "hp" => 1.05m,
            "lenovo" => 1.1m,
            "asus" => 1.05m,
            "acer" => 0.95m,
            _ => 1.0m
        };
    }

    /// <summary>
    /// 分页查询案例列表
    /// </summary>
    /// <param name="request">查询请求参数</param>
    /// <returns>分页案例列表</returns>
    public async Task<PagedResult<CaseListDto>> GetCasesPagedAsync(CaseQueryRequest request)
    {
        // 模拟数据，实际应该从数据库查询
        var allCases = GetMockCases();

        // 应用筛选条件
        var filteredCases = allCases.AsQueryable();

        if (!string.IsNullOrEmpty(request.Category))
        {
            filteredCases = filteredCases.Where(c => c.Category == request.Category);
        }

        if (!string.IsNullOrEmpty(request.DeviceType))
        {
            filteredCases = filteredCases.Where(c => c.DeviceType == request.DeviceType);
        }

        if (!string.IsNullOrEmpty(request.Scale))
        {
            filteredCases = filteredCases.Where(c => c.Scale == request.Scale);
        }

        var totalCount = filteredCases.Count();
        var items = filteredCases
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<CaseListDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    /// <summary>
    /// 根据ID获取案例详细信息
    /// </summary>
    /// <param name="id">案例ID</param>
    /// <returns>案例详细信息</returns>
    /// <exception cref="NotFoundException">当案例不存在时抛出</exception>
    public async Task<CaseDetailDto> GetCaseByIdAsync(int id)
    {
        var cases = GetMockCaseDetails();
        var caseDetail = cases.FirstOrDefault(c => c.Id == id);

        if (caseDetail == null)
        {
            throw new NotFoundException($"案例ID {id} 未找到");
        }

        return caseDetail;
    }

    /// <summary>
    /// 获取模拟案例数据
    /// </summary>
    /// <returns>案例列表</returns>
    private static List<CaseListDto> GetMockCases()
    {
        return new List<CaseListDto>
        {
            new() { Id = 1, Title = "某大型科技公司IT设备更新回收", Description = "为某知名科技公司提供全面的IT设备回收服务，包括1000+台电脑设备的安全回收处理", Client = "某知名科技公司", Category = "enterprise", DeviceType = "desktop", DeviceCount = 1200, Date = "2024-01-15", Scale = "large", Image = "/images/case1.jpg", Tags = new List<string> { "数据安全", "环保处理", "批量回收" }, Views = 1580 },
            new() { Id = 2, Title = "某重点大学实验室设备回收", Description = "协助某重点大学处理实验室淘汰的计算机设备，确保学术数据安全销毁", Client = "某重点大学", Category = "school", DeviceType = "laptop", DeviceCount = 300, Date = "2024-02-20", Scale = "medium", Image = "/images/case2.jpg", Tags = new List<string> { "学术数据保护", "实验室设备", "教育机构" }, Views = 890 },
            new() { Id = 3, Title = "政府机关办公设备统一回收", Description = "为某市政府机关提供办公设备回收服务，严格按照政府采购和处置规定执行", Client = "某市政府机关", Category = "government", DeviceType = "desktop", DeviceCount = 500, Date = "2024-03-10", Scale = "large", Image = "/images/case3.jpg", Tags = new List<string> { "政府采购", "合规处理", "资产处置" }, Views = 1200 },
            new() { Id = 4, Title = "某三甲医院信息系统设备更新", Description = "为某三甲医院提供信息系统设备回收，确保患者隐私数据安全", Client = "某三甲医院", Category = "hospital", DeviceType = "server", DeviceCount = 150, Date = "2024-04-05", Scale = "medium", Image = "/images/case4.jpg", Tags = new List<string> { "医疗数据安全", "服务器回收", "隐私保护" }, Views = 750 },
            new() { Id = 5, Title = "某制造企业生产线设备回收", Description = "协助某制造企业处理生产线淘汰的工控设备和办公电脑", Client = "某制造企业", Category = "enterprise", DeviceType = "network", DeviceCount = 80, Date = "2024-05-12", Scale = "small", Image = "/images/case5.jpg", Tags = new List<string> { "工控设备", "生产线改造", "工业回收" }, Views = 620 },
            new() { Id = 6, Title = "某金融机构数据中心设备回收", Description = "为某银行数据中心提供服务器设备回收，采用银行级数据安全标准", Client = "某银行", Category = "enterprise", DeviceType = "server", DeviceCount = 200, Date = "2024-06-08", Scale = "medium", Image = "/images/case6.jpg", Tags = new List<string> { "金融数据安全", "数据中心", "银行级标准" }, Views = 980 }
        };
    }

    /// <summary>
    /// 获取模拟案例详细数据
    /// </summary>
    /// <returns>案例详细列表</returns>
    private static List<CaseDetailDto> GetMockCaseDetails()
    {
        return new List<CaseDetailDto>
        {
            new() { Id = 1, Title = "某大型科技公司IT设备更新回收", Description = "为某知名科技公司提供全面的IT设备回收服务，包括1000+台电脑设备的安全回收处理", FullDescription = "该项目是我们承接的大型企业IT设备回收项目之一，客户因办公设备更新换代，需要对旧设备进行环保回收处理。", Client = "某知名科技公司", Category = "enterprise", DeviceType = "desktop", DeviceCount = 1200, Date = "2024-01-15", Duration = "7天", Scale = "large", Image = "/images/case1.jpg", Tags = new List<string> { "数据安全", "环保处理", "批量回收" }, Views = 1580, Rating = 5.0, ProjectDetails = "项目涉及台式机800台、笔记本电脑300台、服务器100台的回收处理。我们提供了专业的数据销毁服务，确保企业信息安全，同时按照环保标准进行设备拆解和材料回收。", Highlights = new List<string> { "专业数据销毁，零信息泄露风险", "7天内完成1200台设备回收", "100%环保处理，获得环保认证", "为客户节省处理成本30万元" } },
            new() { Id = 2, Title = "某重点大学实验室设备回收", Description = "协助某重点大学处理实验室淘汰的计算机设备，确保学术数据安全销毁", FullDescription = "该项目为某重点大学计算机学院实验室设备更新项目，涉及多个实验室的设备回收。", Client = "某重点大学", Category = "school", DeviceType = "laptop", DeviceCount = 300, Date = "2024-02-20", Duration = "5天", Scale = "medium", Image = "/images/case2.jpg", Tags = new List<string> { "学术数据保护", "实验室设备", "教育机构" }, Views = 890, Rating = 4.8, ProjectDetails = "项目包括实验室台式机200台、笔记本100台的回收。特别注意学术研究数据的安全处理，采用军用级数据销毁标准。", Highlights = new List<string> { "军用级数据销毁标准", "配合学校假期时间安排", "提供设备处理证明文件", "部分设备捐赠给贫困地区学校" } }
        };
    }
}