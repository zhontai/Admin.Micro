using System.ComponentModel.DataAnnotations;
using ZhonTai.Api.Rpc;

namespace ZhonTai.Module.App.Api.Services.Module.Input
{
    /// <summary>
    /// 修改模块
    /// </summary>
    public partial class ModuleUpdateInput : ModuleAddInput
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [ValidateRequired("请选择模块")]
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}