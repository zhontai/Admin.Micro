using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.Admin.HttpApi.Domain.DictType;
using ZhonTai.Module.Admin.HttpApi.Domain.Dict;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 数据字典类型服务
/// </summary>
[Order(61)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictTypeService : BaseService, IDictTypeService, IDynamicApi
{
    private readonly IDictTypeRepository _dictTypeRepository;
    private readonly IDictRepository _dictRepository;
    public DictTypeService(IDictTypeRepository DictionaryTypeRepository, IDictRepository dictRepository)
    {
        _dictTypeRepository = DictionaryTypeRepository;
        _dictRepository = dictRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictTypeGetResponse> GetAsync(long id)
    {
        var result = await _dictTypeRepository.GetAsync<DictTypeGetResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<DictTypeGetPageResponse>> GetPageAsync(PageRequest<DictTypeGetPageFilterRequest> request)
    {
        var key = request.Filter?.Name;

        var list = await _dictTypeRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .Count(out var total)
        .OrderByDescending(a => a.Sort)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync<DictTypeGetPageResponse>();

        var data = new PageResponse<DictTypeGetPageResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictTypeAddRequest request)
    {
        if (await _dictTypeRepository.Select.AnyAsync(a => a.Name == request.Name))
        {
            throw Response.Exception($"字典类型已存在");
        }

        if (request.Code.NotNull() && await _dictTypeRepository.Select.AnyAsync(a => a.Code == request.Code))
        {
            throw Response.Exception($"字典类型编码已存在");
        }

        var entity = Mapper.Map<DictTypeEntity>(request);
        if (entity.Sort == 0)
        {
            var sort = await _dictRepository.Select.MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictTypeRepository.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictTypeUpdateRequest request)
    {
        var entity = await _dictTypeRepository.GetAsync(request.Id);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("数据字典不存在");
        }

        if (await _dictTypeRepository.Select.AnyAsync(a => a.Id != request.Id && a.Name == request.Name))
        {
            throw Response.Exception($"字典类型已存在");
        }

        if (request.Code.NotNull() && await _dictTypeRepository.Select.AnyAsync(a => a.Id != request.Id && a.Code == request.Code))
        {
            throw Response.Exception($"字典类型编码已存在");
        }

        Mapper.Map(request, entity);
        await _dictTypeRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //删除字典数据
        await _dictRepository.DeleteAsync(a => a.DictTypeId == id);

        //删除数据字典类型
        await _dictTypeRepository.DeleteAsync(a => a.Id == id);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        //删除字典数据
        await _dictRepository.DeleteAsync(a => ids.Contains(a.DictTypeId));

        //删除数据字典类型
        await _dictTypeRepository.DeleteAsync(a => ids.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _dictRepository.SoftDeleteAsync(a => a.DictTypeId == id);
        await _dictTypeRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _dictRepository.SoftDeleteAsync(a => ids.Contains(a.DictTypeId));
        await _dictTypeRepository.SoftDeleteAsync(ids);
    }
}