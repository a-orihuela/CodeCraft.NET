# CodeCraft.NET Clean Architecture Template

## ?? Installation
# Install the template
dotnet new install .

# Or from NuGet (when published)
dotnet new install CodeCraft.NET.Template
## ?? Usage
# Create new project
dotnet new codecraft -n "MyNewProject"

# With custom options
dotnet new codecraft -n "MyProject" --CompanyName "MyCompany" --DatabaseProvider "PostgreSQL" --UseDocker true
## ?? Available Parameters

| Parameter | Description | Values | Default |
|-----------|-------------|---------|---------|
| `ProjectName` | Project name | string | MyProject |
| `CompanyName` | Company name | string | MyCompany |
| `DatabaseProvider` | Database provider | SqlServer, PostgreSQL | SqlServer |
| `UseDocker` | Include Docker support | true, false | false |

## ??? Generated Structure
MyProject/
??? MyProject.Domain/          # Entities and domain logic
??? MyProject.Application/     # CQRS, DTOs, Contracts
??? MyProject.Infrastructure/  # Entity Framework, Repositories
??? MyProject.WebAPI/         # Controllers, Configuration
??? MyProject.Cross/          # Shared utilities
??? MyProject.Generator/      # Code generator
## ?? What is CodeCraft.NET?

CodeCraft.NET is a **powerful project template** that combines **Clean Architecture** with **automatic code generation**. It's designed to help developers create enterprise-grade .NET applications in minutes, not hours.

### Key Features:
- ??? **Clean Architecture** with proper layer separation
- ?? **CQRS + MediatR** for scalable command/query operations
- ??? **Entity Framework Core** with automatic migrations
- ? **Automatic Code Generator** that creates boilerplate code
- ?? **JWT Authentication** and Azure AD integration
- ?? **Swagger/OpenAPI** documentation
- ?? **Repository + Unit of Work** patterns
- ??? **AutoMapper** for object mapping
- ? **FluentValidation** for input validation

## ?? Quick Start Guide (5 Minutes)

### Step 1: Create Your First Entity

Create a new domain entity in `MyProject.Domain/Model/`:
using CodeCraft.NET.Cross.Domain;

namespace MyProject.Domain.Model;

public class Product : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
}
### Step 2: Run the Code Generator
dotnet run --project MyProject.Generator
### Step 3: Apply Database Migrations
dotnet ef database update --project MyProject.Infrastructure --startup-project MyProject.WebAPI
### Step 4: Run Your API
dotnet run --project MyProject.WebAPI
### Step 5: Test Your API

Navigate to `https://localhost:7202/swagger` and you'll see:

? **Automatically Generated Endpoints:**
- `GET /api/Product` - Get all products with pagination
- `GET /api/Product/{id}` - Get product by ID
- `POST /api/Product` - Create new product
- `PUT /api/Product/{id}` - Update existing product
- `DELETE /api/Product/{id}` - Delete product

## ?? What the Generator Creates

When you run the generator, it automatically creates:

### ?? In Application Layer:Application/
??? CQRS/Features/Products/
?   ??? Commands/
?   ?   ??? Create/ProductCreate.cs, ProductCreateHandler.cs, ProductCreateValidator.cs
?   ?   ??? Update/ProductUpdate.cs, ProductUpdateHandler.cs, ProductUpdateValidator.cs
?   ?   ??? Delete/ProductDelete.cs, ProductDeleteHandler.cs
?   ??? Queries/
?       ??? GetProductById.cs, GetProductByIdHandler.cs
?       ??? GetProductWithRelated.cs, GetProductWithRelatedHandler.cs
??? Contracts/Persistence/Repositories/IProductRepository.cs
??? Mapping/MappingProfile.cs (updated)
### ?? In Infrastructure Layer:Infrastructure/
??? Persistence/Repositories/ProductRepository.cs
??? Persistence/Custom/Repositories/ProductRepository.Custom.cs
??? ApplicationDbContext.cs (updated with Product DbSet)
### ?? In WebAPI Layer:WebAPI/Controllers/ProductController.cs
## ?? Extending Functionality with Custom Features

### Adding Custom Commands

Create custom business logic in the `Custom` folders:
// Application/CQRS/Custom/Features/Products/Commands/DiscountProduct/
public class DiscountProductCommand : IRequest<bool>
{
    public int ProductId { get; set; }
    public decimal DiscountPercentage { get; set; }
}

public class DiscountProductHandler : IRequestHandler<DiscountProductCommand, bool>
{
    private readonly ICodeCraftUnitOfWork _unitOfWork;
    
    public DiscountProductHandler(ICodeCraftUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> Handle(DiscountProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId);
        product.Price = product.Price * (1 - request.DiscountPercentage / 100);
        
        _unitOfWork.Repository<Product>().UpdateEntity(product);
        return await _unitOfWork.Complete() > 0;
    }
}
### Adding Custom Repository Methods

Extend repositories with custom queries:
// Application/Contracts/Persistence/Custom/IProductRepository.Custom.cs
public partial interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByCategory(string category);
    Task<IEnumerable<Product>> GetLowStockProducts(int threshold);
}

// Infrastructure/Persistence/Custom/Repositories/ProductRepository.Custom.cs
public partial class ProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
    {
        return await _context.Products
            .Where(p => p.Category == category && p.Active)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Product>> GetLowStockProducts(int threshold)
    {
        return await _context.Products
            .Where(p => p.Stock <= threshold && p.Active)
            .ToListAsync();
    }
}
### Adding Custom Controllers

Create specialized endpoints:
// WebAPI/Controllers/Custom/ProductManagementController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductManagementController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProductManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("{id}/discount")]
    public async Task<IActionResult> ApplyDiscount(int id, [FromBody] DiscountProductCommand command)
    {
        command.ProductId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
## ?? Authentication & Authorization

The template includes built-in authentication:

### JWT Authentication// Login endpoint automatically available
POST /api/Auth/login
{
    "email": "admin@localhost.com",
    "password": "admin"
}
### Register New UsersPOST /api/Auth/register
{
    "email": "user@example.com",
    "password": "SecurePassword123!",
    "fullName": "John Doe"
}
### Protect Your Endpoints[Authorize]
[HttpGet]
public async Task<IActionResult> GetProducts()
{
    // Only authenticated users can access
}

[Authorize(Roles = "Administrator")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteProduct(int id)
{
    // Only administrators can delete
}
## ?? Database Features

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

## ?? Testing Your API

Use the built-in Swagger UI at `https://localhost:7202/swagger`:

1. **Authenticate**: Use the Auth endpoints to get a JWT token
2. **Authorize**: Click "Authorize" and paste your token
3. **Test Endpoints**: Try all the CRUD operations
4. **View Schemas**: See all DTOs and models

## ?? Configuration

### Database Provider
Switch between SQL Server and PostgreSQL in `appsettings.json`:
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
## ?? Development Workflow

1. **Define Domain Entities** in `Domain/Model/`
2. **Run Code Generator** with `dotnet run --project Generator`
3. **Customize Generated Code** in `Custom/` folders
4. **Add Business Logic** in handlers and repositories
5. **Test with Swagger** or your preferred API client
6. **Deploy** your application

## ??? Advanced Features

### Pagination
All list endpoints support pagination:GET /api/Product?PageNumber=1&PageSize=10&OrderBy=Name
### Filtering
Use specifications for complex queries:// Application/CQRS/Specifications/Products/ProductSpecification.cs
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
FluentValidation is integrated automatically:public class ProductCreateValidator : AbstractValidator<ProductCreate>
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
## ?? Architecture Deep Dive

### Clean Architecture Layers
???????????????????
?    WebAPI       ?  ? Controllers, Middleware, Configuration
???????????????????
?  Application    ?  ? CQRS, Validators, DTOs, Contracts
???????????????????
? Infrastructure  ?  ? EF Core, Repositories, External Services
???????????????????
?    Domain       ?  ? Entities, Value Objects, Domain Logic
???????????????????
### CQRS Pattern
- **Commands**: Modify data (Create, Update, Delete)
- **Queries**: Read data (GetById, GetAll, Search)
- **Handlers**: Business logic implementation
- **Validators**: Input validation rules

## ?? Contributing

This template is designed to be a starting point for .NET projects following Clean Architecture principles. Feel free to customize it according to your specific needs.

## ?? License

This template is provided as-is for educational and commercial use.
