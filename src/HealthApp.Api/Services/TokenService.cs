using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthApp.Api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace HealthApp.Api.Services;

public class TokenService(IConfiguration config)
{
    public string CreateToken(User user, int? patientId)
    {
        var jwtKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("patientId", patientId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
