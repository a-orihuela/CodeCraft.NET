# Contributing to CodeCraft.NET

Thank you for your interest in contributing to CodeCraft.NET! This document provides guidelines and information for contributors.

## ?? Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022 or VS Code
- Git

### Setup Development Environment

1. **Fork the repository**
   ```bash
   git clone https://github.com/a-orihuela/CodeCraft.NET.git
   cd CodeCraft.NET
   ```

2. **Install the template locally**
   ```bash
   dotnet new install ./Template
   ```

3. **Test the template**
   ```bash
   dotnet new codecraft -n TestProject
   cd TestProject
   dotnet build
   ```

## ?? Development Workflow

### Making Changes

1. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes**
   - Follow the existing code style and patterns
   - Update templates in the appropriate directories
   - Test your changes thoroughly

3. **Test the template**
   ```bash
   # Navigate to Template directory
   cd Template
   # Use the build script to test
   .\build-template.ps1 -SkipTests  # Quick build without testing
   .\build-template.ps1             # Full build with testing
   ```

4. **Commit your changes**
   ```bash
   git add .
   git commit -m "feat: add your feature description"
   ```

### Template Structure

```
CodeCraft.NET/
??? .template.config/          # Template configuration (deprecated - use Template folder)
??? CodeCraft.NET.Application/ # Application layer
??? CodeCraft.NET.Domain/      # Domain layer
??? CodeCraft.NET.Infrastructure/ # Infrastructure layer
??? CodeCraft.NET.WebAPI/      # Web API layer
??? CodeCraft.NET.Cross/       # Cross-cutting concerns
??? CodeCraft.NET.Generator/   # Code generation engine
??? Template/                  # Template package
    ??? .template.config/      # Template configuration
    ??? Template.csproj        # Template package configuration
    ??? build-template.ps1     # Template build script
```

## ?? Code Style Guidelines

### C# Code Style
- Use C# 12 features and modern syntax
- Follow Microsoft's C# coding conventions
- Use nullable reference types
- Prefer `var` when type is obvious
- Use meaningful variable and method names

### Template Guidelines
- Use Scriban template syntax for code generation
- Keep templates simple and readable
- Add comments for complex template logic
- Follow the existing template structure

### Documentation
- Update README.md for new features
- Add XML documentation for public APIs
- Include examples in documentation
- Update CHANGELOG.md

## ?? Testing

### Testing Your Changes

1. **Build and test locally**
   ```bash
   # Build solution and create template package (patch version)
   .\build.ps1
   
   # Minor version increment (new features)
   .\build.ps1 -Minor
   
   # Major version increment (breaking changes)
   .\build.ps1 -Major
   
   # Publish to NuGet
   .\build.ps1 -PublishToNuGet -ApiKey YOUR_API_KEY
   ```