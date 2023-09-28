# 动态 Api

## 配置动态 Api

在 MyApp.Host 项目下的`Program.cs`文件中配置动态 Api

```cs
new HostApp(new HostAppOptions
{
    ConfigureDynamicApi = options =>
    {
        // 接口以小驼峰方式命名
        options.NamingConvention = NamingConventionEnum.CamelCase;

        // 指定全局默认的 api 前缀
        options.DefaultApiPrefix = "api";

        /*
        * 清空API结尾，不删除API结尾;
        * 若不清空 CreatUserAsync 将变为 CreateUser
        */
        options.RemoveActionPostfixes.Clear();

        // 自定义 ActionName 处理函数;
        options.GetRestFulActionName = (actionName) => actionName;


        // 指定程序集 配置所有的api请求方式都为 POST
        options.AddAssemblyOptions(GetType().Assembly, httpVerb: "POST");

        /*
        * 指定程序集 配置 url 前缀为 api
        * 如: http://localhost:8000/api/User/CreateUser
        */
        options.AddAssemblyOptions(GetType().Assembly, apiPreFix: "api");

        /*
        * 指定程序集 配置 url 前缀为 api, 且所有请求方式都为POST
        * 如: http://localhost:8000/api/User/CreateUser
        */
        options.AddAssemblyOptions(GetType().Assembly, apiPreFix: "api", httpVerb: "POST");

        //生成请求方式 ["方法名开头"] = "动词"
        AppConsts.HttpVerbs = new Dictionary<string, string>()
        {
           ["add"] = "POST",
           ["create"] = "POST",
           ["insert"] = "POST",
           ["submit"] = "POST",
           ["post"] = "POST",

           ["get"] = "GET",
           ["find"] = "GET",
           ["fetch"] = "GET",
           ["query"] = "GET",

           ["update"] = "PUT",
           ["change"] = "PUT",
           ["put"] = "PUT",
           ["batch"] = "PUT",

           ["delete"] = "DELETE",
           ["soft"] = "DELETE",
           ["remove"] = "DELETE",
           ["clear"] = "DELETE",
        };
    }
}).Run(args);
```

> 根据前缀动词没有匹配到请求方式则使用`HttpPost`方式请求

NamingConvention 接口命名可选值：

- CamelCase：小驼峰命名 camelCase
- PascalCase：大驼峰命名 PascalCase
- SnakeCase：蛇形命名 snake_case
- KebabCase：烤肉串命名 kebab-case
- ExtensionCase：拓展名命名 extension.case
- Custom：自定义接口名称，通过 options.GetRestFulActionName = (actionName) => actionName;设置

## 使用动态 Api

在服务上增加 **[DynamicApi(Area = ApiConsts.AreaName)]** 特性，且继承 IDynamicApi 接口自动生成动态 Api

```cs
/// <summary>
/// 模块服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public class ModuleService : BaseService, IModuleService, IDynamicApi
{
    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ModuleListOutput>> GetPageAsync(PageInput<ModuleGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _moduleRepository.Select
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<ModuleListOutput>();

        var data = new PageOutput<ModuleListOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }
}
```

::: tip 提示
ApiConsts.AreaName 定义/api/app/module/get 路径中的`app`

动态 Api 会根据前缀动词生成请求方式，使用`[HttpPost]`特性可更改请求方式。

新接口文档地址：http://localhost:8000/admin/index.html

swagger 接口文档地址：http://localhost:8000/admin/swagger/index.html

如果服务是动态 Api，开启事物的方法务必定义为`virtual`虚方法，才能正常使用事物拦截。
:::

## 禁用动态 Api

在服务方法上添加`[NonAction]`特性禁用生成动态 Api，变成服务公共方法使用

```cs
[NonAction]
public async Task BatchSoftDeleteAsync(long[] ids)
{
    await _moduleRepository.SoftDeleteAsync(ids);
}
```

## 动态 Api 重命名

在服务方法上添加以下特性重命名

> api 路径需使用绝对路径，才能正常重命名

```cs
[HttpPost(template: "/api/[area]/[controller]/[action]")]
public async Task BatchSoftDeleteAsync(long[] ids)
{
    await _moduleRepository.SoftDeleteAsync(ids);
}
```

## 动态 Api 排序

在服务上添加`[Order]`特性排序

> 值越小该服务越靠前

```cs
/// <summary>
/// 模块服务
/// </summary>
[Order(1010)]
[DynamicApi(Area = ApiConsts.AreaName)]
public class ModuleService : BaseService, IModuleService, IDynamicApi
{

}
```

在服务方法上添加`[Order]`特性排序

> 值越大该服务方法越靠前

```cs
[Order(1010)]
public async Task BatchSoftDeleteAsync(long[] ids)
{
    await _moduleRepository.SoftDeleteAsync(ids);
}
```

## 动态 Api 多分组

在服务上添加`[DynamicApi]`特性指定不同分组

```cs
/// <summary>
/// 模块服务
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName, GroupNames = new string[] { AdminConsts.AreaName })]
public class ModuleService : BaseService, IModuleService, IDynamicApi
{

}
```

在服务方法上添加`[ApiGroup]`特性指定不同分组

> 接口不分组：[ApiGroup(NonGroup = true)]

```cs
[ApiGroup(ApiConsts.AreaName, AdminConsts.AreaName)]
//[ApiGroup(GroupNames = new string[] { ApiConsts.AreaName, AdminConsts.AreaName })]
public async Task BatchSoftDeleteAsync(long[] ids)
{
    await _moduleRepository.SoftDeleteAsync(ids);
}
```
