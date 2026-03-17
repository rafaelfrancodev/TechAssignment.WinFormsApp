using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence;

/// <summary>
/// Entity Framework Core database context for the Dog Walking application.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>Gets the Clients table.</summary>
    public DbSet<Client> Clients => Set<Client>();

    /// <summary>Gets the Dogs table.</summary>
    public DbSet<Dog> Dogs => Set<Dog>();

    /// <summary>Gets the WalkEvents table.</summary>
    public DbSet<WalkEvent> WalkEvents => Set<WalkEvent>();

    /// <summary>Gets the Users table.</summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Initializes a new instance with the specified options.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
