#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7070
EXPOSE 7071

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["REPF.PriceForecasterService/REPF.PriceForecasterService.csproj", "REPF.PriceForecasterService/"]
RUN dotnet restore "./REPF.PriceForecasterService/REPF.PriceForecasterService.csproj"
COPY . .
WORKDIR "/src/REPF.PriceForecasterService"
RUN dotnet build "./REPF.PriceForecasterService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./REPF.PriceForecasterService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "REPF.PriceForecasterService.dll"]