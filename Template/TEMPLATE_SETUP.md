# Template Setup Instructions

## Initial Template State

This template includes base files that allow the project to compile immediately after installation. However, to get the full functionality with generated CRUD operations, you need to run the code generator.

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

4. **Verify the project builds**:
   ```bash
   dotnet build
   ```

5. **Run the code generator** to create CRUD operations for your entities:
   ```bash
   dotnet run --project MyProject.Generator
   ```

6. **Build again** to see all generated files:
   ```bash
   dotnet build
   ```

## What Gets Generated

After running the generator, the following files will be created/updated:

- **Application Layer**: CQRS commands, queries, handlers, validators, specifications
- **Infrastructure Layer**: Repositories, Unit of Work, DbContext updates
- **WebAPI Layer**: Controllers with CRUD endpoints
- **Database**: EF Core migrations

## Important Notes

- The template comes with base files that ensure compilation
- Generated files will **replace** the base files with full functionality
- Always run the generator after adding new entities to the Domain project
- Use `dotnet run clean --project MyProject.Generator` to clean generated files
- Use `dotnet run cleanAll --project MyProject.Generator` for complete reset

## Troubleshooting

If you encounter build errors:

1. Make sure you've run `dotnet restore`
2. Check that you have .NET 9 SDK installed
3. Run the generator: `dotnet run --project MyProject.Generator`
4. If issues persist, clean and regenerate: `dotnet run cleanAll --project MyProject.Generator`

For more information, see the main README.md file.