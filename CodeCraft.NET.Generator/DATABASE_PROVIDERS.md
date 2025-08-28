# Database Provider Selection Guide

CodeCraft.NET Generator supports both SQL Server and SQLite database providers. This guide explains how to use each one.

## Command Line Usage

### Generate with SQL Server (Default)
```bash
dotnet run --project CodeCraft.NET.Generator
# or explicitly
dotnet run --project CodeCraft.NET.Generator --sqlserver
```

### Generate with SQLite
```bash
dotnet run --project CodeCraft.NET.Generator --sqlite
```

### Combined with other options
```bash
# SQLite with force migration
dotnet run --project CodeCraft.NET.Generator --sqlite --force-migration

# SQL Server with force migration
dotnet run --project CodeCraft.NET.Generator --sqlserver -f
```

## Configuration

You can set the default provider in `codecraft.config.json`:

```json
{
  "DataBaseConfig": {
    "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=CodeCraftDb;Trusted_Connection=true;MultipleActiveResultSets=true;",
    "SqliteConnectionString": "Data Source=CodeCraftDb.db",
    "MigrationsAssembly": "CodeCraft.NET.Infrastructure",
    "DatabaseProvider": "SqlServer"
  }
}
```

To use SQLite by default, change `"DatabaseProvider": "SQLite"`.

## Generated Code

The generator automatically creates the appropriate database configuration based on your selection:

### SQL Server Configuration
```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
    )
);
```

### SQLite Configuration
```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        connectionString,
        x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
    )
);
```

## Usage in Applications

### Web API Applications
Web API applications automatically use the configuration from `appsettings.json`:

```csharp
// Program.cs - Uses configuration-based setup
builder.Services.AddInfrastructureServices(builder.Configuration, connectionString);
```

### Desktop/MAUI Applications
Desktop and MAUI applications can explicitly specify the provider:

```csharp
// MauiProgram.cs - Explicit provider selection
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        // For SQLite (recommended for MAUI)
        var sqliteConnection = "Data Source=MyApp.db";
        builder.Services.AddDesktopApiServices(sqliteConnection, DatabaseProvider.SQLite);
        
        // For SQL Server
        // var sqlServerConnection = "Server=(localdb)\\mssqllocaldb;Database=MyAppDb;Trusted_Connection=true;";
        // builder.Services.AddDesktopApiServices(sqlServerConnection, DatabaseProvider.SqlServer);
        
        return builder.Build();
    }
}
```

## When to Use Each Provider

### Use SQL Server When:
- Building web applications
- Need advanced database features
- Deploying to Azure or enterprise environments
- Require high concurrency and scalability
- Working with complex queries and stored procedures

### Use SQLite When:
- Building MAUI/Desktop applications
- Need offline-first functionality
- Working with single-user scenarios
- Prototyping or development
- Deploying lightweight applications
- Want zero-configuration database setup

## Migration Considerations

### SQL Server Migrations
- Migrations work with SQL Server Express, LocalDB, or full SQL Server
- Connection string determines the target database
- Supports all Entity Framework features

### SQLite Migrations
- Creates a single `.db` file in your application directory
- Automatically created if it doesn't exist
- Some Entity Framework features may have limitations
- Perfect for portable applications

## Package Dependencies

The generator automatically includes both providers:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.7" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.7" />
```

## Examples

### Console Application Example
```csharp
var services = new ServiceCollection();

// SQLite for portable console app
services.AddInfrastructureServices("Data Source=console.db", DatabaseProvider.SQLite);

// SQL Server for enterprise console app
// services.AddInfrastructureServices(connectionString, DatabaseProvider.SqlServer);

var serviceProvider = services.BuildServiceProvider();
```

### Testing Example
```csharp
// Use in-memory SQLite for testing
services.AddInfrastructureServices("Data Source=:memory:", DatabaseProvider.SQLite);
```

This flexibility allows you to choose the right database provider for each scenario while maintaining the same business logic and API structure.