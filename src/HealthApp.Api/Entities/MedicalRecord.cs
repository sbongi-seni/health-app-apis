namespace HealthApp.Api.Entities;

public class MedicalRecord
{
    public int RecordId { get; set; }
    public int PatientId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string? Treatment { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateOnly VisitDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Patient Patient { get; set; } = null!;
}
