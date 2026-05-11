namespace HealthApp.Api.DTOs;

public record DashboardStatsDto(
    int TotalPatients,
    int TodayAppointments,
    int UpcomingAppointments,
    int TotalRecords,
    IEnumerable<RecentPatientActivity> RecentActivity
);

public record RecentPatientActivity(
    string PatientName,
    string Condition,
    string DoctorName,
    string Status,
    DateOnly LastVisit
);
