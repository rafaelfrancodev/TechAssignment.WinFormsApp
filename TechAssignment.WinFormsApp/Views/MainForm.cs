using System.Windows.Forms;
using Application.DTOs;
using Application.Interfaces;
using TechAssignment.WinFormsApp.Presenters;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Main application window with SplitContainer layout.
/// Left panel: client list. Right panel: dog list for selected client.
/// </summary>
public class MainForm : Form
{
    private readonly IClientService _clientService;
    private readonly IDogService _dogService;
    private readonly IWalkEventService _walkService;
    private readonly SplitContainer splitContainer = new();
    private readonly ClientListForm clientListForm;
    private readonly DogListForm dogListForm;
    private ClientListPresenter? _clientPresenter;
    private DogListPresenter? _dogPresenter;
    private int? _currentClientId;

    /// <summary>
    /// Initializes the main form with service dependencies.
    /// </summary>
    public MainForm(IClientService clientService, IDogService dogService, IWalkEventService walkService)
    {
        _clientService = clientService;
        _dogService = dogService;
        _walkService = walkService;

        Text = "Dog Walking Business Manager";
        Size = new Size(1200, 700);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;

        // SplitContainer — 50/50 split
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Orientation = Orientation.Vertical;
        splitContainer.SplitterWidth = 4;
        Load += (_, _) => splitContainer.SplitterDistance = ClientSize.Width / 2;

        // Client list (left)
        clientListForm = new ClientListForm();
        clientListForm.AddRequested += OnAddClient;
        clientListForm.EditRequested += OnEditClient;
        splitContainer.Panel1.Controls.Add(clientListForm);

        // Dog list (right) — presenter created once, reloaded on client change
        dogListForm = new DogListForm();
        dogListForm.AddRequested += OnAddDog;
        dogListForm.EditRequested += OnEditDog;
        dogListForm.ViewWalksRequested += OnViewWalks;
        splitContainer.Panel2.Controls.Add(dogListForm);

        Controls.Add(splitContainer);

        // Wire client presenter (will auto-load via HandleCreated)
        _clientPresenter = new ClientListPresenter(clientListForm, _clientService);

        // When client selection changes, reload dogs
        clientListForm.ClientBindingSource.CurrentChanged += OnClientSelectionChanged;
    }

    private void OnClientSelectionChanged(object? sender, EventArgs e)
    {
        var clientId = clientListForm.SelectedClientId;
        if (clientId == _currentClientId) return; // avoid redundant reloads
        _currentClientId = clientId;

        if (clientId.HasValue)
            LoadDogsForClient(clientId.Value);
        else
            dogListForm.DogBindingSource.DataSource = null;
    }

    private void LoadDogsForClient(int clientId)
    {
        // Create presenter once per client; it subscribes to delete events on the view
        _dogPresenter = new DogListPresenter(dogListForm, _dogService, clientId);
        _ = _dogPresenter.LoadAsync();
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

    private void OnAddDog(object? sender, EventArgs e)
    {
        var clientId = clientListForm.SelectedClientId;
        if (clientId is null)
        {
            MessageBox.Show("Please select a client first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var detailForm = new DogDetailForm();
        _ = new DogDetailPresenter(detailForm, _dogService, clientId.Value, null);
        if (detailForm.ShowDialog(this) == DialogResult.OK)
        {
            _ = _dogPresenter?.LoadAsync();
            _ = _clientPresenter?.LoadAsync(); // refresh dog count
        }
    }

    private void OnEditDog(object? sender, EventArgs e)
    {
        var clientId = clientListForm.SelectedClientId;
        var dogId = dogListForm.SelectedDogId;
        if (clientId is null || dogId is null) return;

        var detailForm = new DogDetailForm();
        _ = new DogDetailPresenter(detailForm, _dogService, clientId.Value, dogId.Value);
        if (detailForm.ShowDialog(this) == DialogResult.OK)
            _ = _dogPresenter?.LoadAsync();
    }

    private void OnViewWalks(object? sender, EventArgs e)
    {
        var dogId = dogListForm.SelectedDogId;
        if (dogId is null)
        {
            MessageBox.Show("Please select a dog first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var walkForm = new WalkEventForm();
        _ = new WalkEventPresenter(walkForm, _walkService, dogId.Value);
        walkForm.ShowDialog(this);
    }
}
