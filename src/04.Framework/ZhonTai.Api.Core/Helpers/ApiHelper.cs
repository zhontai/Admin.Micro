using System.Collections.Generic;
using System.Linq;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.GrpcServices;

namespace ZhonTai.Api.Core.Helpers;

/// <summary>
/// Api帮助类
/// </summary>
[SingleInstance]
public class ApiHelper
{
    private List<ApiHelperDto> _apis;
    private static readonly object _lockObject = new();
    private readonly IApiGrpcService _apiGrpcService;

    public ApiHelper(IApiGrpcService apiGrpcService)
    {
        _apiGrpcService = apiGrpcService;
    }

    public List<ApiHelperDto> GetApis()
    {
        if (_apis != null && _apis.Any())
            return _apis;

        lock (_lockObject)
        {
            if (_apis != null && _apis.Any())
                return _apis;

            _apis = new List<ApiHelperDto>();

            var res = _apiGrpcService.GetApis();
            var apis = res.Data;
            foreach (var api in apis)
            {
                var parentLabel = apis.FirstOrDefault(a => a.Id == api.ParentId)?.Label;

                _apis.Add(new ApiHelperDto
                {
                    Label = parentLabel.NotNull() ? $"{parentLabel} / {api.Label}" : api.Label,
                    Path = api.Path?.ToLower().Trim('/')
                });
            }

            return _apis;
        }
    }
}

public class ApiHelperDto
{
    /// <summary>
    /// 接口名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; }
}