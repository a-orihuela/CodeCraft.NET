# Template Setup Instructions

## Initial Template State

This template includes base files that allow the project to compile immediately after installation. However, to get the full functionality with generated CRUD operations, you need to run the code generator.

### Important: Base vs Generated Files

The template comes with **base versions** of essential files that provide minimal functionality to ensure the project compiles. When you run the generator, these base files are **replaced** with fully functional versions that include:

- **Complete CRUD operations** for all your entities
- **Repository implementations** with Unit of Work pattern
- **CQRS commands and queries** with handlers and validators
- **Entity Framework DbContext** with all your entity DbSets
- **Controllers** with full REST API endpoints

## Quick Start

1. **Install the template** (if not already done):
   ```bash
   dotnet new install CodeCraft.NET.CleanArchitecture.Template
   ```

2. **Create a new project**:
   ```bash
   dotnet new codecraft -n "MyProject"
   cd MyProject
   ```

3. **Restore packages**:
   ```bash
   dotnet restore
   ```

4. **Verify the project builds** (with base functionality):
   ```bash
   dotnet build
   ```
   ? **Success!** Your project compiles with basic functionality.

5. **Run the code generator** to unlock full functionality:
   ```bash
   dotnet run --project MyProject.Generator
   ```
   ?? **Magic happens!** All CRUD operations, repositories, and controllers are generated.

6. **Build again** to see all generated files:
   ```bash
   dotnet build
   ```

7. **Run your API**:
   ```bash
   dotnet run --project MyProject.WebAPI
   ```

## What Gets Generated

After running the generator, the following files will be created/updated:

- **Application Layer**: CQRS commands, queries, handlers, validators, specifications
- **Infrastructure Layer**: Repositories, Unit of Work, DbContext updates  
- **WebAPI Layer**: Controllers with CRUD endpoints
- **Database**: EF Core migrations

## File Replacement Strategy

| File Type | Base Template | After Generator |
|-----------|---------------|-----------------|
| `InfrastructureServiceRegistration.cs` | ? Basic DI setup | ?? Full repository registration |
| `ApplicationDbContext.cs` | ? Empty DbContext | ?? DbSets for all entities |
| `ApplicationDbContextFactory.cs` | ? Basic factory | ?? Complete design-time factory |
| Controllers | ? None | ?? Full CRUD endpoints |
| Repositories | ? None | ?? Complete implementations |
| CQRS Features | ? None | ?? Commands, queries, handlers |

## Template Workflow

```
?? Install Template ? ??? Create Project ? ? Base Files (Compiles) 
                                            ?
?? Add Entities ? ?? Run Generator ? ?? Full Functionality
```

## Important Notes

- **? Immediate compilation**: Template works out-of-the-box
- **?? File replacement**: Generated files replace base files completely
- **?? Iterative development**: Add entities ? run generator ? repeat
- **?? Cleanup options**: Use `clean` to remove generated files only
- **??? Complete reset**: Use `cleanAll` to start fresh

## Development Commands

```bash
# Generate CRUD for all entities
dotnet run --project MyProject.Generator

# Clean generated files (keep your entities)
dotnet run clean --project MyProject.Generator

# Complete reset (remove everything including example entities)
dotnet run cleanAll --project MyProject.Generator

# Force migration creation (if needed)
dotnet run --force-migration --project MyProject.Generator
```

## Troubleshooting

### Build Errors After Template Creation
1. Make sure you've run `dotnet restore`
2. Check that you have .NET 9 SDK installed
3. The base template should compile - if not, check your .NET installation

### No CRUD Operations Available
1. Run the generator: `dotnet run --project MyProject.Generator`
2. Make sure you have entities in the `Domain/Model/` folder
3. Check that entities inherit from `BaseDomainModel`

### Missing Functionality
1. Verify the generator completed successfully
2. Check for compilation errors after generation
3. If issues persist: `dotnet run cleanAll --project MyProject.Generator`

### CI/CD Issues
- The template is designed to compile immediately after installation
- No need to run the generator in CI/CD for basic compilation
- Generated files are not included in the template package

For more information, see the main README.md file.