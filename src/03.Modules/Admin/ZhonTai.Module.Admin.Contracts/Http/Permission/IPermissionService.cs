namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 权限接口
/// </summary>
public partial interface IPermissionService
{
    Task<PermissionGetGroupResponse> GetGroupAsync(long id);

    Task<PermissionGetMenuResponse> GetMenuAsync(long id);

    Task<PermissionGetApiResponse> GetApiAsync(long id);

    Task<PermissionGetDotResponse> GetDotAsync(long id);

    Task<IEnumerable<dynamic>> GetPermissionListAsync();

    Task<List<long>> GetRolePermissionListAsync(long roleId);

    Task<List<long>> GetTenantPermissionListAsync(long tenantId);

    Task<List<PermissionGetListResponse>> GetListAsync(string key, DateTime? start, DateTime? end);

    Task<long> AddGroupAsync(PermissionAddGroupRequest input);

    Task<long> AddMenuAsync(PermissionAddMenuRequest input);

    Task<long> AddApiAsync(PermissionAddApiRequest input);

    Task<long> AddDotAsync(PermissionAddDotRequest input);

    Task UpdateGroupAsync(PermissionUpdateGroupRequest input);

    Task UpdateMenuAsync(PermissionUpdateMenuRequest input);

    Task UpdateApiAsync(PermissionUpdateApiRequest input);

    Task UpdateDotAsync(PermissionUpdateDotRequest input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task AssignAsync(PermissionAssignRequest input);

    Task SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsRequest input);
}