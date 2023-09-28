using ProtoBuf;

namespace ZhonTai.Api.Core.GrpcServices;

/// <summary>
/// 数据权限
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class DataPermissionGrpcResponse
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public List<long> OrgIds { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; } = DataScope.Self;
}