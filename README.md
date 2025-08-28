# CodeCraft.NET Clean Architecture Template

A comprehensive Clean Architecture template for .NET 9 with CQRS, automatic code generation, and dual API support (Web + Desktop).

## Features

- **Clean Architecture** with proper layer separation
- **CQRS + MediatR** for scalable operations  
- **Dual API Support**: Web API (HTTP) + Desktop API (Direct)
- **Multi-Database Support**: SQL Server and SQLite providers
- **Entity Framework Core** with SQL Server support
- **Automatic Code Generator** for complete CRUD operations
- **JWT Authentication** with role-based authorization
- **Swagger/OpenAPI** documentation
- **MAUI/Desktop Ready** for offline-first applications

## Database Provider Support

CodeCraft.NET supports both SQL Server and SQLite database providers:

### Configuration
Set your preferred provider in `codecraft.config.json`:

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

### Command Line Usage
```bash
# Use SQL Server (default)
dotnet run --project MyProject.Generator --sqlserver

# Use SQLite (perfect for MAUI)
dotnet run --project MyProject.Generator --sqlite

# Combined with other options
dotnet run --project MyProject.Generator --sqlite --force-migration
```

### When to Use Each Provider
- **SQL Server**: Web applications, enterprise scenarios, high concurrency
- **SQLite**: MAUI/Desktop apps, offline-first, single-user scenarios

For detailed information, see [Database Providers Guide](CodeCraft.NET.Generator/DATABASE_PROVIDERS.md).

## Installation

```bash
dotnet new install CodeCraft.NET.CleanArchitecture.Template
```

## Usage

```bash
# Create new project
dotnet new codecraft -n "MyProject"
cd MyProject

# Build and run
dotnet build
dotnet run --project MyProject.WebAPI
```

## Code Generator

The generator analyzes entities in `Domain/Model/` and automatically creates:

### Generated Components:
- **Web API Controllers** - HTTP endpoints with full CRUD operations
- **Desktop API Services** - Direct access for MAUI/Desktop applications  
- **CQRS Commands & Queries** - Scalable command/query separation
- **Repository Patterns** - Generic repositories with Unit of Work
- **Entity Framework** - DbContext and automatic migrations
- **Validation** - FluentValidation rules for all operations
- **AutoMapper** - DTO mapping profiles
- **Specifications** - Query specification patterns

### Generator Commands:

```bash
# Generate for all entities
dotnet run --project MyProject.Generator

# Clean generated files only (keep your entities)
dotnet run clean --project MyProject.Generator

# Complete reset (remove everything including examples)
dotnet run cleanAll --project MyProject.Generator

# Force migration creation
dotnet run --project MyProject.Generator --force-migration

# Database provider selection
dotnet run --project MyProject.Generator --sqlite          # Use SQLite
dotnet run --project MyProject.Generator --sqlserver       # Use SQL Server

# Combined options
dotnet run --project MyProject.Generator --sqlite -f       # SQLite with force migration
```

## Architecture

The template follows Clean Architecture principles with clear separation of concerns:

- **Domain**: Entities, value objects, and business logic
- **Application**: CQRS, DTOs, validation, and business rules
- **Infrastructure**: Data access, repositories, and external services
- **WebAPI**: HTTP controllers, authentication, and API configuration
- **DesktopAPI**: Direct services for MAUI/Desktop applications
- **Generator**: Automatic code generation engine

## Desktop API for MAUI Applications

### Key Benefits:
- **No HTTP Overhead** - Direct method calls
- **Offline Support** - Local database access
- **Shared Business Logic** - Same validation and rules as Web API
- **Performance** - Zero network latency
- **Easy Debugging** - Direct access to business layer

### 1. Configuration in MauiProgram.cs

```csharp
using CodeCraft.NET.DesktopAPI;
using Microsoft.Extensions.Configuration;

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
            });

        // Configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        // Connection string for local database
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=YourAppDb;Trusted_Connection=true;MultipleActiveResultSets=true;";

        // Register Desktop API services
        builder.Services.AddDesktopApiServices(configuration, connectionString);

        return builder.Build();
    }
}
```

### 2. Dependency Injection in ViewModels

```csharp
public partial class ProductViewModel : ObservableObject
{
    private readonly ProductService _productService;

    public ProductViewModel(ProductService productService)
    {
        _productService = productService;
    }

    [RelayCommand]
    async Task LoadProducts()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            // Process products...
        }
        catch (Exception ex)
        {
            // Handle errors...
        }
    }

    [RelayCommand]
    async Task CreateProduct(ProductCreateDto productDto)
    {
        try
        {
            var productId = await _productService.CreateAsync(new ProductCreate 
            { 
                Name = productDto.Name,
                Price = productDto.Price 
            });
            // Product created with ID: productId
        }
        catch (Exception ex)
        {
            // Handle errors...
        }
    }
}
```

### 3. Available Methods for Each Entity

For each generated entity (e.g., Product), the following methods are automatically created:

```csharp
// Create
int productId = await productService.CreateAsync(createCommand);

// Update
bool success = await productService.UpdateAsync(updateCommand);

// Delete
bool deleted = await productService.DeleteAsync(productId);

// Get by ID
ProductDto? product = await productService.GetByIdAsync(productId);

// Get with relationships
ProductWithRelatedDto? productWithRelated = await productService.GetWithRelatedAsync(productId);
```

### 4. Integration with MAUI Applications

The Desktop API is specifically designed for MAUI applications that need:

- **Offline Capabilities**: Full functionality without internet connection
- **Local Data Storage**: SQLite or LocalDB integration
- **Performance**: Zero network latency for data operations
- **Security**: Data stays on the device
- **Simplified Architecture**: No need for HTTP client configuration

### 5. Service Registration

The `AddDesktopApiServices` extension method automatically registers:

- All generated entity services
- MediatR handlers for CQRS operations
- Repository patterns and Unit of Work
- Entity Framework DbContext
- AutoMapper profiles
- FluentValidation rules

### 6. Best Practices for Desktop API

- Use async/await patterns for all database operations
- Implement proper error handling in ViewModels
- Leverage dependency injection for testability
- Use the same DTOs and commands as the Web API
- Handle database migrations in production scenarios

## Generated API Methods

For each entity (e.g., `Product`), the generator creates:

### Web API Controllers:
```csharp
[HttpPost] CreateProduct(ProductCreate command)
[HttpPut] UpdateProduct(ProductUpdate command)  
[HttpDelete("{id}")] DeleteProduct(int id)
[HttpGet("{id}")] GetProductById(int id)
[HttpGet("{id}/related")] GetProductWithRelated(int id)
```

### Desktop API Services:
```csharp
Task<int> CreateAsync(ProductCreate command)
Task<bool> UpdateAsync(ProductUpdate command)
Task<bool> DeleteAsync(int id)
Task<ProductDto?> GetByIdAsync(int id)
Task<ProductWithRelatedDto?> GetWithRelatedAsync(int id)
```

## Quick Start

1. **Create Project**:
   ```bash
   dotnet new codecraft -n "MyAwesomeProject"
   cd MyAwesomeProject
   ```

2. **Add Your Entities** to `MyAwesomeProject.Domain/Model/`:
   ```csharp
   public class Product : BaseDomainModel
   {
       public string Name { get; set; } = string.Empty;
       public decimal Price { get; set; }
       public string Description { get; set; } = string.Empty;
   }
   ```

3. **Generate Code**:
   ```bash
   # For web applications (SQL Server)
   dotnet run --project MyAwesomeProject.Generator --sqlserver
   
   # For MAUI/Desktop applications (SQLite)
   dotnet run --project MyAwesomeProject.Generator --sqlite
   ```

4. **Run Web API**:
   ```bash
   dotnet run --project MyAwesomeProject.WebAPI
   ```

5. **Test APIs**: Navigate to `https://localhost:7202/swagger`

## Comparison: Web API vs Desktop API

| Feature | Web API | Desktop API |
|---------|---------|-------------|
| **Transport** | HTTP/REST | Direct Method Calls |
| **Performance** | Network Latency | Instant |
| **Offline Support** | Requires Connection | Full Offline |
| **Caching** | Manual/HTTP Cache | Local Database |
| **Security** | JWT/OAuth | Application-level |
| **Debugging** | Remote Debugging | Direct Debugging |
| **Scalability** | Horizontal Scaling | Per-device |
| **Use Cases** | Web Apps, Mobile Apps | MAUI, Desktop, Offline |
| **Configuration** | HTTP Client Setup | Service Registration |
| **Error Handling** | HTTP Status Codes | Direct Exception Handling |
| **Data Transfer** | JSON Serialization | Direct Object Transfer |
| **Authentication** | Token-based | Application-level Security |

## Development Workflow

### Building and Packaging
```bash
# Build solution and create template package (patch version)
.\build.ps1

# Create new minor version (new features)
.\build.ps1 -Minor

# Create new major version (breaking changes) 
.\build.ps1 -Major

# Publish to NuGet
.\build.ps1 -PublishToNuGet -ApiKey YOUR_API_KEY
```

### Semantic Versioning
- **PATCH** (default): Bug fixes and minor updates
- **MINOR** (`-Minor`): New features, backward compatible
- **MAJOR** (`-Major`): Breaking changes

## Project Structure

```
MyProject/
??? MyProject.Domain/          # Entities and business logic
??? MyProject.Application/      # CQRS, DTOs, validation
??? MyProject.Infrastructure/   # Data access, repositories
??? MyProject.WebAPI/          # HTTP controllers
??? MyProject.DesktopAPI/      # Direct services for MAUI
??? MyProject.Generator/       # Code generation engine
```

## Pro Tips

- **Start Simple**: Begin with basic entities, then add complexity
- **Use Specifications**: Leverage the specification pattern for complex queries
- **Validation First**: FluentValidation rules are generated automatically
- **Test Both APIs**: Web API for integration, Desktop API for unit tests
- **Migration Strategy**: Use `--force-migration` when needed
- **Custom Logic**: Extend generated services with your own business logic
- **MAUI Integration**: Use Desktop API for offline-first MAUI applications
- **Database Strategy**: Consider SQLite for MAUI, SQL Server for Web API
- **Error Handling**: Implement consistent error handling across both APIs
- **Performance**: Desktop API is ideal for high-frequency operations
- **Security**: Web API for public access, Desktop API for trusted environments
- **Development**: Use the same entities and commands for both APIs

## Dual API Development

### Shared Components

Both Web API and Desktop API share the same business logic:

- **Domain Entities**: Same entity definitions
- **CQRS Operations**: Identical commands and queries
- **Business Rules**: Same validation logic
- **Data Layer**: Shared repositories and Unit of Work
- **DTOs**: Common data transfer objects

### Different Implementations

| Component | Web API | Desktop API |
|-----------|---------|-------------|
| **Controllers** | HTTP endpoints | Service classes |
| **Authentication** | JWT tokens | Application-level |
| **Serialization** | JSON over HTTP | Direct object transfer |
| **Error Handling** | HTTP status codes | Exception propagation |
| **Documentation** | Swagger/OpenAPI | IntelliSense |

### Development Workflow

1. **Design Entities**: Start with domain models
2. **Generate Code**: Run the generator for both APIs
3. **Test Web API**: Use Swagger for HTTP testing
4. **Test Desktop API**: Create unit tests with direct calls
5. **Deploy**: Web API to cloud, Desktop API embedded in MAUI

### Migration Scenarios

- **Web to Desktop**: Reuse existing business logic
- **Desktop to Web**: Expose local services via HTTP
- **Hybrid Apps**: Use both APIs in the same solution

## Use Cases

### Web API Perfect For:
- Public APIs and microservices
- Web applications and SPAs
- Mobile apps with connectivity
- Integration with external systems
- Scalable cloud deployments
- Cross-platform client access
- Real-time data synchronization

### Desktop API Perfect For:
- MAUI cross-platform applications
- Desktop applications with local data
- Offline-first scenarios
- High-security environments
- Performance-critical applications
- Point-of-sale systems
- Local reporting and analytics
- Air-gapped environments

## What's Generated

When you run the generator, you get:

### For Each Entity:
- **Commands**: Create, Update, Delete with validation
- **Queries**: GetById, GetWithRelated with specifications
- **Controllers**: Full REST API with Swagger documentation
- **Services**: Direct method access for desktop applications
- **Repositories**: Generic repository with specifications
- **DTOs**: Data transfer objects for all operations
- **Mappings**: AutoMapper profiles for conversions
- **Validations**: FluentValidation rules for business logic

### Infrastructure:
- **DbContext**: Entity Framework configuration
- **Migrations**: Automatic database schema updates
- **Unit of Work**: Transaction management
- **Service Registration**: Dependency injection setup
- **Desktop Services**: MAUI-ready service registration
- **API Documentation**: Swagger for Web API testing
- **Error Handling**: Consistent exception management

## License

MIT License - Feel free to use in your projects!

---

**Made with love for .NET developers who love Clean Architecture and rapid development**
