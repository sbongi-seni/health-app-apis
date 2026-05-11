// src/app/pages/patient/profile/patient-profile.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { PatientService } from '../../../core/services/patient.service';
import { Patient, UpdatePatientRequest } from '../../../core/models/patient.models';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './patient-profile.component.html',
  styleUrl: './patient-profile.component.scss'
})
export class PatientProfileComponent implements OnInit {
  patient: Patient | null = null;
  loading = true;
  saving = false;
  editMode = false;
  error = '';
  successMessage = '';

  editData: UpdatePatientRequest = {};

  constructor(private authService: AuthService, private patientService: PatientService) {}

  ngOnInit() {
    const user = this.authService.currentUser();
    if (user?.patientId) {
      this.patientService.getById(user.patientId).subscribe({
        next: (p) => { this.patient = p; this.loading = false; },
        error: () => { this.error = 'Failed to load profile.'; this.loading = false; }
      });
    } else {
      this.loading = false;
    }
  }

  startEdit() {
    if (!this.patient) return;
    this.editData = {
      firstName: this.patient.firstName,
      lastName: this.patient.lastName,
      phone: this.patient.phone ?? '',
      bloodGroup: this.patient.bloodGroup ?? '',
      gender: this.patient.gender ?? '',
      address: this.patient.address ?? '',
      emergencyContact: this.patient.emergencyContact ?? ''
    };
    this.editMode = true;
  }

  cancelEdit() {
    this.editMode = false;
    this.successMessage = '';
  }

  saveProfile() {
    const user = this.authService.currentUser();
    if (!user?.patientId || !this.patient) return;
    this.saving = true;
    this.patientService.update(user.patientId, this.editData).subscribe({
      next: (updated) => {
        this.patient = updated;
        this.editMode = false;
        this.saving = false;
        this.successMessage = 'Profile updated successfully.';
      },
      error: () => {
        this.saving = false;
        this.error = 'Failed to update profile.';
      }
    });
  }
}
