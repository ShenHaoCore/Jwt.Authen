using Jwt.Authen.Api.Models;
using Jwt.Authen.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.Authen.Api.Controllers;

/// <summary>
/// 用户
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly TokenService _tokenService;

    /// <summary>
    /// 用户
    /// </summary>
    /// <param name="tokenService"></param>
    public UserController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [EndpointSummary("用户登录API")]
    [EndpointDescription("用户登录获取TOKEN")]
    public IActionResult Login([FromBody] LoginModel request)
    {
        ArgumentNullException.ThrowIfNull(request);
        // 此处应有用户身份验证逻辑
        (string accessToken, string refreshToken) = _tokenService.GenerateTokens("12306");
        return Ok(new { accessToken, refreshToken });
    }

    /// <summary>
    /// 刷新TOKEN
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [EndpointSummary("TOKEN刷新API")]
    [EndpointDescription("用户使用长时效TOKEN获取新TOKEN")]
    public IActionResult RefreshToken([FromBody] TokenApiModel request)
    {
        ArgumentNullException.ThrowIfNull(request);
        // 在实际应用中，这里需要检查refresh token的有效性以及它是否与存储的refresh token匹配
        (bool isValidate, string userId) = _tokenService.ValidateRefreshToken(request.RefreshToken);
        if (!isValidate) { return Unauthorized(); }
        (string accessToken, string refreshToken) = _tokenService.GenerateTokens(userId);
        return Ok(new { accessToken, refreshToken });
    }
}
