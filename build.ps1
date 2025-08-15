# CodeCraft.NET Build Script with Template Packaging and Semantic Versioning
param(
    [switch]$Major,
    [switch]$Minor,
    [switch]$Help,
    [switch]$PublishToNuGet = $false,
    [string]$ApiKey = ""
)

if ($Help) {
    Write-Host "`nCodeCraft.NET Build Script" -ForegroundColor Cyan
    Write-Host "===========================" -ForegroundColor Cyan
    Write-Host "`nUsage:" -ForegroundColor Yellow
    Write-Host "  .\build.ps1                              # Build solution + create template package (patch version)" -ForegroundColor White
    Write-Host "  .\build.ps1 -Minor                       # Increment minor version (1.0.5 -> 1.1.0)" -ForegroundColor White
    Write-Host "  .\build.ps1 -Major                       # Increment major version (1.2.3 -> 2.0.0)" -ForegroundColor White
    Write-Host "  .\build.ps1 -PublishToNuGet -ApiKey KEY  # Build and publish to NuGet" -ForegroundColor White
    Write-Host "`nSemantic Versioning:" -ForegroundColor Yellow
    Write-Host "  MAJOR.MINOR.PATCH" -ForegroundColor White
    Write-Host "  - MAJOR: Breaking changes" -ForegroundColor Gray
    Write-Host "  - MINOR: New features (backward compatible)" -ForegroundColor Gray
    Write-Host "  - PATCH: Bug fixes (backward compatible)" -ForegroundColor Gray
    Write-Host "`nExamples:" -ForegroundColor Yellow
    Write-Host "  .\build.ps1 -Minor" -ForegroundColor Green
    Write-Host "  .\build.ps1 -PublishToNuGet -ApiKey abc123" -ForegroundColor Green
    exit 0
}

Write-Host "?? CodeCraft.NET Build Script" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan

# Variables
$ScriptDirectory = $PSScriptRoot
$SolutionFile = "CodeCraft.NET.sln"
$TemplateProjectFile = "Template\Template.csproj"
$PackageDir = "artifacts"

# Validate parameters
if ($Major -and $Minor) {
    Write-Host "? Error: Cannot specify both -Major and -Minor flags" -ForegroundColor Red
    exit 1
}

if ($PublishToNuGet -and [string]::IsNullOrEmpty($ApiKey)) {
    Write-Host "? Error: API Key is required for publishing to NuGet!" -ForegroundColor Red
    Write-Host "Usage: .\build.ps1 -PublishToNuGet -ApiKey YOUR_API_KEY" -ForegroundColor Yellow
    exit 1
}

# Verify files exist
if (-not (Test-Path $SolutionFile)) {
    Write-Host "? Error: Solution file not found: $SolutionFile" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $TemplateProjectFile)) {
    Write-Host "? Error: Template project not found: $TemplateProjectFile" -ForegroundColor Red
    exit 1
}

# Clean previous builds
Write-Host "?? Cleaning previous builds..." -ForegroundColor Yellow
Remove-Item -Path $PackageDir -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $PackageDir -Force | Out-Null

# Step 1: Build Solution
Write-Host "`n?? Building CodeCraft.NET Solution..." -ForegroundColor Yellow

$projects = @(
    "CodeCraft.NET.Cross/CodeCraft.NET.Cross.csproj",
    "CodeCraft.NET.Domain/CodeCraft.NET.Domain.csproj", 
    "CodeCraft.NET.Application/CodeCraft.NET.Application.csproj",
    "CodeCraft.NET.Infrastructure/CodeCraft.NET.Infrastructure.csproj",
    "CodeCraft.NET.WebAPI/CodeCraft.NET.WebAPI.csproj",
    "CodeCraft.NET.Generator/CodeCraft.NET.Generator.csproj"
)

foreach ($project in $projects) {
    Write-Host "  Building $project..." -ForegroundColor Gray
    dotnet build $project --configuration Release --verbosity quiet --nologo
    if ($LASTEXITCODE -ne 0) {
        Write-Host "? Build failed for $project" -ForegroundColor Red
        exit 1
    }
}

Write-Host "? Solution built successfully!" -ForegroundColor Green

# Step 2: Template Versioning and Packaging
Write-Host "`n?? Creating Template Package..." -ForegroundColor Yellow

# Read current version from Template.csproj
function Get-ProjectVersion {
    [xml]$projectXml = Get-Content $TemplateProjectFile
    
    foreach ($propertyGroup in $projectXml.Project.PropertyGroup) {
        if ($propertyGroup.PackageVersion) {
            return $propertyGroup.PackageVersion
        }
    }
    
    Write-Host "?? Warning: No PackageVersion found, using 1.0.0" -ForegroundColor Yellow
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
        Write-Host "? Invalid version format. Expected x.y.z, got: $currentVersion" -ForegroundColor Red
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
            Write-Host "?? Incrementing MAJOR version (breaking changes)" -ForegroundColor Red
        }
        "minor" {
            $minor += 1
            $patch = 0
            Write-Host "?? Incrementing MINOR version (new features)" -ForegroundColor Yellow
        }
        "patch" {
            $patch += 1
            Write-Host "?? Incrementing PATCH version (bug fixes)" -ForegroundColor Green
        }
    }
    
    return "$major.$minor.$patch"
}

# Update project file with new version
function Update-ProjectVersion {
    param([string]$newVersion)
    
    [xml]$projectXml = Get-Content $TemplateProjectFile
    
    $updated = $false
    foreach ($propertyGroup in $projectXml.Project.PropertyGroup) {
        if ($propertyGroup.PackageVersion) {
            $propertyGroup.PackageVersion = $newVersion
            $updated = $true
            break
        }
    }
    
    if (-not $updated) {
        $firstPropertyGroup = $projectXml.Project.PropertyGroup[0]
        $packageVersionElement = $projectXml.CreateElement("PackageVersion")
        $packageVersionElement.InnerText = $newVersion
        $firstPropertyGroup.AppendChild($packageVersionElement) | Out-Null
    }
    
    $projectXml.Save((Resolve-Path $TemplateProjectFile))
    Write-Host "? Updated template version to $newVersion" -ForegroundColor Green
}

# Determine increment type
$incrementType = "patch"
if ($Major) { $incrementType = "major" }
elseif ($Minor) { $incrementType = "minor" }

# Version management
$CurrentVersion = Get-ProjectVersion
Write-Host "?? Current template version: $CurrentVersion" -ForegroundColor Blue

$PackageVersion = Get-NextVersion $CurrentVersion $incrementType
Write-Host "?? New template version: $PackageVersion" -ForegroundColor Cyan
Update-ProjectVersion $PackageVersion

# Create template package
Write-Host "?? Building template package..." -ForegroundColor Yellow
dotnet restore $TemplateProjectFile --verbosity minimal --nologo
dotnet pack $TemplateProjectFile -o $PackageDir --configuration Release --verbosity minimal --nologo

if ($LASTEXITCODE -ne 0) {
    Write-Host "? Template packaging failed!" -ForegroundColor Red
    exit 1
}

Write-Host "? Template package created successfully!" -ForegroundColor Green

# Find the generated package
$PackageFile = Get-ChildItem "$PackageDir\*.nupkg" | Select-Object -First 1

if ($PackageFile) {
    Write-Host "`n?? Package Information:" -ForegroundColor Blue
    Write-Host "   Name: $($PackageFile.Name)" -ForegroundColor Blue
    Write-Host "   Location: $($PackageFile.FullName)" -ForegroundColor Blue
    Write-Host "   Size: $([math]::Round($PackageFile.Length / 1MB, 2)) MB" -ForegroundColor Blue
    Write-Host "   Version: $PackageVersion" -ForegroundColor Blue
    Write-Host "   Type: $($incrementType.ToUpper())" -ForegroundColor Blue

    # Test the template
    Write-Host "`n?? Testing template..." -ForegroundColor Yellow
    
    # Uninstall previous version
    dotnet new uninstall CodeCraft.NET.CleanArchitecture.Template 2>$null
    
    # Install from local package
    dotnet new install $PackageFile.FullName --force
    if ($LASTEXITCODE -ne 0) {
        Write-Host "? Template installation failed!" -ForegroundColor Red
        exit 1
    }
    
    # Test template creation
    $testDir = "test-output"
    Remove-Item -Path $testDir -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Path $testDir -Force | Out-Null
    
    Push-Location $testDir
    try {
        dotnet new codecraft -n "TestProject" --force
        if ($LASTEXITCODE -ne 0) {
            Write-Host "? Template creation test failed!" -ForegroundColor Red
            Pop-Location
            exit 1
        }
        
        Push-Location "TestProject"
        try {
            $solutionFile = Get-ChildItem -Name -Filter "*.sln" | Select-Object -First 1
            if ($solutionFile) {
                dotnet build $solutionFile --verbosity minimal --nologo
                if ($LASTEXITCODE -ne 0) {
                    Write-Host "? Generated project build failed!" -ForegroundColor Red
                    exit 1
                }
                Write-Host "? Template test completed successfully!" -ForegroundColor Green
            }
        }
        finally {
            Pop-Location
        }
    }
    finally {
        Pop-Location
        Remove-Item -Path $testDir -Recurse -Force -ErrorAction SilentlyContinue
    }

    # Publish to NuGet if requested
    if ($PublishToNuGet) {
        Write-Host "`n?? Publishing to NuGet..." -ForegroundColor Yellow
        dotnet nuget push $PackageFile.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? Successfully published to NuGet!" -ForegroundColor Green
            Write-Host "?? Package URL: https://www.nuget.org/packages/CodeCraft.NET.CleanArchitecture.Template/$PackageVersion" -ForegroundColor Cyan
        } else {
            Write-Host "? Failed to publish to NuGet!" -ForegroundColor Red
            exit 1
        }
    }

    Write-Host "`n?? Build completed successfully!" -ForegroundColor Green
    Write-Host "`n?? Next Steps:" -ForegroundColor Cyan
    
    if (-not $PublishToNuGet) {
        Write-Host "1. Test locally: dotnet new install `"$($PackageFile.FullName)`"" -ForegroundColor White
        Write-Host "2. Create project: dotnet new codecraft -n MyProject" -ForegroundColor White
        Write-Host "3. Publish: .\build.ps1 -PublishToNuGet -ApiKey YOUR_KEY" -ForegroundColor White
    } else {
        Write-Host "1. Install: dotnet new install CodeCraft.NET.CleanArchitecture.Template::$PackageVersion" -ForegroundColor White
        Write-Host "2. Create project: dotnet new codecraft -n MyProject" -ForegroundColor White
        Write-Host "3. Tag release: git tag v$PackageVersion" -ForegroundColor White
    }
} else {
    Write-Host "? No package file found!" -ForegroundColor Red
    exit 1
}