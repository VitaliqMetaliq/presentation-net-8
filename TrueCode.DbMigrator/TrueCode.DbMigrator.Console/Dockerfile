FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TrueCode.DbMigrator/TrueCode.DbMigrator.Console/*.csproj TrueCode.DbMigrator/TrueCode.DbMigrator.Console/
COPY TrueCode.Shared.Infrastructure/*.csproj TrueCode.Shared.Infrastructure/

RUN dotnet restore TrueCode.DbMigrator/TrueCode.DbMigrator.Console/TrueCode.DbMigrator.Console.csproj

COPY . .

WORKDIR /src/TrueCode.DbMigrator/TrueCode.DbMigrator.Console
RUN dotnet publish -c Release -o /app --no-self-contained

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "TrueCode.DbMigrator.Console.dll"]