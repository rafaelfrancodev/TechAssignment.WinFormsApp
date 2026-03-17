using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Repository contract for <see cref="Dog"/> persistence operations.
/// </summary>
public interface IDogRepository
{
    /// <summary>Gets a dog by ID, or null if not found.</summary>
    Task<Dog?> GetByIdAsync(int id);

    /// <summary>Gets all dogs belonging to a specific client.</summary>
    Task<IReadOnlyList<Dog>> GetByClientIdAsync(int clientId);

    /// <summary>Adds a new dog.</summary>
    Task AddAsync(Dog dog);

    /// <summary>Updates an existing dog.</summary>
    Task UpdateAsync(Dog dog);

    /// <summary>Deletes a dog by ID.</summary>
    Task DeleteAsync(int id);
}
