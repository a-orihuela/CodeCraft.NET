#!/bin/bash

echo "Initializing CodeCraft.NET project..."

# Get project name from current directory
PROJECT_NAME=$(basename "$PWD")
echo "Project: $PROJECT_NAME"

# Rename solution files
if [ -f "CodeCraft.NET.sln" ]; then
    mv "CodeCraft.NET.sln" "$PROJECT_NAME.sln"
    echo "Solution renamed to $PROJECT_NAME.sln"
fi

# Restore NuGet packages
echo "Restoring NuGet packages..."
dotnet restore

# Apply migrations if database exists
echo "Setting up database..."
if dotnet ef migrations list --project "$PROJECT_NAME.Infrastructure" --startup-project "$PROJECT_NAME.WebAPI" > /dev/null 2>&1; then
    dotnet ef database update --project "$PROJECT_NAME.Infrastructure" --startup-project "$PROJECT_NAME.WebAPI"
    echo "Database updated"
else
    echo "No migrations found. Run the generator first."
fi

# Build the solution
echo "Building solution..."
dotnet build

echo "Project initialized successfully!"
echo ""
echo "Next steps:"
echo "  1. Run: dotnet run --project $PROJECT_NAME.WebAPI"
echo "  2. Visit: https://localhost:7202/swagger"
echo "  3. Generate code: dotnet run --project $PROJECT_NAME.Generator"