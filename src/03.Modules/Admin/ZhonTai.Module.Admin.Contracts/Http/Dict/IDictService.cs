namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 数据字典接口
/// </summary>
public partial interface IDictService
{
    Task<DictGetResponse> GetAsync(long id);

    Task<PageResponse<DictGetPageResponse>> GetPageAsync(PageRequest<DictGetPageFilter> request);

    Task<long> AddAsync(DictAddRequest request);

    Task UpdateAsync(DictUpdateRequest request);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}