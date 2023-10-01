namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 操作日志接口
/// </summary>
public interface IOprationLogService
{
    Task<PageResponse<OprationLogGetPageResponse>> GetPageAsync(PageRequest<OprationLogGetPageFilterRequest> input);

    Task<long> AddAsync(OprationLogAddRequest input);
}