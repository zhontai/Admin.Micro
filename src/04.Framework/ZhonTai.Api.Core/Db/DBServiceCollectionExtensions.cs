﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FreeSql;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Api.Core.Auth;
using ZhonTai.Api.Core.Startup;
using System.Collections.Concurrent;
using System.Reflection;
using ZhonTai.Api.Core.Db.Transaction;

namespace ZhonTai.Api.Core.Db;

public static class DBServiceCollectionExtensions
{
    /// <summary>
    /// 添加数据库
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="appHostOptions"></param>
    /// <returns></returns>
    public static void AddDb(this IServiceCollection services, IHostEnvironment env, AppHostOptions appHostOptions)
    {
        var dbConfig = AppInfo.GetOptions<DbConfig>();
        var appConfig = AppInfo.GetOptions<AppConfig>();
        var user = services.BuildServiceProvider().GetService<IUser>();
        var freeSqlCloud = appConfig.DistributeKey.IsNull() ? new FreeSqlCloud() : new FreeSqlCloud(appConfig.DistributeKey);
        DbHelper.RegisterDb(freeSqlCloud, user, dbConfig, appConfig, appHostOptions);

        //注册多数据库
        if (dbConfig.Dbs?.Length > 0)
        {
            foreach (var db in dbConfig.Dbs)
            {
                DbHelper.RegisterDb(freeSqlCloud, user, db, appConfig, null);
            }
        }

        services.AddSingleton<IFreeSql>(freeSqlCloud);
        services.AddSingleton(freeSqlCloud);
        services.AddScoped<UnitOfWorkManagerCloud>();
        //定义主库
        var fsql = freeSqlCloud.Use(dbConfig.Key);
        services.AddSingleton(provider => fsql);
        //运行主库
        fsql.Select<object>();
    }

    /// <summary>
    /// 添加TiDb数据库
    /// </summary>
    /// <param name="_"></param>
    /// <param name="context"></param>
    /// <param name="version">版本</param>
    public static void AddTiDb(this IServiceCollection _, AppHostContext context, string version = "8.0")
    {
        var dbConfig = AppInfo.GetOptions<DbConfig>();
        var _dicMySqlVersion = typeof(FreeSqlGlobalExtensions).GetField("_dicMySqlVersion", BindingFlags.NonPublic | BindingFlags.Static);
        var dicMySqlVersion = new ConcurrentDictionary<string, string>();
        dicMySqlVersion[dbConfig.ConnectionString] = version;
        _dicMySqlVersion.SetValue(new ConcurrentDictionary<string, string>(), dicMySqlVersion);
    }
}