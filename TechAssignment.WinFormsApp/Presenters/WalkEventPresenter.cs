using System.ComponentModel;
using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Presenters;

/// <summary>
/// Coordinates walk event management interactions.
/// </summary>
public class WalkEventPresenter
{
    private readonly IWalkEventView _view;
    private readonly IWalkEventService _service;
    private readonly int _dogId;

    /// <summary>
    /// Initializes a new instance and subscribes to view events.
    /// </summary>
    public WalkEventPresenter(IWalkEventView view, IWalkEventService service, int dogId)
    {
        _view = view;
        _service = service;
        _dogId = dogId;
        _view.LoadRequested += async (_, _) =>
        {
            try { await LoadAsync(); }
            catch (Exception ex) { _view.ShowError($"Failed to load: {ex.Message}"); }
        };
        _view.LogWalkRequested += async (_, _) =>
        {
            try { await LogWalkAsync(); }
            catch (Exception ex) { _view.ShowError($"Failed to log walk: {ex.Message}"); }
        };
        _view.DeleteRequested += async (_, _) =>
        {
            try { await DeleteAsync(); }
            catch (Exception ex) { _view.ShowError($"Delete failed: {ex.Message}"); }
        };
    }

    /// <summary>
    /// Loads walk history for the current dog.
    /// </summary>
    public async Task LoadAsync()
    {
        _view.SetLoading(true);
        try
        {
            var walks = await _service.GetByDogAsync(_dogId);
            _view.WalkBindingSource.DataSource = new BindingList<WalkEventDto>(walks.ToList());
        }
        finally { _view.SetLoading(false); }
    }

    private async Task LogWalkAsync()
    {
        try
        {
            await _service.LogWalkAsync(_dogId, _view.WalkDate, _view.DurationMinutes, _view.Notes);
            await LoadAsync();
            _view.ShowMessage("Walk logged successfully.");
        }
        catch (ApplicationException ex)
        {
            _view.ShowError(ex.Message);
        }
    }

    private async Task DeleteAsync()
    {
        if (_view.SelectedWalkId is not int id) return;

        await _service.DeleteAsync(id);
        await LoadAsync();
        _view.ShowMessage("Walk event deleted successfully.");
    }
}
