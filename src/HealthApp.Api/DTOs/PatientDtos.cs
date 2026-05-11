namespace HealthApp.Api.DTOs;

public record PatientDto(
    int PatientId,
    int UserId,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? Phone,
    string? BloodGroup,
    string? Gender,
    DateOnly? DateOfBirth,
    string? Address,
    string? EmergencyContact,
    DateTime CreatedAt
);

public record CreatePatientRequest(
    string Username,
    string Password,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string? BloodGroup,
    string? Gender,
    DateOnly? DateOfBirth,
    string? Address,
    string? EmergencyContact
);

public record UpdatePatientRequest(
    string? FirstName,
    string? LastName,
    string? Phone,
    string? BloodGroup,
    string? Gender,
    DateOnly? DateOfBirth,
    string? Address,
    string? EmergencyContact
);
