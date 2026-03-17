using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

/// <summary>
/// Maps <see cref="WalkEvent"/> domain entities to DTOs.
/// </summary>
public static class WalkEventMapper
{
    /// <summary>
    /// Converts a WalkEvent entity to a WalkEventDto, including dog name.
    /// </summary>
    public static WalkEventDto ToDto(WalkEvent walkEvent, string dogName) =>
        new(walkEvent.Id, walkEvent.DogId, dogName, walkEvent.WalkDate, walkEvent.DurationMinutes, walkEvent.Notes);
}
