namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// api接口
/// </summary>
public interface IApiService
{
    Task<ApiGetResponse> GetAsync(long id);

    Task<List<ApiGetListResponse>> GetListAsync(string key);

    Task<long> AddAsync(ApiAddRequest request);

    Task UpdateAsync(ApiUpdatedRequest request);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ApiSyncdRequest request);
}