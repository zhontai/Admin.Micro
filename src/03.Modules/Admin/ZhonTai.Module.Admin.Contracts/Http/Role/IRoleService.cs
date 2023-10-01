namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 角色接口
/// </summary>
public interface IRoleService
{
    Task<RoleGetResponse> GetAsync(long id);

    Task<List<RoleGetListResponse>> GetListAsync(RoleGetListRequest input);

    Task<PageResponse<RoleGetPageResponse>> GetPageAsync(PageRequest<RoleGetPageFilterRequest> input);

    Task<long> AddAsync(RoleAddRequest input);

    Task AddRoleUserAsync(RoleAddRoleUserListRequest input);

    Task RemoveRoleUserAsync(RoleAddRoleUserListRequest input);

    Task UpdateAsync(RoleUpdateRequest input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SetDataScopeAsync(RoleSetDataScopeRequest input);
}