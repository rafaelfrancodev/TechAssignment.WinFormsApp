using System.Windows.Forms;
using Application.DTOs;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Dog list form with DataGridView (complex binding).
/// </summary>
public class DogListForm : UserControl, IDogListView
{
    private readonly DataGridView dgvDogs = new();
    private readonly ToolStripTextBox txtSearch = new();
    private readonly ToolStrip toolStrip = new();
    private readonly StatusStrip statusStrip = new();
    private readonly ToolStripStatusLabel statusLabel = new();

    /// <inheritdoc />
    public BindingSource DogBindingSource { get; } = new();

    /// <inheritdoc />
    public int? SelectedDogId
    {
        get
        {
            if (DogBindingSource.Current is DogDto dto)
                return dto.Id;
            return null;
        }
    }

    /// <inheritdoc />
    public string SearchTerm => txtSearch.Text ?? "";

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
    public event EventHandler? ViewWalksRequested;

    /// <summary>
    /// Initializes the dog list form with all controls.
    /// </summary>
    public DogListForm()
    {
        Dock = DockStyle.Fill;

        // Header label
        var lblHeader = new Label
        {
            Text = "Dogs",
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
            if (SelectedDogId is null) return;
            var current = DogBindingSource.Current as DogDto;
            var result = MessageBox.Show(
                $"Delete {current?.Name}? This cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                DeleteRequested?.Invoke(this, e);
        };

        var btnWalks = new ToolStripButton("\uD83D\uDEB6 Walks") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnWalks.Click += (s, e) => ViewWalksRequested?.Invoke(this, e);

        txtSearch.Size = new Size(150, 25);
        txtSearch.ToolTipText = "Search by name or breed";
        txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchRequested?.Invoke(this, e); };

        var btnSearch = new ToolStripButton("\uD83D\uDD0D Search") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnSearch.Click += (s, e) => SearchRequested?.Invoke(this, e);

        var btnClear = new ToolStripButton("Clear") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnClear.Click += (s, e) => { txtSearch.Text = ""; SearchRequested?.Invoke(this, e); };

        toolStrip.Items.AddRange([btnAdd, btnEdit, btnDelete, new ToolStripSeparator(), btnWalks,
            new ToolStripSeparator(), txtSearch, btnSearch, btnClear]);

        // DataGridView - complex binding
        dgvDogs.Dock = DockStyle.Fill;
        dgvDogs.AutoGenerateColumns = false;
        dgvDogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDogs.MultiSelect = false;
        dgvDogs.ReadOnly = true;
        dgvDogs.AllowUserToAddRows = false;
        dgvDogs.AllowUserToDeleteRows = false;
        dgvDogs.AllowUserToResizeRows = false;
        dgvDogs.RowHeadersVisible = false;
        dgvDogs.BackgroundColor = SystemColors.Window;
        dgvDogs.BorderStyle = BorderStyle.None;
        dgvDogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDogs.DataSource = DogBindingSource;

        dgvDogs.Columns.AddRange(
            new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", FillWeight = 10, MinimumWidth = 40 },
            new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", FillWeight = 35 },
            new DataGridViewTextBoxColumn { DataPropertyName = "Breed", HeaderText = "Breed", FillWeight = 35 },
            new DataGridViewTextBoxColumn { DataPropertyName = "Age", HeaderText = "Age", FillWeight = 10, MinimumWidth = 40 }
        );

        dgvDogs.DoubleClick += (s, e) => EditRequested?.Invoke(this, e);

        // StatusStrip
        statusLabel.Spring = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        statusStrip.Items.Add(statusLabel);

        // Layout order: bottom-up for Dock
        Controls.Add(dgvDogs);
        Controls.Add(toolStrip);
        Controls.Add(lblHeader);
        Controls.Add(statusStrip);
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
