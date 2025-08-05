#!/bin/bash

echo "?? Inicializando proyecto CodeCraft.NET..."

# Obtener el nombre del proyecto del directorio actual
PROJECT_NAME=$(basename "$PWD")
echo "?? Proyecto: $PROJECT_NAME"

# Renombrar archivos de solución
if [ -f "CodeCraft.NET.sln" ]; then
    mv "CodeCraft.NET.sln" "$PROJECT_NAME.sln"
    echo "? Solución renombrada a $PROJECT_NAME.sln"
fi

# Restaurar paquetes NuGet
echo "?? Restaurando paquetes NuGet..."
dotnet restore

# Aplicar migraciones si existe la base de datos
echo "??? Configurando base de datos..."
if dotnet ef migrations list --project "$PROJECT_NAME.Infrastructure" --startup-project "$PROJECT_NAME.WebAPI" > /dev/null 2>&1; then
    dotnet ef database update --project "$PROJECT_NAME.Infrastructure" --startup-project "$PROJECT_NAME.WebAPI"
    echo "? Base de datos actualizada"
else
    echo "?? No se encontraron migraciones. Ejecute el generador primero."
fi

# Compilar la solución
echo "?? Compilando solución..."
dotnet build

echo "? ¡Proyecto inicializado correctamente!"
echo ""
echo "?? Próximos pasos:"
echo "  1. Ejecutar: dotnet run --project $PROJECT_NAME.WebAPI"
echo "  2. Visitar: https://localhost:7202/swagger"
echo "  3. Generar código: dotnet run --project $PROJECT_NAME.Generator"