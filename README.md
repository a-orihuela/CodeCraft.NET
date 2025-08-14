# CodeCraft.NET Clean Architecture Template

[![NuGet](https://img.shields.io/nuget/v/CodeCraft.NET.CleanArchitecture.Template.svg)](https://www.nuget.org/packages/CodeCraft.NET.CleanArchitecture.Template/)
[![Downloads](https://img.shields.io/nuget/dt/CodeCraft.NET.CleanArchitecture.Template.svg)](https://www.nuget.org/packages/CodeCraft.NET.CleanArchitecture.Template/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Installation

### From NuGet (Recommended)
# Install the template
    dotnet new install CodeCraft.NET.CleanArchitecture.Template

# Verify installation
    dotnet new list codecraft

### From Source
# Clone repository
    git clone https://github.com/a-orihuela/CodeCraft.NET.git
    cd CodeCraft.NET

# Install locally

    dotnet new install .

## Usage
# Create new project with default settings

    dotnet new codecraft -n "MyNewProject"

# Create with custom options

    dotnet new codecraft -n "MyProject" \
      --CompanyName "MyCompany"

### Available Parameters

| Parameter | Description | Values | Default |
|-----------|-------------|---------|---------|
| `ProjectName` | Project name | string | MyProject |
| `CompanyName` | Company name | string | MyCompany |

## Generated Structure
- MyProject/
    - MyProject.Domain/         # Entities and domain logic
    - MyProject.Application/    # CQRS, DTOs, Contracts
    - MyProject.Infrastructure/  # Entity Framework, Repositories
    - MyProject.WebAPI/         # Controllers, Configuration
    - MyProject.Cross/          # Shared utilities
    - MyProject.Generator/      # Code generator

## What is CodeCraft.NET?

CodeCraft.NET is a **powerful project template** that combines **Clean Architecture** with **automatic code generation**. It's designed to help developers create enterprise-grade .NET applications in minutes, not hours.

### Key Features

- **Clean Architecture** with proper layer separation
- **CQRS + MediatR** for scalable command/query operations
- **Entity Framework Core with SQL Server** for data persistence
- **Automatic Code Generator** that creates boilerplate code
- **Swagger/OpenAPI** documentation
- **Repository + Unit of Work** patterns
- **AutoMapper** for object mapping
- **FluentValidation** for input validation

## Quick Start Guide (5 Minutes)

### Step 1: Install Template

    dotnet new install CodeCraft.NET.CleanArchitecture.Template

### Step 2: Create Project

    dotnet new codecraft -n "ProductCatalog"
    cd ProductCatalog

### Step 3: Create Your First Entity

Create a new domain entity in `ProductCatalog.Domain/Model/`:

    using CodeCraft.NET.Cross.Domain;

    namespace ProductCatalog.Domain.Model;

    public class Product : BaseDomainModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
        public bool IsAvailable { get; set; }
    }

### Step 4: Run the Code Generator

    dotnet run --project ProductCatalog.Generator

### Step 5: Run Your API

    dotnet run --project ProductCatalog.WebAPI

### Step 6: Test Your API

Navigate to `https://localhost:7202/swagger` and you'll see:

**Automatically Generated Endpoints:**
- `GET /api/Product` - Get all products with pagination
- `GET /api/Product/{id}` - Get product by ID
- `POST /api/Product` - Create new product
- `PUT /api/Product/{id}` - Update existing product
- `DELETE /api/Product/{id}` - Delete product

### Clean Template (Optional)

Want to start fresh or clean up example files?

    # Remove all generated files and example entities
    dotnet run clean --project ProductCatalog.Generator

This removes:
- All generated CRUD operations 
- Example entities (Product.cs)
- Auto-generated migrations
- Empty directories

Perfect for starting with a clean template base!

## Code Generator Commands

CodeCraft.NET includes a powerful code generator that creates all the boilerplate for you:

### Available Commands

```bash
# Generate CRUD for all entities in Domain project  
dotnet run --project YourProject.Generator

# Clean all generated files (reset template)
dotnet run clean --project YourProject.Generator  

# Show help and usage information
dotnet run help --project YourProject.Generator
```

### Generator Workflow

1. **Analyze Domain**: Scans `Domain/Model/` for entities inheriting from `BaseDomainModel`
2. **Generate Code**: Creates CQRS commands, queries, handlers, validators, repositories, and controllers
3. **Update DbContext**: Adds DbSets for new entities
4. **Create Migrations**: Automatically generates EF Core migrations
5. **Build & Validate**: Ensures all generated code compiles successfully

### What Gets Generated

For each entity (e.g., `Product`), the generator creates:

**Application Layer** (30+ files):
- Commands: `ProductCreate`, `ProductUpdate`, `ProductDelete` with handlers and validators
- Queries: `GetProductById`, `GetProductWithRelated` with handlers  
- Specifications: `ProductSpecification`, `ProductSpecificationParams`
- Contracts: `IProductRepository`
- Mapping: Updates `MappingProfile.cs`

**Infrastructure Layer**:
- Repository: `ProductRepository.cs` implementing `IProductRepository`
- DbContext: Updates `ApplicationDbContext.cs` with `DbSet<Product>`
- UnitOfWork: Updates or creates `CodeCraftUnitOfWork.cs`

**WebAPI Layer**:
- Controller: `ProductController.cs` with full CRUD endpoints
- HTTP Tests: `ProductRequests.http` for testing

**Database**:
- Migration: Auto-generated EF Core migration

### Example: Adding a Customer Entity

```csharp
// 1. Create Domain/Model/Customer.cs
public class Customer : BaseDomainModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

// 2. Run generator
dotnet run --project YourProject.Generator

// 3. Instantly get 30+ generated files:
//    - CustomerController with CRUD endpoints
//    - CustomerCreate, CustomerUpdate, CustomerDelete commands
//    - GetCustomerById, GetCustomerWithRelated queries  
//    - CustomerRepository and ICustomerRepository
//    - CustomerSpecification for filtering
//    - CustomerCreateValidator, CustomerUpdateValidator
//    - Updated DbContext with DbSet<Customer>
//    - Auto-generated migration
```

### Clean Template

Use the clean command to reset your template:

```bash
dotnet run clean --project YourProject.Generator
```

This removes:
- ? All generated CRUD files (Commands, Queries, Handlers, Validators)
- ? Generated Controllers and API endpoints  
- ? Generated Repositories and UnitOfWork files
- ? Generated DbContext and Mapping files
- ? Example entities (Product.cs, User.cs, etc.)
- ? Auto-generated migrations
- ? Empty directories and cleanup

Perfect for:
- ?? **Testing**: Generate ? Test ? Clean ? Repeat
- ?? **Demos**: Clean template for presentations
- ?? **Production**: Start with a clean base for real projects

## Database Features

### Automatic Migrations

The generator creates and applies migrations automatically:

# Generated migration includes your new entities

    dotnet ef migrations add AutoGen_20240101120000 --project Infrastructure --startup-project WebAPI

### Built-in Auditing

Every entity inherits from `BaseDomainModel`:
- `Id` - Primary key
- `CreatedDate` - When record was created
- `CreatedBy` - Who created the record
- `LastModifiedDate` - When record was last updated
- `LastModifiedBy` - Who last updated the record
- `Active` - Soft delete flag

## Testing Your API

Use the built-in Swagger UI at `https://localhost:7202/swagger`:

1. **Authenticate**: Use the Auth endpoints to get a JWT token
2. **Authorize**: Click "Authorize" and paste your token
3. **Test Endpoints**: Try all the CRUD operations
4. **View Schemas**: See all DTOs and models

## Configuration

### Database Connection

Configure your SQL Server connection in `appsettings.json`:

    {
      "ConnectionStrings": {
        "Application": "Server=(localdb)\\mssqllocaldb;Database=MyProjectDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
      }
    }

### JWT Settings

Configure authentication in `appsettings.Development.json`:

    {
        "JwtSettings": {
        "Issuer": "MyProject.API",
        "Audience": "frontend",
        "Key": "YourSuperSecretSecurityKeyOfAtLeast32Chars",
        "DurationInMinutes": 60
        }
    }

## Development Workflow

1. **Define Domain Entities** in `Domain/Model/`
2. **Run Code Generator** with `dotnet run --project Generator`
3. **Customize Generated Code** in `Custom/` folders
4. **Add Business Logic** in handlers and repositories
5. **Test with Swagger** or your preferred API client
6. **Deploy** your application

## Advanced Features

### Pagination

All list endpoints support pagination:

    GET /api/Product?PageNumber=1&PageSize=10&OrderBy=Name

### Filtering

Use specifications for complex queries:

    // Application/CQRS/Specifications/Products/ProductSpecification.cs
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecificationParams @params)
        {
            if (!string.IsNullOrEmpty(@params.Category))
                AndCriteria(x => x.Category == @params.Category);
            
            if (@params.MinPrice.HasValue)
                AndCriteria(x => x.Price >= @params.MinPrice.Value);
        }
    }

### Validation

FluentValidation is integrated automatically:
    
    public class ProductCreateValidator : AbstractValidator<ProductCreate>
    {
        public ProductCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100);
            
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

## Architecture Deep Dive

### Clean Architecture Layers

- **WebAPI Layer**
  - Controllers, Middleware, Configuration
- **Application Layer**
  - CQRS, Validators, DTOs, Contracts
- **Infrastructure Layer**
  - EF Core, Repositories, External Services
- **Domain Layer**
  - Entities, Value Objects, Domain Logic

### CQRS Pattern

- **Commands**: Modify data (Create, Update, Delete)
- **Queries**: Read data (GetById, GetAll, Search)
- **Handlers**: Business logic implementation
- **Validators**: Input validation rules

## What's New

### Version 1.0.2

- Improved template build script with semantic versioning
- Enhanced PowerShell build automation
- Cleaner template structure
- Better documentation formatting

### Version 1.0.0

- Initial release
- Clean Architecture foundation
- CQRS + MediatR implementation
- Automatic code generation
- Swagger documentation
- Repository + Unit of Work patterns
- FluentValidation integration

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Clean Architecture by Robert C. Martin
- CQRS pattern implementation
- MediatR for request/response patterns
- Entity Framework Core team
- FluentValidation for validation rules

**Star this repository** if you find it helpful!

**Happy coding with CodeCraft.NET!**
