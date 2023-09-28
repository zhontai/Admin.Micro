using Microsoft.AspNetCore.Http;
using ZhonTai.Api.Core.Attributes;
namespace ZhonTai.Api.Core.Helpers;

/// <summary>
/// 文件上传帮助类
/// </summary>
[SingleInstance]
public class FileHelper
{
    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(filePath);
        await file.CopyToAsync(stream, cancellationToken);
    }
}