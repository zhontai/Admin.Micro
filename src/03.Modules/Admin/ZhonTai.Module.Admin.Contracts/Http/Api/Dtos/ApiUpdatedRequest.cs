using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 修改
/// </summary>
public partial class ApiUpdatedRequest : ApiAddRequest
{
    /// <summary>
    /// 接口Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择接口")]
    public long Id { get; set; }
}