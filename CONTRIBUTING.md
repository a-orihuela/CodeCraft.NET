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
   dotnet new install .
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
??? .template.config/          # Template configuration
??? CodeCraft.NET.Application/ # Application layer
??? CodeCraft.NET.Domain/      # Domain layer
??? CodeCraft.NET.Infrastructure/ # Infrastructure layer
??? CodeCraft.NET.WebAPI/      # Web API layer
??? CodeCraft.NET.Cross/       # Cross-cutting concerns
??? CodeCraft.NET.Generator/   # Code generation engine
??? template.csproj           # Template package configuration
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
   # Patch version increment with testing
   .\build-template.ps1
   
   # Minor version increment (new features)
   .\build-template.ps1 -Minor
   
   # Major version increment (breaking changes)
   .\build-template.ps1 -Major
   
   # Quick build without tests
   .\build-template.ps1 -SkipTests
   ```

2. **Create a test project**
   ```bash
   dotnet new codecraft -n ContributionTest
   cd ContributionTest
   dotnet build
   dotnet run --project ContributionTest.WebAPI
   ```

3. **Test code generation**
   ```bash
   # Add a test entity and run generator
   dotnet run --project ContributionTest.Generator
   ```

4. **Verify all features work**
   - Authentication endpoints
   - CRUD operations
   - Swagger documentation
   - Database migrations

## ?? Pull Request Process

### Before Submitting

- [ ] Code builds without errors
- [ ] All tests pass (`.\build-template.ps1` completes successfully)
- [ ] Template generates working projects
- [ ] Documentation is updated
- [ ] CHANGELOG.md is updated

### Pull Request Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix (patch version)
- [ ] New feature (minor version)
- [ ] Breaking change (major version)
- [ ] Documentation update

## Testing
- [ ] Template builds successfully (`.\build-template.ps1`)
- [ ] Generated project compiles
- [ ] All features work as expected
- [ ] Documentation updated

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Changes are documented
- [ ] Tests added/updated
```

## ?? Reporting Bugs

### Bug Report Template

```markdown
**Describe the bug**
A clear description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:
1. Install template with '...'
2. Create project with '...'
3. Run '...'
4. See error

**Expected behavior**
What you expected to happen.

**Environment:**
- OS: [e.g. Windows 11]
- .NET Version: [e.g. 9.0]
- Template Version: [e.g. 1.0.3]

**Additional context**
Any other context about the problem.
```

## ?? Feature Requests

### Feature Request Template

```markdown
**Is your feature request related to a problem?**
A clear description of what the problem is.

**Describe the solution you'd like**
A clear description of what you want to happen.

**Describe alternatives you've considered**
Any alternative solutions or features you've considered.

**Additional context**
Any other context or screenshots about the feature request.
```

## ?? Recognition

Contributors will be:
- Added to the Contributors section in README.md
- Mentioned in release notes
- Given credit in the NuGet package description

## ?? Getting Help

- **Discussions**: Use GitHub Discussions for questions
- **Issues**: Use GitHub Issues for bugs and feature requests
- **Email**: Contact maintainers directly for private matters

## ?? Code of Conduct

### Our Pledge

We pledge to make participation in our project a harassment-free experience for everyone, regardless of age, body size, disability, ethnicity, gender identity and expression, level of experience, nationality, personal appearance, race, religion, or sexual identity and orientation.

### Our Standards

Examples of behavior that contributes to creating a positive environment include:
- Using welcoming and inclusive language
- Being respectful of differing viewpoints and experiences
- Gracefully accepting constructive criticism
- Focusing on what is best for the community
- Showing empathy towards other community members

### Enforcement

Instances of abusive, harassing, or otherwise unacceptable behavior may be reported by contacting the project team. All complaints will be reviewed and investigated promptly and fairly.

## ?? Thank You

Thank you for contributing to CodeCraft.NET! Your contributions help make this template better for developers worldwide.

---

**Happy Coding!** ??