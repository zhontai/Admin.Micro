namespace ZhonTai.Module.Admin.Contracts.Http;

public class DocAddImageRequest
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long DocId { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Url { get; set; }
}