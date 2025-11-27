# Étape 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers .csproj et restaurer les dépendances
COPY ["BlazorGameQuestGameAPI/BlazorGameQuestGameAPI.csproj", "BlazorGameQuestGameAPI/"]
COPY ["BlazorGameQuestClassLib/BlazorGameQuestClassLib.csproj", "BlazorGameQuestClassLib/"]
RUN dotnet restore "BlazorGameQuestGameAPI/BlazorGameQuestGameAPI.csproj"

# Copier tout le code source
COPY . .

# Build l'application
WORKDIR "/src/BlazorGameQuestGameAPI"
RUN dotnet build "BlazorGameQuestGameAPI.csproj" -c Release -o /app/build

# Étape 2 : Publish
FROM build AS publish
RUN dotnet publish "BlazorGameQuestGameAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape 3 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 5001

# Copier les fichiers publiés
COPY --from=publish /app/publish .

# Copier les fichiers de configuration nécessaires depuis l'étape build
COPY --from=build /src/BlazorGameQuestGameAPI/conf ./conf
COPY --from=build /src/BlazorGameQuestGameAPI/Donjons ./Donjons

ENTRYPOINT ["dotnet", "BlazorGameQuestGameAPI.dll"]

