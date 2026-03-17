## Dog Walking Business Manager

### Prerequisites
- Visual Studio 2022 (or later)
- .NET 10 SDK (Preview)
- **One of the following** for the database:
  - SQL Server LocalDB (ships with Visual Studio)
  - Docker Desktop (for the Docker Compose option)

### Setup with Docker Compose (recommended)

1. Clone the repository
2. Start the SQL Server container:
   ```bash
   docker compose up -d
   ```
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Apply database migrations:
   ```bash
   dotnet ef database update --project Infraestructure --startup-project TechAssignment.WinFormsApp
   ```
5. Run the application (F5 in Visual Studio or `dotnet run --project TechAssignment.WinFormsApp`)

The `App.config` is pre-configured for Docker (`localhost,1433`). Migrations run automatically on app startup via `ctx.Database.Migrate()`, but you can also apply them manually with the command above.

### Setup with LocalDB (no Docker)

1. In `TechAssignment.WinFormsApp/App.config`, comment out the Docker connection string and uncomment the LocalDB one:
   ```xml
   <!-- Docker (comment out) -->
   <!-- <add name="DogWalking"
        connectionString="Server=localhost,1433;Database=DogWalkingDb;User Id=sa;Password=DogWalk@2024!;TrustServerCertificate=True;"
        ... /> -->

   <!-- LocalDB (uncomment) -->
   <add name="DogWalking"
        connectionString="Server=(localdb)\mssqllocaldb;Database=DogWalkingDb;Trusted_Connection=True;"
        providerName="Microsoft.EntityFrameworkCore.SqlServer" />
   ```
2. Restore and run as above.

### Connection Strings

| Environment | Connection String |
|---|---|
| Docker Compose | `Server=localhost,1433;Database=DogWalkingDb;User Id=sa;Password=DogWalk@2024!;TrustServerCertificate=True;` |
| LocalDB | `Server=(localdb)\mssqllocaldb;Database=DogWalkingDb;Trusted_Connection=True;` |

### First Login
On first run, register a user via the login screen's "Register" button.

### Running Tests
```bash
dotnet test
```

### Stopping the Database
```bash
docker compose down       # stop container, keep data
docker compose down -v    # stop container and delete data volume
```
