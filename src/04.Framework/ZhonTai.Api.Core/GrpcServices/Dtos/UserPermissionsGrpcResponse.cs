using ProtoBuf;

namespace ZhonTai.Api.Core.GrpcServices;

/// <summary>
/// 用户权限
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class UserPermissionsGrpcResponse
{
    public string HttpMethods { get; set; }

    public string Path { get; set; }
}
