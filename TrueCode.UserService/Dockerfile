FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TrueCode.UserService/TrueCode.UserService.Application/*.csproj \ 
    ./TrueCode.UserService/TrueCode.UserService.Application/
COPY TrueCode.UserService/TrueCode.UserService.Infrastructure/*.csproj \ 
    ./TrueCode.UserService/TrueCode.UserService.Infrastructure/
COPY TrueCode.UserService/TrueCode.UserService.WebApi/*.csproj \ 
    ./TrueCode.UserService/TrueCode.UserService.WebApi/

COPY TrueCode.Shared.Contracts/*.csproj ./TrueCode.Shared.Contracts/
COPY TrueCode.Shared.Infrastructure/*csproj ./TrueCode.Shared.Infrastructure/

RUN dotnet restore ./TrueCode.UserService/TrueCode.UserService.WebApi/TrueCode.UserService.WebApi.csproj

COPY . .

WORKDIR /src/TrueCode.UserService/TrueCode.UserService.WebApi
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "TrueCode.UserService.WebApi.dll"]