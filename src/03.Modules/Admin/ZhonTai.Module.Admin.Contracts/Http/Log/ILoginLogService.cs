namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 登录日志接口
/// </summary>
public interface ILoginLogService
{
    Task<PageResponse<LoginLogGetPageResponse>> GetPageAsync(PageRequest<LoginLogGetPageFilterRequest> input);

    Task<long> AddAsync(LoginLogAddRequest input);
}