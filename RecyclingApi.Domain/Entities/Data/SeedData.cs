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
        var categories = new ProductCategory[]
        {
            new ProductCategory
            {
                Name = "台式电脑",
                Icon = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Code = "desktop",
                Description = "各类品牌台式电脑及一体机",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductCategory
            {
                Name = "手机",
                Icon = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Code = "mobile",
                Description = "智能手机及功能机",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductCategory
            {
                Name = "网络设备",
                Icon = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Code = "network",
                Description = "交换机、路由器、防火墙等网络设备",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductCategory
            {
                Name = "显示器",
                Icon = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Code = "monitor",
                Description = "液晶显示器、LED显示器等",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductCategory
            {
                Name = "笔记本电脑",
                Icon = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Code = "laptop",
                Description = "各类品牌笔记本电脑",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductCategory
            {
                Name = "服务器",
                Icon = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Code = "server",
                Description = "机架式、塔式等服务器设备",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
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
                DetailSpecs = "Intel i7-11700, 16GB RAM, 512GB SSD",
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
                DetailSpecs = "Intel i7-11700, 16GB RAM, 512GB SSD",
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
                DetailSpecs = "Intel i7-11700, 16GB RAM, 512GB SSD",
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
                DetailSpecs = "Intel i7-11700, 16GB RAM, 512GB SSD",
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
                DetailSpecs = "Intel i7-11700, 16GB RAM, 512GB SSD",
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