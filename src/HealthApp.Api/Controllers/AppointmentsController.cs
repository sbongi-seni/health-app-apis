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
public class AppointmentsController(HealthAppDbContext db) : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAdminAppointments()
    {
        var appointments = await db.Appointments
            .Include(a => a.Patient)
            .OrderByDescending(a => a.AppointmentDateTime)
            .Select(a => MapToDto(a))
            .ToListAsync();
        return Ok(appointments);
    }

    [HttpGet("patient/{patientId:int}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetPatientAppointments(int patientId)
    {
        var appointments = await db.Appointments
            .Include(a => a.Patient)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDateTime)
            .Select(a => MapToDto(a))
            .ToListAsync();
        return Ok(appointments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
    {
        var appt = await db.Appointments
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
        if (appt is null) return NotFound();
        return Ok(MapToDto(appt));
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentRequest request)
    {
        var patient = await db.Patients.FindAsync(request.PatientId);
        if (patient is null) return BadRequest(new { message = "Patient not found." });

        var appt = new Appointment
        {
            PatientId = request.PatientId,
            DoctorName = request.DoctorName,
            AppointmentDateTime = request.AppointmentDateTime,
            Status = "Scheduled",
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };
        db.Appointments.Add(appt);
        await db.SaveChangesAsync();

        await db.Entry(appt).Reference(a => a.Patient).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = appt.AppointmentId }, MapToDto(appt));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> Update(int id, [FromBody] UpdateAppointmentRequest request)
    {
        var appt = await db.Appointments
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
        if (appt is null) return NotFound();

        if (request.DoctorName is not null) appt.DoctorName = request.DoctorName;
        if (request.AppointmentDateTime is not null) appt.AppointmentDateTime = request.AppointmentDateTime.Value;
        if (request.Status is not null) appt.Status = request.Status;
        if (request.Notes is not null) appt.Notes = request.Notes;

        await db.SaveChangesAsync();
        return Ok(MapToDto(appt));
    }

    private static AppointmentDto MapToDto(Appointment a) => new(
        a.AppointmentId,
        a.PatientId,
        $"{a.Patient.FirstName} {a.Patient.LastName}",
        a.DoctorName,
        a.AppointmentDateTime,
        a.Status,
        a.Notes,
        a.CreatedAt
    );
}
