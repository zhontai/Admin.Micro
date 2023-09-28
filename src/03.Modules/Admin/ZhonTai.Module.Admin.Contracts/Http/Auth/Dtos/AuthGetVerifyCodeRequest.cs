namespace ZhonTai.Module.Admin.Contracts.Http;

public class AuthGetVerifyCodeResponse
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    public string Img { get; set; }
}