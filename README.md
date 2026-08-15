# AzilEdu — Animal Shelter Management System

Blazor Server + ASP.NET Core Web API application for managing animals, volunteers, donors, employees, and donations in an animal shelter. Includes JWT authentication, role-based authorization, and AI service integration.

---

## Running the application

### Prerequisites

* .NET 10 SDK
* Visual Studio 2022+ or VS Code with the C# extension

### 1. Run the API project

```bash
cd AzilEdu.Api
dotnet run
```

The API runs on:

* HTTPS: `https://localhost:7205`
* HTTP: `http://localhost:5195`
* Swagger UI: `https://localhost:7205/swagger`

On the first run, all EF Core migrations are applied automatically and demo data is seeded (users, animals, volunteers, donors, and employees).

### 2. Run the App project

```bash
cd AzilEdu.App
dotnet run
```

The App runs on:

* HTTPS: `https://localhost:7298`
* HTTP: `http://localhost:5163`

**Important:** The API must be running before the App project because the App immediately communicates with `https://localhost:7205`.

### Running from Visual Studio

Open `AzilEdu.slnx`, right-click the Solution → **Set Startup Projects** → select **Multiple startup projects** and set both the API and App projects to **Start**. Press **F5**.

### Reset the database

```bash
cd AzilEdu.Api
dotnet ef database update 0   # drops all tables
dotnet ef database update     # reapplies all migrations from scratch
```

---

## Demo users

> These credentials are for local development only. Do not use them in production.

| Email                     | Password        | Roles           |
| ------------------------- | --------------- | --------------- |
| `admin@aziledu.local`     | `Admin123!`     | Admin, User     |
| `employee@aziledu.local`  | `Employee123!`  | Employee, User  |
| `volunteer@aziledu.local` | `Volunteer123!` | Volunteer, User |
| `donor@aziledu.local`     | `Donor123!`     | Donor, User     |

The admin account has access to all modules. Volunteers and donors can access only their own data through the `/mine` endpoints.

---

## User account relationships

### AppUser → AppRole (via AppUserRole)

`AppUser` and `AppRole` have a many-to-many relationship through the `AppUserRoles` junction table with a composite primary key `(AppUserId, AppRoleId)`. A single user can have multiple roles (for example, Admin + User). Roles are embedded into the JWT token as `role` claims and are validated on every request using `[Authorize(Roles = "...")]`.

### AppUser → Volunteer

`AppUser` contains an optional foreign key `VolunteerId` that references the `Volunteers` table. This is a one-to-one relationship, meaning one user account can be linked to at most one volunteer profile. When a user is authenticated with the Volunteer role and has a `VolunteerId`, the `/mine` endpoints read that ID from the JWT token (`volunteerId` claim) instead of a URL parameter, preventing access to other volunteers' data.

### AppUser → Donor

The same pattern as Volunteer applies here. `AppUser` contains an optional `DonorId` foreign key. A donor can access only donations associated with their own `DonorId` through `GET /api/donations/mine`.

### AppUser → Employee

`AppUser` contains an optional `EmployeeId` foreign key linking the account to an employee profile. Employees have the `Employee` role and access to operational modules (animals, tasks, donors, and donations).

---

## Difference between 401 and 403

| Status               | Meaning                        | When it occurs                                                                                                                                        |
| -------------------- | ------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| **401 Unauthorized** | Identity could not be verified | The request has no Bearer token, the token has expired, or it is invalid. The API does not know who you are.                                          |
| **403 Forbidden**    | Access denied                  | The token is valid and the API knows who you are, but you do not have sufficient permissions. For example, a volunteer trying to access `/api/users`. |

A practical rule: **401 = not authenticated, 403 = authenticated but not authorized**.

---

## AI endpoints

All AI endpoints send data **only to the configured provider** (Mock or OpenAI). The API never stores AI responses automatically. Responses are returned to the client, which can edit or discard them before optionally saving them through the standard CRUD workflow.

| Endpoint                             | Method | Authorization   | Data sent to the provider                                                                                                   |
| ------------------------------------ | ------ | --------------- | --------------------------------------------------------------------------------------------------------------------------- |
| `GET /api/ai/status`                 | GET    | Admin, Employee | Nothing; returns the provider and model name                                                                                |
| `POST /api/ai/text`                  | POST   | Admin, Employee | Purpose (`animal-adoption`, `donor-thank-you`, `social-post`) and user input (max 4000 characters)                          |
| `GET /api/ai/daily-summary`          | GET    | Admin, Employee | Aggregated database statistics: total animals, available animals, open tasks, overdue tasks, donations from the last 7 days |
| `GET /api/ai/volunteer-summary/mine` | GET    | Volunteer       | Up to 10 open tasks assigned to the authenticated volunteer (title, type, animal, status, due date)                         |
| `POST /api/ai/animal-intake`         | POST   | Admin, Employee | Free-form intake notes from the field (max 4000 characters)                                                                 |
| `POST /api/ai/animal-data-check`     | POST   | Admin, Employee | Animal form data (name, species, breed, sex, age, arrival date, status, description)                                        |

**No personal data (email, password, address, etc.) is ever sent to the AI provider.**

---

## Mock and OpenAI modes

### Mock (default for development)

`appsettings.json` is already configured for Mock mode:

```json
"Ai": {
  "Provider": "Mock",
  "Model": "gpt-5.6-luna",
  "ApiKey": ""
}
```

The Mock service returns predictable local responses without making network calls.

### OpenAI (for production/demo use)

**Never store the API key in `appsettings.json` or commit it to the repository.**

Use .NET User Secrets:

```bash
cd AzilEdu.Api
dotnet user-secrets set "Ai:Provider" "OpenAI"
dotnet user-secrets set "Ai:Model" "gpt-4o-mini"
dotnet user-secrets set "Ai:ApiKey" "sk-..."
```

User Secrets are stored locally in `%APPDATA%\\Microsoft\\UserSecrets\\<id>\\secrets.json` and **are not part of the repository** (`.gitignore` excludes them automatically).

To switch back to Mock mode:

```bash
dotnet user-secrets set "Ai:Provider" "Mock"
```

---

## Why the keys remain in the API project

The JWT `SigningKey` and AI `ApiKey` are configured exclusively in the API project for security reasons:

* **The API** is the only component that validates and signs JWT tokens.
* **The App** (Blazor Server) acts only as an API client and receives already-issued JWT tokens.
* AI requests are performed only on the API side.
* If the App source code is compromised, the JWT signing key and AI API key remain protected.

---

## Authorization flow

```text
UI action
  → DTO object
  → HTTP request with Authorization: Bearer <token>
  → JwtBearerMiddleware validates token signature and expiration (401 if invalid)
  → [Authorize] checks role claims (403 if insufficient permissions)
  → /mine endpoints read volunteerId/donorId claims from the token
  → API controller
  → DbContext → SQLite database
  → (optional) AI service → controlled response
  → JSON response to client
  → User edits or discards the AI suggestion
  → Optional persistence through standard CRUD endpoints
```

---

## Known limitations

1. **No JWT refresh tokens** — tokens expire after 60 minutes and users must log in again.
2. **SQLite is not suitable for production** — limited concurrent writes, no replication; PostgreSQL or SQL Server should be used instead.
3. **Media files are stored locally** (`wwwroot/uploads/animals/`) and are not suitable for multi-instance or cloud deployments without additional configuration.
4. **AI does not retain conversational context** — each request is independent.
5. **Volunteers and donors cannot edit their own profiles** — self-service profile management is not implemented.

---

## Suggested improvements for the next version

1. **Refresh token mechanism** — introduce short-lived access tokens (15 minutes) and rotating refresh tokens to improve security while eliminating frequent re-authentication.

2. **Cloud media storage** — replace local file storage with Azure Blob Storage or AWS S3 using SAS/presigned URLs, enabling horizontal scaling and cloud-friendly deployments.

---

*Documentation generated for the AzilEdu project — August 2026.*
