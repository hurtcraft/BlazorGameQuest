using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorGameQuestClassLib;

public class PlayerServices
{
    private readonly HttpClient _http;

    // HttpClient injecté par Blazor
    public PlayerServices(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Player>> GetAllAsync( )
    {

        var players = await _http.GetFromJsonAsync<List<Player>>("Player/GetAll");
        return players ?? new List<Player>();

    }
 
}
