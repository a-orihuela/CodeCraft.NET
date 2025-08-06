@echo off
setlocal EnableDelayedExpansion

echo Building CodeCraft.NET Template Package...

REM Variables
set PACKAGE_VERSION=1.0.0
set PACKAGE_DIR=.\nupkg
set NUGET_SOURCE=https://api.nuget.org/v3/index.json

echo Template Package Build Process
echo ==================================

REM Clean previous packages
echo Cleaning previous packages...
if exist "%PACKAGE_DIR%" rmdir /s /q "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%"

REM Validate required files
echo Validating required files...

if not exist ".template.config\template.json" (
    echo Missing .template.config\template.json
    exit /b 1
)

if not exist "template.csproj" (
    echo Missing template.csproj
    exit /b 1
)

if not exist "README.md" (
    echo Missing README.md
    exit /b 1
)

echo All required files found

REM Build the package
echo Building NuGet package...
dotnet pack template.csproj -o "%PACKAGE_DIR%" -p:PackageVersion=%PACKAGE_VERSION% --nologo

if %errorlevel% equ 0 (
    echo Package built successfully!
    
    REM Find generated package
    for %%f in ("%PACKAGE_DIR%\*.nupkg") do set PACKAGE_FILE=%%f
    
    echo Generated package: !PACKAGE_FILE!
    echo Package Information:
    echo    Location: !PACKAGE_FILE!
    echo    Version: %PACKAGE_VERSION%
    
) else (
    echo Package build failed!
    exit /b 1
)

REM Ask for publication
echo.
set /p publish="Do you want to publish to NuGet.org? (y/N): "

if /i "!publish!"=="y" (
    set /p NUGET_API_KEY="Please enter your NuGet API key: "
    
    if "!NUGET_API_KEY!"=="" (
        echo API key is required for publishing
        exit /b 1
    )
    
    echo Publishing to NuGet.org...
    dotnet nuget push "!PACKAGE_FILE!" --api-key !NUGET_API_KEY! --source %NUGET_SOURCE% --no-symbols
    
    if !errorlevel! equ 0 (
        echo Successfully published to NuGet.org!
        echo Your template can now be installed with:
        echo    dotnet new install CodeCraft.NET.CleanArchitecture.Template
    ) else (
        echo Publication failed!
        exit /b 1
    )
) else (
    echo Package ready for manual publication:
    echo    1. Go to https://www.nuget.org/packages/manage/upload
    echo    2. Upload: !PACKAGE_FILE!
    echo    3. Or use: dotnet nuget push "!PACKAGE_FILE!" --api-key YOUR_API_KEY --source %NUGET_SOURCE%
)

echo.
echo Build process completed!
pause