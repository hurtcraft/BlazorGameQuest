using Microsoft.AspNetCore.Mvc;
using Service;
using BlazorGameQuestClassLib;
namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task Block(int id)
        {
            //à implémenter
        }
        [HttpPut("Update/{id}")]
        public async Task Update(int id)
        {
            //à implémenter
        }
    }

}