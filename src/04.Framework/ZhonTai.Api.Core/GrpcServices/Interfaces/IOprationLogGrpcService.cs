using ProtoBuf.Grpc;
using System.ServiceModel;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Api.Core.GrpcServices;

/// <summary>
/// 操作日志服务接口
/// </summary>
[ServiceContract(ConfigurationName = AdminConsts.AreaName)]
public interface IOprationLogGrpcService
{
    Task<ProtoLong> AddAsync(OprationLogAddGrpcRequest request, CallContext context = default);
}
