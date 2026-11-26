# Health Care System - MVC (DEPI MVC structure)

The original Web API project has been restructured into three projects following the reference DEPI MVC sample: **HealthCare.DAL**, **HealthCare.BLL**, and **HealthCare.PL**. The domain remains Patient, Doctor, Appointment. MVC Razor CRUD screens are provided for all three.

## Architecture
- DAL: Entities, EF Core contexts, configurations, interceptors, migrations.
- BLL: Generic Repository, Appointment Repository, Unit of Work, Services, Result and PaginatedList.
- PL: MVC Controllers, ViewModels, AutoMapper profile, Razor Views, Identity cookie login/register.

## Run
1. Open `HealthCareSystem.sln` in Visual Studio.
2. Check the SQL Server connection strings in `HealthCare.PL/appsettings.json`.
3. Set `HealthCare.PL` as startup project.
4. Restore NuGet packages and build.
5. Run database migrations if your local database is not already at the included migration level.

The source archive's AppDbContext migrations and Identity migrations were retained. `AppDbContext` now uses the configured connection string rather than a hard-coded one.

## Important
The original JWT API login was replaced by MVC Identity cookie authentication because this is now an MVC application, matching the reference MVC project's Identity-based web flow. The Patient/Doctor/Appointment domain and soft-delete/audit interceptors are retained. The old API DTOs and Swagger endpoints are not part of this MVC version.
