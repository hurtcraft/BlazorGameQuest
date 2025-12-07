using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorGameClient;
using BlazorGameQuestClassLib;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuration de l'authentification OIDC avec Keycloak
builder.Services.AddOidcAuthentication(options =>
{
    // Configuration Keycloak - sera configuré dans Keycloak
    // Ces valeurs seront configurées dans Keycloak (voir guide de configuration)
    builder.Configuration.Bind("Oidc", options.ProviderOptions);
    
    options.ProviderOptions.Authority = "http://localhost:8080/realms/blazorgamequest";
    options.ProviderOptions.ClientId = "blazor-client";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("roles");
    
    // Pour le développement, désactiver la vérification HTTPS
    options.ProviderOptions.MetadataUrl = "http://localhost:8080/realms/blazorgamequest/.well-known/openid-configuration";
    
    // Configuration des chemins de redirection
    options.AuthenticationPaths.LogInPath = "authentication/login";
    options.AuthenticationPaths.LogInCallbackPath = "authentication/login-callback";
    options.AuthenticationPaths.LogOutPath = "authentication/logout";
    options.AuthenticationPaths.LogOutCallbackPath = "authentication/logout-callback";
    options.AuthenticationPaths.LogInFailedPath = "authentication/login-failed";
    options.AuthenticationPaths.RegisterPath = "authentication/register";
    
    // Configuration pour extraire les rôles du token
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.NameClaim = "preferred_username";
})
.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, CustomUserFactory>();

// Configuration de la redirection après authentification
builder.Services.Configure<RemoteAuthenticationOptions<OidcProviderOptions>>(options =>
{
    options.AuthenticationPaths.RemoteRegisterPath = "authentication/register";
    options.AuthenticationPaths.RemoteProfilePath = "authentication/profile";
});

builder.Services.AddScoped<DonjonService>();
builder.Services.AddScoped<PlayerServices>();

builder.Services.AddScoped<InputManager>();

builder.Services.AddMudServices();

// Configuration HttpClient avec authentification
// Utiliser un handler personnalisé qui ajoute le token Bearer
builder.Services.AddScoped<AuthorizedHttpHandler>();

builder.Services.AddHttpClient("BlazorGameQuestGameAPI", client =>
{
    // Utiliser la Gateway comme point d'entrée unique pour les APIs
    client.BaseAddress = new Uri("http://localhost:5000/api/");
})
.AddHttpMessageHandler<AuthorizedHttpHandler>();

// HttpClient pour les appels authentifiés
builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("BlazorGameQuestGameAPI");
});

GameAsset.LoadAssets(
    MapTilePath: "assets/DungeonAssets/2D Pixel Dungeon Asset Pack/character and tileset/tiles",
    MobTilePath: "assets/DungeonAssets/2D Pixel Dungeon Asset Pack/mobs/mobsTiles"
);

await builder.Build().RunAsync();
