# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Unalive_WebManagement/Unalive_WebManagement.csproj", "Unalive_WebManagement/"]
COPY ["Unalive_WebManagement.BLL/Unalive_WebManagement.BLL.csproj", "Unalive_WebManagement.BLL/"]
COPY ["Unalive_WebManagement.DAL/Unalive_WebManagement.DAL.csproj", "Unalive_WebManagement.DAL/"]
COPY ["Unalive_WebManagement.Domain/Unalive_WebManagement.Domain.csproj", "Unalive_WebManagement.Domain/"]
RUN dotnet restore "./Unalive_WebManagement/Unalive_WebManagement.csproj"
COPY . .
WORKDIR "/src/Unalive_WebManagement"
RUN dotnet build "./Unalive_WebManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unalive_WebManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unalive_WebManagement.dll"]