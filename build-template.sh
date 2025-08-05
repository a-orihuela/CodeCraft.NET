#!/bin/bash

echo "?? Creando package del template CodeCraft.NET..."

# Crear directorio para el package
PACKAGE_DIR="./template-package"
rm -rf $PACKAGE_DIR
mkdir -p $PACKAGE_DIR

# Copiar archivos del template
echo "?? Copiando archivos del template..."
cp -r . $PACKAGE_DIR/

# Limpiar archivos innecesarios del package
echo "?? Limpiando archivos innecesarios..."
cd $PACKAGE_DIR

# Eliminar directorios de build y temporales
find . -name "bin" -type d -exec rm -rf {} + 2>/dev/null
find . -name "obj" -type d -exec rm -rf {} + 2>/dev/null
find . -name ".vs" -type d -exec rm -rf {} + 2>/dev/null
find . -name "*.user" -delete 2>/dev/null

# Eliminar archivos específicos que no deben estar en el template
rm -f .gitignore
rm -f *.md 2>/dev/null
rm -f build-template.sh

cd ..

# Crear el package NuGet
echo "?? Creando package NuGet..."
dotnet pack $PACKAGE_DIR -o ./packages --include-source --include-symbols

echo "? Template package creado en ./packages/"
echo ""
echo "?? Para instalar el template:"
echo "  dotnet new install ./packages/CodeCraft.NET.Template.*.nupkg"
echo ""
echo "?? Para usar el template:"
echo "  dotnet new codecraft -n \"MiNuevoProyecto\""