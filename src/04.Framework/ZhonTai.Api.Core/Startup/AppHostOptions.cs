using Autofac;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Api.DynamicApi;

namespace ZhonTai.Api.Core.Startup;

/// <summary>
/// 应用宿主配置
/// </summary>
public class AppHostOptions
{
    /// <summary>
    /// 配置前置应用程序构建器
    /// </summary>
    public Action<WebApplicationBuilder> ConfigurePreWebApplicationBuilder { get; set; }

    /// <summary>
    /// 配置应用程序构建器
    /// </summary>
    public Action<WebApplicationBuilder> ConfigureWebApplicationBuilder { get; set; }

    /// <summary>
    /// 配置前置服务
    /// </summary>
    public Action<AppHostContext> ConfigurePreServices { get; set; }

    /// <summary>
    /// 配置服务
    /// </summary>
    public Action<AppHostContext> ConfigureServices { get; set; }

    /// <summary>
    /// 配置后置服务
    /// </summary>
    public Action<AppHostContext> ConfigurePostServices { get; set; }

    /// <summary>
    /// 配置mvc构建器
    /// </summary>
    public Action<IMvcBuilder, AppHostContext> ConfigureMvcBuilder { get; set; }

    /// <summary>
    /// 配置Autofac容器
    /// </summary>
    public Action<ContainerBuilder, AppHostContext> ConfigureAutofacContainer { get; set; }

    /// <summary>
    /// 配置前置中间件
    /// </summary>
    public Action<AppHostMiddlewareContext> ConfigurePreMiddleware { get; set; }

    /// <summary>
    /// 配置中间件
    /// </summary>
    public Action<AppHostMiddlewareContext> ConfigureMiddleware { get; set; }

    /// <summary>
    /// 配置后置中间件
    /// </summary>
    public Action<AppHostMiddlewareContext> ConfigurePostMiddleware { get; set; }

    /// <summary>
    /// 配置FreeSql构建器
    /// </summary>
    public Action<FreeSqlBuilder, DbConfig> ConfigureFreeSqlBuilder { get; set; }

    /// <summary>
    /// 配置FreeSql
    /// </summary>
    public Action<IFreeSql, DbConfig> ConfigureFreeSql { get; set; }

    /// <summary>
    /// 配置动态Api
    /// </summary>
    public Action<DynamicApiOptions> ConfigureDynamicApi { get; set; }

    /// <summary>
    /// 配置雪花漂移算法
    /// </summary>
    public Action<IdGeneratorOptions> ConfigureIdGenerator { get; set; }

    /// <summary>
    /// 自定义数据库初始化
    /// </summary>
    public bool CustomInitDb { get; set; } = false;

}