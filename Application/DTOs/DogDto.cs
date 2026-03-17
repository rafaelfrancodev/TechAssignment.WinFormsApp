namespace Application.DTOs;

/// <summary>
/// Data transfer object for dog display. Mutable for WinForms data binding.
/// </summary>
public class DogDto
{
    /// <summary>Gets or sets the dog ID.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the owning client ID.</summary>
    public int ClientId { get; set; }

    /// <summary>Gets or sets the owning client name.</summary>
    public string ClientName { get; set; } = "";

    /// <summary>Gets or sets the dog name.</summary>
    public string Name { get; set; } = "";

    /// <summary>Gets or sets the breed.</summary>
    public string Breed { get; set; } = "";

    /// <summary>Gets or sets the age.</summary>
    public int Age { get; set; }

    /// <summary>
    /// Parameterless constructor for data binding.
    /// </summary>
    public DogDto() { }

    /// <summary>
    /// Initializes a new instance with the specified values.
    /// </summary>
    public DogDto(int id, int clientId, string clientName, string name, string breed, int age)
    {
        Id = id;
        ClientId = clientId;
        ClientName = clientName;
        Name = name;
        Breed = breed;
        Age = age;
    }
}
