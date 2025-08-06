# CodeCraft.NET Template Build Script with Semantic Versioning
param(
    [switch]$Major,
    [switch]$Minor,
    [switch]$Help
)

if ($Help) {
    Write-Host "`nCodeCraft.NET Template Build Script" -ForegroundColor Cyan
    Write-Host "====================================" -ForegroundColor Cyan
    Write-Host "`nUsage:" -ForegroundColor Yellow
    Write-Host "  .\build-template.ps1              # Increment patch version (1.0.0 -> 1.0.1)" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -Minor       # Increment minor version (1.0.5 -> 1.1.0)" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -Major       # Increment major version (1.2.3 -> 2.0.0)" -ForegroundColor White
    Write-Host "  .\build-template.ps1 -Help        # Show this help" -ForegroundColor White
    Write-Host "`nSemantic Versioning:" -ForegroundColor Yellow
    Write-Host "  MAJOR.MINOR.PATCH" -ForegroundColor White
    Write-Host "  - MAJOR: Breaking changes" -ForegroundColor Gray
    Write-Host "  - MINOR: New features (backward compatible)" -ForegroundColor Gray
    Write-Host "  - PATCH: Bug fixes (backward compatible)" -ForegroundColor Gray
    exit 0
}

Write-Host "Building CodeCraft.NET Template Package..." -ForegroundColor Cyan

# Variables
$ProjectFile = "template.csproj"
$PackageDir = ".\nupkg"

# Validate parameters
if ($Major -and $Minor) {
    Write-Host "Error: Cannot specify both -Major and -Minor flags" -ForegroundColor Red
    exit 1
}

# Read current version from template.csproj
function Get-ProjectVersion {
    [xml]$projectXml = Get-Content $ProjectFile
    
    # Look for PackageVersion in all PropertyGroup elements
    foreach ($propertyGroup in $projectXml.Project.PropertyGroup) {
        if ($propertyGroup.PackageVersion) {
            return $propertyGroup.PackageVersion
        }
    }
    
    Write-Host "Warning: No PackageVersion found in $ProjectFile, using 1.0.0" -ForegroundColor Yellow
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
        Write-Host "Invalid version format. Expected x.y.z, got: $currentVersion" -ForegroundColor Red
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
            Write-Host "Incrementing MAJOR version (breaking changes)" -ForegroundColor Red
        }
        "minor" {
            $minor += 1
            $patch = 0
            Write-Host "Incrementing MINOR version (new features)" -ForegroundColor Yellow
        }
        "patch" {
            $patch += 1
            Write-Host "Incrementing PATCH version (bug fixes)" -ForegroundColor Green
        }
        default {
            Write-Host "Invalid increment type: $incrementType" -ForegroundColor Red
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
    
    $projectXml.Save($ProjectFile)
    Write-Host "Updated $ProjectFile to version $newVersion" -ForegroundColor Green
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
Write-Host "Current version: $CurrentVersion" -ForegroundColor Blue

$PackageVersion = Get-NextVersion $CurrentVersion $incrementType
Write-Host "New version: $PackageVersion" -ForegroundColor Cyan
Update-ProjectVersion $PackageVersion

# Create output directory
if (Test-Path $PackageDir) {
    Remove-Item $PackageDir -Recurse -Force
}
New-Item -ItemType Directory -Path $PackageDir -Force | Out-Null

Write-Host "`nBuilding package with version $PackageVersion..." -ForegroundColor Yellow

try {
    # Build the package with the auto-incremented version
    dotnet pack $ProjectFile -o $PackageDir -p:PackageVersion=$PackageVersion --verbosity minimal --nologo
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Package built successfully!" -ForegroundColor Green
        
        # Find the generated package
        $PackageFile = Get-ChildItem "$PackageDir\*.nupkg" | Select-Object -First 1
        
        if ($PackageFile) {
            Write-Host "`nPackage Information:" -ForegroundColor Blue
            Write-Host "Generated: $($PackageFile.Name)" -ForegroundColor Blue
            Write-Host "Location: $($PackageFile.FullName)" -ForegroundColor Blue
            Write-Host "Size: $([math]::Round($PackageFile.Length / 1MB, 2)) MB" -ForegroundColor Blue
            Write-Host "Version: $PackageVersion" -ForegroundColor Blue
            Write-Host "Increment Type: $($incrementType.ToUpper())" -ForegroundColor Blue
            
            Write-Host "`nNext Steps:" -ForegroundColor Cyan
            Write-Host "1. Test locally: dotnet new install `"$($PackageFile.FullName)`"" -ForegroundColor White
            Write-Host "2. Test template: dotnet new codecraft -n TestProject" -ForegroundColor White
            Write-Host "3. Uninstall: dotnet new uninstall CodeCraft.NET.CleanArchitecture.Template" -ForegroundColor White
            Write-Host "4. Publish: dotnet nuget push `"$($PackageFile.FullName)`" --api-key YOUR_KEY --source https://api.nuget.org/v3/index.json" -ForegroundColor White
        }
    } else {
        Write-Host "Package build failed!" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host "`nBuild completed!" -ForegroundColor Green