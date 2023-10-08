using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 用户接口
/// </summary>
public interface IUserService: IUserClientService
{
    Task<UserGetResponse> GetAsync(long id);

    Task<DataPermissionResponse> GetDataPermissionAsync();

    Task<long> AddAsync(UserAddRequest input);

    Task<long> AddMemberAsync(UserAddMemberRequest input);

    Task UpdateAsync(UserUpdateRequest input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task ChangePasswordAsync(UserChangePasswordRequest input);

    Task<string> ResetPasswordAsync(UserResetPasswordRequest input);

    Task SetManagerAsync(UserSetManagerRequest input);

    Task UpdateBasicAsync(UserUpdateBasicRequest input);

    Task<UserGetBasicResponse> GetBasicAsync();

    Task<IList<UserPermissionsResponse>> GetPermissionsAsync();

    Task<string> AvatarUpload([FromForm] IFormFile file, bool autoUpdate = false);

    Task<dynamic> OneClickLoginAsync(string userName);
}