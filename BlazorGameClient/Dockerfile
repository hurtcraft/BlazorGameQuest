# Étape 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers .csproj et restaurer les dépendances
COPY ["BlazorGameClient/BlazorGameClient.csproj", "BlazorGameClient/"]
COPY ["BlazorGameQuestClassLib/BlazorGameQuestClassLib.csproj", "BlazorGameQuestClassLib/"]
RUN dotnet restore "BlazorGameClient/BlazorGameClient.csproj"

# Copier tout le code source
COPY . .

# Build et publier l'application Blazor WebAssembly
WORKDIR "/src/BlazorGameClient"
RUN dotnet publish "BlazorGameClient.csproj" -c Release -o /app/publish

# Étape 2 : Serveur web pour servir les fichiers statiques
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Supprimer les fichiers par défaut de nginx
RUN rm -rf ./*

# Copier les fichiers publiés de Blazor
COPY --from=build /app/publish/wwwroot .

# Copier la configuration nginx personnalisée depuis l'étape build
COPY --from=build /src/BlazorGameClient/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

