# CodeCraft.NET Template Build Script with Semantic Versioning
param(
    [switch]$Major,
    [switch]$Minor,
    [switch]$Help,
    [switch]$PublishToNuGet = $false,
    [string]$ApiKey = "",
    [switch]$SkipTests = $false
)

if ($Help) {
    Write-Host "`nCodeCraft.NET Template Build Script" -ForegroundColor Cyan
    Write-Host "====================================" -ForegroundColor Cyan
    Write-Host "`nUsage:" -ForegroundColor Yellow
    Write-Host "  .\build-template.ps1                    # Increment patch version (1.0.0 -> 1.0.1)" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -Minor             # Increment minor version (1.0.5 -> 1.1.0)" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -Major             # Increment major version (1.2.3 -> 2.0.0)" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -PublishToNuGet -ApiKey YOUR_KEY  # Build and publish to NuGet" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -SkipTests         # Skip template testing" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -Help              # Show this help" -ForegroundColor White
    Write-Host "`nSemantic Versioning:" -ForegroundColor Yellow
    Write-Host "  MAJOR.MINOR.PATCH" -ForegroundColor White
    Write-Host "  - MAJOR: Breaking changes" -ForegroundColor Gray
    Write-Host "  - MINOR: New features (backward compatible)" -ForegroundColor Gray
    Write-Host "  - PATCH: Bug fixes (backward compatible)" -ForegroundColor Gray
    Write-Host "`nExamples:" -ForegroundColor Yellow
    Write-Host "  .\build-template.ps1 -Minor -PublishToNuGet -ApiKey abc123" -ForegroundColor Green
    Write-Host "  .\build-template.ps1 -Major -SkipTests" -ForegroundColor Green
    exit 0
}

Write-Host "🚀 Building CodeCraft.NET Template Package..." -ForegroundColor Cyan

# Variables - Automatically detect paths
$ScriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Definition
$ProjectFile = Join-Path $ScriptDirectory "Template.csproj"
$PackageDir = Join-Path $ScriptDirectory "nupkg"

# Validate parameters
if ($Major -and $Minor) {
    Write-Host "❌ Error: Cannot specify both -Major and -Minor flags" -ForegroundColor Red
    exit 1
}

if ($PublishToNuGet -and [string]::IsNullOrEmpty($ApiKey)) {
    Write-Host "❌ Error: API Key is required for publishing to NuGet!" -ForegroundColor Red
    Write-Host "Usage: .\build-template.ps1 -PublishToNuGet -ApiKey YOUR_API_KEY" -ForegroundColor Yellow
    exit 1
}

# Verify Template.csproj exists
if (-not (Test-Path $ProjectFile)) {
    Write-Host "❌ Error: Template.csproj not found at: $ProjectFile" -ForegroundColor Red
    exit 1
}

# Ensure project is restored to prevent NETSDK1004 errors
Write-Host "🔄 Ensuring project is restored..." -ForegroundColor Yellow
dotnet restore $ProjectFile --verbosity minimal --nologo
if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠️ Warning: Restore completed with warnings, continuing..." -ForegroundColor Yellow
}

# Clean previous builds
Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
Remove-Item -Path $PackageDir -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path (Join-Path $ScriptDirectory "bin") -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path (Join-Path $ScriptDirectory "obj") -Recurse -Force -ErrorAction SilentlyContinue

# Read current version from Template.csproj
function Get-ProjectVersion {
    [xml]$projectXml = Get-Content $ProjectFile
    
    # Look for PackageVersion in all PropertyGroup elements
    foreach ($propertyGroup in $projectXml.Project.PropertyGroup) {
        if ($propertyGroup.PackageVersion) {
            return $propertyGroup.PackageVersion
        }
    }
    
    Write-Host "⚠️ Warning: No PackageVersion found in $ProjectFile, using 1.0.0" -ForegroundColor Yellow
    return "1.0.0"
}

# Increment version based on semantic versioning
function Get-NextVersion {
    param(
        [string]$currentVersion,
        [string]$incrementType = "patch"
    )
    
    $versionParts = $currentVersion.Split('.')
    if ($versionParts.Length -lt 3) {
        Write-Host "❌ Invalid version format. Expected x.y.z, got: $currentVersion" -ForegroundColor Red
        exit 1
    }
    
    $major = [int]$versionParts[0]
    $minor = [int]$versionParts[1]
    $patch = [int]$versionParts[2]
    
    switch ($incrementType.ToLower()) {
        "major" {
            $major += 1
            $minor = 0
            $patch = 0
            Write-Host "🔴 Incrementing MAJOR version (breaking changes)" -ForegroundColor Red
        }
        "minor" {
            $minor += 1
            $patch = 0
            Write-Host "🟡 Incrementing MINOR version (new features)" -ForegroundColor Yellow
        }
        "patch" {
            $patch += 1
            Write-Host "🟢 Incrementing PATCH version (bug fixes)" -ForegroundColor Green
        }
        default {
            Write-Host "❌ Invalid increment type: $incrementType" -ForegroundColor Red
            exit 1
        }
    }
    
    return "$major.$minor.$patch"
}

# Update project file with new version
function Update-ProjectVersion {
    param([string]$newVersion)
    
    [xml]$projectXml = Get-Content $ProjectFile
    
    # Find and update PackageVersion in the correct PropertyGroup
    $updated = $false
    foreach ($propertyGroup in $projectXml.Project.PropertyGroup) {
        if ($propertyGroup.PackageVersion) {
            $propertyGroup.PackageVersion = $newVersion
            $updated = $true
            break
        }
    }
    
    # If PackageVersion wasn't found, add it to the first PropertyGroup
    if (-not $updated) {
        $firstPropertyGroup = $projectXml.Project.PropertyGroup[0]
        $packageVersionElement = $projectXml.CreateElement("PackageVersion")
        $packageVersionElement.InnerText = $newVersion
        $firstPropertyGroup.AppendChild($packageVersionElement) | Out-Null
    }
    
    # Save using absolute path
    $projectXml.Save($ProjectFile)
    Write-Host "✅ Updated $ProjectFile to version $newVersion" -ForegroundColor Green
}

# Determine increment type based on parameters
$incrementType = "patch"  # Default
if ($Major) {
    $incrementType = "major"
} elseif ($Minor) {
    $incrementType = "minor"
}

# Auto-increment version
$CurrentVersion = Get-ProjectVersion
Write-Host "📋 Current version: $CurrentVersion" -ForegroundColor Blue

$PackageVersion = Get-NextVersion $CurrentVersion $incrementType
Write-Host "🔄 New version: $PackageVersion" -ForegroundColor Cyan
Update-ProjectVersion $PackageVersion

# Create output directory
New-Item -ItemType Directory -Path $PackageDir -Force | Out-Null

# Note: Skipping solution build for template creation since we've already restored dependencies
Write-Host "ℹ️ Skipping solution build - creating template package directly..." -ForegroundColor Cyan

Write-Host "📦 Creating NuGet package with version $PackageVersion..." -ForegroundColor Yellow

try {
    # Build only the template package (project is already restored)
    dotnet pack $ProjectFile -o $PackageDir -p:PackageVersion=$PackageVersion --configuration Release --verbosity minimal --nologo --no-build
    
    if ($LASTEXITCODE -ne 0) {
        # Try without --no-build if the first attempt fails (includes implicit restore)
        Write-Host "🔄 Retrying package creation with build..." -ForegroundColor Yellow
        dotnet pack $ProjectFile -o $PackageDir -p:PackageVersion=$PackageVersion --configuration Release --verbosity minimal --nologo
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "❌ Pack failed!" -ForegroundColor Red
            exit 1
        }
    }
    
    Write-Host "✅ Package created successfully!" -ForegroundColor Green
    
    # Find the generated package
    $PackageFile = Get-ChildItem "$PackageDir\*.nupkg" | Select-Object -First 1
    
    if ($PackageFile) {
        Write-Host "`n📊 Package Information:" -ForegroundColor Blue
        Write-Host "   Generated: $($PackageFile.Name)" -ForegroundColor Blue
        Write-Host "   Location: $($PackageFile.FullName)" -ForegroundColor Blue
        Write-Host "   Size: $([math]::Round($PackageFile.Length / 1MB, 2)) MB" -ForegroundColor Blue
        Write-Host "   Version: $PackageVersion" -ForegroundColor Blue
        Write-Host "   Increment Type: $($incrementType.ToUpper())" -ForegroundColor Blue
        
        # Test the template if not skipped
        if (-not $SkipTests) {
            Write-Host "`n🧪 Testing template installation..." -ForegroundColor Yellow
            
            # Uninstall previous version
            dotnet new uninstall CodeCraft.NET.CleanArchitecture.Template 2>$null
            
            # Install from local package
            dotnet new install $PackageFile.FullName --force
            if ($LASTEXITCODE -ne 0) {
                Write-Host "❌ Template installation failed!" -ForegroundColor Red
                exit 1
            }
            
            # Test template creation
            Write-Host "🧪 Testing template creation..." -ForegroundColor Yellow
            $testDir = Join-Path $ScriptDirectory "test-output"
            Remove-Item -Path $testDir -Recurse -Force -ErrorAction SilentlyContinue
            New-Item -ItemType Directory -Path $testDir -Force | Out-Null
            
            Push-Location $testDir
            try {
                dotnet new codecraft -n "TestProject" --DatabaseProvider "SqlServer" --force
                if ($LASTEXITCODE -ne 0) {
                    Write-Host "❌ Template creation test failed!" -ForegroundColor Red
                    Pop-Location
                    exit 1
                }
                
                # Move to the generated project directory
                Push-Location "TestProject"
                try {
                    # Test build
                    Write-Host "🔨 Testing generated project build..." -ForegroundColor Yellow
                    
                    # Debug information
                    Write-Host "   Current directory: $(Get-Location)" -ForegroundColor Gray
                    Write-Host "   Directory contents:" -ForegroundColor Gray
                    Get-ChildItem | ForEach-Object { Write-Host "     $($_.Name)" -ForegroundColor Gray }
                    
                    # Find the solution file dynamically
                    $solutionFile = Get-ChildItem -Name -Filter "*.sln" | Select-Object -First 1
                    if (-not $solutionFile) {
                        Write-Host "❌ No solution file found in generated project!" -ForegroundColor Red
                        exit 1
                    }
                    
                    Write-Host "   Found solution: $solutionFile" -ForegroundColor Gray
                    dotnet build $solutionFile --verbosity minimal --nologo
                    if ($LASTEXITCODE -ne 0) {
                        Write-Host "❌ Generated project build failed!" -ForegroundColor Red
                        exit 1
                    }
                    
                    Write-Host "✅ Template testing completed successfully!" -ForegroundColor Green
                }
                finally {
                    Pop-Location  # Exit TestProject directory
                }
            }
            finally {
                Pop-Location
                Remove-Item -Path $testDir -Recurse -Force -ErrorAction SilentlyContinue
            }
        }
        
        # Publish to NuGet if requested
        if ($PublishToNuGet) {
            Write-Host "`n🚀 Publishing to NuGet..." -ForegroundColor Yellow
            dotnet nuget push $PackageFile.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✅ Successfully published to NuGet!" -ForegroundColor Green
                Write-Host "🌐 Package URL: https://www.nuget.org/packages/CodeCraft.NET.CleanArchitecture.Template/$PackageVersion" -ForegroundColor Cyan
            } else {
                Write-Host "❌ Failed to publish to NuGet!" -ForegroundColor Red
                exit 1
            }
        }
        
        Write-Host "`n🎉 CodeCraft.NET Template v$PackageVersion is ready!" -ForegroundColor Green
        Write-Host "`n📋 Next Steps:" -ForegroundColor Cyan
        
        if (-not $PublishToNuGet) {
            Write-Host "1. Test locally: dotnet new install `"$($PackageFile.FullName)`"" -ForegroundColor White
            Write-Host "2. Test template: dotnet new codecraft -n MyTestProject" -ForegroundColor White
            Write-Host "3. Uninstall: dotnet new uninstall CodeCraft.NET.CleanArchitecture.Template" -ForegroundColor White
            Write-Host "4. Publish: .\build-template.ps1 -PublishToNuGet -ApiKey YOUR_API_KEY" -ForegroundColor White
        } else {
            Write-Host "1. Install published version: dotnet new install CodeCraft.NET.CleanArchitecture.Template::$PackageVersion" -ForegroundColor White
            Write-Host "2. Create project: dotnet new codecraft -n MyProject" -ForegroundColor White
            Write-Host "3. Update GitHub repository and create release tag: git tag v$PackageVersion" -ForegroundColor White
            Write-Host "4. Share on social media and communities!" -ForegroundColor White
        }
    }
    
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host "`n✅ Build completed successfully!" -ForegroundColor Green