using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorGameClient;
using BlazorGameQuestClassLib;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<DonjonService>();
builder.Services.AddScoped<PlayerServices>();

builder.Services.AddScoped<InputManager>();

builder.Services.AddMudServices();
// URL de l'API - fonctionne en développement local et avec Docker (port exposé)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5001/api/")
});
GameAsset.LoadAssets(
    MapTilePath: "assets/DungeonAssets/2D Pixel Dungeon Asset Pack/character and tileset/tiles",
    MobTilePath: "assets/DungeonAssets/2D Pixel Dungeon Asset Pack/mobs/mobsTiles"
);

await builder.Build().RunAsync();
