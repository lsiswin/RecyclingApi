using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyclingApi.Application.Services.Chat;

/// <summary>
/// 聊天统计服务接口
/// </summary>
public interface IChatStatisticsService
{
    /// <summary>
    /// 记录消息统计
    /// </summary>
    Task RecordMessageAsync(string userId, string messageType, DateTime timestamp);

    /// <summary>
    /// 记录用户活跃度
    /// </summary>
    Task RecordUserActivityAsync(string userId, string activityType, DateTime timestamp);

    /// <summary>
    /// 记录客服工作统计
    /// </summary>
    Task RecordStaffWorkAsync(string staffId, string workType, DateTime timestamp, int duration = 0);

    /// <summary>
    /// 获取今日消息统计
    /// </summary>
    Task<ChatStatistics> GetTodayStatisticsAsync();

    /// <summary>
    /// 获取指定日期范围的统计
    /// </summary>
    Task<ChatStatistics> GetStatisticsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// 获取客服工作统计
    /// </summary>
    Task<List<StaffWorkStatistics>> GetStaffStatisticsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// 获取实时统计
    /// </summary>
    Task<RealTimeStatistics> GetRealTimeStatisticsAsync();
}

/// <summary>
/// 聊天统计数据
/// </summary>
public class ChatStatistics
{
    public DateTime Date { get; set; }
    public int TotalMessages { get; set; }
    public int TotalUsers { get; set; }
    public int TotalSessions { get; set; }
    public double AverageResponseTime { get; set; }
    public double AverageSessionDuration { get; set; }
    public Dictionary<string, int> MessagesByHour { get; set; } = new();
    public Dictionary<string, int> MessagesByType { get; set; } = new();
}

/// <summary>
/// 客服工作统计
/// </summary>
public class StaffWorkStatistics
{
    public string StaffId { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public int TotalSessions { get; set; }
    public int TotalMessages { get; set; }
    public double AverageResponseTime { get; set; }
    public double OnlineHours { get; set; }
    public double UtilizationRate { get; set; }
    public int CustomerSatisfactionScore { get; set; }
}

/// <summary>
/// 实时统计
/// </summary>
public class RealTimeStatistics
{
    public int OnlineUsers { get; set; }
    public int OnlineStaff { get; set; }
    public int ActiveSessions { get; set; }
    public int QueuedVisitors { get; set; }
    public double AverageWaitTime { get; set; }
    public double SystemLoad { get; set; }
}