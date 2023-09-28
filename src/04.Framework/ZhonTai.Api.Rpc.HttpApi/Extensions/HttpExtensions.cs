using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Refit;
using System.Reflection;
using ZhonTai;
using ZhonTai.Api.Rpc.Configs;
using ZhonTai.Api.Rpc.Interfaces;
using ZhonTai.Api.Rpc.Attributes;
using ZhonTai.Api.Rpc.HttpApi.Handlers;
using Polly;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpExtensions
{
    public static IServiceCollection AddMyHttpClients(this IServiceCollection services, IEnumerable<Assembly> assemblies, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies)
    {
        ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        foreach (var assembly in assemblies)
        {
            var interfaceTypes = assembly.GetTypes()
            .Where(type => typeof(IHttpClientService).IsAssignableFrom(type) && type.IsInterface)
            .ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                var method = typeof(HttpExtensions)
                    .GetMethod(nameof(AddMyRefitClient))
                    ?.MakeGenericMethod(interfaceType)
                    ?.Invoke(null, new object[] { services, rpcConfig, policies });
            }
        }

        return services;
    }

    public static IServiceCollection AddMyRefitClient<T>(this IServiceCollection services, RpcConfig rpcConfig, List<IAsyncPolicy<HttpResponseMessage>> policies) where T : class, IHttpClientService
    {
        ArgumentNullException.ThrowIfNull(rpcConfig, nameof(rpcConfig));

        var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss.FFFFFFFK"
        }));

        services.TryAddScoped<ResponseDelegatingHandler>();

        services
        .AddRefitClient<T>(refitSettings)
        .AddHttpMessageHandler<ResponseDelegatingHandler>()
        .ConfigureHttpClient(c =>
        {
            var httpClientContractAttribute = typeof(T).GetCustomAttributes<HttpClientContractAttribute>(true).FirstOrDefault();
            if (httpClientContractAttribute is null)
                throw new NullReferenceException(nameof(httpClientContractAttribute));

            var address = rpcConfig.AddressList.FirstOrDefault(a => a.ModuleName.EqualsIgnoreCase(httpClientContractAttribute.ModuleName));
            if (address is null)
                throw new NullReferenceException(nameof(address));

            c.BaseAddress = new Uri(address.ApiUrl);

            var httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
            var authorization = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault();
            if (authorization.NotNull())
            {
                c.DefaultRequestHeaders.Add("Authorization", authorization);
            }
            var userAgent = httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].FirstOrDefault();
            if (userAgent.NotNull())
            {
                c.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }
        })
        .AddPolicyHandlerList(policies);

        return services;
    }
}
