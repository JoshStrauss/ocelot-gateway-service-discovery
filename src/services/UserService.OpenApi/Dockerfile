#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/services/UserService.OpenApi/UserService.OpenApi.csproj", "src/services/UserService.OpenApi/"]
COPY ["src/common/Services.AspNetCore/Services.AspNetCore.csproj", "src/common/Services.AspNetCore/"]
COPY ["src/common/Services.Consul/Services.Consul.csproj", "src/common/Services.Consul/"]
RUN dotnet restore "./src/services/UserService.OpenApi/UserService.OpenApi.csproj"
COPY . .
WORKDIR "/src/src/services/UserService.OpenApi"
RUN dotnet build "./UserService.OpenApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./UserService.OpenApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.OpenApi.dll"]