// src/app/core/services/appointment.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Appointment, CreateAppointmentRequest, UpdateAppointmentRequest } from '../models/appointment.models';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private readonly base = `${environment.apiUrl}/api/appointments`;

  constructor(private http: HttpClient) {}

  getAdminAppointments() {
    return this.http.get<Appointment[]>(`${this.base}/admin`);
  }

  getPatientAppointments(patientId: number) {
    return this.http.get<Appointment[]>(`${this.base}/patient/${patientId}`);
  }

  getById(id: number) {
    return this.http.get<Appointment>(`${this.base}/${id}`);
  }

  create(request: CreateAppointmentRequest) {
    return this.http.post<Appointment>(this.base, request);
  }

  update(id: number, request: UpdateAppointmentRequest) {
    return this.http.put<Appointment>(`${this.base}/${id}`, request);
  }
}
