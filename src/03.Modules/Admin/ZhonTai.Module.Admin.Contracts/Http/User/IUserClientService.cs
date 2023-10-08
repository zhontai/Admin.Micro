using Refit;
using ZhonTai.Api.Rpc.Attributes;
using ZhonTai.Api.Rpc.Interfaces;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 用户客户端接口
/// </summary>
[HttpClientContract(HttpConsts.ModuleName)]
public interface IUserClientService : IHttpClientService
{
    [Post("/api/admin/user/get-page")]
    Task<PageResponse<UserGetPageResponse>> GetPageAsync(PageRequest<UserGetPageFilterRequest> input);
}