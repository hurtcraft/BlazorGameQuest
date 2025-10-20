using System.Text.Json;
using BlazorGameQuestClassLib;

namespace Service
{
    public class DonjonService
    {
        private readonly IWebHostEnvironment _env;

        public DonjonService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task SaveDonjonAsync(Donjon donjon)
        {
            if (donjon == null) return;

            // Sérialiser le donjon en JSON
            string json = JsonSerializer.Serialize(donjon, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            string DonjonsDirectory= Path.Combine(Directory.GetCurrentDirectory(), "Donjons");
            Console.WriteLine("DONJON " + donjon.GameGrid.ToCsv());
            // Chemin complet vers wwwroot/maps
            //string folderPath = Path.Combine(_env.WebRootPath, "maps");
            
            Directory.CreateDirectory(DonjonsDirectory);

            string filePath = Path.Combine(DonjonsDirectory, donjon.Name);

            await File.WriteAllTextAsync(filePath, "test msg");
        }
    }
}