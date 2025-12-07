using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorGameQuestClassLib;

public class DonjonService
{
    private readonly HttpClient _http;

    // HttpClient injecté par Blazor avec authentification
    public DonjonService(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient("BlazorGameQuestGameAPI");
    }

    public async Task SaveDonjonAsync(Donjon donjon)
    {
        if (donjon == null) return;

        var response = await _http.PostAsJsonAsync("Donjon/save", donjon);
        response.EnsureSuccessStatusCode();
    }
    public async Task<string[]> GetListDonjon()
    {
        var response = await _http.GetAsync("Donjon/allDonjons");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>();
    }

    public async Task<Donjon?> GetDonjon(string name)
    {
        string encodedName = Uri.EscapeDataString(name);

        var response = await _http.GetAsync($"Donjon/Load/{encodedName}");

        if (response.IsSuccessStatusCode)
        {
            var donjon = await response.Content.ReadFromJsonAsync<Donjon>();
            return donjon;
        }
        else
        {
            return null;
        }
    }
    public async Task<List<Donjon>> GetRandomDonjon(int nbRandomDonjon)
    {
        var response = await _http.GetAsync($"Donjon/getRandomDonjons/{nbRandomDonjon}");

        if (response.IsSuccessStatusCode)
        {
            var donjons = await response.Content.ReadFromJsonAsync<List<Donjon>>();
            return donjons ?? new List<Donjon>();
        }
        else
        {
            return new List<Donjon>();
        }
    }
    public async Task<Dictionary<string, List<int>>> GetDonjonEltConf()
    {
        var response = await _http.GetAsync("Donjon/getDonjonEltConf/");
        if (response.IsSuccessStatusCode)
        {
            var config = await response.Content.ReadFromJsonAsync<Dictionary<string, List<int>>>();
            return config??new();
        }
        else
        {
            return new();
        }

    }

    public async Task<Dictionary<string, Dictionary<string, AnimationConfig>>> GetAnimationsConfig()
    {
        var response = await _http.GetAsync("Donjon/getAnimationConf/");
        if (response.IsSuccessStatusCode)
        {
            var config = await response.Content.ReadFromJsonAsync<Dictionary<string, Dictionary<string, AnimationConfig>>>();
            return config??new();
        }
        else
        {
            return new();
        }

    }
    
    public async Task<string> TestAsync()
    {
        var response = await _http.GetAsync("Donjon/test");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return $"Erreur : {response.StatusCode}";
        }
    }
}
