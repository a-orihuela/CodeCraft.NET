# CodeCraft.NET Template

This directory contains the template packaging configuration for the CodeCraft.NET Clean Architecture Template.

## ?? Quick Start

### Build and Test Template

```bash
# Navigate to Template directory
cd Template

# Build and test template (patch version increment)
.\build-template.ps1

# Build with version increments
.\build-template.ps1 -Minor      # New features
.\build-template.ps1 -Major      # Breaking changes

# Quick build without tests
.\build-template.ps1 -SkipTests
```

### Publish to NuGet

```bash
.\build-template.ps1 -PublishToNuGet -ApiKey YOUR_NUGET_API_KEY
```

## ?? Directory Structure

```
Template/
??? .template.config/
?   ??? template.json           # Template configuration
??? Template.csproj             # Template package project
??? build-template.ps1          # Build and publish script
??? README.md                   # This file
```

## ?? Template Configuration

The template configuration is defined in `.template.config/template.json`:

- **Identity**: `CodeCraft.NET.CleanArchitecture.Template`
- **Short Name**: `codecraft`
- **Parameters**:
  - `ProjectName`: The name of the generated project
  - `DatabaseProvider`: SqlServer or PostgreSQL

## ?? Package Configuration

The `Template.csproj` file defines:

- Package metadata (ID, version, description, etc.)
- Content inclusion rules for the template
- Build configuration for template packaging

## ??? Build Script Features

The `build-template.ps1` script provides:

- **Semantic Versioning**: Automatic version increment
- **Testing**: Template installation and project generation tests
- **Publishing**: Direct publishing to NuGet
- **Validation**: Build and compilation checks

### Script Parameters

| Parameter | Description | Example |
|-----------|-------------|---------|
| `-Major` | Increment major version (breaking changes) | `.\build-template.ps1 -Major` |
| `-Minor` | Increment minor version (new features) | `.\build-template.ps1 -Minor` |
| `-SkipTests` | Skip template testing | `.\build-template.ps1 -SkipTests` |
| `-PublishToNuGet` | Publish to NuGet | `.\build-template.ps1 -PublishToNuGet -ApiKey KEY` |
| `-ApiKey` | NuGet API key for publishing | Required with `-PublishToNuGet` |
| `-Help` | Show help information | `.\build-template.ps1 -Help` |

## ?? Development Workflow

1. **Make changes** to the template source code in the parent directories
2. **Navigate** to the Template directory: `cd Template`
3. **Build and test**: `.\build-template.ps1`
4. **Verify** the generated package works correctly
5. **Publish** when ready: `.\build-template.ps1 -PublishToNuGet -ApiKey YOUR_KEY`

## ?? Notes

- The template includes all source files from the parent directories
- Exclusions are configured to avoid packaging build artifacts and the Template directory itself
- The build script automatically handles solution building and package generation
- Testing includes template installation, project creation, and compilation verification

---

For more information, see the main project [README](../README.md) and [CONTRIBUTING](../CONTRIBUTING.md) guide.