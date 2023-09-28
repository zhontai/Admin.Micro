using Mapster;
using ProtoBuf.Grpc;
using ZhonTai.Api.Core.GrpcServices;

namespace ZhonTai.Module.Admin.HttpApi.GrpcServices;

public class OprationLogGrpcService : IOprationLogGrpcService
{
    private readonly IOprationLogService _oprationLogService;

    public OprationLogGrpcService(IOprationLogService oprationLogService)
    {
        _oprationLogService = oprationLogService;
    }

    public async Task<ProtoLong> AddAsync(OprationLogAddGrpcRequest request, CallContext context = default)
    {
        return await _oprationLogService.AddAsync(request.Adapt<OprationLogAddRequest>());
    }
}
