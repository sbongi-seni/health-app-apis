using HealthApp.Api.Data;
using HealthApp.Api.DTOs;
using HealthApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController(HealthAppDbContext db) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
    {
        var patients = await db.Patients
            .Include(p => p.User)
            .OrderBy(p => p.LastName)
            .Select(p => MapToDto(p))
            .ToListAsync();
        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        var patient = await db.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.PatientId == id);

        if (patient is null) return NotFound();
        return Ok(MapToDto(patient));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientRequest request)
    {
        if (await db.Users.AnyAsync(u => u.Username == request.Username))
            return Conflict(new { message = "Username already exists." });

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "Patient",
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var patient = new Patient
        {
            UserId = user.UserId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            BloodGroup = request.BloodGroup,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            EmergencyContact = request.EmergencyContact,
            CreatedAt = DateTime.UtcNow
        };
        db.Patients.Add(patient);
        await db.SaveChangesAsync();

        patient.User = user;
        return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, MapToDto(patient));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PatientDto>> Update(int id, [FromBody] UpdatePatientRequest request)
    {
        var patient = await db.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.PatientId == id);

        if (patient is null) return NotFound();

        if (request.FirstName is not null) patient.FirstName = request.FirstName;
        if (request.LastName is not null) patient.LastName = request.LastName;
        if (request.Phone is not null) patient.Phone = request.Phone;
        if (request.BloodGroup is not null) patient.BloodGroup = request.BloodGroup;
        if (request.Gender is not null) patient.Gender = request.Gender;
        if (request.DateOfBirth is not null) patient.DateOfBirth = request.DateOfBirth;
        if (request.Address is not null) patient.Address = request.Address;
        if (request.EmergencyContact is not null) patient.EmergencyContact = request.EmergencyContact;

        await db.SaveChangesAsync();
        return Ok(MapToDto(patient));
    }

    private static PatientDto MapToDto(Patient p) => new(
        p.PatientId,
        p.UserId,
        p.FirstName,
        p.LastName,
        $"{p.FirstName} {p.LastName}",
        p.User.Email,
        p.Phone,
        p.BloodGroup,
        p.Gender,
        p.DateOfBirth,
        p.Address,
        p.EmergencyContact,
        p.CreatedAt
    );
}
