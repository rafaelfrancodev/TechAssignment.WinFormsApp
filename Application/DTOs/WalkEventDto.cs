namespace Application.DTOs;

/// <summary>
/// Data transfer object for walk event display. Mutable for WinForms data binding.
/// </summary>
public class WalkEventDto
{
    /// <summary>Gets or sets the walk event ID.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the dog ID.</summary>
    public int DogId { get; set; }

    /// <summary>Gets or sets the dog name.</summary>
    public string DogName { get; set; } = "";

    /// <summary>Gets or sets the walk date.</summary>
    public DateTime WalkDate { get; set; }

    /// <summary>Gets or sets the duration in minutes.</summary>
    public int DurationMinutes { get; set; }

    /// <summary>Gets or sets optional notes.</summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Parameterless constructor for data binding.
    /// </summary>
    public WalkEventDto() { }

    /// <summary>
    /// Initializes a new instance with the specified values.
    /// </summary>
    public WalkEventDto(int id, int dogId, string dogName, DateTime walkDate, int durationMinutes, string? notes)
    {
        Id = id;
        DogId = dogId;
        DogName = dogName;
        WalkDate = walkDate;
        DurationMinutes = durationMinutes;
        Notes = notes;
    }
}
