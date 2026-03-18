using System.Windows.Forms;
using Application.DTOs;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Client list form with DataGridView (complex binding).
/// </summary>
public class ClientListForm : UserControl, IClientListView
{
    private readonly DataGridView dgvClients = new();
    private readonly ToolStripTextBox txtSearch = new();
    private readonly ToolStrip toolStrip = new();
    private readonly StatusStrip statusStrip = new();
    private readonly ToolStripStatusLabel statusLabel = new();

    /// <inheritdoc />
    public BindingSource ClientBindingSource { get; } = new();

    /// <inheritdoc />
    public string SearchTerm => txtSearch.Text ?? "";

    /// <inheritdoc />
    public int? SelectedClientId
    {
        get
        {
            if (ClientBindingSource.Current is ClientDto dto)
                return dto.Id;
            return null;
        }
    }

    /// <inheritdoc />
    public event EventHandler? LoadRequested;
    /// <inheritdoc />
    public event EventHandler? SearchRequested;
    /// <inheritdoc />
    public event EventHandler? AddRequested;
    /// <inheritdoc />
    public event EventHandler? EditRequested;
    /// <inheritdoc />
    public event EventHandler? DeleteRequested;
    /// <inheritdoc />
    public event EventHandler? ManageDogsRequested;

    /// <summary>
    /// Initializes the client list form with all controls.
    /// </summary>
    public ClientListForm()
    {
        Dock = DockStyle.Fill;

        // Header label
        var lblHeader = new Label
        {
            Text = "Clients",
            Dock = DockStyle.Top,
            Font = new Font(Font.FontFamily, 12, FontStyle.Bold),
            Height = 30,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(5, 0, 0, 0),
            BackColor = Color.FromArgb(45, 45, 48),
            ForeColor = Color.White
        };

        // ToolStrip
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.RenderMode = ToolStripRenderMode.System;

        var btnAdd = new ToolStripButton("\u2795 New") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnAdd.Click += (s, e) => AddRequested?.Invoke(this, e);

        var btnEdit = new ToolStripButton("\u270F Edit") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnEdit.Click += (s, e) => EditRequested?.Invoke(this, e);

        var btnDelete = new ToolStripButton("\u274C Delete") { DisplayStyle = ToolStripItemDisplayStyle.Text, ForeColor = Color.DarkRed };
        btnDelete.Click += (s, e) =>
        {
            if (SelectedClientId is null) return;
            var current = ClientBindingSource.Current as ClientDto;
            var result = MessageBox.Show(
                $"Delete {current?.Name}? This cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                DeleteRequested?.Invoke(this, e);
        };

        txtSearch.Size = new Size(200, 25);
        txtSearch.ToolTipText = "Search by name or phone";
        txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchRequested?.Invoke(this, e); };

        var btnSearch = new ToolStripButton("\uD83D\uDD0D Search") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnSearch.Click += (s, e) => SearchRequested?.Invoke(this, e);

        var btnClear = new ToolStripButton("Clear") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnClear.Click += (s, e) => { txtSearch.Text = ""; SearchRequested?.Invoke(this, e); };

        var btnDogs = new ToolStripButton("\uD83D\uDC36 Dogs") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnDogs.Click += (s, e) => ManageDogsRequested?.Invoke(this, e);

        toolStrip.Items.AddRange([btnAdd, btnEdit, btnDelete, new ToolStripSeparator(),
            btnDogs, new ToolStripSeparator(),
            txtSearch, btnSearch, btnClear]);

        // DataGridView - complex binding
        dgvClients.Dock = DockStyle.Fill;
        dgvClients.AutoGenerateColumns = false;
        dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvClients.MultiSelect = false;
        dgvClients.ReadOnly = true;
        dgvClients.AllowUserToAddRows = false;
        dgvClients.AllowUserToDeleteRows = false;
        dgvClients.AllowUserToResizeRows = false;
        dgvClients.RowHeadersVisible = false;
        dgvClients.BackgroundColor = SystemColors.Window;
        dgvClients.BorderStyle = BorderStyle.None;
        dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvClients.DataSource = ClientBindingSource;

        dgvClients.Columns.AddRange(
            new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", FillWeight = 10, MinimumWidth = 40 },
            new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", FillWeight = 40 },
            new DataGridViewTextBoxColumn { DataPropertyName = "PhoneNumber", HeaderText = "Phone", FillWeight = 30 },
            new DataGridViewTextBoxColumn { DataPropertyName = "DogCount", HeaderText = "Dogs", FillWeight = 10, MinimumWidth = 40 }
        );

        dgvClients.DoubleClick += (s, e) => EditRequested?.Invoke(this, e);

        // StatusStrip
        statusLabel.Spring = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        statusStrip.Items.Add(statusLabel);

        // Layout order matters: bottom-up for Dock
        Controls.Add(dgvClients);
        Controls.Add(toolStrip);
        Controls.Add(lblHeader);
        Controls.Add(statusStrip);

        HandleCreated += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public void ShowMessage(string message) => statusLabel.Text = message;

    /// <inheritdoc />
    public void ShowError(string message) =>
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    /// <inheritdoc />
    public void SetLoading(bool loading)
    {
        statusLabel.Text = loading ? "Loading..." : "Ready";
        Cursor = loading ? Cursors.WaitCursor : Cursors.Default;
    }
}
