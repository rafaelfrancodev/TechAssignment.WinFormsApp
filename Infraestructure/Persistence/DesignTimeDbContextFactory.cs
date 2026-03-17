using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infraestructure.Persistence;

/// <summary>
/// Factory for creating AppDbContext at design time (migrations, scaffolding).
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates a new AppDbContext for EF Core tooling.
    /// </summary>
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=DogWalkingDb;User Id=sa;Password=DogWalk@2024!;TrustServerCertificate=True;");
        return new AppDbContext(optionsBuilder.Options);
    }
}
