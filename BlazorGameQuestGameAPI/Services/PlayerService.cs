using BlazorGameQuestClassLib;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Service pour gérer les opérations CRUD sur les joueurs (Player) via EF Core.
    /// </summary>
    public class PlayerService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructeur injectant le DbContext.
        /// </summary>
        /// <param name="context">Le DbContext de l'application.</param>
        public PlayerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task InitializeAsync()
        {
            await SeedPlayersAsync();
        }
        /// <summary>
        /// Récupère tous les joueurs de la base.
        /// </summary>
        /// <returns>Liste de tous les joueurs.</returns>
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        /// <summary>
        /// Ajoute un joueur à la base.
        /// </summary>
        /// <param name="p">Le joueur à ajouter.</param>
        public async Task AddPlayerAsync(Player p)
        {
            await _context.Players.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Récupère un joueur par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du joueur.</param>
        /// <returns>Le joueur trouvé ou null si inexistant.</returns>
        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        /// <summary>
        /// Supprime un joueur de la base à partir de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du joueur à supprimer.</param>
        /// <returns>True si la suppression a réussi, false si le joueur n'existe pas.</returns>
        public async Task<bool> RemovePlayerByIdAsync(int id)
        {
            var player = await GetPlayerByIdAsync(id);
            if (player == null) return false;

            _context.Remove(player);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Met à jour les informations d'un joueur existant.
        /// </summary>
        /// <param name="player">Le joueur avec les nouvelles informations.</param>
        /// <returns>True si la modification a réussi, false si le joueur n'existe pas.</returns>
        public async Task<bool> UpdatePlayerAsync(Player p)
        {
            var player = await GetPlayerByIdAsync(p.Id);
            if (player == null) return false;

            _context.Players.Update(player);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task SeedPlayersAsync()
        {
            if (await _context.Players.AnyAsync())
                return; 

            var joueurs = new List<Player>
            {
                new Player {  Name = "Alpha", Score = 1200 },
                new Player {  Name = "Bravo", Score = 950 },
                new Player {  Name = "Charlie", Score = 1300 },
                new Player {  Name = "Delta", Score = 1100 },
                new Player {  Name = "Echo", Score = 1400 },
                new Player {  Name = "Foxtrot", Score = 1250 },
                new Player {  Name = "Golf", Score = 870 },
                new Player {  Name = "Hotel", Score = 1180 },
                new Player {  Name = "India", Score = 1020 },
                new Player {  Name = "Juliet", Score = 1360 },
                new Player {  Name = "Kilo", Score = 940 },
                new Player {  Name = "Lima", Score = 1280 },
                new Player {  Name = "Mike", Score = 1110 },
                new Player {  Name = "November", Score = 1220 },
            };

            await _context.Players.AddRangeAsync(joueurs);
            await _context.SaveChangesAsync();
        }
    }
}
