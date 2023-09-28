using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class FileDeleteRequest
{
    /// <summary>
    /// 文件Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择文件")]
    public long Id { get; set; }
}