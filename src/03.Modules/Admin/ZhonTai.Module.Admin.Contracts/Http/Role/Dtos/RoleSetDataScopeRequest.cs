using System.ComponentModel.DataAnnotations;
using ZhonTai.Api.Rpc.Enums;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 设置数据范围
/// </summary>
public class RoleSetDataScopeRequest
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择角色")]
    public long RoleId { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; }

    /// <summary>
    /// 指定部门
    /// </summary>
    public List<long> OrgIds { get; set; }
}