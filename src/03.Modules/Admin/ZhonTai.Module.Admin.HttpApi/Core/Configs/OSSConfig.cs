using OnceMi.AspNetCore.OSS;

namespace ZhonTai.Module.Admin.HttpApi.Core.Configs;

/// <summary>
/// OSS配置
/// </summary>
public class OSSOption: OSSOptions
{
    /// <summary>
    /// 存储桶
    /// </summary>
    public string BucketName { get; set; } = "admin";

    /// <summary>
    /// 文件地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 文件Md5码
    /// </summary>
    public bool Md5 { get; set; } = false;

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;
}

/// <summary>
/// 本地上传配置
/// </summary>
public class LocalUploadConfig
{
    /// <summary>
    /// 上传目录
    /// </summary>
    public string Directory { get; set; } = "upload";

    /// <summary>
    /// 日期目录
    /// </summary>
    public string DateTimeDirectory { get; set; } = "yyyy/MM/dd";

    /// <summary>
    /// 文件Md5码
    /// </summary>
    public bool Md5 { get; set; } = false;

    /// <summary>
    /// 文件大小
    /// </summary>
    public long MaxSize { get; set; } = 104857600;

    /// <summary>
    /// 包含文件拓展名列表
    /// </summary>
    public string[] IncludeExtension { get; set; }

    /// <summary>
    /// 排除文件拓展名列表
    /// </summary>
    public string[] ExcludeExtension { get; set; }
}

/// <summary>
/// OSS配置
/// </summary>
public class OSSConfig
{
    /// <summary>
    /// 本地上传配置
    /// </summary>
    public LocalUploadConfig LocalUploadConfig { get; set; }

    /// <summary>
    /// 文件存储供应商
    /// </summary>
    public OSSProvider Provider { get; set; } = OSSProvider.Minio;

    /// <summary>
    /// OSS配置列表
    /// </summary>
    public List<OSSOption> OSSConfigs { get; set; }
}