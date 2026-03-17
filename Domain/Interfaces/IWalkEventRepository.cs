using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Repository contract for <see cref="WalkEvent"/> persistence operations.
/// </summary>
public interface IWalkEventRepository
{
    /// <summary>Gets all walk events for a specific dog.</summary>
    Task<IReadOnlyList<WalkEvent>> GetByDogIdAsync(int dogId);

    /// <summary>Adds a new walk event.</summary>
    Task AddAsync(WalkEvent walkEvent);

    /// <summary>Deletes a walk event by ID.</summary>
    Task DeleteAsync(int id);
}
