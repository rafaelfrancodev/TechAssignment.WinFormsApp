using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Repository contract for <see cref="User"/> persistence operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>Gets a user by username, or null if not found.</summary>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>Adds a new user.</summary>
    Task AddAsync(User user);
}
