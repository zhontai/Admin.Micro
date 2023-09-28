using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class PkgSetPkgPermissionsRequest
{
    [Required(ErrorMessage = "套餐不能为空！")]
    public long PkgId { get; set; }

    [Required(ErrorMessage = "权限不能为空！")]
    public List<long> PermissionIds { get; set; }
}