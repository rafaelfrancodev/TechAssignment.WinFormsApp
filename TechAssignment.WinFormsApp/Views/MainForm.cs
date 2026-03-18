using System.Windows.Forms;
using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Presenters;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Main application window. Shows the client list. Dogs are managed in a separate dialog.
/// </summary>
public class MainForm : Form
{
    private readonly IClientService _clientService;
    private readonly IDogService _dogService;
    private readonly IWalkEventService _walkService;
    private readonly ClientListForm clientListForm;
    private ClientListPresenter? _clientPresenter;

    /// <summary>
    /// Initializes the main form with service dependencies.
    /// </summary>
    public MainForm(IClientService clientService, IDogService dogService, IWalkEventService walkService)
    {
        _clientService = clientService;
        _dogService = dogService;
        _walkService = walkService;

        Text = "Dog Walking Business Manager";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;

        clientListForm = new ClientListForm { Dock = DockStyle.Fill };
        clientListForm.AddRequested += OnAddClient;
        clientListForm.EditRequested += OnEditClient;
        clientListForm.ManageDogsRequested += OnManageDogs;
        Controls.Add(clientListForm);

        _clientPresenter = new ClientListPresenter(clientListForm, _clientService);
    }

    private void OnAddClient(object? sender, EventArgs e)
    {
        var detailForm = new ClientDetailForm();
        _ = new ClientDetailPresenter(detailForm, _clientService, null);
        if (detailForm.ShowDialog(this) == DialogResult.OK)
            _ = _clientPresenter?.LoadAsync();
    }

    private void OnEditClient(object? sender, EventArgs e)
    {
        var clientId = clientListForm.SelectedClientId;
        if (clientId is null) return;

        var detailForm = new ClientDetailForm();
        _ = new ClientDetailPresenter(detailForm, _clientService, clientId.Value);
        if (detailForm.ShowDialog(this) == DialogResult.OK)
            _ = _clientPresenter?.LoadAsync();
    }

    private void OnManageDogs(object? sender, EventArgs e)
    {
        var clientId = clientListForm.SelectedClientId;
        if (clientId is null)
        {
            MessageBox.Show("Please select a client first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var current = clientListForm.ClientBindingSource.Current as ClientDto;
        var clientName = current?.Name ?? "Unknown";

        var dogListForm = new DogListForm(clientName);
        var dogPresenter = new DogListPresenter(dogListForm, _dogService, clientId.Value);

        dogListForm.AddRequested += (_, _) => OnAddDog(dogListForm, dogPresenter, clientId.Value);
        dogListForm.EditRequested += (_, _) => OnEditDog(dogListForm, dogPresenter, clientId.Value);
        dogListForm.ViewWalksRequested += (_, _) => OnViewWalks(dogListForm);

        dogListForm.ShowDialog(this);

        // Refresh client list to update dog count
        _ = _clientPresenter?.LoadAsync();
    }

    private void OnAddDog(DogListForm dogListForm, DogListPresenter dogPresenter, int clientId)
    {
        var detailForm = new DogDetailForm();
        _ = new DogDetailPresenter(detailForm, _dogService, clientId, null);
        if (detailForm.ShowDialog(dogListForm) == DialogResult.OK)
            _ = dogPresenter.LoadAsync();
    }

    private void OnEditDog(DogListForm dogListForm, DogListPresenter dogPresenter, int clientId)
    {
        var dogId = dogListForm.SelectedDogId;
        if (dogId is null) return;

        var detailForm = new DogDetailForm();
        _ = new DogDetailPresenter(detailForm, _dogService, clientId, dogId.Value);
        if (detailForm.ShowDialog(dogListForm) == DialogResult.OK)
            _ = dogPresenter.LoadAsync();
    }

    private void OnViewWalks(DogListForm dogListForm)
    {
        var dogId = dogListForm.SelectedDogId;
        if (dogId is null)
        {
            MessageBox.Show("Please select a dog first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var walkForm = new WalkEventForm();
        _ = new WalkEventPresenter(walkForm, _walkService, dogId.Value);
        walkForm.ShowDialog(dogListForm);
    }
}
