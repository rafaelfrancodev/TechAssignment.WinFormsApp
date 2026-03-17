using Domain.Entities;
using Domain.Exceptions;

namespace Tests.Unit.Domain;

public class ClientTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesClient()
    {
        var client = new Client("John Doe", "555-123-4567");

        Assert.Equal("John Doe", client.Name);
        Assert.Equal("555-123-4567", client.PhoneNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    public void Constructor_InvalidName_ThrowsDomainException(string? name)
    {
        Assert.Throws<DomainException>(() => new Client(name!, "555-123-4567"));
    }

    [Fact]
    public void Constructor_NameTooLong_ThrowsDomainException()
    {
        var longName = new string('A', 101);
        Assert.Throws<DomainException>(() => new Client(longName, "555-123-4567"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("12345")]
    public void Constructor_InvalidPhone_ThrowsDomainException(string? phone)
    {
        Assert.Throws<DomainException>(() => new Client("John Doe", phone!));
    }

    [Theory]
    [InlineData("555-123-4567")]
    [InlineData("(555)123-4567")]
    [InlineData("555.123.4567")]
    [InlineData("5551234567")]
    public void Constructor_ValidPhoneFormats_Succeeds(string phone)
    {
        var client = new Client("John Doe", phone);
        Assert.Equal(phone, client.PhoneNumber);
    }

    [Fact]
    public void UpdateContact_ValidInput_UpdatesFields()
    {
        var client = new Client("John Doe", "555-123-4567");
        client.UpdateContact("Jane Smith", "555-987-6543");

        Assert.Equal("Jane Smith", client.Name);
        Assert.Equal("555-987-6543", client.PhoneNumber);
    }

    [Fact]
    public void AddDog_NullDog_ThrowsArgumentNullException()
    {
        var client = new Client("John Doe", "555-123-4567");
        Assert.Throws<ArgumentNullException>(() => client.AddDog(null!));
    }

    [Fact]
    public void AddDog_ValidDog_AddsToDogs()
    {
        var client = new Client("John Doe", "555-123-4567");
        var dog = new Dog(1, "Rex", "Labrador", 3);
        client.AddDog(dog);

        Assert.Single(client.Dogs);
    }
}
