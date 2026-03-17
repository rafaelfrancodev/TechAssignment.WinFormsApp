using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Implements user authentication operations using BCrypt.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Initializes a new instance with the specified repository.
    /// </summary>
    public AuthService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var user = await _userRepo.GetByUsernameAsync(username);
        if (user is null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    /// <inheritdoc />
    public async Task RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ApplicationException("Password must be at least 6 characters.");

        var existing = await _userRepo.GetByUsernameAsync(username);
        if (existing is not null)
            throw new ApplicationException("Username is already taken.");

        try
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(username, hash);
            await _userRepo.AddAsync(user);
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
