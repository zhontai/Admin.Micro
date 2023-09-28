namespace ZhonTai.Module.Admin.Contracts.Http;

public class UserGetResponse : UserUpdateRequest
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public ICollection<UserGetRoleFilter> Roles { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public ICollection<UserGetOrgFilter> Orgs { get; set; }

    /// <summary>
    /// 所属部门Ids
    /// </summary>
    public override long[] OrgIds => Orgs?.Select(a => a.Id)?.ToArray();

    /// <summary>
    /// 角色Ids
    /// </summary>
    public override long[] RoleIds => Roles?.Select(a => a.Id)?.ToArray();
}