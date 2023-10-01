using FreeSql.DataAnnotations;

namespace ZhonTai.Module.Admin.HttpApi.Domain;

/// <summary>
/// 登录日志
/// </summary>
[Table(Name = "ad_login_log")]
public partial class LoginLogEntity : LogAbstract
{
}