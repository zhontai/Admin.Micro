using ProtoBuf;

namespace ZhonTai.Api.Core.GrpcServices;

/// <summary>
/// 接口
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ApiGrpcResponse
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 所属模块
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; }
}