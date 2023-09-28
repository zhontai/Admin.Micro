using ProtoBuf;

namespace ZhonTai.Api.Rpc.Grpc.ProtoTypes;

/// <summary>
/// Grpc响应
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class GrpcResponse<T>
{
    /// <summary>
    /// 是否成功标记
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T Data { get; set; }
}
