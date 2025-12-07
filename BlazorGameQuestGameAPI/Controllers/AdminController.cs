using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service;
using BlazorGameQuestClassLib;
namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly PlayerService _playerService;
        public AdminController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _playerService.RemovePlayerByIdAsync(id);
            return NoContent();

        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Player>>> GetAll()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpPost("Block/{id}")]
        public async Task<IActionResult> Block(int id)
        {
            var result = await _playerService.BlockPlayerAsync(id);
            if (!result)
                return NotFound($"Joueur avec l'id {id} introuvable.");
            
            return Ok($"Le statut de blocage du joueur {id} a été inversé.");
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Player player)
        {
            // S'assurer que l'ID dans l'URL correspond à celui dans l'objet
            if (id != player.Id)
                return BadRequest("L'ID dans l'URL ne correspond pas à celui du joueur.");
            
            var result = await _playerService.UpdatePlayerAsync(player);
            if (!result)
                return NotFound($"Joueur avec l'id {id} introuvable.");
            
            return Ok($"Joueur {id} mis à jour avec succès.");
        }
    }

}