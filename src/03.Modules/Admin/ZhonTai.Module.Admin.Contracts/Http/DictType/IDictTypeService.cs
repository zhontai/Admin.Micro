namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 数据字典类型接口
/// </summary>
public partial interface IDictTypeService
{
    Task<DictTypeGetResponse> GetAsync(long id);

    Task<PageResponse<DictTypeGetPageResponse>> GetPageAsync(PageRequest<DictTypeGetPageFilter> request);

    Task<long> AddAsync(DictTypeAddRequest request);

    Task UpdateAsync(DictTypeUpdateRequest request);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}