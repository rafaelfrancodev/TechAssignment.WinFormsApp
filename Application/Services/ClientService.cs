using Application.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Implements client business operations.
/// </summary>
public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepo;

    /// <summary>
    /// Initializes a new instance with the specified repository.
    /// </summary>
    public ClientService(IClientRepository clientRepo)
    {
        _clientRepo = clientRepo;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ClientDto>> GetAllAsync()
    {
        var clients = await _clientRepo.GetAllAsync();
        return clients.Select(ClientMapper.ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ClientDto>> SearchAsync(string term)
    {
        var clients = await _clientRepo.SearchAsync(term);
        return clients.Select(ClientMapper.ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<ClientDetailDto> GetDetailAsync(int clientId)
    {
        var client = await _clientRepo.GetByIdAsync(clientId)
            ?? throw new ApplicationException($"Client with ID {clientId} not found.");

        var clientDto = ClientMapper.ToDto(client);
        var dogDtos = client.Dogs.Select(d => DogMapper.ToDto(d, client.Name)).ToList();
        return new ClientDetailDto(clientDto, dogDtos);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(string name, string phone)
    {
        try
        {
            var client = new Client(name, phone);
            await _clientRepo.AddAsync(client);
            return client.Id;
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, string name, string phone)
    {
        var client = await _clientRepo.GetByIdAsync(id)
            ?? throw new ApplicationException($"Client with ID {id} not found.");

        try
        {
            client.UpdateContact(name, phone);
            await _clientRepo.UpdateAsync(client);
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await _clientRepo.DeleteAsync(id);
    }
}
