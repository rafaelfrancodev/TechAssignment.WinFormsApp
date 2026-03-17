using System.Windows.Forms;

namespace TechAssignment.WinFormsApp.Views.Contracts;

/// <summary>
/// View contract for the dog list screen.
/// </summary>
public interface IDogListView
{
    /// <summary>Gets the binding source for the dog list grid.</summary>
    BindingSource DogBindingSource { get; }

    /// <summary>Gets the currently selected dog ID, or null if none.</summary>
    int? SelectedDogId { get; }

    /// <summary>Gets the current search term.</summary>
    string SearchTerm { get; }

    /// <summary>Raised when the view loads and data should be fetched.</summary>
    event EventHandler LoadRequested;

    /// <summary>Raised when the user submits a search.</summary>
    event EventHandler SearchRequested;

    /// <summary>Raised when the user wants to add a new dog.</summary>
    event EventHandler AddRequested;

    /// <summary>Raised when the user wants to edit the selected dog.</summary>
    event EventHandler EditRequested;

    /// <summary>Raised when the user wants to delete the selected dog.</summary>
    event EventHandler DeleteRequested;

    /// <summary>Raised when the user wants to view walk history.</summary>
    event EventHandler ViewWalksRequested;

    /// <summary>Displays an informational message.</summary>
    void ShowMessage(string message);

    /// <summary>Displays an error message.</summary>
    void ShowError(string message);

    /// <summary>Sets the loading state of the view.</summary>
    void SetLoading(bool loading);
}
