using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;

namespace ZhonTai.Api.Core.Configs;

/// <summary>
/// Jwt配置
/// </summary>
public class JwtConfig
{
    public bool ValidateIssuer { get; set; }

    /// <summary>
    /// 发行者
    /// </summary>
    public string ValidIssuer { get; set; }

    public bool ValidateAudience { get; set; }

    /// <summary>
    /// 订阅者
    /// </summary>
    public string ValidAudience { get; set; }

    public bool ValidateIssuerSigningKey { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    public string IssuerSigningKey { get; set; }

    /// <summary>
    /// 时钟偏差（秒）
    /// </summary>
    public int ClockSkew { get; set; }

    /// <summary>
    /// 要求过期时间
    /// </summary>
    public bool RequireExpirationTime { get; set; }

    public bool ValidateLifetime { get; set; }

    /// <summary>
    /// 过期时间(分钟)
    /// </summary>
    public int Expires { get; set; }

    /// <summary>
    /// 刷新过期时间(分钟)
    /// </summary>
    public int RefreshExpires { get; set; }
}