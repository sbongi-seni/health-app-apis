namespace HealthApp.Api.DTOs;

public record AppointmentDto(
    int AppointmentId,
    int PatientId,
    string PatientName,
    string DoctorName,
    DateTime AppointmentDateTime,
    string Status,
    string? Notes,
    DateTime CreatedAt
);

public record CreateAppointmentRequest(
    int PatientId,
    string DoctorName,
    DateTime AppointmentDateTime,
    string? Notes
);

public record UpdateAppointmentRequest(
    string? DoctorName,
    DateTime? AppointmentDateTime,
    string? Status,
    string? Notes
);
