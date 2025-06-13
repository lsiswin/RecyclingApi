using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Domain.Entities.Data;

/// <summary>
/// 内容相关数据种子
/// </summary>
public static class ContentSeedData
{
    /// <summary>
    /// 初始化内容相关数据
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public static void Initialize(ApplicationDbContext context)
    {
        // 初始化轮播图数据
        InitializeBanners(context);

        // 初始化案例数据
        InitializeCases(context);

        // 初始化公司信息数据
        InitializeCompanyInfo(context);

        // 初始化公司优势数据
        InitializeCompanyAdvantages(context);

        // 初始化公司发展历程数据
        InitializeCompanyMilestones(context);

        // 初始化公司展示信息数据
        InitializeCompanyProfile(context);

        // 初始化团队成员类型数据
        InitializeTeamMemberTypes(context);

        // 初始化团队成员数据
        InitializeTeamMembers(context);
    }

    /// <summary>
    /// 初始化轮播图数据
    /// </summary>
    private static void InitializeBanners(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.Banners.Any())
        {
            return; // 数据库已有数据
        }

        var banners = new List<Banner>();
        
        for (int i = 1; i <= 11; i++)
        {
            banners.Add(new Banner
            {
                Title = $"轮播图标题 {i}",
                Description = $"轮播图描述内容 {i}，展示公司的产品、服务或活动信息。",
                ImageUrl = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                LinkUrl = $"/page/details/{i}",
                Sort = i,
                IsActive = i <= 8, // 前8个启用
                CreatedAt = DateTime.Now.AddDays(-i)
            });
        }

        context.Banners.AddRange(banners);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化案例数据
    /// </summary>
    private static void InitializeCases(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.Cases.Any())
        {
            return; // 数据库已有数据
        }

        var categories = new string[] { "enterprise", "school", "government", "hospital" };
        var deviceTypes = new string[] { "desktop", "laptop", "server", "network" };
        var scales = new string[] { "small", "medium", "large" };
        
        var cases = new List<Case>();
        
        for (int i = 1; i <= 11; i++)
        {
            cases.Add(new Case
            {
                Title = $"回收案例 {i}",
                Description = $"案例简短描述 {i}，概述项目背景和主要成果。",
                FullDescription = $"案例完整描述 {i}，详细介绍项目背景、实施过程和最终成果。这是一个成功的IT设备回收项目，通过专业的处理流程，既保证了数据安全，又实现了资源的最大化利用。",
                Client = $"客户{i}公司",
                Category = categories[i % categories.Length],
                DeviceType = deviceTypes[i % deviceTypes.Length],
                DeviceCount = i * 50,
                Date = DateTime.Now.AddMonths(-i),
                Duration = $"{i % 6 + 1}个月",
                Scale = scales[i % scales.Length],
                Image = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                Tags = $"标签1,标签2,标签{i}",
                Views = i * 100,
                Rating = (decimal)(3.5 + (i % 3) * 0.5),
                ProjectDetails = $"项目详情 {i}，包含项目的各个阶段、具体实施方案和技术细节。此项目共回收设备{i * 50}台，通过专业的数据擦除技术确保信息安全，并对设备进行分类处理，实现了资源的循环利用。",
                Highlights = $"服务亮点1: 专业数据擦除; 服务亮点2: 高效回收流程; 服务亮点{i}: 资源最大化利用",
                IsActive = true,
                Sort = i,
                CreatedAt = DateTime.Now.AddDays(-i * 5)
            });
        }

        context.Cases.AddRange(cases);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化公司信息数据
    /// </summary>
    private static void InitializeCompanyInfo(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.CompanyInfos.Any())
        {
            return; // 数据库已有数据
        }

        var companyInfo = new CompanyInfo
        {
            Name = "绿色循环环保科技有限公司",
            Introduction = "绿色循环环保科技有限公司成立于2010年，是一家专注于IT设备回收、数据安全销毁和电子废弃物处理的高科技企业。公司秉承绿色环保、资源循环的理念，致力于为企业和机构提供专业、安全、环保的IT设备回收解决方案。",
            ImageUrl = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
            EstablishmentDate = new DateTime(2010, 5, 15),
            Phone = "400-123-4567",
            Email = "contact@greencycle.com",
            Address = "上海市浦东新区环保科技园区A栋10楼",
            CreatedAt = DateTime.Now.AddYears(-1)
        };

        context.CompanyInfos.Add(companyInfo);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化公司优势数据
    /// </summary>
    private static void InitializeCompanyAdvantages(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.CompanyAdvantages.Any())
        {
            return; // 数据库已有数据
        }

        var advantages = new List<CompanyAdvantage>
        {
            new CompanyAdvantage { Title = "专业团队", Description = "拥有10年以上行业经验的专业团队", Icon = "team", Sort = 1, CreatedAt = DateTime.Now,CompanyInfoId=1 },
            new CompanyAdvantage { Title = "安全保障", Description = "严格的数据安全销毁流程和认证", Icon = "safety", Sort = 2, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyAdvantage { Title = "环保处理", Description = "符合国际标准的环保处理工艺", Icon = "eco", Sort = 3, CreatedAt = DateTime.Now,CompanyInfoId=1 },
            new CompanyAdvantage { Title = "全国服务", Description = "覆盖全国的服务网络和物流体系", Icon = "global", Sort = 4, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyAdvantage { Title = "资质认证", Description = "拥有完善的行业资质和认证", Icon = "certificate", Sort = 5, CreatedAt = DateTime.Now,CompanyInfoId=1 },
            new CompanyAdvantage { Title = "高效流程", Description = "标准化、流程化的高效服务体系", Icon = "process", Sort = 6, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyAdvantage { Title = "增值服务", Description = "提供IT资产管理等多项增值服务", Icon = "value", Sort = 7, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyAdvantage { Title = "技术创新", Description = "持续的技术创新和研发投入", Icon = "innovation", Sort = 8, CreatedAt = DateTime.Now ,CompanyInfoId=1 },
            new CompanyAdvantage { Title = "合规运营", Description = "严格遵守国家法律法规和行业标准", Icon = "compliance", Sort = 9, CreatedAt = DateTime.Now,CompanyInfoId=1 },
            new CompanyAdvantage { Title = "价值最大化", Description = "帮助客户实现IT资产价值最大化", Icon = "maximize", Sort = 10, CreatedAt = DateTime.Now,CompanyInfoId=1 },
            new CompanyAdvantage { Title = "定制方案", Description = "根据客户需求提供定制化解决方案", Icon = "customize", Sort = 11, CreatedAt = DateTime.Now,CompanyInfoId=1 }
        };

        context.CompanyAdvantages.AddRange(advantages);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化公司发展历程数据
    /// </summary>
    private static void InitializeCompanyMilestones(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.CompanyMilestones.Any())
        {
            return; // 数据库已有数据
        }

        var milestones = new List<CompanyMilestone>
        {
            new CompanyMilestone { Year = "2010", Title = "公司成立", Description = "绿色循环环保科技有限公司在上海成立", Sort = 1, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2012", Title = "首个处理中心", Description = "建成上海浦东废弃物处理中心，年处理能力5万台", Sort = 2, CreatedAt = DateTime.Now,CompanyInfoId=1 },
            new CompanyMilestone { Year = "2014", Title = "技术突破", Description = "自主研发的数据安全擦除技术获得国家专利", Sort = 3, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2015", Title = "业务扩展", Description = "服务网络扩展至华东地区，建立10个服务中心", Sort = 4, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2016", Title = "获得认证", Description = "获得ISO9001、ISO14001、ISO27001三体系认证", Sort = 5, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2017", Title = "规模扩大", Description = "年处理设备量突破10万台，成为行业领先企业", Sort = 6, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2018", Title = "全国布局", Description = "服务网络覆盖全国，建立5个区域处理中心", Sort = 7, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2019", Title = "国际合作", Description = "与国际环保组织建立战略合作伙伴关系", Sort = 8, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2020", Title = "十周年", Description = "公司成立十周年，累计服务客户超过1000家", Sort = 9, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2021", Title = "科技创新", Description = "建立研发中心，加大技术创新投入", Sort = 10, CreatedAt = DateTime.Now ,CompanyInfoId=1},
            new CompanyMilestone { Year = "2022", Title = "品牌升级", Description = "完成品牌升级，推出新的企业标识和服务理念", Sort = 11, CreatedAt = DateTime.Now ,CompanyInfoId=1}
        };

        context.CompanyMilestones.AddRange(milestones);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化公司展示信息数据
    /// </summary>
    private static void InitializeCompanyProfile(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.CompanyProfiles.Any())
        {
            return; // 数据库已有数据
        }

        var advantages = @"[
            {""title"":""专业团队"",""description"":""拥有300多名环保领域专业人才"",""iconUrl"":""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""},
            {""title"":""技术创新"",""description"":""自主研发多项废弃物处理专利技术"",""iconUrl"":""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""},
            {""title"":""全链服务"",""description"":""提供从收集、分类、处理到资源化利用的一站式服务"",""iconUrl"":""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""},
            {""title"":""资质齐全"",""description"":""拥有危险废物经营许可证等多项专业资质"",""iconUrl"":""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""},
            {""title"":""规模优势"",""description"":""全国50多个回收网点，5个大型处理中心"",""iconUrl"":""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""},
            {""title"":""社会责任"",""description"":""积极参与环保公益，推动绿色教育"",""iconUrl"":""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""}
        ]";

        var milestones = @"[
            {""year"":""2010"",""title"":""公司成立"",""description"":""绿色循环环保科技有限公司在上海成立""},
            {""year"":""2012"",""title"":""首个处理中心"",""description"":""建成上海浦东废弃物处理中心，年处理能力5万吨""},
            {""year"":""2014"",""title"":""技术突破"",""description"":""自主研发的废旧电子产品资源化处理技术获国家专利""},
            {""year"":""2016"",""title"":""业务扩展"",""description"":""成功拓展华东地区市场，建立20个回收网点""},
            {""year"":""2018"",""title"":""资质认证"",""description"":""获得ISO14001环境管理体系认证和危险废物经营许可证""},
            {""year"":""2020"",""title"":""规模扩大"",""description"":""全国网点扩展至50个，年处理能力提升至20万吨""},
            {""year"":""2022"",""title"":""科技创新"",""description"":""建立省级环保技术研发中心，推出智能回收系统""}
        ]";

        var certifications = @"[
            ""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png"",
            ""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png"",
            ""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png"",
            ""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png"",
            ""https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png""
        ]";

        var companyProfile = new CompanyProfile
        {
            Name = "绿色循环环保科技有限公司",
            Introduction = "绿色循环环保科技有限公司成立于2010年，是一家专注于废弃物回收处理和资源再利用的高新技术企业。公司拥有完整的废弃物处理产业链，涵盖收集、分类、处理、再生利用等环节，致力于为社会提供专业的环保解决方案。",
            LogoUrl = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
            CoverImageUrl = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
            EstablishDate = new DateTime(2010, 5, 18),
            Phone = "400-123-4567",
            Email = "contact@greencycle.com",
            Address = "上海市浦东新区环保科技园区A栋10楼",
            Vision = "成为中国领先的废弃物处理与资源循环利用服务提供商，引领行业可持续发展。",
            Mission = "通过科技创新和优质服务，推动废弃物减量化、资源化、无害化处理，促进循环经济发展，共建绿色家园。",
            AdvantagesJson = advantages,
            MilestonesJson = milestones,
            CertificationsJson = certifications,
            CreatedAt = DateTime.Now.AddMonths(-6)
        };

        context.CompanyProfiles.Add(companyProfile);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化团队成员类型数据
    /// </summary>
    private static void InitializeTeamMemberTypes(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.TeamMemberTypes.Any())
        {
            return; // 数据库已有数据
        }

        var types = new List<TeamMemberType>
        {
            new TeamMemberType { Name = "管理团队", Description = "公司高管及各部门负责人", Sort = 1, ShowOnHome = true, CreatedAt = DateTime.Now },
            new TeamMemberType { Name = "技术团队", Description = "技术研发和工程实施人员", Sort = 2, ShowOnHome = true, CreatedAt = DateTime.Now },
            new TeamMemberType { Name = "销售团队", Description = "负责市场开拓和客户维护", Sort = 3, ShowOnHome = false, CreatedAt = DateTime.Now },
            new TeamMemberType { Name = "运营团队", Description = "负责日常运营和服务保障", Sort = 4, ShowOnHome = false, CreatedAt = DateTime.Now },
            new TeamMemberType { Name = "顾问团队", Description = "行业专家和技术顾问", Sort = 5, ShowOnHome = true, CreatedAt = DateTime.Now }
        };

        context.TeamMemberTypes.AddRange(types);
        context.SaveChanges();
    }

    /// <summary>
    /// 初始化团队成员数据
    /// </summary>
    private static void InitializeTeamMembers(ApplicationDbContext context)
    {
        // 检查是否已有数据
        if (context.TeamMembers.Any())
        {
            return; // 数据库已有数据
        }

        // 获取已创建的成员类型
        var types = context.TeamMemberTypes.ToList();
        
        var members = new List<TeamMember>();

        // 为每个类型创建成员
        foreach (var type in types)
        {
            for (int i = 1; i <= 3; i++)
            {
                members.Add(new TeamMember
                {
                    Name = $"{type.Name}成员{i}",
                    Position = $"{type.Name}职位{i}",
                    AvatarUrl = "https://pic4.zhuanstatic.com/zhuanzh/63e79742-28be-45d2-b0a0-3f334b26fa1b.png",
                    Description = $"{type.Name}成员{i}简介，拥有多年行业经验，专注于{type.Name.Replace("团队", "")}领域的研究和实践。",
                    TeamMemberTypeId = type.Id,
                    Sort = i,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-i)
                });
            }
        }

        context.TeamMembers.AddRange(members);
        context.SaveChanges();
    }
} 