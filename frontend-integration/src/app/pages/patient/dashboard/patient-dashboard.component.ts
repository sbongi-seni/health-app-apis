// src/app/pages/patient/dashboard/patient-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { RecordService } from '../../../core/services/record.service';
import { PatientService } from '../../../core/services/patient.service';
import { Appointment } from '../../../core/models/appointment.models';
import { MedicalRecord } from '../../../core/models/record.models';
import { Patient } from '../../../core/models/patient.models';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './patient-dashboard.component.html',
  styleUrl: './patient-dashboard.component.scss'
})
export class PatientDashboardComponent implements OnInit {
  patient: Patient | null = null;
  appointments: Appointment[] = [];
  records: MedicalRecord[] = [];
  loading = true;
  error = '';

  constructor(
    public authService: AuthService,
    private appointmentService: AppointmentService,
    private recordService: RecordService,
    private patientService: PatientService
  ) {}

  ngOnInit() {
    const user = this.authService.currentUser();
    if (user?.patientId) {
      const patientId = user.patientId;
      this.patientService.getById(patientId).subscribe({
        next: (p) => { this.patient = p; },
        error: () => {}
      });
      this.appointmentService.getPatientAppointments(patientId).subscribe({
        next: (a) => { this.appointments = a; this.loading = false; },
        error: () => { this.error = 'Failed to load data.'; this.loading = false; }
      });
      this.recordService.getPatientRecords(patientId).subscribe({
        next: (r) => { this.records = r; },
        error: () => {}
      });
    } else {
      this.loading = false;
    }
  }

  get nextAppointment(): Appointment | null {
    const upcoming = this.appointments
      .filter(a => new Date(a.appointmentDateTime) >= new Date() && a.status === 'Scheduled')
      .sort((a, b) => new Date(a.appointmentDateTime).getTime() - new Date(b.appointmentDateTime).getTime());
    return upcoming.length ? upcoming[0] : null;
  }

  get latestRecord(): MedicalRecord | null {
    return this.records.length ? this.records[0] : null;
  }
}
