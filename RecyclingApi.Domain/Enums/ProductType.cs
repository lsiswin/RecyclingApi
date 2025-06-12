namespace RecyclingApi.Domain.Enums;

/// <summary>
/// 产品类型枚举
/// 定义了IT设备回收系统中支持的各种设备类型
/// </summary>
public enum ProductType
{
    /// <summary>
    /// 服务器设备
    /// </summary>
    Server,
    
    /// <summary>
    /// 台式电脑
    /// </summary>
    Desktop,
    
    /// <summary>
    /// 笔记本电脑
    /// </summary>
    Laptop,
    
    /// <summary>
    /// 网络设备（路由器、交换机等）
    /// </summary>
    NetworkEquipment,
    
    /// <summary>
    /// 存储设备（硬盘、SSD等）
    /// </summary>
    StorageDevice,
    
    /// <summary>
    /// 打印设备
    /// </summary>
    Printer,
    
    /// <summary>
    /// 显示器
    /// </summary>
    Monitor,
    
    /// <summary>
    /// 移动设备（手机等）
    /// </summary>
    Mobile,
    
    /// <summary>
    /// 平板设备
    /// </summary>
    Tablet,
    
    /// <summary>
    /// 其他设备
    /// </summary>
    Other
} 