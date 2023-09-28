# 任务调度

## 配置任务调度

在 MyCompanyName.MyProjectName.Host\Program.cs 文件配置任务调度

```cs
using MyCompanyName.MyProjectName.Api.Core.Handlers;
using FreeScheduler;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Tools.TaskScheduler;

new HostApp(new HostAppOptions()
{
    //配置FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
        if (dbConfig.Key == DbKeys.AppDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, (fsql) =>
            {
                fsql.CodeFirst
                .ConfigEntity<TaskInfo>(a =>
                {
                    a.Name("app_task");
                })
                .ConfigEntity<TaskLog>(a =>
                {
                    a.Name("app_task_log");
                });
            });
        }
    },

	//配置后置服务
	ConfigurePostServices = context =>
	{
		//添加任务调度，默认使用当前主库作为任务调度数据库
        //可通过AddTaskScheduler(DbKeys.AppDb, options => {})数据库键参数修改任务调度数据库
		context.Services.AddTaskScheduler(options =>
		{
			options.ConfigureFreeSql = freeSql =>
			{
				freeSql.CodeFirst
				//配置任务表
				.ConfigEntity<TaskInfo>(a =>
				{
					a.Name("app_task");
				})
				//配置任务日志表
				.ConfigEntity<TaskLog>(a =>
				{
					a.Name("app_task_log");
				});
			};

			//模块任务处理器
            options.TaskHandler = new CloudTaskHandler(options.FreeSqlCloud, DbKeys.AppDb);
            //模块自定义任务处理器，解析cron表达式
            options.CustomTaskHandler = new AppCustomTaskHandler();
		});
	},
	//配置后置中间件
	ConfigurePostMiddleware = context =>
	{
		var app = context.App;

		//使用任务调度
		app.UseTaskScheduler();
	}
}).Run(args);

public partial class Program { }
```

更多任务表和任务日志表配置

```cs
freeSql.CodeFirst
//配置任务表
.ConfigEntity<TaskInfo>(a =>
{
    a.Name("app_task");
    a.Property((TaskInfo b) => b.Id).IsPrimary(value: true);
    a.Property((TaskInfo b) => b.Body).StringLength(-1);
    a.Property((TaskInfo b) => b.Interval).MapType(typeof(int));
    a.Property((TaskInfo b) => b.IntervalArgument).StringLength(1024);
    a.Property((TaskInfo b) => b.Status).MapType(typeof(int));
    a.Property((TaskInfo b) => b.CreateTime).ServerTime(DateTimeKind.Local);
    a.Property((TaskInfo b) => b.LastRunTime).ServerTime(DateTimeKind.Local);
})
//配置任务日志表
.ConfigEntity<TaskLog>(a =>
{
    a.Name("app_task_log");
    a.Property((TaskLog b) => b.Exception).StringLength(-1);
    a.Property((TaskLog b) => b.Remark).StringLength(-1);
    a.Property((TaskLog b) => b.CreateTime).ServerTime(DateTimeKind.Local);
});
```

## 添加任务常量

在 MyCompanyName.MyProjectName.Api\Core\Consts 目录添加任务常量`TaskNames.cs`

```cs
namespace MyCompanyName.MyProjectName.Api.Core.Consts;

/// <summary>
/// 任务常量
/// </summary>
public static partial class TaskNames
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public const string ModuleTaskName = "ModuleTaskName";
}
```

## 添加模块任务处理器

在 MyCompanyName.MyProjectName.Api\Core\Handlers 目录添加模块任务处理器`ModuleTaskHandler.cs`

```cs
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeScheduler;
using ZhonTai.Admin.Tools.TaskScheduler;
using ZhonTai.Common.Extensions;
using MyCompanyName.MyProjectName.Api.Services.Module;
using MyCompanyName.MyProjectName.Api.Core.Consts;
using TaskStatus = FreeScheduler.TaskStatus;

namespace MyCompanyName.MyProjectName.Api.Core.Handlers;

/// <summary>
/// 模块任务处理器
/// </summary>
public class ModuleTaskHandler : TaskHandler
{
    public ModuleTaskHandler(IFreeSql fsql) : base(fsql)
    {

    }

    /// <summary>
    /// 模块任务
    /// </summary>
    /// <param name="task"></param>
    private static async Task ModuleTask(TaskInfo task)
    {
        using var scope = TaskSchedulerServiceExtensions.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
        var dics = JsonConvert.DeserializeObject<Dictionary<string, string>>(task.Body);
        var moduleId = dics["moduleId"];
        var result = await moduleService.GetAsync(moduleId.ToLong());
        if (result.Success)
        {
            //完成并结束任务
            task.Status = TaskStatus.Completed;
        }
    }

    public override void OnExecuting(Scheduler scheduler, TaskInfo task)
    {
        switch (task.Topic)
        {
            //模块任务
            case TaskNames.ModuleTaskName:
                Task.Run(async () => {
                    await ModuleTask(task);
                }).Wait();
                break;
        }
    }
}
```

## 添加模块自定义任务处理器

在 MyCompanyName.MyProjectName.Api\Core\Handlers 目录添加模块自定义任务处理器`AppCustomTaskHandler.cs`

```cs
using FreeScheduler;
using System;

namespace MyCompanyName.MyProjectName.Api.Core.Handlers;

/// <summary>
/// 模块自定义任务处理器
/// </summary>
public class AppCustomTaskHandler : ITaskIntervalCustomHandler
{
    public TimeSpan? NextDelay(TaskInfo task)
    {
        //利用 cron 功能库解析 task.IntervalArgument 得到下一次执行时间
        //与当前时间相减，得到 TimeSpan，若返回 null 则任务完成
        return TimeSpan.FromSeconds(5);
    }
}
```

## 使用任务调度

```cs
namespace MyCompanyName.MyProjectName.Api.Services.Module;

/// <summary>
/// 模块服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public class ModuleService : BaseService, IModuleService, IDynamicApi
{
    private IModuleRepository _moduleRepository => LazyGetRequiredService<IModuleRepository>();

    public ModuleService()
    {
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <returns></returns>
    public void ExecuteTask()
    {
        var scheduler = LazyGetRequiredService<Scheduler>();
        //方式1：添加任务组，第一组每次间隔15秒，第二组每次间隔2分钟
        scheduler.AddTask(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), new int[] { 15, 15, 120, 120 });

        //方式2：添加任务，每次间隔15秒
        scheduler.AddTask(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 2, 15);

        //方式3：无限循环任务，每次间隔10分钟
        scheduler.AddTask(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), -1, 600);

        //方式4：每天凌晨执行一次
        scheduler.AddTaskRunOnDay(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 1, "0:00:00");

        //方式5：每周一晚上11点半执行一次，0为周日，1-6为周一至周六
        scheduler.AddTaskRunOnWeek(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 1, "1:23:30:00");

        //方式6：每个月1号下午4点执行1次
        scheduler.AddTaskRunOnMonth(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), 1, "1:16:00:00");

        //方式7：自定义cron表达式，从0秒开始每10秒执行一次
        scheduler.AddTaskCustom(TaskNames.ModuleTaskName, JsonConvert.SerializeObject(new
        {
            moduleId = 1
        }), "0/10 * * * * ?");
    }
}
```
