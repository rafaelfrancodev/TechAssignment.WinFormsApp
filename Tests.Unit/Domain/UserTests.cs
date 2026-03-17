using Domain.Entities;
using Domain.Exceptions;

namespace Tests.Unit.Domain;

public class UserTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesUser()
    {
        var user = new User("admin", "$2a$12$hashvalue");

        Assert.Equal("admin", user.Username);
        Assert.Equal("$2a$12$hashvalue", user.PasswordHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ab")]
    public void Constructor_InvalidUsername_ThrowsDomainException(string? username)
    {
        Assert.Throws<DomainException>(() => new User(username!, "hash"));
    }

    [Fact]
    public void Constructor_UsernameTooLong_ThrowsDomainException()
    {
        var longUsername = new string('a', 51);
        Assert.Throws<DomainException>(() => new User(longUsername, "hash"));
    }

    [Theory]
    [InlineData("user!")]
    [InlineData("user name")]
    [InlineData("user@name")]
    public void Constructor_NonAlphanumericUsername_ThrowsDomainException(string username)
    {
        Assert.Throws<DomainException>(() => new User(username, "hash"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_InvalidPasswordHash_ThrowsDomainException(string? hash)
    {
        Assert.Throws<DomainException>(() => new User("admin", hash!));
    }
}
