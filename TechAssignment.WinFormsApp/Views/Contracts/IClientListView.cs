using System.Windows.Forms;

namespace TechAssignment.WinFormsApp.Views.Contracts;

/// <summary>
/// View contract for the client list screen.
/// </summary>
public interface IClientListView
{
    /// <summary>Gets the binding source for the client list grid.</summary>
    BindingSource ClientBindingSource { get; }

    /// <summary>Gets the current search term.</summary>
    string SearchTerm { get; }

    /// <summary>Gets the currently selected client ID, or null if none.</summary>
    int? SelectedClientId { get; }

    /// <summary>Raised when the view loads and data should be fetched.</summary>
    event EventHandler LoadRequested;

    /// <summary>Raised when the user submits a search.</summary>
    event EventHandler SearchRequested;

    /// <summary>Raised when the user wants to add a new client.</summary>
    event EventHandler AddRequested;

    /// <summary>Raised when the user wants to edit the selected client.</summary>
    event EventHandler EditRequested;

    /// <summary>Raised when the user wants to delete the selected client.</summary>
    event EventHandler DeleteRequested;

    /// <summary>Raised when the user wants to manage dogs for the selected client.</summary>
    event EventHandler ManageDogsRequested;

    /// <summary>Displays an informational message.</summary>
    void ShowMessage(string message);

    /// <summary>Displays an error message.</summary>
    void ShowError(string message);

    /// <summary>Sets the loading state of the view.</summary>
    void SetLoading(bool loading);
}
