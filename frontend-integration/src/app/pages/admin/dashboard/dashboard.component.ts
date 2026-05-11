// src/app/pages/admin/dashboard/dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../../core/services/admin.service';
import { DashboardStats } from '../../../core/models/dashboard.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  loading = true;
  error = '';

  constructor(private adminService: AdminService) {}

  ngOnInit() {
    this.adminService.getDashboard().subscribe({
      next: (data) => { this.stats = data; this.loading = false; },
      error: () => { this.error = 'Failed to load dashboard.'; this.loading = false; }
    });
  }

  get statCards() {
    if (!this.stats) return [];
    return [
      { label: 'Total Patients', value: this.stats.totalPatients, icon: '👥', color: 'primary' },
      { label: 'Appointments Today', value: this.stats.todayAppointments, icon: '📅', color: 'success' },
      { label: 'Upcoming Appointments', value: this.stats.upcomingAppointments, icon: '🗓️', color: 'warning' },
      { label: 'Total Records', value: this.stats.totalRecords, icon: '🗂️', color: 'dark' }
    ];
  }
}
