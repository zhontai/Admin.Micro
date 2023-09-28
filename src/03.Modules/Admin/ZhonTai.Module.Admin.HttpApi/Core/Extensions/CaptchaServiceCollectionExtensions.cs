using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Generator;
using Lazy.SlideCaptcha.Core.Resources;
using Lazy.SlideCaptcha.Core.Resources.Handler;
using Lazy.SlideCaptcha.Core.Resources.Provider;
using Lazy.SlideCaptcha.Core.Storage;
using Lazy.SlideCaptcha.Core.Validator;
using Microsoft.Extensions.Configuration;
using ZhonTai.Api.Core;
using ZhonTai.Module.Admin.HttpApi.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class CaptchaServiceCollectionExtensions
{
    public static CaptchaBuilder AddMySlideCaptcha(this IServiceCollection services, IConfiguration configuration = null, Action<CaptchaOptions> optionsAction = null)
    {
        services.Configure<CaptchaOptions>(configuration ?? AppInfo.Configuration.GetSection("ThirdPartyConfig:SlideCaptcha"));
        if (optionsAction != null)
        {
            services.PostConfigure(optionsAction);
        }

        CaptchaBuilder result = new(services);
        services.AddSingleton<IResourceProvider, OptionsResourceProvider>();
        services.AddSingleton<IResourceProvider, EmbeddedResourceProvider>();
        services.AddSingleton<IResourceHandlerManager, CachedResourceHandlerManager>();
        services.AddSingleton<IResourceManager, DefaultResourceManager>();
        services.AddSingleton<IResourceHandler, FileResourceHandler>();
        services.AddSingleton<IResourceHandler, EmbeddedResourceHandler>();
        services.AddScoped<ICaptchaImageGenerator, DefaultCaptchaImageGenerator>();
        services.AddScoped<ICaptcha, DefaultCaptcha>();
        services.AddScoped<IStorage, DefaultStorage>();
        services.AddScoped<IValidator, SimpleValidator>();
        services.AddScoped<ISlideCaptcha, SlideCaptcha>();

        return result;
    }
}