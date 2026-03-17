using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a single walk event for a dog.
/// </summary>
public class WalkEvent
{
    /// <summary>Gets the unique identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the associated dog's identifier.</summary>
    public int DogId { get; private set; }

    /// <summary>Gets the date of the walk.</summary>
    public DateTime WalkDate { get; private set; }

    /// <summary>Gets the walk duration in minutes.</summary>
    public int DurationMinutes { get; private set; }

    /// <summary>Gets optional notes about the walk.</summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Creates a new walk event with validated properties.
    /// </summary>
    public WalkEvent(int dogId, DateTime walkDate, int durationMinutes, string? notes)
    {
        if (dogId <= 0)
            throw new DomainException("Dog ID must be a positive integer.");
        ValidateWalkDate(walkDate);
        ValidateDuration(durationMinutes);
        DogId = dogId;
        WalkDate = walkDate;
        DurationMinutes = durationMinutes;
        Notes = notes;
    }

    private static void ValidateWalkDate(DateTime walkDate)
    {
        if (walkDate < new DateTime(2000, 1, 1))
            throw new DomainException("Walk date cannot be before year 2000.");
        if (walkDate > DateTime.Now.AddYears(1))
            throw new DomainException("Walk date cannot be more than 1 year in the future.");
    }

    private static void ValidateDuration(int durationMinutes)
    {
        if (durationMinutes < 15 || durationMinutes > 240)
            throw new DomainException("Walk duration must be between 15 and 240 minutes.");
    }

    /// <summary>
    /// EF Core parameterless constructor.
    /// </summary>
    private WalkEvent() { }
}
