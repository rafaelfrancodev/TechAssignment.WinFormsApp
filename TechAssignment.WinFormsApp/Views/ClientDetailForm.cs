using System.ComponentModel;
using System.Windows.Forms;
using Application.DTOs;
using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Client detail/edit form with TextBox simple binding.
/// </summary>
public class ClientDetailForm : Form, IClientDetailView
{
    private readonly TextBox txtName = new();
    private readonly TextBox txtPhone = new();
    private readonly Button btnSave = new();
    private readonly Button btnCancel = new();
    private readonly ErrorProvider errorProvider = new();

    /// <inheritdoc />
    public BindingSource ClientBindingSource { get; } = new();

    /// <inheritdoc />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsEditMode { get; set; }

    /// <inheritdoc />
    public event EventHandler? SaveRequested;
    /// <inheritdoc />
    public event EventHandler? CancelRequested;

    /// <summary>
    /// Initializes the client detail form with simple binding controls.
    /// </summary>
    public ClientDetailForm()
    {
        Text = "Client Details";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Size = new Size(480, 240);
        Padding = new Padding(20);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            AutoSize = true
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        var lblName = new Label { Text = "Name:", Anchor = AnchorStyles.Left, AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        txtName.Dock = DockStyle.Fill;
        txtName.TabIndex = 0;

        var lblPhone = new Label { Text = "Phone:", Anchor = AnchorStyles.Left, AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        txtPhone.Dock = DockStyle.Fill;
        txtPhone.TabIndex = 1;

        // Initialize BindingSource before adding bindings
        ClientBindingSource.DataSource = new ClientDto();

        // SIMPLE BINDING: each TextBox bound to one property of the current record
        txtName.DataBindings.Add("Text", ClientBindingSource,
            nameof(ClientDto.Name), true, DataSourceUpdateMode.OnPropertyChanged);
        txtPhone.DataBindings.Add("Text", ClientBindingSource,
            nameof(ClientDto.PhoneNumber), true, DataSourceUpdateMode.OnPropertyChanged);

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom
        };

        btnSave.Text = "Save";
        btnSave.TabIndex = 2;
        btnSave.Width = 90;
        btnSave.Height = 32;
        btnSave.Click += (s, e) => SaveRequested?.Invoke(this, e);

        btnCancel.Text = "Cancel";
        btnCancel.TabIndex = 3;
        btnCancel.Width = 90;
        btnCancel.Height = 32;
        btnCancel.Click += (s, e) => { CancelRequested?.Invoke(this, e); DialogResult = DialogResult.Cancel; Close(); };

        buttonPanel.Controls.AddRange([btnCancel, btnSave]);
        layout.SetColumnSpan(buttonPanel, 2);

        layout.Controls.Add(lblName, 0, 0);
        layout.Controls.Add(txtName, 1, 0);
        layout.Controls.Add(lblPhone, 0, 1);
        layout.Controls.Add(txtPhone, 1, 1);
        layout.Controls.Add(buttonPanel, 0, 3);

        Controls.Add(layout);
        AcceptButton = btnSave;
        CancelButton = btnCancel;
        errorProvider.ContainerControl = this;
    }

    /// <inheritdoc />
    public void ShowValidationError(string field, string message)
    {
        var control = field switch
        {
            "Name" => (Control)txtName,
            "PhoneNumber" => txtPhone,
            _ => txtName
        };
        errorProvider.SetError(control, message);
    }

    /// <inheritdoc />
    public void ClearErrors() => errorProvider.Clear();

    /// <inheritdoc />
    public void CloseWithSuccess()
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    /// <inheritdoc />
    public void ShowError(string message) =>
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
