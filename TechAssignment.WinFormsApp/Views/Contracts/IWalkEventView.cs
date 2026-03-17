using System.Windows.Forms;

namespace TechAssignment.WinFormsApp.Views.Contracts;

/// <summary>
/// View contract for the walk event management screen.
/// </summary>
public interface IWalkEventView
{
    /// <summary>Gets the binding source for the walk history grid.</summary>
    BindingSource WalkBindingSource { get; }

    /// <summary>Gets the selected walk date.</summary>
    DateTime WalkDate { get; }

    /// <summary>Gets the walk duration in minutes.</summary>
    int DurationMinutes { get; }

    /// <summary>Gets the optional walk notes.</summary>
    string? Notes { get; }

    /// <summary>Gets the currently selected walk event ID, or null if none.</summary>
    int? SelectedWalkId { get; }

    /// <summary>Raised when the view loads and data should be fetched.</summary>
    event EventHandler LoadRequested;

    /// <summary>Raised when the user wants to log a new walk.</summary>
    event EventHandler LogWalkRequested;

    /// <summary>Raised when the user wants to delete a walk event.</summary>
    event EventHandler DeleteRequested;

    /// <summary>Displays an informational message.</summary>
    void ShowMessage(string message);

    /// <summary>Displays an error message.</summary>
    void ShowError(string message);

    /// <summary>Sets the loading state of the view.</summary>
    void SetLoading(bool loading);
}
