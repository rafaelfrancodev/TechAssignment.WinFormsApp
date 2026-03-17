using Application.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Implements dog business operations.
/// </summary>
public class DogService : IDogService
{
    private readonly IDogRepository _dogRepo;
    private readonly IClientRepository _clientRepo;

    /// <summary>
    /// Initializes a new instance with the specified repositories.
    /// </summary>
    public DogService(IDogRepository dogRepo, IClientRepository clientRepo)
    {
        _dogRepo = dogRepo;
        _clientRepo = clientRepo;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DogDto>> GetByClientAsync(int clientId)
    {
        var client = await _clientRepo.GetByIdAsync(clientId)
            ?? throw new ApplicationException($"Client with ID {clientId} not found.");

        var dogs = await _dogRepo.GetByClientIdAsync(clientId);
        return dogs.Select(d => DogMapper.ToDto(d, client.Name)).ToList();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(int clientId, string name, string breed, int age)
    {
        var client = await _clientRepo.GetByIdAsync(clientId)
            ?? throw new ApplicationException($"Client with ID {clientId} not found.");

        try
        {
            var dog = new Dog(clientId, name, breed, age);
            await _dogRepo.AddAsync(dog);
            return dog.Id;
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, string name, string breed, int age)
    {
        var dog = await _dogRepo.GetByIdAsync(id)
            ?? throw new ApplicationException($"Dog with ID {id} not found.");

        try
        {
            dog.Update(name, breed, age);
            await _dogRepo.UpdateAsync(dog);
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await _dogRepo.DeleteAsync(id);
    }
}
