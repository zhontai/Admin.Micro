using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZhonTai;
using ZhonTai.Api.Core;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Api.Core.Startup;
using ZhonTai.Api.Doc;
using ZhonTai.Api.Rpc.Configs;
using ZhonTai.Api.Rpc.Helpers;
using ZhonTai.Module.App.Api.Core.Consts;

new AppHost(new AppHostOptions()
{
    //配置前置服务
    ConfigurePreServices = context =>
    {
        var dbConfig = AppInfo.GetOptions<DbConfig>();
        if (dbConfig.Key.NotNull())
        {
            DbKeys.AppDb = dbConfig.Key;
        }
    },
    //配置后置服务
    ConfigurePostServices = context =>
    {
        var policies = PolicyHelper.GetPolicyList();
        context.Services.AddMyHttpClients(AppInfo.EffectiveAssemblies, AppInfo.GetOptions<RpcConfig>(), policies);
    },
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
    },
    //配置Autofac容器
    ConfigureAutofacContainer = (builder, context) =>
    {
    },
    //配置Mvc
    ConfigureMvcBuilder = (builder, context) =>
    {
    },
    //配置后置中间件
    ConfigurePostMiddleware = context =>
    {
        var app = context.App;
        var env = app.Environment;
        var appConfig = AppInfo.GetOptions<AppConfig>();

        #region 新版Api文档
        if (env.IsDevelopment() || appConfig.ApiUI.Enable)
        {
            app.UseApiUI(options =>
            {
                options.RoutePrefix = appConfig.ApiUI.RoutePrefix;
                var routePath = options.RoutePrefix.NotNull() ? $"{options.RoutePrefix}/" : "";
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Title);
                });
            });
        }
        #endregion
    }
}).Run(args);

public partial class Program { }