namespace Application.DTOs;

/// <summary>
/// Data transfer object for client display. Mutable for WinForms data binding.
/// </summary>
public class ClientDto
{
    /// <summary>Gets or sets the client ID.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the client name.</summary>
    public string Name { get; set; } = "";

    /// <summary>Gets or sets the phone number.</summary>
    public string PhoneNumber { get; set; } = "";

    /// <summary>Gets or sets the dog count.</summary>
    public int DogCount { get; set; }

    /// <summary>
    /// Parameterless constructor for data binding.
    /// </summary>
    public ClientDto() { }

    /// <summary>
    /// Initializes a new instance with the specified values.
    /// </summary>
    public ClientDto(int id, string name, string phoneNumber, int dogCount)
    {
        Id = id;
        Name = name;
        PhoneNumber = phoneNumber;
        DogCount = dogCount;
    }
}
