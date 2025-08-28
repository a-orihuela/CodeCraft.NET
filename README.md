# CodeCraft.NET Clean Architecture

## Features

- **Clean Architecture** with proper layer separation
- **CQRS + MediatR** for scalable operations  
- **Entity Framework Core** with SQL Server
- **Automatic Code Generator** for CRUD operations
- **Swagger/OpenAPI** documentation

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
