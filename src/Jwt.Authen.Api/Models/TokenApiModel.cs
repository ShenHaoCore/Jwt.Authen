namespace Jwt.Authen.Api.Models;

/// <summary>
/// 
/// </summary>
public class TokenApiModel
{
    /// <summary>
    /// 
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
