// src/app/core/models/auth.models.ts
export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  role: string;
  userId: number;
  patientId: number | null;
  name: string;
  email: string;
}

export interface CurrentUser {
  token: string;
  role: string;
  userId: number;
  patientId: number | null;
  name: string;
  email: string;
}
