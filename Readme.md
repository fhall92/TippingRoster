# Tipping Roster — Simple Demo

A small demo app (React + TypeScript frontend, ASP.NET Core (.NET 8) backend) for managing a weekly roster and splitting weekly tips by hours worked.

---

## Prerequisites

- .NET 8 SDK
- Node.js (16+) and npm

---

## Projects

- `TippingRoster.Api` — ASP.NET Core Web API (in-memory data)
- `TippingRoster.Application` — application layer (commands, services, interfaces)
- `TippingRoster.Domain` — domain entities
- `TippingRoster.Infrastructure` — in-memory data, repositories, seeding
- `tipping-roster-ui` — React + TypeScript (Vite) frontend
- `TippingRoster.Tests` — xUnit unit tests

---

## Quick start — run locally

Pick one of two approaches: run API with HTTPS (recommended) or run HTTP and point frontend to it.

### Option A — HTTPS (recommended)

1. Trust the .NET dev certificate (one-time, macOS/Windows will prompt):
   - __dotnet dev-certs https --trust__

2. Run the API using the HTTPS launch profile:
   - __dotnet run --project TippingRoster.Api --launch-profile "https"__

   API will listen on `https://localhost:7229` (and `http://localhost:5249`).

3. Start the frontend:

   ```cd tipping-roster-ui npm install npm run dev```
Vite will print a local URL (e.g. `http://localhost:5173`).

4. Open:
- API Swagger: `https://localhost:7229/swagger`
- Frontend: the Vite URL
  
---

## API endpoints (examples)

- GET /api/employees — list employees
- GET /api/shifts/week — roster for current week (server-side week)
- POST /api/shifts — create shift (body is a CreateShiftCommand)
- GET /api/summary/week — weekly tips summary

Example POST body (client sends):
```{ "employeeId": "GUID", "date": "2025-12-15", "startTime": "2025-12-15T09:00:00", "endTime": "2025-12-15T13:00:00" }```


Note: frontend currently sends `date` as `YYYY-MM-DD` and `startTime`/`endTime` as `YYYY-MM-DDTHH:MM:SS`.

---

## Tests

Run all backend tests:
```dotnet test```

Test project uses xUnit and Moq. Ensure packages are restored.

---

## Important notes / known issues

- DateOnly binding: the backend should register a System.Text.Json converter for `DateOnly` (if not present the model binding will fail for `DateOnly` properties). Add and register a `DateOnlyJsonConverter` in `Program.cs` if needed.
- The app uses an in-memory store (seeded on startup). Data is not persisted across runs.

---

## What I would improve with more time

- Persist data to a small SQLite or JSON file so demo data survives restart.
- Use DTOs for all API responses (decouple domain from contract).
- Add integration tests (WebApplicationFactory) and end-to-end smoke tests for the frontend.
