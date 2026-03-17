using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

/// <summary>
/// Maps <see cref="Client"/> domain entities to DTOs.
/// </summary>
public static class ClientMapper
{
    /// <summary>
    /// Converts a Client entity to a ClientDto.
    /// </summary>
    public static ClientDto ToDto(Client client) =>
        new(client.Id, client.Name, client.PhoneNumber, client.Dogs.Count);
}
