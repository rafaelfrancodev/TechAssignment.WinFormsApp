using TechAssignment.WinFormsApp.Views.Contracts;

namespace TechAssignment.WinFormsApp.Views;

/// <summary>
/// Login form implementing the ILoginView contract.
/// </summary>
public class LoginForm : Form, ILoginView
{
    private readonly TextBox txtUsername = new();
    private readonly TextBox txtPassword = new();
    private readonly Button btnLogin = new();
    private readonly Button btnRegister = new();
    private readonly Button btnCancel = new();
    private readonly Label lblError = new();

    /// <inheritdoc />
    public string Username => txtUsername.Text;
    /// <inheritdoc />
    public string Password => txtPassword.Text;

    /// <inheritdoc />
    public event EventHandler? LoginRequested;
    /// <inheritdoc />
    public event EventHandler? RegisterRequested;
    /// <inheritdoc />
    public event EventHandler? CancelRequested;

    /// <summary>
    /// Initializes the login form with all controls.
    /// </summary>
    public LoginForm()
    {
        Text = "Dog Walking Manager";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Size = new Size(420, 300);

        // Title banner
        var lblTitle = new Label
        {
            Text = "Dog Walking Manager",
            Dock = DockStyle.Top,
            Height = 50,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(45, 45, 48),
            ForeColor = Color.White
        };

        // Main layout
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30, 15, 30, 15),
            ColumnCount = 2,
            RowCount = 4
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var lblUser = new Label { Text = "Username:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) };
        txtUsername.Dock = DockStyle.Fill;
        txtUsername.Font = new Font("Segoe UI", 10);
        txtUsername.TabIndex = 0;

        var lblPass = new Label { Text = "Password:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) };
        txtPassword.Dock = DockStyle.Fill;
        txtPassword.Font = new Font("Segoe UI", 10);
        txtPassword.UseSystemPasswordChar = true;
        txtPassword.TabIndex = 1;

        // Error/success message
        lblError.Dock = DockStyle.Fill;
        lblError.ForeColor = Color.Red;
        lblError.AutoSize = true;
        lblError.Font = new Font("Segoe UI", 9);
        layout.SetColumnSpan(lblError, 2);

        // Buttons
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 5, 0, 0)
        };

        btnCancel.Text = "Cancel";
        btnCancel.Width = 85;
        btnCancel.Height = 32;
        btnCancel.TabIndex = 4;
        btnCancel.Click += (s, e) => { CancelRequested?.Invoke(this, e); DialogResult = DialogResult.Cancel; Close(); };

        btnRegister.Text = "Register";
        btnRegister.Width = 85;
        btnRegister.Height = 32;
        btnRegister.TabIndex = 3;
        btnRegister.Click += (s, e) => RegisterRequested?.Invoke(this, e);

        btnLogin.Text = "Login";
        btnLogin.Width = 85;
        btnLogin.Height = 32;
        btnLogin.TabIndex = 2;
        btnLogin.BackColor = Color.FromArgb(0, 122, 204);
        btnLogin.ForeColor = Color.White;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.Click += (s, e) => LoginRequested?.Invoke(this, e);

        buttonPanel.Controls.AddRange([btnCancel, btnRegister, btnLogin]);
        layout.SetColumnSpan(buttonPanel, 2);

        layout.Controls.Add(lblUser, 0, 0);
        layout.Controls.Add(txtUsername, 1, 0);
        layout.Controls.Add(lblPass, 0, 1);
        layout.Controls.Add(txtPassword, 1, 1);
        layout.Controls.Add(lblError, 0, 2);
        layout.Controls.Add(buttonPanel, 0, 3);

        Controls.Add(layout);
        Controls.Add(lblTitle);

        AcceptButton = btnLogin;
        CancelButton = btnCancel;
    }

    /// <inheritdoc />
    public void ShowError(string message)
    {
        lblError.ForeColor = Color.Red;
        lblError.Text = message;
    }

    /// <inheritdoc />
    public void ShowMessage(string message)
    {
        lblError.ForeColor = Color.FromArgb(0, 150, 0);
        lblError.Text = message;
    }

    /// <inheritdoc />
    public void CloseWithSuccess()
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
