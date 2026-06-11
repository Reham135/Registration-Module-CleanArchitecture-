FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["src/Registration.Domain/Registration.Domain.csproj", "src/Registration.Domain/"]
COPY ["src/Registration.Application/Registration.Application.csproj", "src/Registration.Application/"]
COPY ["src/Registration.Infrastructure/Registration.Infrastructure.csproj", "src/Registration.Infrastructure/"]
COPY ["src/Registration.Persistence/Registration.Persistence.csproj", "src/Registration.Persistence/"]
COPY ["src/Registration.Api/Registration.Api.csproj", "src/Registration.Api/"]
RUN dotnet restore "src/Registration.Api/Registration.Api.csproj"

COPY . .
WORKDIR "/src/src/Registration.Api"
RUN dotnet build "Registration.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Registration.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Registration.Api.dll"]
