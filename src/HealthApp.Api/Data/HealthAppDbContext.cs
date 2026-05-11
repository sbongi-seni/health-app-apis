using HealthApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data;

public class HealthAppDbContext(DbContextOptions<HealthAppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.HasIndex(u => u.Username).IsUnique();
            e.Property(u => u.Username).HasMaxLength(100).IsRequired();
            e.Property(u => u.Role).HasMaxLength(20).IsRequired();
            e.Property(u => u.Email).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(p => p.PatientId);
            e.HasOne(p => p.User).WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            e.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            e.Property(p => p.LastName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.HasKey(a => a.AppointmentId);
            e.HasOne(a => a.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.Property(a => a.DoctorName).HasMaxLength(200).IsRequired();
            e.Property(a => a.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<MedicalRecord>(e =>
        {
            e.HasKey(r => r.RecordId);
            e.HasOne(r => r.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(r => r.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.Property(r => r.Diagnosis).HasMaxLength(500).IsRequired();
            e.Property(r => r.DoctorName).HasMaxLength(200).IsRequired();
        });
    }
}
