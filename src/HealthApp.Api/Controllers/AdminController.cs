using HealthApp.Api.Data;
using HealthApp.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(HealthAppDbContext db) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashboard()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var totalPatients = await db.Patients.CountAsync();
        var todayAppointments = await db.Appointments
            .CountAsync(a => a.AppointmentDateTime >= today && a.AppointmentDateTime < tomorrow);
        var upcomingAppointments = await db.Appointments
            .CountAsync(a => a.AppointmentDateTime >= today && a.Status == "Scheduled");
        var totalRecords = await db.MedicalRecords.CountAsync();

        var recentActivity = await db.MedicalRecords
            .Include(r => r.Patient)
            .OrderByDescending(r => r.VisitDate)
            .Take(5)
            .Select(r => new RecentPatientActivity(
                $"{r.Patient.FirstName} {r.Patient.LastName}",
                r.Diagnosis,
                r.DoctorName,
                "Active",
                r.VisitDate
            ))
            .ToListAsync();

        return Ok(new DashboardStatsDto(
            totalPatients,
            todayAppointments,
            upcomingAppointments,
            totalRecords,
            recentActivity
        ));
    }
}
