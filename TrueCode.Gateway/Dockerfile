FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TrueCode.Gateway/TrueCode.Gateway.Main/*.csproj ./TrueCode.Gateway/TrueCode.Gateway.Main/
COPY TrueCode.Gateway/TrueCode.Gateway.Infrastructure/*.csproj ./TrueCode.Gateway/TrueCode.Gateway.Infrastructure/
COPY TrueCode.Gateway/TrueCode.Gateway.Application/*.csproj ./TrueCode.Gateway/TrueCode.Gateway.Application/
#COPY TrueCode.Gateway/TrueCode.Gateway.sln ./TrueCode.Gateway/

RUN dotnet restore ./TrueCode.Gateway/TrueCode.Gateway.Main/TrueCode.Gateway.Main.csproj

COPY . .

WORKDIR /src/TrueCode.Gateway/TrueCode.Gateway.Main
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "TrueCode.Gateway.Main.dll"]