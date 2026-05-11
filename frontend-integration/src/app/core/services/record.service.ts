// src/app/core/services/record.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CreateRecordRequest, MedicalRecord } from '../models/record.models';

@Injectable({ providedIn: 'root' })
export class RecordService {
  private readonly base = `${environment.apiUrl}/api/records`;

  constructor(private http: HttpClient) {}

  getAdminRecords() {
    return this.http.get<MedicalRecord[]>(`${this.base}/admin`);
  }

  getPatientRecords(patientId: number) {
    return this.http.get<MedicalRecord[]>(`${this.base}/patient/${patientId}`);
  }

  getById(id: number) {
    return this.http.get<MedicalRecord>(`${this.base}/${id}`);
  }

  create(request: CreateRecordRequest) {
    return this.http.post<MedicalRecord>(this.base, request);
  }
}
