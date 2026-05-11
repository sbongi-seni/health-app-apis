// src/app/pages/patient/patient.routes.ts
import { Routes } from '@angular/router';
import { PatientDashboardComponent } from './dashboard/patient-dashboard.component';
import { PatientProfileComponent } from './profile/patient-profile.component';
import { AppointmenthistoryComponent } from './appointmenthistory/appointmenthistory.component';
import { ChatbotComponent } from './chatbot/chatbot.component';

export const PatientRoutes: Routes = [
  { path: '', component: PatientDashboardComponent },
  { path: 'dashboard', component: PatientDashboardComponent },
  { path: 'appointmenthistory', component: AppointmenthistoryComponent },
  { path: 'chatbot', component: ChatbotComponent },
  { path: 'profile', component: PatientProfileComponent }
];
