using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 修改
/// </summary>
public class ViewUpdateRequest : ViewAddRequest
{
    /// <summary>
    /// 视图Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择视图")]
    public long Id { get; set; }
}