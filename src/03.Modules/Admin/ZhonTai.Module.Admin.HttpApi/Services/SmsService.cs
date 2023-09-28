using Microsoft.AspNetCore.Authorization;
using static Lazy.SlideCaptcha.Core.ValidateResult;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.Admin.Contracts.Events;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Rpc.EventBus;
using ZhonTai.Utils.Helpers;
using Mapster;
using Lazy.SlideCaptcha.Core.Validator;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 短信服务
/// </summary>
[Order(210)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class SmsService : BaseService, IDynamicApi
{
    private readonly Lazy<ISlideCaptcha> _captcha;
    private readonly Lazy<IEventPublisher> _eventPublisher;

    public SmsService(Lazy<ISlideCaptcha> captcha, Lazy<IEventPublisher> eventPublisher) 
    {
        _captcha = captcha;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// 发送短信验证码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<string> SendCodeAsync(SmsSendCodeRequest input)
    {
        if (input.Mobile.IsNull())
        {
            throw Response.Exception("请输入手机号");
        }

        if (input.CaptchaId.IsNull() || input.Track == null)
        {
            throw Response.Exception("请完成安全验证");
        }

        var validateResult = _captcha.Value.Validate(input.CaptchaId, input.Track);
        if (validateResult.Result != ValidateResultType.Success)
        {
            throw Response.Exception($"安全{validateResult.Message}");
        }

        var codeId = input.CodeId.IsNull() ? Guid.NewGuid().ToString() : input.CodeId;
        var code = StringHelper.GenerateRandomNumber();
        await Cache.SetAsync(CacheKeys.GetSmsCodeKey(input.Mobile, codeId), code, TimeSpan.FromMinutes(5));

        //发送短信
        var smsSendCodeEvent = new SmsSendCodeEvent
        {
            Mobile = input.Mobile,
            Text = code
        };
        await _eventPublisher.Value.PublishAsync(smsSendCodeEvent);

        return codeId;
    }
}
