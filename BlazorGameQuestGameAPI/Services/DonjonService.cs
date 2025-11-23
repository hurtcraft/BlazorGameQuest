using System.Text.Json;
using BlazorGameQuestClassLib;

namespace Service
{
    public class DonjonService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string DonjonsDirectory;
        private readonly string ConfDirectory;
        public DonjonService(IWebHostEnvironment env)
        {
            _env = env;
            DonjonsDirectory = Path.Combine(_env.ContentRootPath, "Donjons");
            ConfDirectory = Path.Combine(_env.ContentRootPath, "conf");
        }
        public async Task SaveDonjonAsync(Donjon donjon)
        {
            if (donjon == null || donjon.GameGrid == null) return;

            // Sérialiser le donjon en JSON
            string json = JsonSerializer.Serialize(donjon, new JsonSerializerOptions
            {
                WriteIndented = true
            });



            Directory.CreateDirectory(DonjonsDirectory);
            int Difficulty = donjon.Difficulty;
            string filePath = Path.Combine(DonjonsDirectory, donjon.Name + "_D" + Difficulty + ".csv");

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
            donjon.GameGrid = GameGrid.StringToGameGrid(contenu);
            donjon.Difficulty = GetDonjonDifficulty(fileName);
            donjon.Name = fileName;
            return donjon;
        }
        public async Task<List<Donjon>> RequestRandomDonjon(int nbDonjon)
        {
            string[] listDonjon = await GetListDonjon();
            List<Donjon> selectedDonjons = new List<Donjon>();

            if (listDonjon == null || listDonjon.Length == 0)
                return selectedDonjons;
            Random rnd = new Random();
            nbDonjon = Math.Min(nbDonjon, listDonjon.Length);

            for (int i = 0; i < nbDonjon; i++)
            {
                int randomIndexDonjon = rnd.Next(0, listDonjon.Length);
                Donjon d = await LoadDonjon(listDonjon[randomIndexDonjon]);
                selectedDonjons.Add(d);
            }
            return selectedDonjons;


        }
        public async Task<Dictionary<string, List<int>>> GetDonjonEltConf()
        {
            Dictionary<string, List<int>> dict = new();

            string filePath = Path.Combine(ConfDirectory, "donjonElt.json");
            string json = await File.ReadAllTextAsync(filePath);
            var res = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(json);
            if (res != null)
            {
                dict = res;
            }
            return dict;

        }

        public async Task<Dictionary<string, Dictionary<string, AnimationConfig>>> GetAnimationsConf()
        {
            Dictionary<string, Dictionary<string, AnimationConfig>> dict = new();

            string filePath = Path.Combine(ConfDirectory, "animationsConfig.json");
            string json = await File.ReadAllTextAsync(filePath);

            var res = JsonSerializer.Deserialize<
                Dictionary<string, Dictionary<string, AnimationConfig>>>(json);

            if (res != null)
            {
                dict = res;
            }

            return dict;
        }

        private int GetDonjonDifficulty(string fileName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            fileNameWithoutExtension.Last();
            return int.Parse(fileNameWithoutExtension.Last().ToString());
        }


    }
}