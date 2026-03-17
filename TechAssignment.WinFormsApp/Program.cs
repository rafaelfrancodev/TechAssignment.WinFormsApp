using System.Configuration;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infraestructure.Persistence;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TechAssignment.WinFormsApp.Presenters;
using TechAssignment.WinFormsApp.Views;

namespace TechAssignment.WinFormsApp;

/// <summary>
/// Application entry point and composition root.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// All dependency wiring happens here — nowhere else.
    /// </summary>
    [STAThread]
    static void Main()
    {
        System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

        // Catch unhandled exceptions so async void event handlers don't crash the app
        System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        System.Windows.Forms.Application.ThreadException += (_, args) =>
            MessageBox.Show(args.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            MessageBox.Show(((Exception)args.ExceptionObject).Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        var connStr = ConfigurationManager.ConnectionStrings["DogWalking"]?.ConnectionString
            ?? "Server=(localdb)\\mssqllocaldb;Database=DogWalkingDb;Trusted_Connection=True;";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connStr)
            .Options;

        // Apply pending migrations on startup
        using (var ctx = new AppDbContext(options))
        {
            ctx.Database.Migrate();
        }

        // DbContext factory — each repository operation gets its own context
        var factory = new AppDbContextFactory(options);

        // Repositories
        IClientRepository clientRepo = new ClientRepository(factory);
        IDogRepository dogRepo = new DogRepository(factory);
        IWalkEventRepository walkRepo = new WalkEventRepository(factory);
        IUserRepository userRepo = new UserRepository(factory);

        // Services
        IClientService clientService = new ClientService(clientRepo);
        IDogService dogService = new DogService(dogRepo, clientRepo);
        IWalkEventService walkService = new WalkEventService(walkRepo, dogRepo);
        IAuthService authService = new AuthService(userRepo);

        // Login gate
        var loginForm = new LoginForm();
        var loginPresenter = new LoginPresenter(loginForm, authService);
        var loginResult = loginForm.ShowDialog();
        if (loginResult != DialogResult.OK) return;

        // Main window
        var mainForm = new MainForm(clientService, dogService, walkService);
        System.Windows.Forms.Application.Run(mainForm);
    }
}
