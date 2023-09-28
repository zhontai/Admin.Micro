using Newtonsoft.Json;
using ZhonTai.Api.Rpc.Enums;

namespace ZhonTai.Module.Admin.Contracts.Http;

public class UserGetPageResponse
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType Type { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public List<string> RoleNames { get; set; }

    /// <summary>
    /// 是否主管
    /// </summary>
    public bool IsManager { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}