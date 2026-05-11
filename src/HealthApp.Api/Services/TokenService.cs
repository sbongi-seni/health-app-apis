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

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        if (patientId.HasValue)
            claims.Add(new Claim("patientId", patientId.Value.ToString()));

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
