using BlazorGameQuestClassLib;
using Microsoft.AspNetCore.Mvc;
using Service;

[ApiController]
[Route("api/[Controller]")]
public class DonjonController : ControllerBase
{
    private readonly DonjonService _donjonService;

    public DonjonController(DonjonService donjonService)
    {
        _donjonService = donjonService;
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveMap([FromBody] Donjon donjon)
    {
        if (donjon == null)
            return BadRequest("Donjon is null");

        await _donjonService.SaveDonjonAsync(donjon);

        return Ok(new { Message = "Donjon sauvegardé !" });
    }
    [HttpGet("load/{donjonId}")]
    public string load(int donjonId)
    {
        _donjonService.LoadDonjon(donjonId);
        
        return "taata";
    }
}