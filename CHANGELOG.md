# CodeCraft.NET Template - CHANGELOG

## [1.0.3] - 2024-01-XX

### Added
- Enhanced PowerShell build automation script
- Improved template packaging and testing
- Better semantic versioning support

### Improved
- Cleaner template structure and organization
- Enhanced documentation formatting
- Better error handling in code generation

### Fixed
- Template packaging issues
- Build script optimization

## [1.0.2] - 2024-01-XX

### Added
- Improved template build script with semantic versioning
- Enhanced PowerShell build automation
- Cleaner template structure
- Better documentation formatting

### Changed
- Updated project structure for better maintainability
- Improved code generation templates

### Fixed
- Template configuration issues
- Build process improvements

## [1.0.0] - 2024-01-XX

### Added
- Initial release of CodeCraft.NET Clean Architecture Template
- Clean Architecture foundation with proper layer separation
- CQRS + MediatR implementation for scalable operations
- Automatic code generation for entities, commands, queries, and controllers
- JWT Authentication with role-based authorization
- Entity Framework Core with SQL Server and PostgreSQL support
- Swagger/OpenAPI documentation integration
- Repository + Unit of Work patterns
- AutoMapper for object mapping
- FluentValidation for input validation
- Built-in auditing with CreatedBy, ModifiedBy tracking
- Pagination and filtering support
- Specification pattern implementation
- Custom extensibility through Custom/ folders

### Features
- **Entity Generation**: Automatically generates CRUD operations for domain entities
- **CQRS Pattern**: Command and Query separation with MediatR
- **Clean Architecture**: Proper dependency inversion and layer separation
- **Authentication**: JWT-based authentication with role support
- **Database Support**: SQL Server and PostgreSQL providers
- **Code Templates**: Scriban-based template engine for code generation
- **API Documentation**: Integrated Swagger UI for API testing
- **Validation**: FluentValidation integration for business rules
- **Mapping**: AutoMapper profiles for DTO transformations
- **Testing**: Built-in testing infrastructure

### Technical Stack
- .NET 9.0 target framework
- Entity Framework Core for data access
- MediatR for CQRS implementation
- AutoMapper for object mapping
- FluentValidation for validation
- Swagger/OpenAPI for documentation
- JWT for authentication
- Scriban for template rendering