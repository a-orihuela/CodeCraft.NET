@echo off
echo ?? Inicializando proyecto CodeCraft.NET...

REM Obtener el nombre del proyecto del directorio actual
for %%i in ("%CD%") do set PROJECT_NAME=%%~ni
echo ?? Proyecto: %PROJECT_NAME%

REM Renombrar archivos de soluci�n
if exist "CodeCraft.NET.sln" (
    ren "CodeCraft.NET.sln" "%PROJECT_NAME%.sln"
    echo ? Soluci�n renombrada a %PROJECT_NAME%.sln
)

REM Restaurar paquetes NuGet
echo ?? Restaurando paquetes NuGet...
dotnet restore

REM Aplicar migraciones si existe la base de datos
echo ??? Configurando base de datos...
dotnet ef migrations list --project "%PROJECT_NAME%.Infrastructure" --startup-project "%PROJECT_NAME%.WebAPI" >nul 2>&1
if %errorlevel% equ 0 (
    dotnet ef database update --project "%PROJECT_NAME%.Infrastructure" --startup-project "%PROJECT_NAME%.WebAPI"
    echo ? Base de datos actualizada
) else (
    echo ?? No se encontraron migraciones. Ejecute el generador primero.
)

REM Compilar la soluci�n
echo ?? Compilando soluci�n...
dotnet build

echo ? �Proyecto inicializado correctamente!
echo.
echo ?? Pr�ximos pasos:
echo   1. Ejecutar: dotnet run --project %PROJECT_NAME%.WebAPI
echo   2. Visitar: https://localhost:7202/swagger
echo   3. Generar c�digo: dotnet run --project %PROJECT_NAME%.Generator

pause