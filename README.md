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


## Optional: Running on Different Hosts

To simulate different hostnames (e.g., demoproject.local and api.demoproject.local):

1. Edit hosts file:

C:\Windows\System32\drivers\etc\hosts

Add:

127.0.0.1 demoproject.local
127.0.0.1 api.demoproject.local

2. Update launchSettings.json

3. Update Api:BaseUrl in MVC

4. Update allowed origins in CORS configuration

Note: Custom HTTPS hostnames may require trusting a development certificate.

## Architecture Notes

1. MVC does not reference database entities.
2. DTOs are shared via a separate Contracts project.
3. HttpClient is registered using typed clients.
4. CancellationToken is propagated across layers.
5. AsNoTracking is used for read-only queries.
