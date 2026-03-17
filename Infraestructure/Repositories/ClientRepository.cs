using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IClientRepository"/>.
/// </summary>
public class ClientRepository : IClientRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    /// <summary>
    /// Initializes a new instance with the specified context factory.
    /// </summary>
    public ClientRepository(IDbContextFactory<AppDbContext> factory) => _factory = factory;

    /// <inheritdoc />
    public async Task<Client?> GetByIdAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Clients
            .Include(c => c.Dogs)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Client>> GetAllAsync()
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Clients
            .Include(c => c.Dogs)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Client>> SearchAsync(string term)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Clients
            .Where(c => c.Name.Contains(term) || c.PhoneNumber.Contains(term))
            .Include(c => c.Dogs)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Client client)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.Clients.Add(client);
        await ctx.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Client client)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        // Attach the detached entity and mark only its own properties as modified
        ctx.Entry(client).State = EntityState.Modified;
        await ctx.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var client = await ctx.Clients.FindAsync(id);
        if (client is not null)
        {
            ctx.Clients.Remove(client);
            await ctx.SaveChangesAsync();
        }
    }
}
