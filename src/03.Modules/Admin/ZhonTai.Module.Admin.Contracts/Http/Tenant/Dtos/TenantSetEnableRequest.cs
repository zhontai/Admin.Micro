namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 设置启用
/// </summary>
public class TenantSetEnableRequest
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}