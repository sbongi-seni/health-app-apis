// src/app/core/models/record.models.ts
export interface MedicalRecord {
  recordId: number;
  patientId: number;
  patientName: string;
  diagnosis: string;
  treatment: string | null;
  doctorName: string;
  visitDate: string;
  notes: string | null;
  createdAt: string;
}

export interface CreateRecordRequest {
  patientId: number;
  diagnosis: string;
  treatment?: string;
  doctorName: string;
  visitDate: string;
  notes?: string;
}
