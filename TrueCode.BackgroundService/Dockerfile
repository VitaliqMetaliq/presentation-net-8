FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TrueCode.BackgroundService/TrueCode.BackgroundService.Application/*.csproj \ 
    ./TrueCode.BackgroundService/TrueCode.BackgroundService.Application/
COPY TrueCode.BackgroundService/TrueCode.BackgroundService.Infrastructure/*.csproj \ 
    ./TrueCode.BackgroundService/TrueCode.BackgroundService.Infrastructure/
COPY TrueCode.BackgroundService/TrueCode.BackgroundService.Main/*.csproj \ 
    ./TrueCode.BackgroundService/TrueCode.BackgroundService.Main/

COPY TrueCode.Shared.Contracts/*.csproj ./TrueCode.Shared.Contracts/
COPY TrueCode.Shared.Infrastructure/*.csproj ./TrueCode.Shared.Infrastructure/

COPY TrueCode.BackgroundService/TrueCode.BackgroundService.sln ./TrueCode.BackgroundService/

RUN dotnet restore ./TrueCode.BackgroundService/TrueCode.BackgroundService.sln

COPY . .

WORKDIR /src/TrueCode.BackgroundService/TrueCode.BackgroundService.Main
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "TrueCode.BackgroundService.Main.dll"]