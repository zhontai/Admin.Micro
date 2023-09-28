using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class PermissionAssignRequest
{
    [Required(ErrorMessage = "角色不能为空！")]
    public long RoleId { get; set; }

    [Required(ErrorMessage = "权限不能为空！")]
    public List<long> PermissionIds { get; set; }
}