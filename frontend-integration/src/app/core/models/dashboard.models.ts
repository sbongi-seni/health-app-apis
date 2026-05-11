// src/app/core/models/dashboard.models.ts
export interface DashboardStats {
  totalPatients: number;
  todayAppointments: number;
  upcomingAppointments: number;
  totalRecords: number;
  recentActivity: RecentPatientActivity[];
}

export interface RecentPatientActivity {
  patientName: string;
  condition: string;
  doctorName: string;
  status: string;
  lastVisit: string;
}
