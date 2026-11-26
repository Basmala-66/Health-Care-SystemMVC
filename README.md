# Health Care System - MVC

## Run with Docker Compose

1. Copy `.env.example` to `.env` and set `MSSQL_SA_PASSWORD` to a strong local development password:

   ```powershell
   Copy-Item .env.example .env
   ```
2. Start the application and SQL Server:

   ```powershell
   docker compose up --build
   ```

3. Open <http://localhost:8080>.

The app waits for SQL Server to become healthy and applies the EF Core migrations when it starts. SQL Server data is stored in the `sqlserver-data` volume.

Run the unit tests in their container separately:

```powershell
docker compose --profile test run --build --rm tests
```

