FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "MagicVillaAPI.csproj" -c Release -o ./published -p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app 
COPY --from=build /src/published . 
ENTRYPOINT ["dotnet", "MagicVillaAPI.dll"]