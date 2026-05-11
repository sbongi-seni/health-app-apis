// src/app/pages/admin/patient-records/patient-records.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecordService } from '../../../core/services/record.service';
import { MedicalRecord } from '../../../core/models/record.models';

@Component({
  selector: 'app-patient-records',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-records.component.html',
  styleUrl: './patient-records.component.scss'
})
export class PatientRecordsComponent implements OnInit {
  records: MedicalRecord[] = [];
  loading = true;
  error = '';

  constructor(private recordService: RecordService) {}

  ngOnInit() {
    this.recordService.getAdminRecords().subscribe({
      next: (data) => { this.records = data; this.loading = false; },
      error: () => { this.error = 'Failed to load records.'; this.loading = false; }
    });
  }
}
