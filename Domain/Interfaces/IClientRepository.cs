using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Repository contract for <see cref="Client"/> persistence operations.
/// </summary>
public interface IClientRepository
{
    /// <summary>Gets a client by ID, or null if not found.</summary>
    Task<Client?> GetByIdAsync(int id);

    /// <summary>Gets all clients.</summary>
    Task<IReadOnlyList<Client>> GetAllAsync();

    /// <summary>Searches clients by name or phone number.</summary>
    Task<IReadOnlyList<Client>> SearchAsync(string term);

    /// <summary>Adds a new client.</summary>
    Task AddAsync(Client client);

    /// <summary>Updates an existing client.</summary>
    Task UpdateAsync(Client client);

    /// <summary>Deletes a client by ID.</summary>
    Task DeleteAsync(int id);
}
