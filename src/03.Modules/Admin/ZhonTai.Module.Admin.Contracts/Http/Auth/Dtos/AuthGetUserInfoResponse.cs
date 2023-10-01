namespace ZhonTai.Module.Admin.Contracts.Http;

public class AuthGetUserInfoResponse
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public AuthGetUserProfileResponse User { get; set; }

    /// <summary>
    /// 用户菜单列表
    /// </summary>
    public List<AuthGetUserMenusResponse> Menus { get; set; }

    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}