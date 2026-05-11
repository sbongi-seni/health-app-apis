using HealthApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(HealthAppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (await db.Users.AnyAsync()) return;

        var adminUser = new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            Email = "admin@healthapp.com",
            CreatedAt = DateTime.UtcNow
        };

        var patientUser1 = new User
        {
            Username = "john.doe",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Patient@123"),
            Role = "Patient",
            Email = "john.doe@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var patientUser2 = new User
        {
            Username = "jane.smith",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Patient@123"),
            Role = "Patient",
            Email = "jane.smith@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var patientUser3 = new User
        {
            Username = "sipho.zulu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Patient@123"),
            Role = "Patient",
            Email = "sipho.zulu@example.com",
            CreatedAt = DateTime.UtcNow
        };

        db.Users.AddRange(adminUser, patientUser1, patientUser2, patientUser3);
        await db.SaveChangesAsync();

        var patient1 = new Patient
        {
            UserId = patientUser1.UserId,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1990, 3, 15),
            Gender = "Male",
            Phone = "+27 71 234 5678",
            BloodGroup = "O+",
            EmergencyContact = "Mary Doe - +27 72 345 6789",
            Address = "123 Main St, Johannesburg",
            CreatedAt = DateTime.UtcNow
        };

        var patient2 = new Patient
        {
            UserId = patientUser2.UserId,
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateOnly(1985, 7, 22),
            Gender = "Female",
            Phone = "+27 82 456 7890",
            BloodGroup = "A+",
            EmergencyContact = "Tom Smith - +27 83 567 8901",
            Address = "456 Oak Ave, Cape Town",
            CreatedAt = DateTime.UtcNow
        };

        var patient3 = new Patient
        {
            UserId = patientUser3.UserId,
            FirstName = "Sipho",
            LastName = "Zulu",
            DateOfBirth = new DateOnly(1995, 11, 8),
            Gender = "Male",
            Phone = "+27 63 123 4567",
            BloodGroup = "B+",
            EmergencyContact = "Thandi Zulu - +27 64 234 5678",
            Address = "789 Pine Rd, Durban",
            CreatedAt = DateTime.UtcNow
        };

        db.Patients.AddRange(patient1, patient2, patient3);
        await db.SaveChangesAsync();

        var appointments = new[]
        {
            new Appointment
            {
                PatientId = patient1.PatientId,
                DoctorName = "Dr. Smith",
                AppointmentDateTime = DateTime.UtcNow.AddDays(5).Date.AddHours(10),
                Status = "Scheduled",
                Notes = "Hypertension follow-up",
                CreatedAt = DateTime.UtcNow
            },
            new Appointment
            {
                PatientId = patient1.PatientId,
                DoctorName = "Dr. Patel",
                AppointmentDateTime = DateTime.UtcNow.AddDays(-10).Date.AddHours(14),
                Status = "Completed",
                Notes = "General checkup",
                CreatedAt = DateTime.UtcNow
            },
            new Appointment
            {
                PatientId = patient2.PatientId,
                DoctorName = "Dr. Patel",
                AppointmentDateTime = DateTime.UtcNow.AddDays(2).Date.AddHours(9),
                Status = "Scheduled",
                Notes = "Diabetes follow-up",
                CreatedAt = DateTime.UtcNow
            },
            new Appointment
            {
                PatientId = patient3.PatientId,
                DoctorName = "Dr. Johnson",
                AppointmentDateTime = DateTime.UtcNow.AddDays(7).Date.AddHours(11),
                Status = "Scheduled",
                Notes = "Asthma check",
                CreatedAt = DateTime.UtcNow
            }
        };

        db.Appointments.AddRange(appointments);

        var records = new[]
        {
            new MedicalRecord
            {
                PatientId = patient1.PatientId,
                Diagnosis = "Hypertension",
                Treatment = "Lisinopril 10mg daily",
                DoctorName = "Dr. Smith",
                VisitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)),
                Notes = "Blood pressure elevated, started medication",
                CreatedAt = DateTime.UtcNow
            },
            new MedicalRecord
            {
                PatientId = patient2.PatientId,
                Diagnosis = "Type 2 Diabetes",
                Treatment = "Metformin 500mg twice daily, dietary changes",
                DoctorName = "Dr. Patel",
                VisitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20)),
                Notes = "HbA1c at 7.2%, monitoring glucose levels",
                CreatedAt = DateTime.UtcNow
            },
            new MedicalRecord
            {
                PatientId = patient3.PatientId,
                Diagnosis = "Asthma",
                Treatment = "Salbutamol inhaler as needed",
                DoctorName = "Dr. Johnson",
                VisitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15)),
                Notes = "Mild intermittent asthma, avoiding triggers",
                CreatedAt = DateTime.UtcNow
            }
        };

        db.MedicalRecords.AddRange(records);
        await db.SaveChangesAsync();
    }
}
