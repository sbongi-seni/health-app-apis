// src/app/core/models/patient.models.ts
export interface Patient {
  patientId: number;
  userId: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phone: string | null;
  bloodGroup: string | null;
  gender: string | null;
  dateOfBirth: string | null;
  address: string | null;
  emergencyContact: string | null;
  createdAt: string;
}

export interface CreatePatientRequest {
  username: string;
  password: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  bloodGroup?: string;
  gender?: string;
  dateOfBirth?: string;
  address?: string;
  emergencyContact?: string;
}

export interface UpdatePatientRequest {
  firstName?: string;
  lastName?: string;
  phone?: string;
  bloodGroup?: string;
  gender?: string;
  dateOfBirth?: string;
  address?: string;
  emergencyContact?: string;
}
