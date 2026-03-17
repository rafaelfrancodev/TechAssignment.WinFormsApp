using System.ComponentModel;
using System.Windows.Forms;
using Application.DTOs;
using Application.Interfaces;
using Moq;
using TechAssignment.WinFormsApp.Presenters;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace Tests.Unit.Presenters;

public class ClientListPresenterTests
{
    private readonly Mock<IClientListView> _viewMock = new();
    private readonly Mock<IClientService> _serviceMock = new();
    private readonly BindingSource _bs = new();

    public ClientListPresenterTests()
    {
        _viewMock.Setup(v => v.ClientBindingSource).Returns(_bs);
    }

    [Fact]
    public async Task Load_PopulatesBindingSource_WithAllClients()
    {
        var clients = new List<ClientDto>
        {
            new(1, "John Doe", "555-0100", 2),
            new(2, "Jane Smith", "555-0101", 1)
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(clients);
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.LoadRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        var list = _bs.DataSource as BindingList<ClientDto>;
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task Load_WhenServiceThrows_ShowsError()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("DB down"));
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.LoadRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _viewMock.Verify(v => v.ShowError(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Load_SetsLoadingTrueAndFalse()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ClientDto>());
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.LoadRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _viewMock.Verify(v => v.SetLoading(true), Times.AtLeastOnce);
        _viewMock.Verify(v => v.SetLoading(false), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Search_WithTerm_CallsSearchService()
    {
        _viewMock.Setup(v => v.SearchTerm).Returns("John");
        _serviceMock.Setup(s => s.SearchAsync("John")).ReturnsAsync(new List<ClientDto>());
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.SearchRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _serviceMock.Verify(s => s.SearchAsync("John"), Times.Once);
    }

    [Fact]
    public async Task Search_EmptyTerm_LoadsAll()
    {
        _viewMock.Setup(v => v.SearchTerm).Returns("");
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ClientDto>());
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.SearchRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_NoSelection_DoesNothing()
    {
        _viewMock.Setup(v => v.SelectedClientId).Returns((int?)null);
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.DeleteRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WithSelection_DeletesAndReloads()
    {
        _viewMock.Setup(v => v.SelectedClientId).Returns(1);
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ClientDto>());
        var presenter = new ClientListPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.DeleteRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }
}
