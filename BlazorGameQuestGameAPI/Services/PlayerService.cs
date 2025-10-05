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
    }
}
