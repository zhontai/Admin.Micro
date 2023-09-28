namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 租户接口
/// </summary>
public interface ITenantService
{
    Task<TenantGetResponse> GetAsync(long id);

    Task<PageResponse<TenantGetPageResponse>> GetPageAsync(PageRequest<TenantGetPageFilter> request);

    Task<long> AddAsync(TenantAddRequest request);

    Task UpdateAsync(TenantUpdateRequest request);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}