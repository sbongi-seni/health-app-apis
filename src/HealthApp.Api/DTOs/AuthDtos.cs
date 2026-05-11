namespace HealthApp.Api.DTOs;

public record LoginRequest(string Username, string Password);

public record LoginResponse(
    string Token,
    string Role,
    int UserId,
    int? PatientId,
    string Name,
    string Email
);
