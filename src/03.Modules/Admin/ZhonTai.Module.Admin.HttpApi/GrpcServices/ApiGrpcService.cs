using ProtoBuf.Grpc;
using ZhonTai.Api.Core.GrpcServices;
using ZhonTai.Module.Admin.HttpApi.Domain.Api;

namespace ZhonTai.Module.Admin.HttpApi.GrpcServices;

public class ApiGrpcService : IApiGrpcService
{
    private readonly IApiRepository _apiRepository;

    public ApiGrpcService(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    public GrpcResponse<List<ApiGrpcResponse>> GetApis(CallContext context = default)
    {
        var data = _apiRepository.Select.ToList<ApiGrpcResponse>();
        return new GrpcResponse<List<ApiGrpcResponse>>()
        {
            Data = data
        };
    }
}
