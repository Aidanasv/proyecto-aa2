# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY ./backend/proyecto-aa2.csproj ./
RUN dotnet restore "proyecto-aa2.csproj"

# Copiar todo el backend y compilar
COPY ./backend ./
RUN dotnet publish "proyecto-aa2.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar la build publicada
COPY --from=build /app/publish .

# Comando para ejecutar la aplicación

# Exponer el puerto de la API
EXPOSE 8080

# Entrypoint
ENTRYPOINT ["dotnet", "proyecto-aa2.dll"]
