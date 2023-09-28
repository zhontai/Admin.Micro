using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class DocUpdateGroupRequest : DocAddGroupRequest
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择分组")]
    public long Id { get; set; }
}