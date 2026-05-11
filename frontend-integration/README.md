# Frontend Integration Guide

This folder contains all Angular files needed to wire the `health-app` Angular frontend to the `health-app-apis` backend.

## Copy instructions

Copy each file from `frontend-integration/src/` into the corresponding path in your `health-app` Angular project under `src/`.

```
frontend-integration/src/
├── environments/
│   ├── environment.ts                    → src/environments/environment.ts
│   └── environment.prod.ts               → src/environments/environment.prod.ts
└── app/
    ├── app.config.ts                     → src/app/app.config.ts  (replace existing)
    ├── core/
    │   ├── models/
    │   │   ├── auth.models.ts
    │   │   ├── patient.models.ts
    │   │   ├── appointment.models.ts
    │   │   ├── record.models.ts
    │   │   └── dashboard.models.ts
    │   ├── services/
    │   │   ├── auth.service.ts
    │   │   ├── patient.service.ts
    │   │   ├── appointment.service.ts
    │   │   ├── record.service.ts
    │   │   └── admin.service.ts
    │   ├── interceptors/
    │   │   └── auth.interceptor.ts
    │   └── guards/
    │       └── auth.guard.ts
    └── pages/
        ├── auth/
        │   └── login.component.ts        (inline template, no separate HTML)
        ├── patient/
        │   ├── patient.routes.ts         → replace existing
        │   ├── dashboard/
        │   │   ├── patient-dashboard.component.ts   → replace existing
        │   │   └── patient-dashboard.component.html → replace existing
        │   ├── profile/
        │   │   ├── patient-profile.component.ts     → replace existing
        │   │   └── patient-profile.component.html   → replace existing
        │   └── appointmenthistory/
        │       ├── appointmenthistory.component.ts  → replace existing
        │       └── appointmenthistory.component.html → replace existing
        └── admin/
            ├── admin.routes.ts           → replace existing
            ├── dashboard/
            │   ├── dashboard.component.ts   → replace existing
            │   └── dashboard.component.html → replace existing
            ├── appointment/
            │   ├── appointment.component.ts  → replace existing
            │   └── appointment.component.html → replace existing
            └── patient-records/
                ├── patient-records.component.ts  → replace existing
                └── patient-records.component.html → replace existing
```

## Setup steps

### 1. Set the API URL

Edit `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'   // match your backend launch URL
};
```

### 2. Replace app.config.ts

Replace your existing `src/app/app.config.ts` with the one provided. This wires up:
- `HttpClient`
- The JWT `authInterceptor` (automatically attaches `Authorization: Bearer <token>`)

### 3. Add the login route

In your `src/app/app.routes.ts`, add a route for the login page:

```typescript
import { LoginComponent } from './pages/auth/login.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  // ... your existing main and admin routes
];
```

### 4. Sidebar sign-out button

In your patient sidebar (`src/app/layouts/main/main.component.html`) and admin sidebar (`src/app/layouts/admin/admin.component.html`), update the sign-out button to call the auth service:

```typescript
// In main.component.ts / admin.component.ts:
import { AuthService } from '../../core/services/auth.service';

constructor(public authService: AuthService) {}
```

```html
<!-- Sign out link -->
<a class="dropdown-item" href="#" (click)="authService.logout(); $event.preventDefault()">Sign out</a>
```

### 5. Show logged-in user name in sidebars

In patient sidebar, replace static "Patient User" with:
```html
<strong>{{ authService.currentUser()?.name ?? 'Patient' }}</strong>
```

In admin sidebar, replace static "Admin User" with:
```html
<strong>{{ authService.currentUser()?.name ?? 'Admin' }}</strong>
```

## How auth works

1. User visits `/login` → enters credentials
2. `AuthService.login()` calls `POST /api/auth/login`
3. Backend returns `{ token, role, userId, patientId, name, email }`
4. Token + user info stored in `localStorage`
5. `authInterceptor` attaches `Authorization: Bearer <token>` to every HTTP request
6. On success, user is redirected to `/main/dashboard` (patient) or `/admin/dashboard` (admin)
7. `AuthService.logout()` clears localStorage and redirects to `/login`

## Seed credentials (for testing)

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `john.doe` | `Patient@123` | Patient |
| `jane.smith` | `Patient@123` | Patient |
| `sipho.zulu` | `Patient@123` | Patient |
