using Microsoft.Extensions.Logging;
using CodeCraft.NET.Infrastructure;
using CodeCraft.NET.DesktopAPI;
using CodeCraft.NET.Cross.Enums;

namespace CodeCraft.NET.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Add logging
		builder.Services.AddLogging(configure => configure.AddDebug());

		// Database configuration for MAUI (SQLite with proper location)
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "CodeCraftDb.db");
		var connectionString = $"Data Source={dbPath}";

		// Log database location for debugging
#if DEBUG
		System.Diagnostics.Debug.WriteLine($"SQLite Database Location: {dbPath}");
#endif

		// Add Infrastructure services (for data access)
		builder.Services.AddInfrastructureServices(connectionString, DatabaseProvider.SQLite);

		// Add Desktop API services (for business logic)
		builder.Services.AddDesktopApiServices(connectionString, DatabaseProvider.SQLite);

		// Add MAUI-specific services
		builder.Services.AddMauiServices();

		var app = builder.Build();

		// Register Shell routes
		ShellRouting.RegisterRoutes();

		return app;
	}
}