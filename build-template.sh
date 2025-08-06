#!/bin/bash

echo "Building CodeCraft.NET Template Package..."

# Variables
PACKAGE_VERSION="1.0.0"
PACKAGE_DIR="./nupkg"
NUGET_SOURCE="https://api.nuget.org/v3/index.json"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}Template Package Build Process${NC}"
echo "=================================="

# Clean previous packages
echo -e "${YELLOW}Cleaning previous packages...${NC}"
rm -rf $PACKAGE_DIR
mkdir -p $PACKAGE_DIR

# Validate required files
echo -e "${YELLOW}Validating required files...${NC}"

if [ ! -f ".template.config/template.json" ]; then
    echo -e "${RED}Missing .template.config/template.json${NC}"
    exit 1
fi

if [ ! -f "template.csproj" ]; then
    echo -e "${RED}Missing template.csproj${NC}"
    exit 1
fi

if [ ! -f "README.md" ]; then
    echo -e "${RED}Missing README.md${NC}"
    exit 1
fi

echo -e "${GREEN}All required files found${NC}"

# Build the package
echo -e "${YELLOW}Building NuGet package...${NC}"
dotnet pack template.csproj -o $PACKAGE_DIR -p:PackageVersion=$PACKAGE_VERSION --nologo

if [ $? -eq 0 ]; then
    echo -e "${GREEN}Package built successfully!${NC}"
    
    # List generated package
    PACKAGE_FILE=$(ls $PACKAGE_DIR/*.nupkg | head -1)
    echo -e "${BLUE}Generated package: $(basename $PACKAGE_FILE)${NC}"
    
    # Show package info
    echo -e "${BLUE}Package Information:${NC}"
    echo "   Location: $PACKAGE_FILE"
    echo "   Size: $(ls -lh $PACKAGE_FILE | awk '{print $5}')"
    echo "   Version: $PACKAGE_VERSION"
    
else
    echo -e "${RED}Package build failed!${NC}"
    exit 1
fi

# Ask for publication
echo ""
echo -e "${YELLOW}Do you want to publish to NuGet.org? (y/N)${NC}"
read -r response

if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
    echo -e "${YELLOW}Please enter your NuGet API key:${NC}"
    read -s NUGET_API_KEY
    
    if [ -z "$NUGET_API_KEY" ]; then
        echo -e "${RED}API key is required for publishing${NC}"
        exit 1
    fi
    
    echo -e "${YELLOW}Publishing to NuGet.org...${NC}"
    dotnet nuget push $PACKAGE_FILE --api-key $NUGET_API_KEY --source $NUGET_SOURCE --no-symbols
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}Successfully published to NuGet.org!${NC}"
        echo -e "${BLUE}Your template can now be installed with:${NC}"
        echo "   dotnet new install CodeCraft.NET.CleanArchitecture.Template"
    else
        echo -e "${RED}Publication failed!${NC}"
        exit 1
    fi
else
    echo -e "${BLUE}Package ready for manual publication:${NC}"
    echo "   1. Go to https://www.nuget.org/packages/manage/upload"
    echo "   2. Upload: $PACKAGE_FILE"
    echo "   3. Or use: dotnet nuget push $PACKAGE_FILE --api-key YOUR_API_KEY --source $NUGET_SOURCE"
fi

echo ""
echo -e "${GREEN}Build process completed!${NC}"