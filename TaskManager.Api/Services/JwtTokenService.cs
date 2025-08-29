using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Api.Models;


namespace TaskManager.Api.Services;


public class JwtOptions
{
public string Key { get; set; } = string.Empty;
public string Issuer { get; set; } = string.Empty;
public string Audience { get; set; } = string.Empty;
public int ExpiresInMinutes { get; set; } = 60;
}


public interface IJwtTokenService
{
(string token, DateTime expiresAtUtc) GenerateToken(ApplicationUser user);
}


public class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
private readonly JwtOptions _options = options.Value;


public (string token, DateTime expiresAtUtc) GenerateToken(ApplicationUser user)
{
var claims = new List<Claim>
{
new(JwtRegisteredClaimNames.Sub, user.Id),
new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
new(ClaimTypes.NameIdentifier, user.Id)
};


var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
var expires = DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes);


var token = new JwtSecurityToken(
issuer: _options.Issuer,
audience: _options.Audience,
claims: claims,
expires: expires,
signingCredentials: creds
);


return (new JwtSecurityTokenHandler().WriteToken(token), expires);
}
}