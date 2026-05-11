// src/app/pages/patient/appointmenthistory/appointmenthistory.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { Appointment } from '../../../core/models/appointment.models';

@Component({
  selector: 'app-appointmenthistory',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './appointmenthistory.component.html'
})
export class AppointmenthistoryComponent implements OnInit {
  appointments: Appointment[] = [];
  loading = true;
  error = '';

  constructor(
    private authService: AuthService,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit() {
    const user = this.authService.currentUser();
    if (user?.patientId) {
      this.appointmentService.getPatientAppointments(user.patientId).subscribe({
        next: (data) => { this.appointments = data; this.loading = false; },
        error: () => { this.error = 'Failed to load appointments.'; this.loading = false; }
      });
    } else {
      this.loading = false;
    }
  }
}
