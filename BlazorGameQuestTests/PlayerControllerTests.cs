using BlazorGameQuestClassLib;
using Controller;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service;
using Xunit;

namespace BlazorGameQuestTests;

// Tests unitaires pour PlayerController : endpoints GetAll et GetById avec gestion des erreurs
public class PlayerControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly PlayerService _playerService;
    private readonly PlayerController _controller;

    public PlayerControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _playerService = new PlayerService(_context);
        _controller = new PlayerController(_playerService);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAll_WithNoPlayers_ShouldReturnEmptyList()
    {
        var result = await _controller.GetAll();

        var okResult = Assert.IsType<ActionResult<List<Player>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var players = Assert.IsType<List<Player>>(actionResult.Value);
        Assert.Empty(players);
    }

    [Fact]
    public async Task GetAll_WithPlayers_ShouldReturnAllPlayers()
    {
        var player1 = new Player { Name = "Player1", Score = 100 };
        var player2 = new Player { Name = "Player2", Score = 200 };
        await _playerService.AddPlayerAsync(player1);
        await _playerService.AddPlayerAsync(player2);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<ActionResult<List<Player>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var players = Assert.IsType<List<Player>>(actionResult.Value);
        Assert.Equal(2, players.Count);
    }

    [Fact]
    public async Task GetById_WithExistingPlayer_ShouldReturnPlayer()
    {
        var player = new Player { Name = "TestPlayer", Score = 150 };
        await _playerService.AddPlayerAsync(player);
        int playerId = player.Id;

        var result = await _controller.GetById(playerId);

        var okResult = Assert.IsType<ActionResult<Player>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var returnedPlayer = Assert.IsType<Player>(actionResult.Value);
        Assert.Equal("TestPlayer", returnedPlayer.Name);
        Assert.Equal(150, returnedPlayer.Score);
    }

    [Fact]
    public async Task GetById_WithNonExistingPlayer_ShouldReturnNotFound()
    {
        var result = await _controller.GetById(999);

        var actionResult = Assert.IsType<ActionResult<Player>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }
}
