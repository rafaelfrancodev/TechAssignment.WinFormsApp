using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Presenters;

/// <summary>
/// Coordinates client detail/edit form interactions.
/// </summary>
public class ClientDetailPresenter
{
    private readonly IClientDetailView _view;
    private readonly IClientService _service;
    private readonly int? _clientId;

    /// <summary>
    /// Initializes a new instance. Pass clientId for edit mode, null for create mode.
    /// </summary>
    public ClientDetailPresenter(IClientDetailView view, IClientService service, int? clientId)
    {
        _view = view;
        _service = service;
        _clientId = clientId;
        _view.IsEditMode = clientId.HasValue;
        _view.SaveRequested += async (_, _) =>
        {
            try { await SaveAsync(); }
            catch (Exception ex) { _view.ShowError($"Failed to save: {ex.Message}"); }
        };

        if (clientId.HasValue)
            _ = LoadAsync(clientId.Value);
        else
            _view.ClientBindingSource.DataSource = new ClientDto();
    }

    private async Task LoadAsync(int id)
    {
        try
        {
            var detail = await _service.GetDetailAsync(id);
            _view.ClientBindingSource.DataSource = detail.Client;
        }
        catch (Exception ex)
        {
            _view.ShowError($"Failed to load client: {ex.Message}");
        }
    }

    private async Task SaveAsync()
    {
        _view.ClearErrors();
        _view.ClientBindingSource.EndEdit();
        var current = _view.ClientBindingSource.Current as ClientDto;
        if (current is null) return;

        try
        {
            if (_view.IsEditMode && _clientId.HasValue)
                await _service.UpdateAsync(_clientId.Value, current.Name, current.PhoneNumber);
            else
                await _service.CreateAsync(current.Name, current.PhoneNumber);

            _view.CloseWithSuccess();
        }
        catch (ApplicationException ex)
        {
            _view.ShowError(ex.Message);
        }
    }
}
