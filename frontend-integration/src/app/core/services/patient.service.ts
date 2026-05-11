// src/app/core/services/patient.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CreatePatientRequest, Patient, UpdatePatientRequest } from '../models/patient.models';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly base = `${environment.apiUrl}/api/patients`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Patient[]>(this.base);
  }

  getById(id: number) {
    return this.http.get<Patient>(`${this.base}/${id}`);
  }

  create(request: CreatePatientRequest) {
    return this.http.post<Patient>(this.base, request);
  }

  update(id: number, request: UpdatePatientRequest) {
    return this.http.put<Patient>(`${this.base}/${id}`, request);
  }
}
