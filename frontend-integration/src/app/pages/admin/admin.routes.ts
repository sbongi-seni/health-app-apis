// src/app/pages/admin/admin.routes.ts
import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AppointmentComponent } from './appointment/appointment.component';
import { AdminProfileComponent } from './profile/admin-profile.component';
import { PatientRecordsComponent } from './patient-records/patient-records.component';

export const AdminRoutes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'appointments', component: AppointmentComponent },
  { path: 'profile', component: AdminProfileComponent },
  { path: 'patient-records', component: PatientRecordsComponent }
];
