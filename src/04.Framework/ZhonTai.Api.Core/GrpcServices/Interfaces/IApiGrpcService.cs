using ProtoBuf.Grpc;
using System.ServiceModel;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Api.Core.GrpcServices;

/// <summary>
/// Api服务接口
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IApiGrpcService
{
    GrpcResponse<List<ApiGrpcResponse>> GetApis(CallContext context = default);
}
