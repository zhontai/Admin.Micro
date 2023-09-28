using System;

namespace ZhonTai.Api.DynamicApi;

public static class FormatResultContext
{
    internal static Type _formatResultType = typeof(ResponseResult<>);
}