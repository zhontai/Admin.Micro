using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ZhonTai.Api.Core.Startup;

/// <summary>
/// 应用宿主中间件上下文
/// </summary>
public class AppHostMiddlewareContext
{
    /// <summary>
    /// 应用
    /// </summary>
    public WebApplication App { get; set; }

    /// <summary>
    /// 环境
    /// </summary>
    public IHostEnvironment Environment { get; set; }

    /// <summary>
    /// 配置
    /// </summary>
    public IConfiguration Configuration { get; set; }
}

