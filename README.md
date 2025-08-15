# CodeCraft.NET Clean Architecture Template

A Clean Architecture template for .NET 9 with CQRS and automatic code generation.

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

## Features

- **Clean Architecture** with proper layer separation
- **CQRS + MediatR** for scalable operations  
- **Entity Framework Core** with SQL Server
- **Automatic Code Generator** for CRUD operations
- **Swagger/OpenAPI** documentation

## Quick Start

1. **Create project**: `dotnet new codecraft -n "MyProject"`
2. **Build**: `dotnet build`
3. **Generate CRUD**: `dotnet run --project MyProject.Generator`
4. **Run API**: `dotnet run --project MyProject.WebAPI`
5. **Test**: Navigate to `https://localhost:7202/swagger`

## Code Generator

The generator analyzes entities in `Domain/Model/` and creates:
- CQRS commands and queries
- Controllers with CRUD endpoints
- Repository implementations
- EF Core DbContext and migrations

```bash
# Generate for all entities
dotnet run --project MyProject.Generator

# Clean generated files only
dotnet run clean --project MyProject.Generator

# Complete reset
dotnet run cleanAll --project MyProject.Generator
```

## Architecture

- **Domain**: Entities and business logic
- **Application**: CQRS, DTOs, validation
- **Infrastructure**: Data access, repositories
- **WebAPI**: Controllers, configuration
- **Generator**: Code generation engine

## License

MIT License
