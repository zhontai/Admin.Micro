
using Newtonsoft.Json;
using ZhonTai.Api.Rpc.Dtos;
using ZhonTai.Api.Rpc.Exceptions;

namespace ZhonTai.Api.Rpc.HttpApi.Handlers;

/// <summary>
/// 响应处理器
/// </summary>
public class ResponseDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonConvert.DeserializeObject<Response<object>>(content);
            if (!res.Success && res.Msg.NotNull())
            {
                throw new AppException(res.Msg);
            }

            response.Content = new StringContent(JsonConvert.SerializeObject(res.Data));
        }

        return response;
    }
}
