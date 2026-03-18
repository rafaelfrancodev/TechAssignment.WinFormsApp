using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IDogRepository"/>.
/// </summary>
public class DogRepository : IDogRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    /// <summary>
    /// Initializes a new instance with the specified context factory.
    /// </summary>
    public DogRepository(IDbContextFactory<AppDbContext> factory) => _factory = factory;

    /// <inheritdoc />
    public async Task<Dog?> GetByIdAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Dogs
            .Include(d => d.WalkHistory)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Dog>> GetByClientIdAsync(int clientId)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Dogs
            .Where(d => d.ClientId == clientId)
            .Include(d => d.WalkHistory)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Dog>> SearchByClientIdAsync(int clientId, string term)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.Dogs
            .Where(d => d.ClientId == clientId &&
                (d.Name.Contains(term) || d.Breed.Contains(term)))
            .Include(d => d.WalkHistory)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Dog dog)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.Dogs.Add(dog);
        await ctx.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Dog dog)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.Entry(dog).State = EntityState.Modified;
        await ctx.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var dog = await ctx.Dogs
            .Include(d => d.WalkHistory)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (dog is not null)
        {
            ctx.Dogs.Remove(dog);
            await ctx.SaveChangesAsync();
        }
    }
}
