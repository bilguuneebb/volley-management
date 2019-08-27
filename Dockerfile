FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /build-dir
COPY ./src ./src
COPY ./tests ./tests
RUN dotnet build "./src/VolleyManagement.sln" -c Release -o /artifacts

FROM build AS publish
RUN dotnet publish "src/Domain/VolleyM.Domain.Contracts/VolleyM.Domain.Contracts.csproj" -c Release -o /app \
    && dotnet publish "src/Domain/VolleyM.Domain.Contributors/VolleyM.Domain.Contributors.csproj" -c Release -o /app \
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Bootstrap/VolleyM.Infrastructure.Bootstrap.csproj" -c Release -o /app \
    && dotnet publish "src/Infrastructure/VolleyM.Infrastructure.Hardcoded/VolleyM.Infrastructure.Hardcoded.csproj" -c Release -o /app\
    && dotnet publish "src/VolleyManagement.API/VolleyManagement.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VolleyManagement.API.dll"]