namespace Jwt.Authen.Api.Models;

/// <summary>
/// 
/// </summary>
public class LoginModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
