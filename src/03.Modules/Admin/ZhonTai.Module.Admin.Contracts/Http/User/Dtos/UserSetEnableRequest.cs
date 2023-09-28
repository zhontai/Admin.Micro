namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 设置启用
/// </summary>
public class UserSetEnableRequest
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}