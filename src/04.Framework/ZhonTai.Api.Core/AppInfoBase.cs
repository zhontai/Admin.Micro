using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ZhonTai.Api.Core;

internal static class AppInfoBase
{
    internal static IServiceCollection Services;

    internal static IServiceProvider ServiceProvider;

    internal static IWebHostEnvironment WebHostEnvironment;

    internal static IHostEnvironment HostEnvironment;

    internal static IConfiguration Configuration;

    internal static ModuleInfo ModuleInfo;

    internal static void ConfigureApplication(this WebApplicationBuilder webApplicationBuilder, Assembly assembly)
    {
        HostEnvironment = webApplicationBuilder.Environment;
        WebHostEnvironment = webApplicationBuilder.Environment;
        Services = webApplicationBuilder.Services;
        Configuration = webApplicationBuilder.Configuration;
        ModuleInfo = ModuleInfo.CreateInstance(assembly);
    }

    internal static void ConfigureApplication(this WebApplication app)
    {
        ServiceProvider = app.Services;
    }
}
