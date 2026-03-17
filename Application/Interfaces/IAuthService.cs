namespace Application.Interfaces;

/// <summary>
/// Service contract for user authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>Validates a username/password combination.</summary>
    Task<bool> ValidateAsync(string username, string password);

    /// <summary>Registers a new user with the given credentials.</summary>
    Task RegisterAsync(string username, string password);
}
