using Microsoft.AspNetCore.Http;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class DocUploadImageRequest
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public IFormFile File { get; set; }

    /// <summary>
    /// 文档编号
    /// </summary>
    public long Id { get; set; }
}