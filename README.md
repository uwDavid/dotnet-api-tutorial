# .NET API Tutorial

Demonstrates some usefule .NET 8 API controller patterns

The return results of the endpoints follow API Convention.

## To Enable Database

Run PostgreSQL using Docker container

```bash
docker compose up -d
```

Then run database migrations using EF Core

```bash
dotnet ef migrations add InitialCreate
dotnet ef database updates
```

Troubleshooting DB Update
Try adding `TrustServerCertificate=True` in the connection string.
