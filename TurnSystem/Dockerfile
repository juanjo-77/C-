# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#  Asegurarse de que la versión de .NET coincida con la del proyecto

WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código fuente
COPY . ./

# Publicar la aplicación en modo Release
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /out .

# Exponer el puerto (siempre 8080 para contenedores ASP.NET)
EXPOSE 8080

#  Reemplazar "TuProyecto.dll" con el nombre real del archivo .dll del proyecto
ENTRYPOINT ["dotnet", "TurnSystem.dll"]