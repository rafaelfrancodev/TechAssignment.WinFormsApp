using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Presenters;

/// <summary>
/// Coordinates dog detail/edit form interactions.
/// </summary>
public class DogDetailPresenter
{
    private readonly IDogDetailView _view;
    private readonly IDogService _service;
    private readonly int _clientId;
    private readonly int? _dogId;

    /// <summary>
    /// Initializes a new instance. Pass dogId for edit mode, null for create mode.
    /// </summary>
    public DogDetailPresenter(IDogDetailView view, IDogService service, int clientId, int? dogId)
    {
        _view = view;
        _service = service;
        _clientId = clientId;
        _dogId = dogId;
        _view.IsEditMode = dogId.HasValue;
        _view.SaveRequested += async (_, _) =>
        {
            try { await SaveAsync(); }
            catch (Exception ex) { _view.ShowError($"Failed to save: {ex.Message}"); }
        };

        if (dogId.HasValue)
            _ = LoadAsync(dogId.Value);
        else
            _view.DogBindingSource.DataSource = new DogDto { ClientId = _clientId };
    }

    private async Task LoadAsync(int id)
    {
        try
        {
            var dogs = await _service.GetByClientAsync(_clientId);
            var dog = dogs.FirstOrDefault(d => d.Id == id);
            if (dog is not null)
                _view.DogBindingSource.DataSource = dog;
            else
                _view.ShowError("Dog not found.");
        }
        catch (Exception ex)
        {
            _view.ShowError($"Failed to load dog: {ex.Message}");
        }
    }

    private async Task SaveAsync()
    {
        _view.ClearErrors();
        _view.DogBindingSource.EndEdit();
        var current = _view.DogBindingSource.Current as DogDto;
        if (current is null) return;

        try
        {
            if (_view.IsEditMode && _dogId.HasValue)
                await _service.UpdateAsync(_dogId.Value, current.Name, current.Breed, current.Age);
            else
                await _service.CreateAsync(_clientId, current.Name, current.Breed, current.Age);

            _view.CloseWithSuccess();
        }
        catch (ApplicationException ex)
        {
            _view.ShowError(ex.Message);
        }
    }
}
