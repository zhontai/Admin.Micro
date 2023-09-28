namespace ZhonTai.Module.Admin.Contracts.Http;

public partial interface IOrgService
{
    Task<OrgGetResponse> GetAsync(long id);

    Task<List<OrgGetListResponse>> GetListAsync(string key);

    Task<long> AddAsync(OrgAddRequest request);

    Task UpdateAsync(OrgUpdateRequest request);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);
}