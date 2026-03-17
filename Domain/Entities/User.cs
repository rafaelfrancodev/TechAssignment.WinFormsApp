using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents an application user for authentication.
/// </summary>
public class User
{
    /// <summary>Gets the unique identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the username.</summary>
    public string Username { get; private set; }

    /// <summary>Gets the BCrypt password hash.</summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Creates a new user with validated username and a pre-hashed password.
    /// </summary>
    public User(string username, string passwordHash)
    {
        ValidateUsername(username);
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required.");
        Username = username;
        PasswordHash = passwordHash;
    }

    private static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username is required.");
        if (username.Length < 3 || username.Length > 50)
            throw new DomainException("Username must be between 3 and 50 characters.");
        if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
            throw new DomainException("Username must contain only alphanumeric characters.");
    }

    /// <summary>
    /// EF Core parameterless constructor.
    /// </summary>
    private User() { Username = null!; PasswordHash = null!; }
}
