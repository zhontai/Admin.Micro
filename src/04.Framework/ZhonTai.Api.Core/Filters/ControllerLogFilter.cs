﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Handlers;

namespace ZhonTai.Api.Core.Filters;

/// <summary>
/// 控制器操作日志记录
/// </summary>
public class ControllerLogFilter : IAsyncActionFilter
{
    private readonly ILogHandler _logHandler;

    public ControllerLogFilter(ILogHandler logHandler)
    {
        _logHandler = logHandler;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(NoOprationLogAttribute)))
        {
            await next();
            return;
        }

        await _logHandler.LogAsync(context, next);
    }
}