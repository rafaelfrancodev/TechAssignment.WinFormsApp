using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Service contract for client business operations.
/// </summary>
public interface IClientService
{
    /// <summary>Gets all clients.</summary>
    Task<IReadOnlyList<ClientDto>> GetAllAsync();

    /// <summary>Searches clients by name or phone.</summary>
    Task<IReadOnlyList<ClientDto>> SearchAsync(string term);

    /// <summary>Gets full client detail including dogs.</summary>
    Task<ClientDetailDto> GetDetailAsync(int clientId);

    /// <summary>Creates a new client and returns the generated ID.</summary>
    Task<int> CreateAsync(string name, string phone);

    /// <summary>Updates an existing client's contact information.</summary>
    Task UpdateAsync(int id, string name, string phone);

    /// <summary>Deletes a client by ID.</summary>
    Task DeleteAsync(int id);
}
