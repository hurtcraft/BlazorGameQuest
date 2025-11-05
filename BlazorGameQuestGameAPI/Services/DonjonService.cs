using System.Text.Json;
using BlazorGameQuestClassLib;

namespace Service
{
    public class DonjonService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string DonjonsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Donjons");
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
            Console.WriteLine("DONJON " + donjon.GameGrid.ToCsv());


            Directory.CreateDirectory(DonjonsDirectory);
            int Difficulty = donjon.Difficulty;
            string filePath = Path.Combine(DonjonsDirectory, donjon.Name +"_D"+Difficulty+ ".csv");

            await File.WriteAllTextAsync(filePath, donjon.GameGrid.ToCsv());
        }
        public async Task<string[]> GetListDonjon()
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(DonjonsDirectory))
                    return Array.Empty<string>();

                return Directory.GetFiles(DonjonsDirectory)
                                .Select(Path.GetFileName)
                                .Where(n => n != null)
                                .Cast<string>()
                                .ToArray();
            });
        }

        public async Task<Donjon> LoadDonjon(string DonjonName)
        {
            string fileName = DonjonName;
            string filePath = Path.Combine(DonjonsDirectory, fileName);

            string contenu = await File.ReadAllTextAsync(filePath);
            Donjon donjon = new Donjon();
            donjon.GameGrid=GameGrid.StringToGameGrid(contenu);
            donjon.Difficulty = GetDonjonDifficulty(fileName);
            donjon.Name = fileName;
            return donjon;
        }
        private int GetDonjonDifficulty(string fileName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            fileNameWithoutExtension.Last();
            return int.Parse(fileNameWithoutExtension.Last().ToString()) ;
        }


    }
}