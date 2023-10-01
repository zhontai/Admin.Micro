using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.Admin.HttpApi.Domain.Api;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 接口服务
/// </summary>
[Order(90)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ApiService : BaseService, IApiService, IDynamicApi
{
    private readonly IApiRepository _apiRepository;

    public ApiService(IApiRepository moduleRepository)
    {
        _apiRepository = moduleRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiGetResponse> GetAsync(long id)
    {
        var result = await _apiRepository.GetAsync<ApiGetResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<ApiGetListResponse>> GetListAsync(string key)
    {
        var data = await _apiRepository
            .WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<ApiGetListResponse>();

        return data;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ApiAddRequest request)
    {
        var path = request.Path;

        var entity = await _apiRepository.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(w => w.Path.Equals(path) && w.IsDeleted).FirstAsync();

        if (entity?.Id > 0)
        {
            Mapper.Map(request, entity);
            entity.IsDeleted = false;
            entity.Enabled = true;
            await _apiRepository.UpdateDiy.DisableGlobalFilter(FilterNames.Delete).SetSource(entity).ExecuteAffrowsAsync();

            return entity.Id;
        }
        entity = Mapper.Map<ApiEntity>(request);

        if (entity.Sort == 0)
        {
            var sort = await _apiRepository.Select.DisableGlobalFilter(FilterNames.Delete).Where(a => a.ParentId == request.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _apiRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ApiUpdatedRequest request)
    {
        var entity = await _apiRepository.GetAsync(request.Id);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("接口不存在！");
        }
        
        Mapper.Map(request, entity);
        await _apiRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _apiRepository.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _apiRepository.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task SoftDeleteAsync(long id)
    {
        await _apiRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _apiRepository.SoftDeleteAsync(ids);
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SyncAsync(ApiSyncdRequest request)
    {
        if (!(request?.Apis?.Count > 0)) return;

        //查询分组下所有模块的api
        var groupPaths = request.Apis.FindAll(a => a.ParentPath.IsNull()).Select(a => a.Path);
        var groups = await _apiRepository.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(a => a.ParentId == 0 && groupPaths.Contains(a.Path)).ToListAsync();
        var groupIds = groups.Select(a => a.Id);
        var modules = await _apiRepository.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(a => groupIds.Contains(a.ParentId)).ToListAsync();
        var moduleIds = modules.Select(a => a.Id);
        var apis = await _apiRepository.Select.DisableGlobalFilter(FilterNames.Delete)
            .Where(a=> moduleIds.Contains(a.ParentId)).ToListAsync();

        apis = groups.Concat(modules).Concat(apis).ToList();
        var paths = apis.Select(a => a.Path).ToList();

        //path处理
        foreach (var api in request.Apis)
        {
            api.Path = api.Path?.Trim().ToLower();
            api.ParentPath = api.ParentPath?.Trim().ToLower();
        }

        #region 执行插入

        //执行父级api插入
        var parentApis = request.Apis.FindAll(a => a.ParentPath.IsNull());
        var pApis = (from a in parentApis where !paths.Contains(a.Path) select a).ToList();
        if (pApis.Count > 0)
        {
            var insertPApis = Mapper.Map<List<ApiEntity>>(pApis);
            insertPApis = await _apiRepository.InsertAsync(insertPApis);
            apis.AddRange(insertPApis);
        }

        //执行子级api插入
        var childApis = request.Apis.FindAll(a => a.ParentPath.NotNull());
        var cApis = (from a in childApis where !paths.Contains(a.Path) select a).ToList();
        if (cApis.Count > 0)
        {
            var insertCApis = Mapper.Map<List<ApiEntity>>(cApis);
            insertCApis = await _apiRepository.InsertAsync(insertCApis);
            apis.AddRange(insertCApis);
        }

        #endregion 执行插入

        #region 修改和禁用

        {
            //父级api修改
            ApiEntity a;
            List<string> labels;
            string label;
            string desc;
            for (int i = 0, len = parentApis.Count; i < len; i++)
            {
                ApiSyncdRequest.Models.Api api = parentApis[i];
                a = apis.Find(a => a.Path == api.Path);
                if (a?.Id > 0)
                {
                    labels = api.Label?.Split("\r\n")?.ToList();
                    label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                    desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count - 1)) : string.Empty;
                    a.ParentId = 0;
                    a.Label = label;
                    a.Description = desc;
                    a.Sort = i + 1;
                    a.Enabled = true;
                    a.IsDeleted = false;
                }
            }
        }

        {
            //子级api修改
            ApiEntity a;
            ApiEntity pa;
            List<string> labels;
            string label;
            string desc;
            for (int i = 0, len = childApis.Count; i < len; i++)
            {
                ApiSyncdRequest.Models.Api api = childApis[i];
                a = apis.Find(a => a.Path == api.Path);
                pa = apis.Find(a => a.Path == api.ParentPath);
                if (a?.Id > 0)
                {
                    labels = api.Label?.Split("\r\n")?.ToList();
                    label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                    desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count - 1)) : string.Empty;

                    a.ParentId = pa.Id;
                    a.Label = label;
                    a.Description = desc;
                    a.HttpMethods = api.HttpMethods;
                    a.Sort = i + 1;
                    a.Enabled = true;
                    a.IsDeleted = false;
                }
            }
        }

        {
            //模块和api禁用
            var inputPaths = request.Apis.Select(a => a.Path).ToList();
            var disabledApis = (from a in apis where !inputPaths.Contains(a.Path) select a).ToList();
            if (disabledApis.Count > 0)
            {
                foreach (var api in disabledApis)
                {
                    api.Enabled = false;
                }
            }
        }

        #endregion 修改和禁用

        //批量更新
        await _apiRepository.UpdateDiy.DisableGlobalFilter(FilterNames.Delete).SetSource(apis)
        .UpdateColumns(a => new { a.ParentId, a.Label, a.HttpMethods, a.Description, a.Sort, a.Enabled, a.IsDeleted, a.ModifiedTime })
        .ExecuteAffrowsAsync();
    }
}