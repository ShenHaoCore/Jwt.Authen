using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jwt.Authen.Api.Services;

/// <summary>
/// 
/// </summary>
public class TokenService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    
    // 模拟数据库中的refresh tokens存储
    private static Dictionary<string, Tuple<string, DateTime>> _refreshTokens = new Dictionary<string, Tuple<string, DateTime>>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public TokenService(IConfiguration configuration)
    {
        _secret = configuration["Jwt:Secret"] ?? string.Empty;
        _issuer = configuration["Jwt:Issuer"] ?? string.Empty;
        _audience = configuration["Jwt:Audience"] ?? string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public (string accessToken, string refreshToken) GenerateTokens(string userId)
    {
        List<Claim> claims = [new Claim(JwtRegisteredClaimNames.Sub, userId), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())];
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new JwtSecurityToken(issuer: _issuer, audience: _audience, claims: claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: creds);
        string refreshToken = GenerateRefreshToken();
        _refreshTokens[refreshToken] = new Tuple<string, DateTime>(userId, DateTime.Now.AddDays(7)); // 将刷新令牌与到期时间一起存储在模拟数据库中
        return (new JwtSecurityTokenHandler().WriteToken(token), refreshToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    /// <summary>
    /// 验证刷新TOKEN
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public (bool isValidate, string? userId) ValidateRefreshToken(string refreshToken)
    {
        if (!_refreshTokens.TryGetValue(refreshToken, out var tokenDetails)) { return (false, null); }
        if (tokenDetails.Item2 <= DateTime.Now) { _refreshTokens.Remove(refreshToken); return (false, null); }
        return (true, tokenDetails.Item1);
    }
}
