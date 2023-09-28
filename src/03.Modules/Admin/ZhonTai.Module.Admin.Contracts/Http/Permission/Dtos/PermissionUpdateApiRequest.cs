using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class PermissionUpdateApiRequest : PermissionAddApiRequest
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择接口")]
    public long Id { get; set; }
}