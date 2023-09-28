namespace ZhonTai.Module.Admin.Contracts.Http;

public class FileUploadFileResponse
{
    public static class Models
    {
        public enum OSSProvider
        {
            Invalid,
            Minio,
            Aliyun,
            QCloud,
            Qiniu,
            HuaweiCloud,
            BaiduCloud,
            Ctyun
        }
    }

    public long Id { get; set; }

    /// <summary>
    /// OSS供应商
    /// </summary>
    public Models.OSSProvider? Provider { get; set; }

    /// <summary>
    /// 存储桶名称
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 文件目录
    /// </summary>
    public string FileDirectory { get; set; }

    /// <summary>
    /// 文件Guid
    /// </summary>
    public Guid FileGuid { get; set; }

    /// <summary>
    /// 保存文件名
    /// </summary>
    public string SaveFileName { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// 文件字节长度
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// 文件大小格式化
    /// </summary>
    public string SizeFormat { get; set; }

    /// <summary>
    /// 链接地址
    /// </summary>
    public string LinkUrl { get; set; }
}