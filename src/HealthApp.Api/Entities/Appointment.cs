namespace HealthApp.Api.Entities;

public class Appointment
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDateTime { get; set; }
    public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Patient Patient { get; set; } = null!;
}
