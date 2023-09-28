using DotNetCore.CAP;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Api.Core;
using ZhonTai.Api.Rpc.EventBus;
using ZhonTai.Utils.Helpers;
using FreeSql;
using FreeRedis;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Microsoft.Extensions.DependencyInjection;

public static class CapExtensions
{
    public static IServiceCollection AddMyCap(this IServiceCollection services)
    {
        var capBuilder = services
        .AddSingleton<IEventPublisher, CapPublisher>()
        .AddCap(options =>
        {
            //var dbConfig = AppInfo.GetOptions<DbConfig>();
            //if (dbConfig.Type == DataType.MySql)
            //{
            //    options.UseMySql(mysqlOptions =>
            //    {
            //        mysqlOptions.TableNamePrefix = "cap";
            //        mysqlOptions.ConnectionString = dbConfig.ConnectionString;
            //    });
            //}

            //var mqOptions = AppInfo.GetOptions<RabbitMQOptions>();
            //options.UseRabbitMQ(rabbitMqOptions =>
            //{
            //    rabbitMqOptions.HostName = mqOptions.HostName;
            //    rabbitMqOptions.VirtualHost = mqOptions.VirtualHost;
            //    rabbitMqOptions.Port = mqOptions.Port;
            //    rabbitMqOptions.UserName = mqOptions.UserName;
            //    rabbitMqOptions.Password = mqOptions.Password;
            //    rabbitMqOptions.ExchangeName = mqOptions.ExchangeName;
            //    rabbitMqOptions.ConnectionFactoryOptions = (facotry) =>
            //    {
            //        facotry.ClientProvidedName = AppInfo.ModuleInfo.Id;
            //    };
            //});

            options.UseInMemoryStorage();
            options.UseInMemoryMessageQueue();

            options.UseDashboard(dashboardOptions =>
            {
                dashboardOptions.PathMatch = $"/{AppInfo.ModuleInfo.Name}/cap";
                dashboardOptions.UseAuth = false;
            });
        });

        var appConfig = AppInfo.GetOptions<AppConfig>();
        var assemblyNames = appConfig.AssemblyNames;
        if (assemblyNames is not null && assemblyNames.Any())
        {
            var assemblies = AssemblyHelper.GetAssemblyList(assemblyNames);

            if (assemblies is not null && assemblies.Any())
            {
                capBuilder.AddSubscriberAssembly(assemblies);
            }
        }

        return services;
    }
}


