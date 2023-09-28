using Autofac;
using System.Linq;
using System.Reflection;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Utils.Helpers;

namespace ZhonTai.Api.Core.RegisterModules;

public class SingleInstanceModule : Autofac.Module
{
    private readonly AppConfig _appConfig;

    /// <summary>
    /// 单例注入
    /// </summary>
    /// <param name="appConfig">AppConfig</param>
    public SingleInstanceModule(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // 获得要注入的程序集
        var assemblies = AppInfo.EffectiveAssemblies.ToArray();

        //无接口注入单例
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>(false) != null)
        .SingleInstance()
        .PropertiesAutowired();

        //有接口注入单例
        builder.RegisterAssemblyTypes(assemblies)
        .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>(false) != null)
        .AsImplementedInterfaces()
        .SingleInstance()
        .PropertiesAutowired();
    }
}
