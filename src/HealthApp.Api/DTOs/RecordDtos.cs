namespace HealthApp.Api.DTOs;

public record RecordDto(
    int RecordId,
    int PatientId,
    string PatientName,
    string Diagnosis,
    string? Treatment,
    string DoctorName,
    DateOnly VisitDate,
    string? Notes,
    DateTime CreatedAt
);

public record CreateRecordRequest(
    int PatientId,
    string Diagnosis,
    string? Treatment,
    string DoctorName,
    DateOnly VisitDate,
    string? Notes
);
