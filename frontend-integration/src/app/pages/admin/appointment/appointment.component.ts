// src/app/pages/admin/appointment/appointment.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppointmentService } from '../../../core/services/appointment.service';
import { Appointment } from '../../../core/models/appointment.models';

@Component({
  selector: 'app-appointment',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './appointment.component.html'
})
export class AppointmentComponent implements OnInit {
  appointments: Appointment[] = [];
  loading = true;
  error = '';

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit() {
    this.appointmentService.getAdminAppointments().subscribe({
      next: (data) => { this.appointments = data; this.loading = false; },
      error: () => { this.error = 'Failed to load appointments.'; this.loading = false; }
    });
  }

  updateStatus(id: number, status: string) {
    this.appointmentService.update(id, { status }).subscribe({
      next: (updated) => {
        const idx = this.appointments.findIndex(a => a.appointmentId === id);
        if (idx !== -1) this.appointments[idx] = updated;
      }
    });
  }
}
