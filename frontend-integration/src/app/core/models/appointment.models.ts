// src/app/core/models/appointment.models.ts
export interface Appointment {
  appointmentId: number;
  patientId: number;
  patientName: string;
  doctorName: string;
  appointmentDateTime: string;
  status: string;
  notes: string | null;
  createdAt: string;
}

export interface CreateAppointmentRequest {
  patientId: number;
  doctorName: string;
  appointmentDateTime: string;
  notes?: string;
}

export interface UpdateAppointmentRequest {
  doctorName?: string;
  appointmentDateTime?: string;
  status?: string;
  notes?: string;
}
