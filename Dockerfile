#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Pan.Affiliation.Api/Pan.Affiliation.Api.csproj", "Pan.Affiliation.Api/"]
RUN dotnet restore "Pan.Affiliation.Api/Pan.Affiliation.Api.csproj"
COPY "src/" .
WORKDIR "Pan.Affiliation.Api"
RUN dotnet build "Pan.Affiliation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pan.Affiliation.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pan.Affiliation.Api.dll"]