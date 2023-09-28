using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 添加
/// </summary>
public class TenantAddRequest
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// 企业名称
    /// </summary>
    [Required(ErrorMessage = "请输入企业名称")]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [Required(ErrorMessage = "请输入编码")]
    public string Code { get; set; }

    /// <summary>
    /// 套餐Ids
    /// </summary>
    public virtual List<long> PkgIds { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "请输入姓名")]
    public string RealName { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "请输入账号")]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }
}