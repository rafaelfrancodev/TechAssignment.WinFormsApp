using Application.Interfaces;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Presenters;

/// <summary>
/// Coordinates login and registration interactions.
/// </summary>
public class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance and subscribes to view events.
    /// </summary>
    public LoginPresenter(ILoginView view, IAuthService authService)
    {
        _view = view;
        _authService = authService;
        _view.LoginRequested += async (_, _) =>
        {
            try { await LoginAsync(); }
            catch (Exception ex) { _view.ShowError($"Login failed: {ex.Message}"); }
        };
        _view.RegisterRequested += async (_, _) =>
        {
            try { await RegisterAsync(); }
            catch (Exception ex) { _view.ShowError($"Registration failed: {ex.Message}"); }
        };
    }

    private async Task LoginAsync()
    {
        var valid = await _authService.ValidateAsync(_view.Username, _view.Password);
        if (valid)
            _view.CloseWithSuccess();
        else
            _view.ShowError("Invalid username or password.");
    }

    private async Task RegisterAsync()
    {
        try
        {
            await _authService.RegisterAsync(_view.Username, _view.Password);
            _view.ShowMessage("Registration successful. You can now log in.");
        }
        catch (ApplicationException ex)
        {
            _view.ShowError(ex.Message);
        }
    }
}
