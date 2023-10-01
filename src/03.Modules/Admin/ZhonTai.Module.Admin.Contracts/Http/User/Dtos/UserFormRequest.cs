using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 用户表单
/// </summary>
public class UserFormRequest
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "请输入账号")]
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "请输入姓名")]
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
    /// 角色Ids
    /// </summary>
    public virtual long[] RoleIds { get; set; }

    /// <summary>
    /// 所属部门Ids
    /// </summary>
    public virtual long[] OrgIds { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 直属主管Id
    /// </summary>
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// 直属主管姓名
    /// </summary>
    public string ManagerUserName { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    public Models.Staff Staff { get; set; }

    public static class Models
    {
        /// <summary>
        /// 添加
        /// </summary>
        public class Staff
        {
            /// <summary>
            /// 职位
            /// </summary>
            public string Position { get; set; }

            /// <summary>
            /// 工号
            /// </summary>
            public string JobNumber { get; set; }

            /// <summary>
            /// 性别
            /// </summary>
            public Sex? Sex { get; set; }

            /// <summary>
            /// 入职时间
            /// </summary>
            public DateTime? EntryTime { get; set; }

            /// <summary>
            /// 企业微信名片
            /// </summary>
            public string WorkWeChatCard { get; set; }

            /// <summary>
            /// 个人简介
            /// </summary>
            public string Introduce { get; set; }
        }
    }
}