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
            string DonjonsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Donjons");
            Console.WriteLine("DONJON " + donjon.GameGrid.ToCsv());


            Directory.CreateDirectory(DonjonsDirectory);

            string filePath = Path.Combine(DonjonsDirectory, donjon.Name+".csv");

            await File.WriteAllTextAsync(filePath, donjon.GameGrid.ToCsv());
        }
        public async void LoadDonjon(int idDonjon)
        {
            string DonjonsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Donjons");
            string fileName = $"donjon{idDonjon}.csv";
            string filePath = Path.Combine(DonjonsDirectory, fileName);

            string contenu = await File.ReadAllTextAsync(filePath);

            Donjon d = StringToDonjon(contenu);

        }


        private Donjon StringToDonjon(string DonjonString)
        {
            Console.WriteLine(DonjonString);
            return null;
        }
    }
}