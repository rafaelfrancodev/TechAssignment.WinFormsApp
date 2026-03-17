using Application.Interfaces;
using Moq;
using TechAssignment.WinFormsApp.Presenters;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace Tests.Unit.Presenters;

public class LoginPresenterTests
{
    private readonly Mock<ILoginView> _viewMock = new();
    private readonly Mock<IAuthService> _serviceMock = new();

    [Fact]
    public async Task Login_ValidCredentials_ClosesWithSuccess()
    {
        _viewMock.Setup(v => v.Username).Returns("admin");
        _viewMock.Setup(v => v.Password).Returns("password123");
        _serviceMock.Setup(s => s.ValidateAsync("admin", "password123")).ReturnsAsync(true);
        var presenter = new LoginPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.LoginRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _viewMock.Verify(v => v.CloseWithSuccess(), Times.Once);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShowsError()
    {
        _viewMock.Setup(v => v.Username).Returns("admin");
        _viewMock.Setup(v => v.Password).Returns("wrong");
        _serviceMock.Setup(s => s.ValidateAsync("admin", "wrong")).ReturnsAsync(false);
        var presenter = new LoginPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.LoginRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _viewMock.Verify(v => v.ShowError(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Register_ValidInput_ShowsSuccessMessage()
    {
        _viewMock.Setup(v => v.Username).Returns("newuser");
        _viewMock.Setup(v => v.Password).Returns("password123");
        var presenter = new LoginPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.RegisterRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _viewMock.Verify(v => v.ShowMessage(It.Is<string>(s => s.Contains("successful"))), Times.Once);
    }

    [Fact]
    public async Task Register_DuplicateUser_ShowsError()
    {
        _viewMock.Setup(v => v.Username).Returns("existing");
        _viewMock.Setup(v => v.Password).Returns("password123");
        _serviceMock.Setup(s => s.RegisterAsync("existing", "password123"))
            .ThrowsAsync(new ApplicationException("Username is already taken."));
        var presenter = new LoginPresenter(_viewMock.Object, _serviceMock.Object);

        _viewMock.Raise(v => v.RegisterRequested += null, EventArgs.Empty);
        await Task.Delay(50);

        _viewMock.Verify(v => v.ShowError(It.Is<string>(s => s.Contains("already taken"))), Times.Once);
    }
}
