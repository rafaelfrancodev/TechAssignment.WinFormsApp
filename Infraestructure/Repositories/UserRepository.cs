using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IUserRepository"/>.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    /// <summary>
    /// Initializes a new instance with the specified context factory.
    /// </summary>
    public UserRepository(IDbContextFactory<AppDbContext> factory) => _factory = factory;

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <inheritdoc />
    public async Task AddAsync(User user)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
    }
}
