using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 认证授权接口
/// </summary>
public interface IAuthService
{
    string GetToken(AuthGetTokenRequest user);

    Task<dynamic> LoginAsync(AuthLoginRequest request);

    Task<AuthGetUserInfoResponse> GetUserInfoAsync();

    Task<AuthGetPasswordEncryptKeyResponse> GetPasswordEncryptKeyAsync();

    Task<dynamic> Refresh([Required] string token);
}