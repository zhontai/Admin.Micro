namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 添加
/// </summary>
public class UserAddRequest: UserFormRequest
{
    /// <summary>
    /// 密码
    /// </summary>
    public virtual string Password { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}