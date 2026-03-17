using System.Configuration;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infraestructure.Persistence;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechAssignment.WinFormsApp.Presenters;
using TechAssignment.WinFormsApp.Views;

namespace TechAssignment.WinFormsApp;

/// <summary>
/// Application entry point and composition root using Microsoft DI container.
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

        // --- DI Container ---
        var services = new ServiceCollection();

        // DbContext — registered as factory so each scope gets its own instance
        services.AddDbContextFactory<AppDbContext>(opts => opts.UseSqlServer(connStr));

        // Repositories — scoped (one per operation)
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDogRepository, DogRepository>();
        services.AddScoped<IWalkEventRepository, WalkEventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Services — scoped
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IDogService, DogService>();
        services.AddScoped<IWalkEventService, WalkEventService>();
        services.AddScoped<IAuthService, AuthService>();

        // Forms — transient (new instance each time)
        services.AddTransient<LoginForm>();
        services.AddTransient<MainForm>();

        var serviceProvider = services.BuildServiceProvider();

        // Apply pending migrations on startup
        using (var scope = serviceProvider.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            ctx.Database.Migrate();
        }

        // Login gate
        var loginForm = serviceProvider.GetRequiredService<LoginForm>();
        var authService = serviceProvider.GetRequiredService<IAuthService>();
        _ = new LoginPresenter(loginForm, authService);
        var loginResult = loginForm.ShowDialog();
        if (loginResult != DialogResult.OK) return;

        // Main window
        var mainForm = serviceProvider.GetRequiredService<MainForm>();
        System.Windows.Forms.Application.Run(mainForm);
    }
}
