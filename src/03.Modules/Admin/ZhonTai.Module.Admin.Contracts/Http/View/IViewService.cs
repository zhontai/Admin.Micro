namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 视图接口
/// </summary>
public interface IViewService
{
    Task<ViewGetResponse> GetAsync(long id);

    Task<List<ViewGetListResponse>> GetListAsync(string key);

    Task<long> AddAsync(ViewAddRequest input);

    Task UpdateAsync(ViewUpdateRequest input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ViewSyncRequest input);
}