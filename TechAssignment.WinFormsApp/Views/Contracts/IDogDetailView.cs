using System.Windows.Forms;

namespace TechAssignment.WinFormsApp.Views.Contracts;

/// <summary>
/// View contract for the dog detail/edit screen.
/// </summary>
public interface IDogDetailView
{
    /// <summary>Gets the binding source for single-record simple binding.</summary>
    BindingSource DogBindingSource { get; }

    /// <summary>Gets or sets whether the view is in edit mode.</summary>
    bool IsEditMode { get; set; }

    /// <summary>Raised when the user requests to save.</summary>
    event EventHandler SaveRequested;

    /// <summary>Raised when the user cancels.</summary>
    event EventHandler CancelRequested;

    /// <summary>Shows a validation error for a specific field.</summary>
    void ShowValidationError(string field, string message);

    /// <summary>Clears all validation errors.</summary>
    void ClearErrors();

    /// <summary>Closes the dialog with a success result.</summary>
    void CloseWithSuccess();

    /// <summary>Displays an error message.</summary>
    void ShowError(string message);
}
