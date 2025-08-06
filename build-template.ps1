# CodeCraft.NET Template Build Script
Write-Host "Building CodeCraft.NET Template Package..." -ForegroundColor Cyan

# Variables
$PackageVersion = "1.0.1"
$PackageDir = ".\nupkg"
$ProjectFile = "template.csproj"

# Create output directory
if (Test-Path $PackageDir) {
    Remove-Item $PackageDir -Recurse -Force
}
New-Item -ItemType Directory -Path $PackageDir -Force | Out-Null

Write-Host "Building package..." -ForegroundColor Yellow

try {
    # Build the package with minimal verbosity
    dotnet pack $ProjectFile -o $PackageDir -p:PackageVersion=$PackageVersion --verbosity minimal --nologo
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Package built successfully!" -ForegroundColor Green
        
        # Find the generated package
        $PackageFile = Get-ChildItem "$PackageDir\*.nupkg" | Select-Object -First 1
        
        if ($PackageFile) {
            Write-Host "Generated: $($PackageFile.Name)" -ForegroundColor Blue
            Write-Host "Location: $($PackageFile.FullName)" -ForegroundColor Blue
            Write-Host "Size: $([math]::Round($PackageFile.Length / 1MB, 2)) MB" -ForegroundColor Blue
            
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