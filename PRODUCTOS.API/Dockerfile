# Etapa 1: Compilación y Publicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto (.csproj) para restaurar dependencias
# Se copian todos para que el compilador vea las referencias entre proyectos
COPY ["PRODUCTOS.API/PRODUCTOS.API.csproj", "PRODUCTOS.API/"]
COPY ["PRODUCTOS.Application/PRODUCTOS.Application.csproj", "PRODUCTOS.Application/"]
COPY ["PRODUCTOS.Domain/PRODUCTOS.Domain.csproj", "PRODUCTOS.Domain/"]
COPY ["PRODUCTOS.Infrastructure/PRODUCTOS.Infrastructure.csproj", "PRODUCTOS.Infrastructure/"]
COPY ["USUARIOS.Persistence/PRODUCTOS.Persistence.csproj", "USUARIOS.Persistence/"]

# Restaurar paquetes NuGet
RUN dotnet restore "PRODUCTOS.API/PRODUCTOS.API.csproj"

# Copiar todo el código fuente de la solución
COPY . .

# Compilar el proyecto de la API en modo Release
WORKDIR "/src/PRODUCTOS.API"
RUN dotnet build "PRODUCTOS.API.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "PRODUCTOS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Imagen de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "PRODUCTOS.API.dll"]