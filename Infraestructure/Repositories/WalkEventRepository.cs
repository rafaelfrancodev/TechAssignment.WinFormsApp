using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IWalkEventRepository"/>.
/// </summary>
public class WalkEventRepository : IWalkEventRepository
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    /// <summary>
    /// Initializes a new instance with the specified context factory.
    /// </summary>
    public WalkEventRepository(IDbContextFactory<AppDbContext> factory) => _factory = factory;

    /// <inheritdoc />
    public async Task<IReadOnlyList<WalkEvent>> GetByDogIdAsync(int dogId)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        return await ctx.WalkEvents
            .Where(w => w.DogId == dogId)
            .OrderByDescending(w => w.WalkDate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(WalkEvent walkEvent)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        ctx.WalkEvents.Add(walkEvent);
        await ctx.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var walkEvent = await ctx.WalkEvents.FindAsync(id);
        if (walkEvent is not null)
        {
            ctx.WalkEvents.Remove(walkEvent);
            await ctx.SaveChangesAsync();
        }
    }
}
