using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Tests.Unit.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_repoMock.Object);
    }

    [Fact]
    public async Task ValidateAsync_CorrectCredentials_ReturnsTrue()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User("admin", hash);
        _repoMock.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync(user);

        var result = await _sut.ValidateAsync("admin", "password123");

        Assert.True(result);
    }

    [Fact]
    public async Task ValidateAsync_WrongPassword_ReturnsFalse()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User("admin", hash);
        _repoMock.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync(user);

        var result = await _sut.ValidateAsync("admin", "wrongpassword");

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateAsync_UserNotFound_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByUsernameAsync("unknown")).ReturnsAsync((User?)null);

        var result = await _sut.ValidateAsync("unknown", "password");

        Assert.False(result);
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("user", "")]
    [InlineData(null, "password")]
    [InlineData("user", null)]
    public async Task ValidateAsync_EmptyCredentials_ReturnsFalse(string? username, string? password)
    {
        var result = await _sut.ValidateAsync(username!, password!);
        Assert.False(result);
    }

    [Fact]
    public async Task RegisterAsync_ValidInput_CreatesUser()
    {
        _repoMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);

        await _sut.RegisterAsync("newuser", "password123");

        _repoMock.Verify(r => r.AddAsync(It.Is<User>(
            u => u.Username == "newuser")), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateUsername_ThrowsApplicationException()
    {
        var existingUser = new User("admin", BCrypt.Net.BCrypt.HashPassword("pass123"));
        _repoMock.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync(existingUser);

        await Assert.ThrowsAsync<ApplicationException>(
            () => _sut.RegisterAsync("admin", "password123"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    [InlineData(null)]
    public async Task RegisterAsync_ShortPassword_ThrowsApplicationException(string? password)
    {
        await Assert.ThrowsAsync<ApplicationException>(
            () => _sut.RegisterAsync("newuser", password!));
    }

    [Fact]
    public async Task RegisterAsync_PasswordIsHashedWithBCrypt()
    {
        _repoMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);
        User? savedUser = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => savedUser = u);

        await _sut.RegisterAsync("newuser", "password123");

        Assert.NotNull(savedUser);
        Assert.True(BCrypt.Net.BCrypt.Verify("password123", savedUser.PasswordHash));
    }
}
