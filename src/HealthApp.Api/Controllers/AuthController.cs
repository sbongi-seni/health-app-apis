using HealthApp.Api.Data;
using HealthApp.Api.DTOs;
using HealthApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(HealthAppDbContext db, TokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await db.Users
            .Include(u => u.Patient)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid username or password." });

        var patientId = user.Patient?.PatientId;
        var name = user.Patient is not null
            ? $"{user.Patient.FirstName} {user.Patient.LastName}"
            : user.Username;

        var token = tokenService.CreateToken(user, patientId);

        return Ok(new LoginResponse(token, user.Role, user.UserId, patientId, name, user.Email));
    }
}
