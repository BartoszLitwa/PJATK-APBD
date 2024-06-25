using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RCS.API.Data;

namespace RCS.API.Common;

public class JwtTokenGenerator(RcsDbContext context, IOptions<AppSettings> appSettings) : IJwtTokenGenerator
{
    private const string _issuer = "RCS.API";
    private const string _audience = "RCS.API";
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(appSettings.Value.JwtSecret));

    public string GenerateToken(string login, string password)
    {
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        
        var client = context.Employees
            .FirstOrDefault(e => e.Login == login && e.Password == password);
        if (client is null)
            return string.Empty;

        var claims = new[]
        {
            new Claim(ClaimTypes.Actor, client.Id.ToString()),
            new Claim(ClaimTypes.Email, login),
            new Claim(ClaimTypes.Role, client.IsAdmin ? "Admin" : "User")
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}