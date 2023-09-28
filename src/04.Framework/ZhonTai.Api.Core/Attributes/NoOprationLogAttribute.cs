using System;

namespace ZhonTai.Api.Core.Attributes;

/// <summary>
/// 禁用操作日志
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class NoOprationLogAttribute : Attribute
{
}