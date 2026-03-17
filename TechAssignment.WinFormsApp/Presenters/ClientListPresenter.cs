using System.ComponentModel;
using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Presenters;

/// <summary>
/// Coordinates all interactions for the client list screen.
/// Does not reference any WinForms controls directly.
/// </summary>
public class ClientListPresenter
{
    private readonly IClientListView _view;
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance and subscribes to view events.
    /// </summary>
    public ClientListPresenter(IClientListView view, IClientService service)
    {
        _view = view;
        _service = service;
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
    /// Loads all clients into the binding source.
    /// </summary>
    public async Task LoadAsync()
    {
        _view.SetLoading(true);
        try
        {
            var clients = await _service.GetAllAsync();
            _view.ClientBindingSource.DataSource = new BindingList<ClientDto>(clients.ToList());
        }
        finally { _view.SetLoading(false); }
    }

    private async Task SearchAsync()
    {
        _view.SetLoading(true);
        try
        {
            var term = _view.SearchTerm;
            var clients = string.IsNullOrWhiteSpace(term)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(term);
            _view.ClientBindingSource.DataSource = new BindingList<ClientDto>(clients.ToList());
        }
        finally { _view.SetLoading(false); }
    }

    private async Task DeleteAsync()
    {
        if (_view.SelectedClientId is not int id) return;

        await _service.DeleteAsync(id);
        await LoadAsync();
        _view.ShowMessage("Client deleted successfully.");
    }
}
