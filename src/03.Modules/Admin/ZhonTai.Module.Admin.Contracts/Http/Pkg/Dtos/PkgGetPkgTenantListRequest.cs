namespace ZhonTai.Module.Admin.Contracts.Http;

public partial class PkgGetPkgTenantListRequest
{
    /// <summary>
    /// 租户名
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 套餐Id
    /// </summary>
    public long? PkgId { get; set; }
}