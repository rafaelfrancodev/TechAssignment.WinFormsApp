using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Service contract for dog business operations.
/// </summary>
public interface IDogService
{
    /// <summary>Gets all dogs for a specific client.</summary>
    Task<IReadOnlyList<DogDto>> GetByClientAsync(int clientId);

    /// <summary>Searches dogs by name or breed within a specific client.</summary>
    Task<IReadOnlyList<DogDto>> SearchByClientAsync(int clientId, string term);

    /// <summary>Creates a new dog and returns the generated ID.</summary>
    Task<int> CreateAsync(int clientId, string name, string breed, int age);

    /// <summary>Updates an existing dog's information.</summary>
    Task UpdateAsync(int id, string name, string breed, int age);

    /// <summary>Deletes a dog by ID.</summary>
    Task DeleteAsync(int id);
}
