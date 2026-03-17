using Domain.Entities;
using Domain.Exceptions;

namespace Tests.Unit.Domain;

public class DogTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesDog()
    {
        var dog = new Dog(1, "Rex", "Labrador", 3);

        Assert.Equal(1, dog.ClientId);
        Assert.Equal("Rex", dog.Name);
        Assert.Equal("Labrador", dog.Breed);
        Assert.Equal(3, dog.Age);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidClientId_ThrowsDomainException(int clientId)
    {
        Assert.Throws<DomainException>(() => new Dog(clientId, "Rex", "Labrador", 3));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A")]
    public void Constructor_InvalidName_ThrowsDomainException(string? name)
    {
        Assert.Throws<DomainException>(() => new Dog(1, name!, "Labrador", 3));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A")]
    public void Constructor_InvalidBreed_ThrowsDomainException(string? breed)
    {
        Assert.Throws<DomainException>(() => new Dog(1, "Rex", breed!, 3));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(31)]
    public void Constructor_InvalidAge_ThrowsDomainException(int age)
    {
        Assert.Throws<DomainException>(() => new Dog(1, "Rex", "Labrador", age));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(30)]
    public void Constructor_ValidAge_Succeeds(int age)
    {
        var dog = new Dog(1, "Rex", "Labrador", age);
        Assert.Equal(age, dog.Age);
    }

    [Fact]
    public void Update_ValidInput_UpdatesFields()
    {
        var dog = new Dog(1, "Rex", "Labrador", 3);
        dog.Update("Buddy", "Poodle", 5);

        Assert.Equal("Buddy", dog.Name);
        Assert.Equal("Poodle", dog.Breed);
        Assert.Equal(5, dog.Age);
    }
}
