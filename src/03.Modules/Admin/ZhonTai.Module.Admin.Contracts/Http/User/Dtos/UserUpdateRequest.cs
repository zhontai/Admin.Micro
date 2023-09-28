using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 修改
/// </summary>
public partial class UserUpdateRequest: UserFormRequest
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择用户")]
    public override long Id { get; set; }
}