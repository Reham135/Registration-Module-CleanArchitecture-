# Registration Module - Clean Architecture

## Database Setup & Migrations

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet) (install once if not already installed):
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- SQL Server running locally. The easiest way is via Docker Compose:
  ```bash
  docker compose up -d sqlserver
  ```
  This starts SQL Server on `localhost,14330` (sa / `Your_password123` by default — see `.env.example`).

### Apply migrations and create the database

From the repository root:

```bash
dotnet ef database update --project src/Registration.Persistence --startup-project src/Registration.Api
```

This creates the `RegistrationDb` database (if it doesn't exist) and applies all pending migrations, including seed data for governorates and cities.

> Note: the API also applies pending migrations automatically on startup in the Development environment, so running `dotnet run` (or `docker compose up`) is usually enough — the manual command above is useful when you want to set up the database without starting the API.

### Adding a new migration

After changing entities or EF Core configurations:

```bash
dotnet ef migrations add <MigrationName> --project src/Registration.Persistence --startup-project src/Registration.Api
```

### Removing the last (unapplied) migration

```bash
dotnet ef migrations remove --project src/Registration.Persistence --startup-project src/Registration.Api
```

### Dropping the database

Useful when you want to start from a clean slate (e.g., after regenerating migrations):

```bash
dotnet ef database drop --project src/Registration.Persistence --startup-project src/Registration.Api --force
```
