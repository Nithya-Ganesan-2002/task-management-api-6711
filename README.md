# Project Repository

This is the initial README file for the project.

## TaskFlow API Backend

The TaskFlow API is a .NET 8 Web API following clean architecture principles (Controllers, Services, Repositories, Models) with EF Core for SQL Server and JWT-based authentication.

### Setup

1. Copy `.env.example` to `.env` in `taskflow_api_backend/` and fill in values:
   - `SQLSERVER_CONNECTION_STRING`
   - `JWT_SECRET`
   - `JWT_ISSUER` (optional)
   - `JWT_AUDIENCE` (optional)
   - `JWT_EXPIRATION_MINUTES` (optional)

2. Restore packages and run:
   - dotnet restore
   - dotnet run --project taskflow_api_backend/dotnet.csproj

3. Open API Docs
   - Swagger UI is available at `/docs`.

### API Summary

- Auth
  - POST `/api/auth/register` Register new user -> JWT token
  - POST `/api/auth/login` Login -> JWT token

- Users
  - GET `/api/users/me` Get current user
  - GET `/api/users` List users

- Tasks
  - POST `/api/tasks` Create task
  - GET `/api/tasks` List tasks for current user
  - GET `/api/tasks/{id}` Get task by ID
  - PUT `/api/tasks/{id}` Update task
  - DELETE `/api/tasks/{id}` Delete task (creator only)

### Notes
- The server will create the database schema if it does not exist (EnsureCreated). For production deployments, use EF Core migrations instead.
