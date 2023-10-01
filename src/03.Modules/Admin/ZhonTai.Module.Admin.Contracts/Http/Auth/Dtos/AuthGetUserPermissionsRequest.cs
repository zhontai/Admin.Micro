namespace ZhonTai.Module.Admin.Contracts.Http;

public class AuthGetUserPermissionsResponse
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public AuthGetUserProfileResponse User { get; set; }

    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}