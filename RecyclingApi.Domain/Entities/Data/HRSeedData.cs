using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Domain.Entities.Data;

/// <summary>
/// HR相关数据种子
/// </summary>
public static class HRSeedData
{
    /// <summary>
    /// 初始化HR相关数据
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public static void Initialize(ApplicationDbContext context)
    {
        // 初始化员工数据
        InitializeEmployees(context);
    }

    /// <summary>
    /// 初始化员工数据
    /// </summary>
    private static void InitializeEmployees(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.Employees.Any())
        {
            return; // 数据库已有数据
        }

        var departments = new string[] { "技术部", "销售部", "运营部", "行政部", "财务部" };
        var positions = new string[] { "总监", "经理", "主管", "专员", "助理" };
        
        var employees = new List<Employee>();
        
        for (int i = 1; i <= 11; i++)
        {
            var department = departments[i % departments.Length];
            var position = positions[i % positions.Length];
            
            employees.Add(new Employee
            {
                Name = $"员工{i}",
                EmployeeNo = $"EMP{10000 + i}",
                Position = $"{department}{position}",
                Department = department,
                Phone = $"1381234{56 + i:D2}",
                Email = $"employee{i}@greencycle.com",
                AvatarUrl = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Introduction = $"员工{i}简介，{department}{position}，负责{department}的{GetResponsibility(department)}工作。拥有{3 + i % 5}年相关工作经验。",
                HireDate = DateTime.Now.AddYears(-(i % 5)).AddMonths(-(i % 12)),
                IsActive = true,
                CreatedAt = DateTime.Now.AddMonths(-i)
            });
        }

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }

    /// <summary>
    /// 根据部门获取职责描述
    /// </summary>
    private static string GetResponsibility(string department)
    {
        return department switch
        {
            "技术部" => "技术研发和项目实施",
            "销售部" => "市场开拓和客户维护",
            "运营部" => "日常运营和服务保障",
            "行政部" => "行政事务和人力资源",
            "财务部" => "财务管理和资金规划",
            _ => "相关业务"
        };
    }
} 