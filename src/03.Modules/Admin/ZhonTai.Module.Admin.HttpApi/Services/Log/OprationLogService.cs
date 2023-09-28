using Microsoft.AspNetCore.Http;
using ZhonTai.Utils.Helpers;
using ZhonTai.Module.Admin.HttpApi.Domain.OprationLog;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 操作日志服务
/// </summary>
[Order(200)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OprationLogService : BaseService, IOprationLogService, IDynamicApi
{
    private readonly IHttpContextAccessor _context;
    private readonly IOprationLogRepository _oprationLogRepository;

    public OprationLogService(
        IHttpContextAccessor context,
        IOprationLogRepository oprationLogRepository
    )
    {
        _context = context;
        _oprationLogRepository = oprationLogRepository;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<OprationLogGetPageResponse>> GetPageAsync(PageRequest<OprationLogGetPageFilter> request)
    {
        var userName = request.Filter?.CreatedUserName;

        var list = await _oprationLogRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(userName.NotNull(), a => a.CreatedUserName.Contains(userName))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync<OprationLogGetPageResponse>();

        var data = new PageResponse<OprationLogGetPageResponse>()
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
    public async Task<long> AddAsync(OprationLogAddRequest request)
    {
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

        request.Name = User.Name;
        request.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

        var entity = Mapper.Map<OprationLogEntity>(request);
        await _oprationLogRepository.InsertAsync(entity);

        return entity.Id;
    }
}