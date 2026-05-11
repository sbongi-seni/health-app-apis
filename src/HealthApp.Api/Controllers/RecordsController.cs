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
public class RecordsController(HealthAppDbContext db) : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<RecordDto>>> GetAdminRecords()
    {
        var records = await db.MedicalRecords
            .Include(r => r.Patient)
            .OrderByDescending(r => r.VisitDate)
            .Select(r => MapToDto(r))
            .ToListAsync();
        return Ok(records);
    }

    [HttpGet("patient/{patientId:int}")]
    public async Task<ActionResult<IEnumerable<RecordDto>>> GetPatientRecords(int patientId)
    {
        var records = await db.MedicalRecords
            .Include(r => r.Patient)
            .Where(r => r.PatientId == patientId)
            .OrderByDescending(r => r.VisitDate)
            .Select(r => MapToDto(r))
            .ToListAsync();
        return Ok(records);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecordDto>> GetById(int id)
    {
        var record = await db.MedicalRecords
            .Include(r => r.Patient)
            .FirstOrDefaultAsync(r => r.RecordId == id);
        if (record is null) return NotFound();
        return Ok(MapToDto(record));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RecordDto>> Create([FromBody] CreateRecordRequest request)
    {
        var patient = await db.Patients.FindAsync(request.PatientId);
        if (patient is null) return BadRequest(new { message = "Patient not found." });

        var record = new MedicalRecord
        {
            PatientId = request.PatientId,
            Diagnosis = request.Diagnosis,
            Treatment = request.Treatment,
            DoctorName = request.DoctorName,
            VisitDate = request.VisitDate,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };
        db.MedicalRecords.Add(record);
        await db.SaveChangesAsync();

        await db.Entry(record).Reference(r => r.Patient).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = record.RecordId }, MapToDto(record));
    }

    private static RecordDto MapToDto(MedicalRecord r) => new(
        r.RecordId,
        r.PatientId,
        $"{r.Patient.FirstName} {r.Patient.LastName}",
        r.Diagnosis,
        r.Treatment,
        r.DoctorName,
        r.VisitDate,
        r.Notes,
        r.CreatedAt
    );
}
