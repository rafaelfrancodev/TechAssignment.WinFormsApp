using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Tests.Unit.Services;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _repoMock = new();
    private readonly ClientService _sut;

    public ClientServiceTests()
    {
        _sut = new ClientService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllClientDtos()
    {
        var clients = new List<Client>
        {
            new("John Doe", "555-123-4567"),
            new("Jane Smith", "555-987-6543")
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(clients);

        var result = await _sut.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("John Doe", result[0].Name);
    }

    [Fact]
    public async Task SearchAsync_ForwardsTermToRepository()
    {
        _repoMock.Setup(r => r.SearchAsync("John")).ReturnsAsync(new List<Client>());

        await _sut.SearchAsync("John");

        _repoMock.Verify(r => r.SearchAsync("John"), Times.Once);
    }

    [Fact]
    public async Task GetDetailAsync_WhenNotFound_ThrowsApplicationException()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Client?)null);

        await Assert.ThrowsAsync<ApplicationException>(() => _sut.GetDetailAsync(99));
    }

    [Fact]
    public async Task GetDetailAsync_WhenFound_ReturnsDetail()
    {
        var client = new Client("John Doe", "555-123-4567");
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(client);

        var result = await _sut.GetDetailAsync(1);

        Assert.Equal("John Doe", result.Client.Name);
        Assert.Empty(result.Dogs);
    }

    [Fact]
    public async Task CreateAsync_ValidInput_CallsRepository()
    {
        await _sut.CreateAsync("John Doe", "555-123-4567");

        _repoMock.Verify(r => r.AddAsync(It.Is<Client>(
            c => c.Name == "John Doe" && c.PhoneNumber == "555-123-4567")), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_InvalidInput_ThrowsApplicationException()
    {
        await Assert.ThrowsAsync<ApplicationException>(
            () => _sut.CreateAsync("", "555-123-4567"));
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ThrowsApplicationException()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Client?)null);

        await Assert.ThrowsAsync<ApplicationException>(
            () => _sut.UpdateAsync(99, "Name", "555-123-4567"));
    }

    [Fact]
    public async Task UpdateAsync_ValidInput_UpdatesAndSaves()
    {
        var client = new Client("Old Name", "555-111-2222");
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(client);

        await _sut.UpdateAsync(1, "New Name", "555-333-4444");

        Assert.Equal("New Name", client.Name);
        _repoMock.Verify(r => r.UpdateAsync(client), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepository()
    {
        await _sut.DeleteAsync(1);
        _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
