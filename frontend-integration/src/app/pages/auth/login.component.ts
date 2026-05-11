// src/app/pages/auth/login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
      <div class="card border-0 shadow-sm" style="width: 100%; max-width: 420px;">
        <div class="card-body p-5">
          <div class="text-center mb-4">
            <h2 class="fw-bold">HealthApp</h2>
            <p class="text-muted">Sign in to your account</p>
          </div>

          <div *ngIf="errorMessage" class="alert alert-danger" role="alert">
            {{ errorMessage }}
          </div>

          <form (ngSubmit)="onLogin()" #loginForm="ngForm">
            <div class="mb-3">
              <label for="username" class="form-label">Username</label>
              <input
                id="username"
                type="text"
                class="form-control"
                [(ngModel)]="username"
                name="username"
                required
                placeholder="Enter username"
              />
            </div>
            <div class="mb-4">
              <label for="password" class="form-label">Password</label>
              <input
                id="password"
                type="password"
                class="form-control"
                [(ngModel)]="password"
                name="password"
                required
                placeholder="Enter password"
              />
            </div>
            <button
              type="submit"
              class="btn btn-success w-100"
              [disabled]="loading || !loginForm.valid"
            >
              <span *ngIf="loading" class="spinner-border spinner-border-sm me-2"></span>
              {{ loading ? 'Signing in...' : 'Sign In' }}
            </button>
          </form>

          <div class="mt-4 p-3 bg-light rounded">
            <small class="text-muted d-block"><strong>Demo credentials:</strong></small>
            <small class="text-muted d-block">Admin: admin / Admin@123</small>
            <small class="text-muted d-block">Patient: john.doe / Patient@123</small>
          </div>
        </div>
      </div>
    </div>
  `
})
export class LoginComponent {
  username = '';
  password = '';
  loading = false;
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    this.loading = true;
    this.errorMessage = '';

    this.authService.login({ username: this.username, password: this.password }).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.role === 'Admin') {
          this.router.navigate(['/admin/dashboard']);
        } else {
          this.router.navigate(['/main/dashboard']);
        }
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Invalid username or password. Please try again.';
      }
    });
  }
}
