using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Core.Consts;
using Lazy.SlideCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZhonTai.Api.Core.Attributes;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 验证码服务
/// </summary>
[Order(210)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CaptchaService : BaseService, IDynamicApi
{
    private readonly Lazy<ICaptcha> _captcha;
    private readonly Lazy<ISlideCaptcha> _slideCaptcha;

    public CaptchaService(Lazy<ICaptcha> captcha, Lazy<ISlideCaptcha> slideCaptcha)
    {
        _captcha = captcha;
        _slideCaptcha = slideCaptcha;
    }

    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public CaptchaData Generate(string captchaId = null)
    {
        return _captcha.Value.Generate(captchaId);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <param name="track">滑动轨迹</param>
    /// <returns></returns>
    [AllowAnonymous]
    [NoOprationLog]
    public ValidateResult CheckAsync([FromQuery] string captchaId, SmsSendCodeRequest.Models.SlideTrack track)
    {
        if (captchaId.IsNull() || track == null)
        {
            throw Response.Exception("请完成安全验证");
        }

        return _slideCaptcha.Value.Validate(captchaId, track, false);
    }
}