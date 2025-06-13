using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Domain.Entities.Chat;
using RecyclingApi.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using RecyclingApi.Domain.Entities.Content;
using RecyclingApi.Domain.Entities.Products;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Domain.Entities.Data;

/// <summary>
/// 应用程序数据库上下文
/// 负责管理IT设备回收系统的数据库连接和实体配置
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// 初始化数据库上下文
    /// </summary>
    /// <param name="options">数据库上下文选项</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // 产品相关数据集

    /// <summary>
    /// 产品数据集
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// 轮播图数据集
    /// </summary>
    public DbSet<Banner> Banners { get; set; }

    /// <summary>
    /// 产品分类数据集
    /// </summary>
    public DbSet<ProductCategory> ProductCategories { get; set; }

    /// <summary>
    /// 处理步骤数据集
    /// </summary>
    public DbSet<ProcessStep> ProcessSteps { get; set; }

    // 内容相关数据集

    /// <summary>
    /// 案例数据集
    /// </summary>
    public DbSet<Case> Cases { get; set; }

    /// <summary>
    /// 公司信息数据集
    /// </summary>
    public DbSet<CompanyInfo> CompanyInfos { get; set; }

    /// <summary>
    /// 公司优势数据集
    /// </summary>
    public DbSet<CompanyAdvantage> CompanyAdvantages { get; set; }

    /// <summary>
    /// 公司发展历程数据集
    /// </summary>
    public DbSet<CompanyMilestone> CompanyMilestones { get; set; }

    /// <summary>
    /// 公司展示信息数据集
    /// </summary>
    public DbSet<CompanyProfile> CompanyProfiles { get; set; }

    /// <summary>
    /// 团队成员数据集
    /// </summary>
    public DbSet<TeamMember> TeamMembers { get; set; }

    /// <summary>
    /// 团队成员类型数据集
    /// </summary>
    public DbSet<TeamMemberType> TeamMemberTypes { get; set; }

    // HR相关数据集

    /// <summary>
    /// 员工数据集
    /// </summary>
    public DbSet<Employee> Employees { get; set; }

    // 聊天相关数据集

    /// <summary>
    /// 聊天用户数据集
    /// </summary>
    public DbSet<ChatUser> ChatUsers { get; set; }

    /// <summary>
    /// 聊天室数据集
    /// </summary>
    public DbSet<ChatRoom> ChatRooms { get; set; }

    /// <summary>
    /// 聊天消息数据集
    /// </summary>
    public DbSet<ChatMessage> ChatMessages { get; set; }

    /// <summary>
    /// 聊天会话数据集
    /// </summary>
    public DbSet<ChatSession> ChatSessions { get; set; }
    /// <summary>
    /// 职位数据集
    /// </summary>
    public DbSet<JobPosition> JobPositions { get; set; }
    /// <summary>
    /// 职位申请数据集
    /// </summary>
    public DbSet<JobApplication> JobApplications { get; set; }
    /// <summary>
    /// 简历数据集
    /// </summary>
    public DbSet<Resume> Resumes { get; set; }

    /// <summary>
    /// 配置实体模型
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置产品相关实体
        ConfigureProductEntities(modelBuilder);

        // 配置内容相关实体
        ConfigureContentEntities(modelBuilder);

        // 配置HR相关实体
        ConfigureHREntities(modelBuilder);

        // 配置聊天相关实体
        ConfigureChatEntities(modelBuilder);
    }

    /// <summary>
    /// 配置产品相关实体
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    private void ConfigureProductEntities(ModelBuilder modelBuilder)
    {
        // Product实体配置
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EstimatedValue).HasColumnType("decimal(18,2)");

            // 配置与ProductCategory的一对多关系
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 配置与ProcessStep的一对多关系
            entity.HasMany(e => e.ProcessSteps)
                  .WithOne(ps => ps.Product)
                  .HasForeignKey(ps => ps.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ProductCategory实体配置
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        // ProcessStep实体配置
        modelBuilder.Entity<ProcessStep>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
    }

    /// <summary>
    /// 配置内容相关实体
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    private void ConfigureContentEntities(ModelBuilder modelBuilder)
    {
        // Banner实体配置
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.LinkUrl).HasMaxLength(500);
        });

        // Case实体配置
        modelBuilder.Entity<Case>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FullDescription).HasColumnType("nvarchar(max)");
            entity.Property(e => e.Client).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.DeviceType).HasMaxLength(50);
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.Scale).HasMaxLength(20);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Tags).HasMaxLength(500);
            entity.Property(e => e.Rating).HasColumnType("decimal(3,1)");
            entity.Property(e => e.ProjectDetails).HasColumnType("nvarchar(max)");
            entity.Property(e => e.Highlights).HasMaxLength(1000);
        });

        // CompanyInfo实体配置
        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Introduction).HasColumnType("nvarchar(max)");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(200);
        });

        // CompanyAdvantage实体配置
        modelBuilder.Entity<CompanyAdvantage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(50);
        });

        // CompanyMilestone实体配置
        modelBuilder.Entity<CompanyMilestone>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Year).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // CompanyProfile实体配置
        modelBuilder.Entity<CompanyProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Introduction).HasColumnType("nvarchar(max)");
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Vision).HasMaxLength(500);
            entity.Property(e => e.Mission).HasMaxLength(500);
            entity.Property(e => e.AdvantagesJson).HasColumnType("nvarchar(max)");
            entity.Property(e => e.MilestonesJson).HasColumnType("nvarchar(max)");
            entity.Property(e => e.CertificationsJson).HasColumnType("nvarchar(max)");
        });

        // TeamMemberType实体配置
        modelBuilder.Entity<TeamMemberType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        // TeamMember实体配置
        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);

            // 配置与TeamMemberType的关系
            entity.HasOne(e => e.TeamMemberType)
                  .WithMany(t => t.TeamMembers)
                  .HasForeignKey(e => e.TeamMemberTypeId)
                  .OnDelete(DeleteBehavior.Restrict);

        });
    }

    /// <summary>
    /// 配置HR相关实体
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    private void ConfigureHREntities(ModelBuilder modelBuilder)
    {
        // Employee实体配置
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EmployeeNo).HasMaxLength(20);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.Introduction).HasMaxLength(500);
        });
    }

    /// <summary>
    /// 配置聊天相关实体
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    private void ConfigureChatEntities(ModelBuilder modelBuilder)
    {
        // ChatUser实体配置
        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RealName).HasMaxLength(100);
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ConnectionId).HasMaxLength(100);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.MuteReason).HasMaxLength(500);
            entity.Property(e => e.BanReason).HasMaxLength(500);

            // 创建唯一索引
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.HasIndex(e => e.ConnectionId);
        });

        // ChatRoom实体配置
        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.WelcomeMessage).HasMaxLength(1000);
            entity.Property(e => e.Announcement).HasMaxLength(2000);
            entity.Property(e => e.Password).HasMaxLength(200);

            // 配置与ChatMessage的一对多关系 - 修改为Restrict避免循环级联
            entity.HasMany(e => e.Messages)
                  .WithOne(m => m.ChatRoom)
                  .HasForeignKey(m => m.ChatRoomId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 配置与ChatSession的一对多关系 - 保持Cascade，因为Session应该随Room删除
            entity.HasMany(e => e.Sessions)
                  .WithOne(s => s.ChatRoom)
                  .HasForeignKey(s => s.ChatRoomId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ChatMessage实体配置
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MessageId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SenderId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SenderName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SenderAvatar).HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.TargetUserId).HasMaxLength(50);
            entity.Property(e => e.TargetUserName).HasMaxLength(100);
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeleteReason).HasMaxLength(500);
            entity.Property(e => e.PinnedBy).HasMaxLength(50);
            entity.Property(e => e.SenderIpAddress).HasMaxLength(50);
            entity.Property(e => e.SenderUserAgent).HasMaxLength(500);

            // 创建索引
            entity.HasIndex(e => e.MessageId).IsUnique();
            entity.HasIndex(e => e.SenderId);
            entity.HasIndex(e => e.ChatRoomId);
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.Timestamp);

            // 配置与ChatUser的关系 - 发送者
            entity.HasOne(e => e.Sender)
                  .WithMany(u => u.SentMessages)
                  .HasForeignKey(e => e.SenderId)
                  .HasPrincipalKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 配置自引用关系（回复消息）
            entity.HasOne(e => e.ReplyToMessage)
                  .WithMany(m => m.Replies)
                  .HasForeignKey(e => e.ReplyToMessageId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 配置与ChatSession的关系 - 修改为SetNull避免循环级联
            entity.HasOne(e => e.Session)
                  .WithMany(s => s.Messages)
                  .HasForeignKey(e => e.SessionId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ChatSession实体配置
        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SessionId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.UserAvatar).HasMaxLength(500);
            entity.Property(e => e.CustomerServiceId).HasMaxLength(50);
            entity.Property(e => e.CustomerServiceName).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.UserProblem).HasMaxLength(2000);
            entity.Property(e => e.Solution).HasMaxLength(2000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.BotId).HasMaxLength(50);
            entity.Property(e => e.LastMessageContent).HasMaxLength(500);
            entity.Property(e => e.LastMessageSender).HasMaxLength(100);
            entity.Property(e => e.UserFeedback).HasMaxLength(1000);
            entity.Property(e => e.Source).HasMaxLength(100);
            entity.Property(e => e.SourceUrl).HasMaxLength(500);
            entity.Property(e => e.UserIpAddress).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.ArchivedBy).HasMaxLength(50);

            // 创建索引
            entity.HasIndex(e => e.SessionId).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CustomerServiceId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.StartTime);

            // 配置与ChatUser的关系 - 用户
            entity.HasOne(e => e.User)
                  .WithMany(u => u.ChatSessions)
                  .HasForeignKey(e => e.UserId)
                  .HasPrincipalKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 配置与ChatUser的关系 - 客服人员（可选）
            entity.HasOne(e => e.CustomerService)
                  .WithMany()
                  .HasForeignKey(e => e.CustomerServiceId)
                  .HasPrincipalKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}