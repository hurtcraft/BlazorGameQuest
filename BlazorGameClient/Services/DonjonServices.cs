using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorGameQuestClassLib;

public class DonjonService
{
    private readonly HttpClient _http;

    // HttpClient injecté par Blazor
    public DonjonService(HttpClient http)
    {
        _http = http;
    }

    public async Task SaveDonjonAsync(Donjon donjon)
    {
        if (donjon == null) return;

        var response = await _http.PostAsJsonAsync("Donjon/save", donjon);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Donjon sauvegardé côté serveur !");
        }
        else
        {
            Console.WriteLine($"Erreur : {response.StatusCode}");
        }
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
            Console.WriteLine($"Erreur lors du chargement du donjon : {response.StatusCode}");
            return null;
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
