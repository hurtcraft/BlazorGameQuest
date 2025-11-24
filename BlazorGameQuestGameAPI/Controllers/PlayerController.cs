
using Microsoft.AspNetCore.Mvc;
using Service;
using BlazorGameQuestClassLib;
namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Player>>> GetAll()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Player>> GetById(int id)
        {

            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null)
                return NotFound();

            return Ok(player);
        }
        [HttpPost("Add")]
        public async Task<ActionResult<Player>> AddPlayer([FromBody] Player p)
        {
            if (p == null)
                return BadRequest("Invalid player data.");

            await _playerService.AddPlayerAsync(p);


            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Player p)
        {
            var success = await _playerService.UpdatePlayerAsync(p);

            if (!success)
                return NotFound(); // joueur non trouvé

            return NoContent(); // mise à jour réussie
        }


    }
}