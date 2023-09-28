using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 重置密码
/// </summary>
public class UserResetPasswordRequest
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择用户")]
    public long Id { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}