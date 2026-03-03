# Northwind Demo

ASP.NET Core solution containing:

- Web API
- MVC client
- Contracts project
- Unit tests
- CORS configuration
- Cancellation token support

## Run

1. Ensure SQL Server is running
2. Set both API and MVC as startup projects
3. Run solution

Default URLs:
- MVC: https://localhost:7265
- API: https://localhost:7013

## Database Setup

1. Install SQL Server (if not installed).
2. Restore the Northwind database.
3. Ensure the SQL Server service is running.
4. Default development connection string:

"ConnectionStrings": {
  "Northwind": "Server=localhost;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;"
}

If needed, update appsettings.Development.json.
