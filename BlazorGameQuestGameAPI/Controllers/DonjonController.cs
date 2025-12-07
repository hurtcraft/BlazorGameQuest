using BlazorGameQuestClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
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
    [HttpGet("load/{DonjonName}")]
    public async Task<Donjon> loadAsync(string DonjonName)
    {
        Donjon donjon = await _donjonService.LoadDonjon(DonjonName);

        return donjon;
    }

    [HttpGet("allDonjons")]
    public Task<string[]> allDonjons()
    {
        return _donjonService.GetListDonjon();
    }
    [HttpGet("getRandomDonjons/{nbRandomDonjon}")]
    public Task<List<Donjon>> GetRandomDonjon(int nbRandomDonjon)
    {
        return _donjonService.RequestRandomDonjon(nbRandomDonjon);
    }
    [HttpGet("getDonjonEltConf")]
    public Task<Dictionary<string, List<int>>> GetDonjonEltConf()
    {
        return _donjonService.GetDonjonEltConf();
    }


    [HttpGet("getAnimationConf")]
    public Task<Dictionary<string, Dictionary<string, AnimationConfig>>> GetAnimationConf()
    {
        return _donjonService.GetAnimationsConf();
    }
}