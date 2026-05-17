# Obtiene SDK de .NET 8.0 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app_build

# Copia el archivo de proyecto y restaurara dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia todo el codigo y lo compila en modo Release
COPY . ./
RUN dotnet publish -c Release -o /app_out

# Usa la imagen de runtime de .NET 8.0 para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app_out .

# Render le asigna un puerto dinamico a tu API mediante esta variable de entorno
ENV ASPNETCORE_URLS=http://*:${PORT:-8080}

ENTRYPOINT ["dotnet", "api.dll"]