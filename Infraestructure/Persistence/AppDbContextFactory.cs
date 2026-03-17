using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence;

/// <summary>
/// Factory that creates a new <see cref="AppDbContext"/> per operation,
/// avoiding DbContext concurrency issues in WinForms async scenarios.
/// </summary>
public class AppDbContextFactory : IDbContextFactory<AppDbContext>
{
    private readonly DbContextOptions<AppDbContext> _options;

    /// <summary>
    /// Initializes a new instance with the specified options.
    /// </summary>
    public AppDbContextFactory(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    /// <summary>
    /// Creates a new AppDbContext instance.
    /// </summary>
    public AppDbContext CreateDbContext()
    {
        return new AppDbContext(_options);
    }
}
