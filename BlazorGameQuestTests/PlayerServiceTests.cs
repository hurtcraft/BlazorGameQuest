using BlazorGameQuestClassLib;
using Database;
using Microsoft.EntityFrameworkCore;
using Service;
using Xunit;

namespace BlazorGameQuestTests;

// Tests unitaires pour PlayerService : opérations CRUD, blocage/déblocage et seed de données
public class PlayerServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly PlayerService _playerService;

    public PlayerServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _playerService = new PlayerService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllPlayersAsync_WithNoPlayers_ShouldReturnEmptyList()
    {
        var players = await _playerService.GetAllPlayersAsync();

        Assert.NotNull(players);
        Assert.Empty(players);
    }

    [Fact]
    public async Task AddPlayerAsync_ShouldAddPlayerToDatabase()
    {
        var player = new Player
        {
            Name = "TestPlayer",
            Score = 100
        };

        await _playerService.AddPlayerAsync(player);

        var players = await _playerService.GetAllPlayersAsync();
        Assert.Single(players);
        Assert.Equal("TestPlayer", players[0].Name);
        Assert.Equal(100, players[0].Score);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithExistingPlayer_ShouldReturnPlayer()
    {
        var player = new Player
        {
            Name = "TestPlayer",
            Score = 100
        };
        await _playerService.AddPlayerAsync(player);
        int playerId = player.Id;

        var retrievedPlayer = await _playerService.GetPlayerByIdAsync(playerId);

        Assert.NotNull(retrievedPlayer);
        Assert.Equal("TestPlayer", retrievedPlayer.Name);
        Assert.Equal(100, retrievedPlayer.Score);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithNonExistingPlayer_ShouldReturnNull()
    {
        var player = await _playerService.GetPlayerByIdAsync(999);

        Assert.Null(player);
    }

    [Fact]
    public async Task RemovePlayerByIdAsync_WithExistingPlayer_ShouldReturnTrue()
    {
        var player = new Player
        {
            Name = "TestPlayer",
            Score = 100
        };
        await _playerService.AddPlayerAsync(player);
        int playerId = player.Id;

        var result = await _playerService.RemovePlayerByIdAsync(playerId);

        Assert.True(result);
        var players = await _playerService.GetAllPlayersAsync();
        Assert.Empty(players);
    }

    [Fact]
    public async Task RemovePlayerByIdAsync_WithNonExistingPlayer_ShouldReturnFalse()
    {
        var result = await _playerService.RemovePlayerByIdAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdatePlayerAsync_WithExistingPlayer_ShouldUpdatePlayer()
    {
        var player = new Player
        {
            Name = "TestPlayer",
            Score = 100
        };
        await _playerService.AddPlayerAsync(player);
        int playerId = player.Id;

        var updatedPlayer = new Player
        {
            Id = playerId,
            Name = "UpdatedPlayer",
            Score = 200,
            IsBlocked = true
        };

        var result = await _playerService.UpdatePlayerAsync(updatedPlayer);

        Assert.True(result);
        var retrievedPlayer = await _playerService.GetPlayerByIdAsync(playerId);
        Assert.NotNull(retrievedPlayer);
        Assert.Equal("UpdatedPlayer", retrievedPlayer.Name);
        Assert.Equal(200, retrievedPlayer.Score);
        Assert.True(retrievedPlayer.IsBlocked);
    }

    [Fact]
    public async Task UpdatePlayerAsync_WithNonExistingPlayer_ShouldReturnFalse()
    {
        var player = new Player
        {
            Id = 999,
            Name = "NonExistent",
            Score = 100
        };

        var result = await _playerService.UpdatePlayerAsync(player);

        Assert.False(result);
    }

    [Fact]
    public async Task BlockPlayerAsync_WithExistingPlayer_ShouldToggleIsBlocked()
    {
        var player = new Player
        {
            Name = "TestPlayer",
            Score = 100,
            IsBlocked = false
        };
        await _playerService.AddPlayerAsync(player);
        int playerId = player.Id;

        var result = await _playerService.BlockPlayerAsync(playerId);

        Assert.True(result);
        var retrievedPlayer = await _playerService.GetPlayerByIdAsync(playerId);
        Assert.NotNull(retrievedPlayer);
        Assert.True(retrievedPlayer.IsBlocked);

        await _playerService.BlockPlayerAsync(playerId);
        retrievedPlayer = await _playerService.GetPlayerByIdAsync(playerId);
        Assert.NotNull(retrievedPlayer);
        Assert.False(retrievedPlayer.IsBlocked);
    }

    [Fact]
    public async Task BlockPlayerAsync_WithNonExistingPlayer_ShouldReturnFalse()
    {
        var result = await _playerService.BlockPlayerAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task SeedPlayersAsync_ShouldAddMultiplePlayers()
    {
        await _playerService.SeedPlayersAsync();

        var players = await _playerService.GetAllPlayersAsync();
        Assert.True(players.Count >= 14);
    }

    [Fact]
    public async Task SeedPlayersAsync_WhenCalledTwice_ShouldNotDuplicatePlayers()
    {
        await _playerService.SeedPlayersAsync();
        var firstCount = (await _playerService.GetAllPlayersAsync()).Count;
        
        await _playerService.SeedPlayersAsync();
        var secondCount = (await _playerService.GetAllPlayersAsync()).Count;

        Assert.Equal(firstCount, secondCount);
    }
}
