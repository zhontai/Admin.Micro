using ZhonTai.Module.Admin.HttpApi.Domain.Dict;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;
using Microsoft.AspNetCore.Authorization;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 数据字典服务
/// </summary>
[Order(60)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictService : BaseService, IDictService, IDynamicApi
{
    private readonly IDictRepository _dictRepository;

    public DictService(IDictRepository dictRepository)
    {
        _dictRepository = dictRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictGetResponse> GetAsync(long id)
    {
        var result = await _dictRepository.GetAsync<DictGetResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<DictGetPageResponse>> GetPageAsync(PageRequest<DictGetPageFilterRequest> request)
    {
        var key = request.Filter?.Name;
        var dictTypeId = request.Filter?.DictTypeId;
        var list = await _dictRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(dictTypeId.HasValue && dictTypeId.Value > 0, a => a.DictTypeId == dictTypeId)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderByDescending(a => a.Sort)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync<DictGetPageResponse>();

        var data = new PageResponse<DictGetPageResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="codes">字典类型编码列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListResponse>>> GetListAsync(string[] codes)
    {
        var list = await _dictRepository.Select
        .Where(a => codes.Contains(a.DictType.Code) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListResponse { DictTypeCode = a.DictType.Code });

        var dicts = new Dictionary<string, List<DictGetListResponse>>();
        foreach (var code in codes)
        {
            if (code.NotNull())
                dicts[code] = list.Where(a => a.DictTypeCode == code).ToList();
        }

        return dicts;
    }

    /// <summary>
    /// 根据字典类型名称列表查询字典列表
    /// </summary>
    /// <param name="names">字典类型名称列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListResponse>>> GetListByNamesAsync(string[] names)
    {
        var list = await _dictRepository.Select
        .Where(a => names.Contains(a.DictType.Name) && a.DictType.Enabled == true && a.Enabled == true)
        .OrderBy(a => a.Sort)
        .ToListAsync(a => new DictGetListResponse { DictTypeName = a.DictType.Name });

        var dicts = new Dictionary<string, List<DictGetListResponse>>();
        foreach (var name in names)
        {
            if (name.NotNull())
                dicts[name] = list.Where(a => a.DictTypeName == name).ToList();
        }

        return dicts;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictAddRequest request)
    {
        if (await _dictRepository.Select.AnyAsync(a => a.DictTypeId == request.DictTypeId && a.Name == request.Name))
        {
            throw Response.Exception($"字典已存在");
        }

        if (request.Code.NotNull() && await _dictRepository.Select.AnyAsync(a => a.DictTypeId == request.DictTypeId && a.Code == request.Code))
        {
            throw Response.Exception($"字典编码已存在");
        }

        if (request.Code.NotNull() && await _dictRepository.Select.AnyAsync(a => a.DictTypeId == request.DictTypeId && a.Code == request.Code))
        {
            throw Response.Exception($"字典编码已存在");
        }

        if (request.Value.NotNull() && await _dictRepository.Select.AnyAsync(a => a.DictTypeId == request.DictTypeId && a.Value == request.Value))
        {
            throw Response.Exception($"字典值已存在");
        }

        var entity = Mapper.Map<DictEntity>(request);
        if (entity.Sort == 0)
        {
            var sort = await _dictRepository.Select.Where(a => a.DictTypeId == request.DictTypeId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictRepository.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictUpdateRequest request)
    {
        var entity = await _dictRepository.GetAsync(request.Id);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("字典不存在");
        }

        if (await _dictRepository.Select.AnyAsync(a => a.Id != request.Id && a.DictTypeId == request.DictTypeId && a.Name == request.Name))
        {
            throw Response.Exception($"字典已存在");
        }

        if (request.Code.NotNull() && await _dictRepository.Select.AnyAsync(a => a.Id != request.Id && a.DictTypeId == request.DictTypeId && a.Code == request.Code))
        {
            throw Response.Exception($"字典编码已存在");
        }

        if (request.Value.NotNull() && await _dictRepository.Select.AnyAsync(a => a.Id != request.Id && a.DictTypeId == request.DictTypeId && a.Value == request.Value))
        {
            throw Response.Exception($"字典值已存在");
        }

        Mapper.Map(request, entity);
        await _dictRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _dictRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _dictRepository.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _dictRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictRepository.SoftDeleteAsync(ids);
    }
}