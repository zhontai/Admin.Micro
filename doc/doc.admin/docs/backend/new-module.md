# 新建模块

### 1、新建模块实体类

在 MyCompanyName.MyProjectName.Api\Domain 目录下新建目录`Module`，并在`Module`目录下新建实体类`ModuleEntity`

```cs
using ZhonTai.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using System;

namespace MyCompanyName.MyProjectName.Api.Domain.Module
{
    /// <summary>
    /// 模块
    /// </summary>
	[Table(Name = "app_module")]
    [Index("idx_{tablename}_01", nameof(Name) + "," + nameof(TenantId), true)]
    public partial class ModuleEntity : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public long? TenantId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }
    }
}
```

> 约定：实体类以 Entity 命名结尾，Module 可替换成实际业务模块名

### 2、新建模块仓储接口

在 MyCompanyName.MyProjectName.Api\Domain\Module 目录下新建仓储接口`IModuleRepository`

```cs
using ZhonTai.Admin.Core.Repositories;

namespace MyCompanyName.MyProjectName.Api.Domain.Module;

public interface IModuleRepository : IRepositoryBase<ModuleEntity>
{
}
```

### 3、新建模块仓储

在 MyCompanyName.MyProjectName.Api\Repositories 目录下新建目录`Module`，并在`Module`目录下新建仓储`ModuleRepository`实现模块仓储接口`IModuleRepository`

```cs
using MyCompanyName.MyProjectName.Api.Core.Repositories;
using MyCompanyName.MyProjectName.Api.Domain.Module;
using ZhonTai.Admin.Core.Db.Transaction;

namespace MyCompanyName.MyProjectName.Api.Repositories;

public class ModuleRepository : AppRepositoryBase<ModuleEntity>, IModuleRepository
{
    public ModuleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
```

::: tip 提示
模块仓储需继承仓储 AppRepositoryBase，使用当前项目主库操作模块
:::

### 4、新建模块服务

在 MyCompanyName.MyProjectName.Api\Services 目录下新建目录`Module`，并在`Module`目录下新建模块服务`ModuleService`

```cs
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using MyCompanyName.MyProjectName.Api.Domain.Module;
using MyCompanyName.MyProjectName.Api.Domain.Module.Dto;
using MyCompanyName.MyProjectName.Api.Services.Module.Input;
using MyCompanyName.MyProjectName.Api.Services.Module.Output;
using MyCompanyName.MyProjectName.Api.Core.Consts;
using Newtonsoft.Json;
using ZhonTai;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using FreeScheduler;

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
    /// 查询模块
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ModuleGetOutput> GetAsync(long id)
    {
        var result = await _moduleRepository.GetAsync<ModuleGetOutput>(id);
        return result;
    }

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

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ModuleAddInput input)
    {
        var entity = Mapper.Map<ModuleEntity>(input);
        await _moduleRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ModuleUpdateInput input)
    {
        var entity = await _moduleRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("模块不存在");
        }

        Mapper.Map(input, entity);
        await _moduleRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _moduleRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _moduleRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _moduleRepository.SoftDeleteAsync(ids);
    }
}
```

如果使用传统 Api，还要在`Module`目录下新建`IModuleService`仓储接口

```cs
using ZhonTai.Admin.Core.Dto;
using System.Threading.Tasks;
using MyCompanyName.MyProjectName.Api.Domain.Module.Dto;
using MyCompanyName.MyProjectName.Api.Services.Module.Input;
using MyCompanyName.MyProjectName.Api.Services.Module.Output;

namespace MyCompanyName.MyProjectName.Api.Services.Module;

public interface IModuleService
{
    Task<ModuleGetOutput> GetAsync(long id);

    Task<PageOutput<ModuleListOutput>> GetPageAsync(PageInput<ModuleGetPageDto> input);

    Task<long> AddAsync(ModuleAddInput input);

    Task UpdateAsync(ModuleUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}
```

> 使用动态 Api 时，可以不添加模块服务接口 IModuleService

### 5、新建输入输出数据传输对象

在 MyCompanyName.MyProjectName.Api\Domain 或在 MyCompanyName.MyProjectName.Api\Services 目录下新建`Input`输入或`Output`输出目录

在`Input`目录下新建添加输入 Dto`ModuleAddInput`

```cs
namespace MyCompanyName.MyProjectName.Api.Services.Module.Input;

/// <summary>
/// 添加
/// </summary>
public class ModuleAddInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}
```

在`Output`目录下新建查询输出 Dto`ModuleGetOutput`

```cs
using MyCompanyName.MyProjectName.Api.Services.Module.Input;

namespace MyCompanyName.MyProjectName.Api.Services.Module.Output;

public class ModuleGetOutput : ModuleUpdateInput
{
}
```

### 6、接口权限说明

- [Login]标记用户需登录才能访问，该标记方便测试未授权的接口

```cs
[Login]
public async Task<ModuleGetOutput> GetAsync(long id)
{
    var result = await _moduleRepository.GetAsync<ModuleGetOutput>(id);
    return result;
}
```

- [AllowAnonymous]标记用户可匿名访问，该标记方便测试未授权且未登录的接口

```cs
[AllowAnonymous]
public async Task<ModuleGetOutput> GetAsync(long id)
{
    var result = await _moduleRepository.GetAsync<ModuleGetOutput>(id);
    return result;
}
```

::: tip 注意
授权接口需删除[Login]和[AllowAnonymous]标记
:::
