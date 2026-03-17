using Application.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Implements walk event business operations.
/// </summary>
public class WalkEventService : IWalkEventService
{
    private readonly IWalkEventRepository _walkRepo;
    private readonly IDogRepository _dogRepo;

    /// <summary>
    /// Initializes a new instance with the specified repositories.
    /// </summary>
    public WalkEventService(IWalkEventRepository walkRepo, IDogRepository dogRepo)
    {
        _walkRepo = walkRepo;
        _dogRepo = dogRepo;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<WalkEventDto>> GetByDogAsync(int dogId)
    {
        var dog = await _dogRepo.GetByIdAsync(dogId)
            ?? throw new ApplicationException($"Dog with ID {dogId} not found.");

        var walks = await _walkRepo.GetByDogIdAsync(dogId);
        return walks.Select(w => WalkEventMapper.ToDto(w, dog.Name)).ToList();
    }

    /// <inheritdoc />
    public async Task LogWalkAsync(int dogId, DateTime date, int durationMinutes, string? notes)
    {
        var dog = await _dogRepo.GetByIdAsync(dogId)
            ?? throw new ApplicationException($"Dog with ID {dogId} not found.");

        try
        {
            var walkEvent = new WalkEvent(dogId, date, durationMinutes, notes);
            await _walkRepo.AddAsync(walkEvent);
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await _walkRepo.DeleteAsync(id);
    }
}
