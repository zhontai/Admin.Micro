using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 手机号登录信息
/// </summary>
public class AuthMobileLoginRequest
{
    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "请输入手机号")]
    public string Mobile { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string Code { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请获取验证码")]
    public string CodeId { get; set; }
}