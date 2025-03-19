using Jwt.Authen.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.Authen.Api.Controllers;

/// <summary>
/// Redis
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class RedisController : ControllerBase
{
    private readonly RedisService _redisService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="redisService"></param>
    public RedisController(RedisService redisService)
    {
        _redisService = redisService;
    }

    /// <summary>
    /// 取值
    /// </summary>
    /// <param name="key">key</param>
    /// <returns></returns>
    [HttpGet("{key}")]
    [EndpointSummary("取值API")]
    [EndpointDescription("取值")]
    public async Task<IActionResult> GetAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        var value = await _redisService.GetStringAsync("TEST");
        return Ok($"SUCCESS-{value}");
    }

    /// <summary>
    /// 赋值
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [EndpointSummary("赋值API")]
    [EndpointDescription("赋值")]
    public async Task<IActionResult> Set()
    {
        await _redisService.SetStringAsync("TEST", "TEST-VALUE");
        return Ok($"SUCCESS");
    }
}
