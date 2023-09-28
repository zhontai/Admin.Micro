using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Api.Core.GrpcServices;

namespace ZhonTai.Api.Core.Auth;

/// <summary>
/// 权限处理
/// </summary>
public class PermissionHandler : IPermissionHandler
{
    private readonly IUser _user;
    private readonly IUserGrpcService _userGrpcService;

    public PermissionHandler(IUser user, IUserGrpcService userGrpcService)
    {
        _user = user;
        _userGrpcService = userGrpcService;
    }

    /// <summary>
    /// 权限验证
    /// </summary>
    /// <param name="api">接口路径</param>
    /// <param name="httpMethod">http请求方法</param>
    /// <returns></returns>
    public async Task<bool> ValidateAsync(string api, string httpMethod)
    {
        if (_user.PlatformAdmin)
        {
            return true;
        }

        var res = await _userGrpcService.GetPermissionsAsync();

        var valid = res.Data.Any(m =>
            m.Path.NotNull() && m.Path.EqualsIgnoreCase($"/{api}")
            && m.HttpMethods.NotNull() && m.HttpMethods.Split(',').Any(n => n.NotNull() && n.EqualsIgnoreCase(httpMethod))
        );

        return valid;
    }
}