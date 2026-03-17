using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a client of the dog walking business.
/// </summary>
public class Client
{
    private static readonly Regex PhoneRegex = new(
        @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", RegexOptions.Compiled);

    /// <summary>Gets the unique identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the client's full name.</summary>
    public string Name { get; private set; }

    /// <summary>Gets the client's phone number.</summary>
    public string PhoneNumber { get; private set; }

    /// <summary>Gets the read-only collection of dogs belonging to this client.</summary>
    public IReadOnlyCollection<Dog> Dogs => _dogs.AsReadOnly();
    private readonly List<Dog> _dogs = [];

    /// <summary>
    /// Creates a new client with validated name and phone number.
    /// </summary>
    public Client(string name, string phoneNumber)
    {
        ValidateName(name);
        ValidatePhone(phoneNumber);
        Name = name;
        PhoneNumber = phoneNumber;
    }

    /// <summary>
    /// Adds a dog to this client's collection.
    /// </summary>
    public void AddDog(Dog dog)
    {
        ArgumentNullException.ThrowIfNull(dog);
        _dogs.Add(dog);
    }

    /// <summary>
    /// Updates the client's contact information.
    /// </summary>
    public void UpdateContact(string name, string phone)
    {
        ValidateName(name);
        ValidatePhone(phone);
        Name = name;
        PhoneNumber = phone;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Client name is required.");
        if (name.Length < 2 || name.Length > 100)
            throw new DomainException("Client name must be between 2 and 100 characters.");
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone number is required.");
        if (!PhoneRegex.IsMatch(phone))
            throw new DomainException("Phone number format is invalid. Expected format: (555) 123-4567.");
    }

    /// <summary>
    /// EF Core parameterless constructor.
    /// </summary>
    private Client() { Name = null!; PhoneNumber = null!; }
}
