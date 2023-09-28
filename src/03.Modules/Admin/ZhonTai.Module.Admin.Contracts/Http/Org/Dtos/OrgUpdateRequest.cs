using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 修改
/// </summary>
public class OrgUpdateRequest : OrgAddRequest
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择部门")]
    public long Id { get; set; }
}