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

Test project uses xUnit and Moq. Ensure packages are restored.

---

## Notes

- The app uses an in-memory store (seeded on startup). Data is not persisted across runs.
- Controllers currently return simple shapes; for a stable public API you may want to add DTOs and mapping.

---

## Useful commands

- Start API (http): `dotnet run --project TippingRoster.Api`
- Start API (https profile): `dotnet run --project TippingRoster.Api --launch-profile "https"`
- Start frontend: `cd tipping-roster-ui && npm install && npm run dev`
- Run tests: `dotnet test`

---

## What I would improve with more time

- Persist data to a small SQLite or JSON file so demo data survives restart.
- Use DTOs for all API responses (decouple domain from contract).
- Add integration tests (WebApplicationFactory) and end-to-end smoke tests for the frontend.