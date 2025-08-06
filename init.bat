@echo off
echo Initializing CodeCraft.NET project...

REM Get project name from current directory
for %%i in ("%CD%") do set PROJECT_NAME=%%~ni
echo Project: %PROJECT_NAME%

REM Rename solution files
if exist "CodeCraft.NET.sln" (
    ren "CodeCraft.NET.sln" "%PROJECT_NAME%.sln"
    echo Solution renamed to %PROJECT_NAME%.sln
)

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore

REM Apply migrations if database exists
echo Setting up database...
dotnet ef migrations list --project "%PROJECT_NAME%.Infrastructure" --startup-project "%PROJECT_NAME%.WebAPI" >nul 2>&1
if %errorlevel% equ 0 (
    dotnet ef database update --project "%PROJECT_NAME%.Infrastructure" --startup-project "%PROJECT_NAME%.WebAPI"
    echo Database updated
) else (
    echo No migrations found. Run the generator first.
)

REM Build the solution
echo Building solution...
dotnet build

echo Project initialized successfully!
echo.
echo Next steps:
echo   1. Run: dotnet run --project %PROJECT_NAME%.WebAPI
echo   2. Visit: https://localhost:7202/swagger
echo   3. Generate code: dotnet run --project %PROJECT_NAME%.Generator

pause