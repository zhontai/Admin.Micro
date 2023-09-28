using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 添加角色用户列表
/// </summary>
public class RoleAddRoleUserListRequest
{
    /// <summary>
    /// 角色
    /// </summary>
    [Required(ErrorMessage = "请选择角色")]
    public long RoleId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public long[] UserIds { get; set; }
}