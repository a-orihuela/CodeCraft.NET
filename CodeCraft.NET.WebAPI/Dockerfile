FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CodeCraft.NET.WebAPI/CodeCraft.NET.WebAPI.csproj", "CodeCraft.NET.WebAPI/"]
COPY ["CodeCraft.NET.Application/CodeCraft.NET.Application.csproj", "CodeCraft.NET.Application/"]
COPY ["CodeCraft.NET.Infrastructure/CodeCraft.NET.Infrastructure.csproj", "CodeCraft.NET.Infrastructure/"]
RUN dotnet restore "./CodeCraft.NET.WebAPI/CodeCraft.NET.WebAPI.csproj"
COPY . .
WORKDIR "/src/CodeCraft.NET.WebAPI"
RUN dotnet build "./CodeCraft.NET.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CodeCraft.NET.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeCraft.NET.WebAPI.dll"]