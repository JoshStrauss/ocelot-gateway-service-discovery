# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/gateway/Services.Gateway/Services.Gateway.csproj", "src/gateway/Services.Gateway/"]
COPY ["src/common/Services.AspNetCore/Services.AspNetCore.csproj", "src/common/Services.AspNetCore/"]
COPY ["src/common/Services.Consul/Services.Consul.csproj", "src/common/Services.Consul/"]
RUN dotnet restore "./src/gateway/Services.Gateway/Services.Gateway.csproj"
COPY . .
WORKDIR "/src/src/gateway/Services.Gateway"
RUN dotnet build "./Services.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Services.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.Gateway.dll"]