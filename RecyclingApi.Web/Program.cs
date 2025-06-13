using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecyclingApi.Application;
using RecyclingApi.Application.Services.Email;
using RecyclingApi.Application.Services.Product;
using RecyclingApi.ChatService.Hubs;
using RecyclingApi.Domain.Entities.Data;
using RecyclingApi.Domain.Entities.User;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using RecyclingApi.Application.Services.Content;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Application.Services.Products;
using RecyclingApi.Application.Services.HR;

var builder = WebApplication.CreateBuilder(args);

// 添加控制器服务
builder.Services.AddControllers();

// 配置Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IT设备回收管理系统 API",
        Version = "v1.0",
        Description = "IT设备回收管理系统的RESTful API接口文档",
        Contact = new OpenApiContact
        {
            Name = "开发团队",
            Email = "dev@recycling.com",
            Url = new Uri("https://www.recycling.com")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // 启用XML注释
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // 包含应用层的XML注释
    var applicationXmlFile = "RecyclingApi.Application.xml";
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    if (File.Exists(applicationXmlPath))
    {
        c.IncludeXmlComments(applicationXmlPath);
    }

    // 包含领域层的XML注释
    var domainXmlFile = "RecyclingApi.Domain.xml";
    var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
    if (File.Exists(domainXmlPath))
    {
        c.IncludeXmlComments(domainXmlPath);
    }

    // 配置枚举显示
    c.SchemaFilter<EnumSchemaFilter>();
    
    // 配置JWT授权
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 配置数据库连接
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("RecyclingApi.Web")));

// 配置Admin Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // 密码设置
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // 锁定设置
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // 用户设置
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 配置JWT认证
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
    // SignalR WebSocket 的 Token 传递方式
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// 添加Application层服务（包含Redis和RabbitMQ）
builder.Services.AddApplicationFull(builder.Configuration);

//添加仓储和工作单元服务
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// 添加聊天服务
builder.Services.AddChatServices(builder.Configuration);
builder.Services.AddSignalR();
// 添加认证服务
builder.Services.AddAuthServices();

// 修改 AllowVueApp 策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // 前端地址
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() // 必须启用凭证
            .SetIsOriginAllowed(_ => true); // 允许 WebSocket 跨域
    });
});

var app = builder.Build();

// 初始化数据库种子数据
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await UserSeeder.SeedUsersAsync(userManager, roleManager);
    context.Database.EnsureCreated();
    ExtendedSeedData.Initialize(context);
}



// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    // 启用Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IT设备回收管理系统 API v1.0");
        c.RoutePrefix = string.Empty; // 设置Swagger UI为根路径
        c.DocumentTitle = "IT设备回收管理系统 API 文档";
        c.DefaultModelsExpandDepth(-1); // 默认不展开模型
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // 默认不展开接口
        c.EnableDeepLinking(); // 启用深度链接
        c.EnableFilter(); // 启用过滤器
        c.ShowExtensions(); // 显示扩展
    });
}
app.UseHttpsRedirection();

// 中间件顺序必须正确！
app.UseRouting();
app.UseCors("AllowVueApp");
app.UseAuthentication();
app.UseAuthorization();

// 映射 SignalR Hub
app.MapHub<ChatHub>("/chathub"); // 确保路径与前端一致
app.MapControllers();

// 开发环境下自动打开浏览器
if (app.Environment.IsDevelopment())
{
    var url = "http://localhost:5279/index.html";
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"无法自动打开浏览器: {ex.Message}");
        Console.WriteLine($"请手动访问: {url}");
    }
}

app.Run();


/// <summary>
/// Swagger枚举架构过滤器
/// 用于在Swagger文档中正确显示枚举类型
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 应用枚举架构过滤器
    /// </summary>
    /// <param name="schema">OpenAPI架构</param>
    /// <param name="context">架构过滤器上下文</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(name)));
        }
    }
}
