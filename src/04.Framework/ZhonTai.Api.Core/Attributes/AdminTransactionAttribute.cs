using System;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Api.Core.Attributes;

/// <summary>
/// 启用权限库事务
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AdminTransactionAttribute : TransactionAttribute
{
    public AdminTransactionAttribute():base(DbKeys.AppDb)
    {
    }
}