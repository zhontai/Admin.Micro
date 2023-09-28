using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 修改
/// </summary>
public class DictTypeUpdateRequest : DictTypeAddRequest
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择数据字典类型")]
    public long Id { get; set; }
}