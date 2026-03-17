using Domain.Entities;
using Domain.Exceptions;

namespace Tests.Unit.Domain;

public class WalkEventTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesWalkEvent()
    {
        var date = DateTime.Today;
        var walk = new WalkEvent(1, date, 30, "Nice walk");

        Assert.Equal(1, walk.DogId);
        Assert.Equal(date, walk.WalkDate);
        Assert.Equal(30, walk.DurationMinutes);
        Assert.Equal("Nice walk", walk.Notes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(14)]
    [InlineData(241)]
    [InlineData(300)]
    public void Constructor_InvalidDuration_ThrowsDomainException(int minutes)
    {
        Assert.Throws<DomainException>(() =>
            new WalkEvent(1, DateTime.Today, minutes, null));
    }

    [Theory]
    [InlineData(15)]
    [InlineData(120)]
    [InlineData(240)]
    public void Constructor_ValidDuration_Succeeds(int minutes)
    {
        var walk = new WalkEvent(1, DateTime.Today, minutes, null);
        Assert.Equal(minutes, walk.DurationMinutes);
    }

    [Fact]
    public void Constructor_DateBefore2000_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new WalkEvent(1, new DateTime(1999, 12, 31), 30, null));
    }

    [Fact]
    public void Constructor_DateTooFarInFuture_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new WalkEvent(1, DateTime.Now.AddYears(1).AddDays(1), 30, null));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidDogId_ThrowsDomainException(int dogId)
    {
        Assert.Throws<DomainException>(() =>
            new WalkEvent(dogId, DateTime.Today, 30, null));
    }

    [Fact]
    public void Constructor_NullNotes_AllowsNull()
    {
        var walk = new WalkEvent(1, DateTime.Today, 30, null);
        Assert.Null(walk.Notes);
    }
}
