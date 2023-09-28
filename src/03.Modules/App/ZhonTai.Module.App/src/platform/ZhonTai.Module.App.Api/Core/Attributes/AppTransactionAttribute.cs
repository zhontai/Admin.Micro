using System;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.App.Api.Core.Consts;

namespace ZhonTai.Module.App.Api.Core.Attributes
{
    /// <summary>
    /// 启用事物
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class AppTransactionAttribute : TransactionAttribute
    {
        public AppTransactionAttribute() : base(DbKeys.AppDb)
        {
        }
    }
}