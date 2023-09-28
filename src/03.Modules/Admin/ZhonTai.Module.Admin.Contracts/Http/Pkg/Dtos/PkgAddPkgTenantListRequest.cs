using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 添加套餐租户列表
/// </summary>
public class PkgAddPkgTenantListRequest
{
    /// <summary>
    /// 套餐
    /// </summary>
    [Required(ErrorMessage = "请选择套餐")]
    public long PkgId { get; set; }

    /// <summary>
    /// 租户列表
    /// </summary>
    public long[] TenantIds { get; set; }
}