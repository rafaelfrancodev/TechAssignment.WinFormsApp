using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Service contract for walk event business operations.
/// </summary>
public interface IWalkEventService
{
    /// <summary>Gets all walk events for a specific dog.</summary>
    Task<IReadOnlyList<WalkEventDto>> GetByDogAsync(int dogId);

    /// <summary>Logs a new walk event for a dog.</summary>
    Task LogWalkAsync(int dogId, DateTime date, int durationMinutes, string? notes);

    /// <summary>Deletes a walk event by ID.</summary>
    Task DeleteAsync(int id);
}
