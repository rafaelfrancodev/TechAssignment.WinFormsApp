using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a dog belonging to a client.
/// </summary>
public class Dog
{
    /// <summary>Gets the unique identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the owning client's identifier.</summary>
    public int ClientId { get; private set; }

    /// <summary>Gets the dog's name.</summary>
    public string Name { get; private set; }

    /// <summary>Gets the dog's breed.</summary>
    public string Breed { get; private set; }

    /// <summary>Gets the dog's age in years.</summary>
    public int Age { get; private set; }

    /// <summary>Gets the read-only walk history for this dog.</summary>
    public IReadOnlyCollection<WalkEvent> WalkHistory => _walkHistory.AsReadOnly();
    private readonly List<WalkEvent> _walkHistory = new();

    /// <summary>
    /// Creates a new dog with validated properties.
    /// </summary>
    public Dog(int clientId, string name, string breed, int age)
    {
        if (clientId <= 0)
            throw new DomainException("Client ID must be a positive integer.");
        ValidateName(name);
        ValidateBreed(breed);
        ValidateAge(age);
        ClientId = clientId;
        Name = name;
        Breed = breed;
        Age = age;
    }

    /// <summary>
    /// Updates the dog's information.
    /// </summary>
    public void Update(string name, string breed, int age)
    {
        ValidateName(name);
        ValidateBreed(breed);
        ValidateAge(age);
        Name = name;
        Breed = breed;
        Age = age;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Dog name is required.");
        if (name.Length < 2 || name.Length > 80)
            throw new DomainException("Dog name must be between 2 and 80 characters.");
    }

    private static void ValidateBreed(string breed)
    {
        if (string.IsNullOrWhiteSpace(breed))
            throw new DomainException("Breed is required.");
        if (breed.Length < 2 || breed.Length > 80)
            throw new DomainException("Breed must be between 2 and 80 characters.");
    }

    private static void ValidateAge(int age)
    {
        if (age < 0 || age > 30)
            throw new DomainException("Dog age must be between 0 and 30.");
    }

    /// <summary>
    /// EF Core parameterless constructor.
    /// </summary>
    private Dog() { Name = null!; Breed = null!; }
}
