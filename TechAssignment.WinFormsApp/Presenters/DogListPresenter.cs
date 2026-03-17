using System.ComponentModel;
using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Presenters;

/// <summary>
/// Coordinates all interactions for the dog list screen.
/// </summary>
public class DogListPresenter
{
    private readonly IDogListView _view;
    private readonly IDogService _service;
    private readonly int _clientId;

    /// <summary>
    /// Initializes a new instance and subscribes to view events.
    /// </summary>
    public DogListPresenter(IDogListView view, IDogService service, int clientId)
    {
        _view = view;
        _service = service;
        _clientId = clientId;
        _view.LoadRequested += async (_, _) =>
        {
            try { await LoadAsync(); }
            catch (Exception ex) { _view.ShowError($"Failed to load: {ex.Message}"); }
        };
        _view.SearchRequested += async (_, _) =>
        {
            try { await SearchAsync(); }
            catch (Exception ex) { _view.ShowError($"Search failed: {ex.Message}"); }
        };
        _view.DeleteRequested += async (_, _) =>
        {
            try { await DeleteAsync(); }
            catch (Exception ex) { _view.ShowError($"Delete failed: {ex.Message}"); }
        };
    }

    /// <summary>
    /// Loads dogs for the current client into the binding source.
    /// </summary>
    public async Task LoadAsync()
    {
        _view.SetLoading(true);
        try
        {
            var dogs = await _service.GetByClientAsync(_clientId);
            _view.DogBindingSource.DataSource = new BindingList<DogDto>(dogs.ToList());
        }
        finally { _view.SetLoading(false); }
    }

    private async Task SearchAsync()
    {
        _view.SetLoading(true);
        try
        {
            var term = _view.SearchTerm;
            var dogs = string.IsNullOrWhiteSpace(term)
                ? await _service.GetByClientAsync(_clientId)
                : await _service.SearchByClientAsync(_clientId, term);
            _view.DogBindingSource.DataSource = new BindingList<DogDto>(dogs.ToList());
        }
        finally { _view.SetLoading(false); }
    }

    private async Task DeleteAsync()
    {
        if (_view.SelectedDogId is not int id) return;

        await _service.DeleteAsync(id);
        await LoadAsync();
        _view.ShowMessage("Dog deleted successfully.");
    }
}
