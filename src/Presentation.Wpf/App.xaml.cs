using System.IO;
using System.Windows;
using Application.Abstractions;
using Application.Devices;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Wpf.Services;
using Presentation.Wpf.ViewModels;
using Application.Validation;



namespace Presentation.Wpf
{
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;
        private IServiceScope? _appScope;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // ---- SQLite file path (LocalAppData) ----
                    var dbPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "WpfDeviceInventory",
                        "devices.db");

                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

                    // ---- EF Core: DbContextFactory (fresh DbContext per operation) ----
                    services.AddDbContextFactory<AppDbContext>(opt =>
                        opt.UseSqlite($"Data Source={dbPath}"));

                    // ---- Repo abstraction -> EF implementation ----
                    services.AddScoped<IDeviceRepository, DeviceRepository>();

                    // ---- Validation ----
                    services.AddScoped<IDeviceValidator, DeviceValidator>();

                    // ---- Use-cases (Application layer) ----
                    services.AddScoped<GetDevicesQuery>();
                    services.AddScoped<CreateDeviceCommand>();
                    services.AddScoped<UpdateDeviceCommand>();
                    services.AddScoped<DeleteDeviceCommand>();

                    // ---- Shared UI state (global flags/messages/navigation) ----
                    services.AddSingleton<AppState>();

                    // ---- ViewModels (Scoped = safe with scoped use-cases) ----
                    services.AddScoped<DeviceListViewModel>();
                    services.AddScoped<DeviceDetailViewModel>();
                    services.AddScoped<MainViewModel>();

                    // ---- Window (Scoped) ----
                    services.AddScoped<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // One scope for the whole app lifetime (common pattern for desktop apps)
            _appScope = _host.Services.CreateScope();

            // Ensure DB exists
            var factory = _appScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
            await using (var db = await factory.CreateDbContextAsync())
            {
                await db.Database.EnsureCreatedAsync();
            }

            // Show main window from the app scope
            var mainWindow = _appScope.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            _appScope?.Dispose();
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
