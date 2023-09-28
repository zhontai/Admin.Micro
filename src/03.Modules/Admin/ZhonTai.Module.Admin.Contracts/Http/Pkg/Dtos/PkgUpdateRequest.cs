using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 修改
/// </summary>
public partial class PkgUpdateRequest : PkgAddRequest
{
    /// <summary>
    /// 套餐Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择套餐")]
    public long Id { get; set; }
}