namespace ZhonTai.Plugin.Cache;

/// <summary>
/// 缓存配置
/// </summary>
public class CacheConfig
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public CacheType Type { get; set; } = CacheType.Memory;

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}