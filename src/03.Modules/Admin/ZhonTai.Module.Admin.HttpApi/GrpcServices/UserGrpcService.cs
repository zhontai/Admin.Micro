using Mapster;
using ProtoBuf.Grpc;
using ZhonTai.Api.Core.GrpcServices;

namespace ZhonTai.Module.Admin.HttpApi.GrpcServices;

public class UserGrpcService : IUserGrpcService
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task GetDataPermissionAsync(CallContext context = default)
    {
        await _userService.GetDataPermissionAsync();
    }

    public async Task<GrpcResponse<List<UserPermissionsGrpcResponse>>> GetPermissionsAsync(CallContext context = default)
    {
        var userPermissions = await _userService.GetPermissionsAsync();
        var data = userPermissions.Adapt<List<UserPermissionsGrpcResponse>>();
        return new GrpcResponse<List<UserPermissionsGrpcResponse>>()
        {
            Data = data,
        };
    }
}
