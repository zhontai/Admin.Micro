namespace ZhonTai.Module.Admin.Contracts.Http;

public partial class RoleGetRoleUserListRequest
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public long? RoleId { get; set; }
}