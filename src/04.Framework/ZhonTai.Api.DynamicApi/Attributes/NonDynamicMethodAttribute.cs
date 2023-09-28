using System;

namespace ZhonTai.Api.DynamicApi.Attributes;

[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class NonDynamicMethodAttribute : Attribute
{

}
