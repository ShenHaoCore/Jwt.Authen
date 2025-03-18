using Jwt.Authen.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.Authen.Api.Controllers;

/// <summary>
/// 订单
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public IActionResult Create([FromBody] CreateOrderModel request)
    {
        if (request is null) { throw new ArgumentNullException(nameof(request)); }
        return Ok("SUCCESS");
    }
}
