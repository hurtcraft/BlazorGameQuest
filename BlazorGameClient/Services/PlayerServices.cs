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

    public async Task<List<Player>> GetAllAsync()
    {

        var players = await _http.GetFromJsonAsync<List<Player>>("Player/GetAll");
        return players ?? new List<Player>();

    }

    public async Task<bool> AddPlayerAsync(Player p)
    {
        var response = await _http.PostAsJsonAsync("Player/Add", p);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdatePlayerAsync(Player p)
    {
        var response = await _http.PutAsJsonAsync("Player/Update", p);
        return response.IsSuccessStatusCode;
    }


    public async Task<bool> AddOrUpdatePlayerIfExist(Player p)
    {
        var players = await GetAllAsync();

        var existing = players.FirstOrDefault(pl => pl.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

        if (existing == null)
        {
            var addResponse = await _http.PostAsJsonAsync("Player/Add", p);
            return addResponse.IsSuccessStatusCode;
        }
        else
        {
            p.Id = existing.Id;
            var updateResponse = await _http.PutAsJsonAsync("Player/Update", p);
            return updateResponse.IsSuccessStatusCode;
        }
    }

}
