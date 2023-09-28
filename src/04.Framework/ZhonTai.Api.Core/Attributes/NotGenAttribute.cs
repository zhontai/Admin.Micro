using System;

namespace ZhonTai.Api.Core.Attributes;

/// <summary>
/// 不生成特性
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NotGenAttribute : Attribute
{
}