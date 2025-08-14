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

    dotnet new codecraft -n "CustomerManagement"
    cd CustomerManagement

### Step 3: Run the Code Generator

The template includes a sample `Customer` entity in `CustomerManagement.Domain/Model/Customer.cs`:

```csharp
public class Customer : BaseDomainModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}
```

Generate all CRUD operations for this entity:

    dotnet run --project CustomerManagement.Generator

### Step 4: Run Your API

    dotnet run --project CustomerManagement.WebAPI

### Step 5: Test Your API

Navigate to `https://localhost:7202/swagger` and you'll see:

**Automatically Generated Endpoints:**
- `GET /api/Customer` - Get all customers with pagination
- `GET /api/Customer/{id}` - Get customer by ID
- `POST /api/Customer` - Create new customer
- `PUT /api/Customer/{id}` - Update existing customer
- `DELETE /api/Customer/{id}` - Delete customer

### Clean Template (Optional)

Want to start fresh or clean up example files?

    # Remove only generated files (keep your Domain entities)
    dotnet run clean --project CustomerManagement.Generator
    
    # Remove all generated files and example entities (complete reset)
    dotnet run cleanAll --project CustomerManagement.Generator

**Two cleaning levels:**
- `clean` - Removes generated files but keeps your Domain entities
- `cleanAll` - Complete reset, removes everything including example entities

Perfect for:
- **Development**: Use `clean` to regenerate code while keeping your entities
- **Fresh Start**: Use `cleanAll` for a completely clean template base!

## Code Generator Commands

CodeCraft.NET includes a powerful code generator that creates all the boilerplate for you:

### Available Commands

```bash
# Generate CRUD for all entities in Domain project  
dotnet run --project YourProject.Generator

# Clean generated files only (keep Domain entities)
dotnet run clean --project YourProject.Generator

# Clean all generated files and example entities (complete reset)
dotnet run cleanAll --project YourProject.Generator

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

For each entity (e.g., `Customer`), the generator creates:

**Application Layer** (30+ files):
- Commands: `CustomerCreate`, `CustomerUpdate`, `CustomerDelete` with handlers and validators
- Queries: `GetCustomerById`, `GetCustomerWithRelated` with handlers  
- Specifications: `CustomerSpecification`, `CustomerSpecificationParams`
- Contracts: `ICustomerRepository`
- Mapping: Updates `MappingProfile.cs`

**Infrastructure Layer**:
- Repository: `CustomerRepository.cs` implementing `ICustomerRepository`
- DbContext: Updates `ApplicationDbContext.cs` with `DbSet<Customer>`
- UnitOfWork: Updates or creates `CodeCraftUnitOfWork.cs`

**WebAPI Layer**:
- Controller: `CustomerController.cs` with full CRUD endpoints
- HTTP Tests: `CustomerRequests.http` for testing

**Database**:
- Migration: Auto-generated EF Core migration

### Example: Adding a Product Entity

```csharp
// 1. Create Domain/Model/Product.cs
public class Product : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
}

// 2. Run generator
dotnet run --project YourProject.Generator

// 3. Instantly get 30+ generated files:
//    - ProductController with CRUD endpoints
//    - ProductCreate, ProductUpdate, ProductDelete commands
//    - GetProductById, GetProductWithRelated queries  
//    - ProductRepository and IProductRepository
//    - ProductSpecification for filtering
//    - ProductCreateValidator, ProductUpdateValidator
//    - Updated DbContext with DbSet<Product>
//    - Auto-generated migration
```

### Clean Template

The generator provides two levels of cleaning:

```bash
# Clean generated files only (keeps your Domain entities)
dotnet run clean --project YourProject.Generator

# Complete template reset (removes everything)
dotnet run cleanAll --project YourProject.Generator
```

**`clean` command removes:**
- ? Generated CRUD files (Commands, Queries, Handlers, Validators)
- ? Generated Controllers and API endpoints  
- ? Generated Repositories and UnitOfWork files
- ? Generated DbContext and Mapping files
- ? Auto-generated migrations
- ? Empty directories and cleanup
- ? **Keeps your Domain entities untouched**

**`cleanAll` command removes:**
- ? Everything from `clean` command above
- ? Example entities (Customer.cs, etc.)
- ? Model directory if empty

Perfect for:
- ?? **Development**: Use `clean` to regenerate without losing your entities
- ?? **Demos**: Use `cleanAll` for clean template presentations
- ?? **Production**: Use `cleanAll` to start fresh for real projects

## Database Features

### Automatic Migrations

The generator creates and applies migrations automatically. You can also create custom migrations:

    dotnet ef migrations add YourMigrationName --project Infrastructure --startup-project WebAPI

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

1. **Test Endpoints**: Try all the CRUD operations
2. **View Schemas**: See all DTOs and models
3. **Interactive Documentation**: Test your API directly from the browser
4. **Response Examples**: See sample request/response formats

## Configuration

### Database Connection

Configure your SQL Server connection in `appsettings.json`:

    {
      "ConnectionStrings": {
        "Application": "Server=(localdb)\\mssqllocaldb;Database=MyProjectDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
      }
    }

### Advanced Features

### Pagination

All list endpoints support pagination:

    GET /api/Customer?PageNumber=1&PageSize=10&OrderBy=FirstName

### Filtering

Use specifications for complex queries:

    // Application/CQRS/Specifications/Customers/CustomerSpecification.cs
    public class CustomerSpecification : BaseSpecification<Customer>
    {
        public CustomerSpecification(CustomerSpecificationParams @params)
        {
            if (!string.IsNullOrEmpty(@params.Email))
                AndCriteria(x => x.Email.Contains(@params.Email));
            
            if (@params.MinAge.HasValue)
                AndCriteria(x => x.DateOfBirth <= DateTime.Now.AddYears(-@params.MinAge.Value));
        }
    }

### Validation

FluentValidation is integrated automatically:
    
    public class CustomerCreateValidator : AbstractValidator<CustomerCreate>
    {
        public CustomerCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50);
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email address is required");
        }
    }
