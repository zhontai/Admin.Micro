using ZhonTai;
using ZhonTai.Api.Doc;
using ZhonTai.Api.Core;
using ZhonTai.Api.Core.Startup;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Module.Admin.HttpApi.Core.Extensions;

new AppHost(new AppHostOptions
{
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
        if (dbConfig.Key == DbKeys.AppDb)
        {
            
        }
    },
    ConfigurePreWebApplicationBuilder = webBuilder =>
    {
        //Nacos配置中心
        //webBuilder.Host.UseNacosConfig(section: "ThirdPartyConfig:NacosConfig", parser: null, logAction: null);
        //.UseNacosConfig(section: "NacosConfig", parser: Nacos.YamlParser.YamlConfigurationStringParser.Instance, logAction: null);
        //.UseNacosConfig(section: "NacosConfig", parser: Nacos.IniParser.IniConfigurationStringParser.Instance, logAction: null);
    },
    ConfigurePreServices = context =>
    {
        
    },
    //配置后置服务
    ConfigurePostServices = context =>
	{
        //context.Services.AddTiDb(context);

        context.Services.AddMyCap();

        //OSS文件上传
        context.Services.AddMyOSS();

        //滑块验证码
        context.Services.AddMySlideCaptcha(optionsAction: options =>
        {
            options.StoreageKeyPrefix = CacheKeys.Captcha;
        });
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

#if DEBUG
public partial class Program { }
#endif