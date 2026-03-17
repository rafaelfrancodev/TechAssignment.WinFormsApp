namespace TechAssignment.WinFormsApp.Views.Contracts;

/// <summary>
/// View contract for the login screen.
/// </summary>
public interface ILoginView
{
    /// <summary>Gets the entered username.</summary>
    string Username { get; }

    /// <summary>Gets the entered password.</summary>
    string Password { get; }

    /// <summary>Raised when the user clicks the login button.</summary>
    event EventHandler LoginRequested;

    /// <summary>Raised when the user wants to register a new account.</summary>
    event EventHandler RegisterRequested;

    /// <summary>Raised when the user cancels login.</summary>
    event EventHandler CancelRequested;

    /// <summary>Displays an error message to the user.</summary>
    void ShowError(string message);

    /// <summary>Displays a success message to the user.</summary>
    void ShowMessage(string message);

    /// <summary>Closes the view with a success result.</summary>
    void CloseWithSuccess();
}
