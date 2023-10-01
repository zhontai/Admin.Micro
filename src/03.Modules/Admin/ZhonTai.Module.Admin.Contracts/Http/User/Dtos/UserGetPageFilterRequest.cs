namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 用户分页查询条件
/// </summary>
public class UserGetPageFilterRequest
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public long? OrgId { get; set; }
}
