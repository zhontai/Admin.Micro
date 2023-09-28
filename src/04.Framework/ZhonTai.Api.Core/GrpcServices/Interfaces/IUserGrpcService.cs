using ProtoBuf.Grpc;
using System.ServiceModel;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Api.Core.GrpcServices;

/// <summary>
/// 用户服务接口
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IUserGrpcService
{
    Task GetDataPermissionAsync(CallContext context = default);

    Task<GrpcResponse<List<UserPermissionsGrpcResponse>>> GetPermissionsAsync(CallContext context = default);
}
