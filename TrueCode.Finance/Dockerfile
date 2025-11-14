FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TrueCode.Finance/TrueCode.Finance.Application/*.csproj \ 
    ./TrueCode.Finance/TrueCode.Finance.Application/
COPY TrueCode.Finance/TrueCode.Finance.Infrastructure/*.csproj \ 
    ./TrueCode.Finance/TrueCode.Finance.Infrastructure/
COPY TrueCode.Finance/TrueCode.Finance.WebApi/*.csproj \ 
    ./TrueCode.Finance/TrueCode.Finance.WebApi/

COPY TrueCode.Shared.Contracts/*.csproj ./TrueCode.Shared.Contracts/
COPY TrueCode.Shared.Infrastructure/*csproj ./TrueCode.Shared.Infrastructure/

# COPY TrueCode.Finance/TrueCode.Finance.sln ./TrueCode.Finance/

RUN dotnet restore ./TrueCode.Finance/TrueCode.Finance.WebApi/TrueCode.Finance.WebApi.csproj

COPY . .

WORKDIR /src/TrueCode.Finance/TrueCode.Finance.WebApi
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "TrueCode.Finance.WebApi.dll"]