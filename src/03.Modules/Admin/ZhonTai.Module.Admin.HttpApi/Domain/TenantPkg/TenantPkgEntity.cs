using ZhonTai.Api.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;
using ZhonTai.Module.Admin.HttpApi.Domain.Pkg;

namespace ZhonTai.Module.Admin.HttpApi.Domain.TenantPkg;

/// <summary>
/// 租户套餐
/// </summary>
[Table(Name = "ad_tenant_pkg")]
[Index("idx_{tablename}_01", nameof(TenantId) + "," + nameof(PkgId), true)]
public class TenantPkgEntity : EntityAdd
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }

    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 套餐Id
    /// </summary>
    public long PkgId { get; set; }

    public PkgEntity Pkg { get; set; }
}