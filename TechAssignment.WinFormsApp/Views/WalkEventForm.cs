using System.Windows.Forms;
using Application.DTOs;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Walk event management form with history grid and input controls.
/// </summary>
public class WalkEventForm : Form, IWalkEventView
{
    private readonly DataGridView dgvWalks = new();
    private readonly DateTimePicker dtpDate = new();
    private readonly NumericUpDown nudDuration = new();
    private readonly TextBox txtNotes = new();
    private readonly Button btnLogWalk = new();
    private readonly Button btnDelete = new();
    private readonly Button btnClose = new();
    private readonly StatusStrip statusStrip = new();
    private readonly ToolStripStatusLabel statusLabel = new();

    /// <inheritdoc />
    public BindingSource WalkBindingSource { get; } = new();
    /// <inheritdoc />
    public DateTime WalkDate => dtpDate.Value;
    /// <inheritdoc />
    public int DurationMinutes => (int)nudDuration.Value;
    /// <inheritdoc />
    public string? Notes => string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text;

    /// <inheritdoc />
    public int? SelectedWalkId
    {
        get
        {
            if (WalkBindingSource.Current is WalkEventDto dto)
                return dto.Id;
            return null;
        }
    }

    /// <inheritdoc />
    public event EventHandler? LoadRequested;
    /// <inheritdoc />
    public event EventHandler? LogWalkRequested;
    /// <inheritdoc />
    public event EventHandler? DeleteRequested;

    /// <summary>
    /// Initializes the walk event form.
    /// </summary>
    public WalkEventForm()
    {
        Text = "Walk History";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(750, 520);
        Padding = new Padding(10);

        // -- Input group --
        var grpNew = new GroupBox
        {
            Text = "Log New Walk",
            Dock = DockStyle.Top,
            Height = 100,
            Padding = new Padding(10)
        };

        var inputLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 5,
            RowCount = 2
        };
        inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

        var lblDate = new Label { Text = "Date:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        dtpDate.Dock = DockStyle.Fill;
        dtpDate.TabIndex = 0;

        var lblDuration = new Label { Text = "Minutes:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        nudDuration.Dock = DockStyle.Fill;
        nudDuration.Minimum = 15;
        nudDuration.Maximum = 240;
        nudDuration.Value = 30;
        nudDuration.TabIndex = 1;

        btnLogWalk.Text = "Log Walk";
        btnLogWalk.Dock = DockStyle.Fill;
        btnLogWalk.TabIndex = 3;
        btnLogWalk.Height = 32;
        btnLogWalk.Click += (s, e) => LogWalkRequested?.Invoke(this, e);

        var lblNotes = new Label { Text = "Notes:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.TabIndex = 2;

        inputLayout.Controls.Add(lblDate, 0, 0);
        inputLayout.Controls.Add(dtpDate, 1, 0);
        inputLayout.Controls.Add(lblDuration, 2, 0);
        inputLayout.Controls.Add(nudDuration, 3, 0);
        inputLayout.Controls.Add(btnLogWalk, 4, 0);
        inputLayout.SetRowSpan(btnLogWalk, 2);
        inputLayout.Controls.Add(lblNotes, 0, 1);
        inputLayout.Controls.Add(txtNotes, 1, 1);
        inputLayout.SetColumnSpan(txtNotes, 3);

        grpNew.Controls.Add(inputLayout);

        // -- DataGridView (complex binding) --
        dgvWalks.Dock = DockStyle.Fill;
        dgvWalks.AutoGenerateColumns = false;
        dgvWalks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvWalks.MultiSelect = false;
        dgvWalks.ReadOnly = true;
        dgvWalks.AllowUserToAddRows = false;
        dgvWalks.AllowUserToDeleteRows = false;
        dgvWalks.AllowUserToResizeRows = false;
        dgvWalks.RowHeadersVisible = false;
        dgvWalks.BackgroundColor = SystemColors.Window;
        dgvWalks.BorderStyle = BorderStyle.None;
        dgvWalks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvWalks.DataSource = WalkBindingSource;

        dgvWalks.Columns.AddRange(
            new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", FillWeight = 8, MinimumWidth = 40 },
            new DataGridViewTextBoxColumn { DataPropertyName = "WalkDate", HeaderText = "Date", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } },
            new DataGridViewTextBoxColumn { DataPropertyName = "DurationMinutes", HeaderText = "Duration (min)", FillWeight = 15 },
            new DataGridViewTextBoxColumn { DataPropertyName = "Notes", HeaderText = "Notes", FillWeight = 50 }
        );

        // -- Bottom buttons --
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 40,
            Padding = new Padding(0, 5, 0, 0)
        };

        btnClose.Text = "Close";
        btnClose.Width = 90;
        btnClose.Height = 32;
        btnClose.TabIndex = 5;
        btnClose.Click += (s, e) => Close();

        btnDelete.Text = "Delete Walk";
        btnDelete.Width = 100;
        btnDelete.Height = 32;
        btnDelete.TabIndex = 4;
        btnDelete.ForeColor = Color.DarkRed;
        btnDelete.Click += (s, e) =>
        {
            if (SelectedWalkId is null) return;
            var result = MessageBox.Show(
                "Delete this walk event? This cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                DeleteRequested?.Invoke(this, e);
        };

        buttonPanel.Controls.AddRange([btnClose, btnDelete]);

        // StatusStrip
        statusLabel.Spring = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        statusStrip.Items.Add(statusLabel);

        // Layout order: bottom-up for Dock
        Controls.Add(dgvWalks);
        Controls.Add(grpNew);
        Controls.Add(buttonPanel);
        Controls.Add(statusStrip);

        CancelButton = btnClose;
        Load += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
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
