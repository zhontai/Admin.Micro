namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 缓存接口
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// 缓存列表
    /// </summary>
    /// <returns></returns>
    List<dynamic> GetList();

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    Task ClearAsync(string cacheKey);
}