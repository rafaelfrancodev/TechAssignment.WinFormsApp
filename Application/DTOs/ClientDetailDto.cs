namespace Application.DTOs;

/// <summary>
/// Data transfer object for client detail view including associated dogs.
/// </summary>
public class ClientDetailDto
{
    /// <summary>Gets or sets the client data.</summary>
    public ClientDto Client { get; set; } = new();

    /// <summary>Gets or sets the list of dogs.</summary>
    public IReadOnlyList<DogDto> Dogs { get; set; } = [];

    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public ClientDetailDto() { }

    /// <summary>
    /// Initializes a new instance with the specified values.
    /// </summary>
    public ClientDetailDto(ClientDto client, IReadOnlyList<DogDto> dogs)
    {
        Client = client;
        Dogs = dogs;
    }
}
