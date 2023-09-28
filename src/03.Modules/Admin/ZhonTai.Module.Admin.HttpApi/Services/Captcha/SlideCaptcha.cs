using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Storage;
using Lazy.SlideCaptcha.Core.Validator;
using Mapster;

namespace ZhonTai.Module.Admin.HttpApi.Services;

public class SlideCaptcha: ISlideCaptcha
{
    private readonly IValidator _validator;
    private readonly IStorage _storage;

    public SlideCaptcha(IValidator validator, IStorage storage)
    {
        _storage = storage;
        _validator = validator;
    }

    public ValidateResult Validate(string captchaId, SmsSendCodeRequest.Models.SlideTrack slideTrack, bool removeIfSuccess = true)
    {
         
        var captchaValidateData = _storage.Get<CaptchaValidateData>(captchaId);
        if (captchaValidateData == null) return ValidateResult.Timeout();
        var success = _validator.Validate(slideTrack.Adapt<SlideTrack>(), captchaValidateData);
        if (!success || (success && removeIfSuccess))
        {
            _storage.Remove(captchaId);
        }

        return success ? ValidateResult.Success() : ValidateResult.Fail();
        
    }
}
