# HealthApp API

ASP.NET Core 10 RESTful Web API for the HealthApp — a health management application supporting Patient and Admin roles.

## Tech Stack

- **ASP.NET Core 10** Web API
- **Entity Framework Core 10** with SQL Server
- **JWT Bearer Authentication**
- **BCrypt** password hashing
- **OpenAPI / Swagger** (development)

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (local or Azure)

### 1. Configure the connection string

Edit `src/HealthApp.Api/appsettings.json` and set your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HealthAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "HealthApp_SuperSecretKey_Replace_In_Production_32chars!",
    "Issuer": "HealthApp.Api",
    "Audience": "HealthApp.Angular"
  }
}
```

> ⚠️ **Important:** Replace the `Jwt:Key` with a strong secret in production. Store it in environment variables or Azure Key Vault — never commit the real key.

### 2. Run the API

```bash
cd src/HealthApp.Api
dotnet run
```

The API starts on `https://localhost:7xxx` / `http://localhost:5xxx`. The database is automatically created and seeded on first run.

### 3. OpenAPI / Swagger

In development, the OpenAPI schema is available at:

```
https://localhost:{port}/openapi/v1.json
```

---

## Seed Data

On first run the database is seeded with:

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `john.doe` | `Patient@123` | Patient |
| `jane.smith` | `Patient@123` | Patient |
| `sipho.zulu` | `Patient@123` | Patient |

---

## API Endpoints

### Authentication

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/auth/login` | Login and receive JWT token | Public |

**Request body:**
```json
{ "username": "admin", "password": "Admin@123" }
```

**Response:**
```json
{
  "token": "eyJ...",
  "role": "Admin",
  "userId": 1,
  "patientId": null,
  "name": "admin",
  "email": "admin@healthapp.com"
}
```

---

### Patients

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/patients` | List all patients | Admin |
| `GET` | `/api/patients/{id}` | Get patient by ID | Any |
| `POST` | `/api/patients` | Create patient + user account | Admin |
| `PUT` | `/api/patients/{id}` | Update patient profile | Any |

---

### Appointments

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/appointments/admin` | List all appointments | Admin |
| `GET` | `/api/appointments/patient/{patientId}` | List patient appointments | Any |
| `GET` | `/api/appointments/{id}` | Get appointment by ID | Any |
| `POST` | `/api/appointments` | Book an appointment | Any |
| `PUT` | `/api/appointments/{id}` | Update appointment | Any |

---

### Medical Records

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/records/admin` | List all records | Admin |
| `GET` | `/api/records/patient/{patientId}` | List patient records | Any |
| `GET` | `/api/records/{id}` | Get record by ID | Any |
| `POST` | `/api/records` | Create medical record | Admin |

---

### Admin Dashboard

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/admin/dashboard` | Summary stats + recent activity | Admin |

**Response:**
```json
{
  "totalPatients": 3,
  "todayAppointments": 1,
  "upcomingAppointments": 3,
  "totalRecords": 3,
  "recentActivity": [
    {
      "patientName": "Sipho Zulu",
      "condition": "Asthma",
      "doctorName": "Dr. Johnson",
      "status": "Active",
      "lastVisit": "2025-11-15"
    }
  ]
}
```

---

## Frontend Integration (Angular)

The Angular app (`sbongi-seni/health-app`) connects to this API.

### Environment configuration

Set the API base URL in `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'
};
```

### CORS

The API allows cross-origin requests from:
- `http://localhost:4200`
- `https://localhost:4200`

### Auth flow

1. POST to `/api/auth/login` with credentials
2. Store the returned `token`, `role`, `userId`, and `patientId` in `localStorage`
3. Include the token as `Authorization: Bearer <token>` on all subsequent requests
4. Use `role` to decide patient vs admin navigation

---

## Data Model

```
User (UserId, Username, PasswordHash, Role, Email)
  └── Patient (PatientId, UserId FK, FirstName, LastName, DOB, Gender, Phone, BloodGroup, ...)
        ├── Appointment (AppointmentId, PatientId FK, DoctorName, DateTime, Status, Notes)
        └── MedicalRecord (RecordId, PatientId FK, Diagnosis, Treatment, DoctorName, VisitDate, Notes)
```
