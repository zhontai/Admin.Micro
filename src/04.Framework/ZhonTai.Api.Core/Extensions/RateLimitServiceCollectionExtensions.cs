using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Plugin.Cache;

namespace ZhonTai.Api.Core.Extensions;

public static class RateLimitServiceCollectionExtensions
{
    /// <summary>
    /// 添加Ip限流
    /// </summary>
    /// <param name="services"></param>
    public static void AddMyIpRateLimit(this IServiceCollection services)
    {
        #region IP限流

        services.Configure<IpRateLimitOptions>(AppInfo.Configuration.GetSection("ThirdPartyConfig:IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(AppInfo.Configuration.GetSection("ThirdPartyConfig:IpRateLimitPolicies"));

        var cacheConfig = AppInfo.GetRequiredService<CacheConfig>(false);
        if (cacheConfig.Type == CacheType.Redis)
        {
            services.AddDistributedRateLimiting();
        }
        else
        {
            services.AddInMemoryRateLimiting();
        }
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        #endregion IP限流
    }
}
