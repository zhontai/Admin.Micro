using Microsoft.AspNetCore.Http;
using ZhonTai.Utils.Helpers;
using ZhonTai.Module.Admin.HttpApi.Domain.LoginLog;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 登录日志服务
/// </summary>
[Order(190)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class LoginLogService : BaseService, ILoginLogService, IDynamicApi
{
    private readonly IHttpContextAccessor _context;
    private readonly ILoginLogRepository _loginLogRepository;

    public LoginLogService(
        IHttpContextAccessor context,
        ILoginLogRepository loginLogRepository
    )
    {
        _context = context;
        _loginLogRepository = loginLogRepository;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<LoginLogGetPageResponse>> GetPageAsync(PageRequest<LoginLogGetPageFilter> request)
    {
        var userName = request.Filter?.CreatedUserName;

        var list = await _loginLogRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(userName.NotNull(), a => a.CreatedUserName.Contains(userName))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync<LoginLogGetPageResponse>();

        var data = new PageResponse<LoginLogGetPageResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(LoginLogAddRequest request)
    {
        request.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

        string ua = _context.HttpContext.Request.Headers["User-Agent"];
        if (ua.NotNull())
        {
            var client = UAParser.Parser.GetDefault().Parse(ua);
            var device = client.Device.Family;
            device = device.ToLower() == "other" ? "" : device;
            request.Browser = client.UA.Family;
            request.Os = client.OS.Family;
            request.Device = device;
            request.BrowserInfo = ua;
        }
        var entity = Mapper.Map<LoginLogEntity>(request);
        await _loginLogRepository.InsertAsync(entity);

        return entity.Id;
    }
}