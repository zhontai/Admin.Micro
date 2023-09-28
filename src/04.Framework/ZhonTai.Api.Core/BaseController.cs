using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Attributes;

namespace ZhonTai.Api.Core;

/// <summary>
/// 基础控制器
/// </summary>
[Route("api/[area]/[controller]/[action]")]
[ApiController]
[ValidatePermission]
[ValidateInput]
public abstract class BaseController : ControllerBase
{
}