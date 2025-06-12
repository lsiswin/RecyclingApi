using RecyclingApi.Domain.Entities.Products;
using RecyclingApi.Domain.Enums;

namespace RecyclingApi.Domain.Entities.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.ProductCategories.Any())
        {
            return; // 数据库已有数据
        }

        // 创建产品分类
        var categories = new[]
        {
            new ProductCategory
            {
                Name = "电脑设备",
                Description = "台式机、笔记本电脑等计算机设备"
            },
            new ProductCategory
            {
                Name = "移动设备",
                Description = "手机、平板电脑等移动设备"
            },
            new ProductCategory
            {
                Name = "网络设备",
                Description = "路由器、交换机、调制解调器等网络设备"
            },
            new ProductCategory
            {
                Name = "外设配件",
                Description = "键盘、鼠标、显示器等外围设备"
            },
            new ProductCategory
            {
                Name = "服务器设备",
                Description = "服务器、存储设备等企业级设备"
            }
        };

        context.ProductCategories.AddRange(categories);
        context.SaveChanges();

        // 创建示例产品
        var products = new[]
        {
            new Product
            {
                Name = "Dell OptiPlex 7090",
                Description = "Dell商用台式机，配置Intel i7处理器",
                Type = ProductType.Desktop,
                EstimatedValue = 3500.00m,
                Specifications = "Intel i7-11700, 16GB RAM, 512GB SSD",
                Brand = "Dell",
                Model = "OptiPlex 7090",
                ManufactureYear = 2021,
                Condition = "良好",
                CategoryId = 1,
                Price = 3000.00m,
                Specs = "i7/16GB/512GB",
                IsActive = true,
                PublishDate = DateTime.Now,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "iPhone 13 Pro",
                Description = "苹果iPhone 13 Pro智能手机",
                Type = ProductType.Mobile,
                EstimatedValue = 6800.00m,
                Specifications = "A15仿生芯片, 128GB存储, 6.1英寸屏幕",
                Brand = "Apple",
                Model = "iPhone 13 Pro",
                ManufactureYear = 2021,
                Condition = "优秀",
                CategoryId = 2,
                Price = 6000.00m,
                Specs = "A15/128GB",
                IsActive = true,
                PublishDate = DateTime.Now,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "ThinkPad X1 Carbon",
                Description = "联想ThinkPad商务笔记本",
                Type = ProductType.Laptop,
                EstimatedValue = 8500.00m,
                Specifications = "Intel i7-1165G7, 16GB RAM, 1TB SSD",
                Brand = "Lenovo",
                Model = "ThinkPad X1 Carbon Gen 9",
                ManufactureYear = 2021,
                Condition = "良好",
                CategoryId = 1,
                Price = 8000.00m,
                Specs = "i7/16GB/1TB",
                IsActive = true,
                PublishDate = DateTime.Now,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Cisco Catalyst 2960",
                Description = "思科企业级交换机",
                Type = ProductType.NetworkEquipment,
                EstimatedValue = 2800.00m,
                Specifications = "24端口千兆以太网交换机",
                Brand = "Cisco",
                Model = "Catalyst 2960-24TT-L",
                ManufactureYear = 2020,
                Condition = "良好",
                CategoryId = 3,
                Price = 2500.00m,
                Specs = "24端口/千兆",
                IsActive = true,
                PublishDate = DateTime.Now,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Dell UltraSharp U2720Q",
                Description = "Dell 27英寸4K显示器",
                Type = ProductType.Monitor,
                EstimatedValue = 2200.00m,
                Specifications = "27英寸, 4K分辨率, IPS面板",
                Brand = "Dell",
                Model = "UltraSharp U2720Q",
                ManufactureYear = 2020,
                Condition = "优秀",
                CategoryId = 4,
                Price = 2000.00m,
                Specs = "27英寸/4K/IPS",
                IsActive = true,
                PublishDate = DateTime.Now,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();

        // 为每个产品创建处理步骤
        var processSteps = new List<ProcessStep>();
        foreach (var product in products)
        {
            var steps = new[]
            {
                new ProcessStep
                {
                    Name = "设备接收",
                    Description = "接收并登记设备信息",
                    Order = 1,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow.AddDays(-5),
                    CompletedBy = "系统管理员",
                    ProductId = product.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new ProcessStep
                {
                    Name = "设备检测",
                    Description = "检测设备功能和性能",
                    Order = 2,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow.AddDays(-4),
                    CompletedBy = "技术员",
                    ProductId = product.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new ProcessStep
                {
                    Name = "数据清除",
                    Description = "安全清除设备中的数据",
                    Order = 3,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow.AddDays(-3),
                    CompletedBy = "数据安全专员",
                    ProductId = product.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new ProcessStep
                {
                    Name = "价值评估",
                    Description = "评估设备的回收价值",
                    Order = 4,
                    IsCompleted = false,
                    ProductId = product.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new ProcessStep
                {
                    Name = "设备处理",
                    Description = "根据评估结果处理设备",
                    Order = 5,
                    IsCompleted = false,
                    ProductId = product.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };
            processSteps.AddRange(steps);
        }

        context.ProcessSteps.AddRange(processSteps);
        context.SaveChanges();
    }
}