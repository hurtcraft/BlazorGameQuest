using BlazorGameQuestClassLib;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Service;
using Xunit;

namespace BlazorGameQuestTests;

// Tests unitaires pour DonjonService : sauvegarde, chargement, liste et génération aléatoire de donjons
public class DonjonServiceTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly Mock<IWebHostEnvironment> _mockEnv;
    private readonly DonjonService _donjonService;

    public DonjonServiceTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);

        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(e => e.ContentRootPath).Returns(_tempDirectory);

        _donjonService = new DonjonService(_mockEnv.Object);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, true);
        }
    }

    [Fact]
    public async Task SaveDonjonAsync_WithValidDonjon_ShouldSaveToFile()
    {
        var donjon = new Donjon
        {
            Name = "TestDonjon",
            Difficulty = 1,
            GameGrid = new GameGrid()
        };

        await _donjonService.SaveDonjonAsync(donjon);

        string expectedFilePath = Path.Combine(_tempDirectory, "Donjons", "TestDonjon_D1.csv");
        Assert.True(File.Exists(expectedFilePath));
    }

    [Fact]
    public async Task SaveDonjonAsync_WithNullDonjon_ShouldNotCreateFile()
    {
        await _donjonService.SaveDonjonAsync(null!);

        string donjonsDir = Path.Combine(_tempDirectory, "Donjons");
        if (Directory.Exists(donjonsDir))
        {
            var files = Directory.GetFiles(donjonsDir);
            Assert.Empty(files);
        }
    }

    [Fact]
    public async Task SaveDonjonAsync_WithNullGameGrid_ShouldNotCreateFile()
    {
        var donjon = new Donjon
        {
            Name = "TestDonjon",
            Difficulty = 1,
            GameGrid = null
        };

        await _donjonService.SaveDonjonAsync(donjon);

        string donjonsDir = Path.Combine(_tempDirectory, "Donjons");
        if (Directory.Exists(donjonsDir))
        {
            var files = Directory.GetFiles(donjonsDir);
            Assert.Empty(files);
        }
    }

    [Fact]
    public async Task GetListDonjon_WithNoDonjons_ShouldReturnEmptyArray()
    {
        var donjons = await _donjonService.GetListDonjon();

        Assert.NotNull(donjons);
        Assert.Empty(donjons);
    }

    [Fact]
    public async Task GetListDonjon_WithDonjons_ShouldReturnFileNames()
    {
        var donjon1 = new Donjon
        {
            Name = "Donjon1",
            Difficulty = 1,
            GameGrid = new GameGrid()
        };
        var donjon2 = new Donjon
        {
            Name = "Donjon2",
            Difficulty = 2,
            GameGrid = new GameGrid()
        };

        await _donjonService.SaveDonjonAsync(donjon1);
        await _donjonService.SaveDonjonAsync(donjon2);

        var donjons = await _donjonService.GetListDonjon();

        Assert.NotNull(donjons);
        Assert.Equal(2, donjons.Length);
        Assert.Contains("Donjon1_D1.csv", donjons);
        Assert.Contains("Donjon2_D2.csv", donjons);
    }

    [Fact]
    public async Task LoadDonjon_WithExistingFile_ShouldLoadDonjon()
    {
        var originalDonjon = new Donjon
        {
            Name = "TestDonjon",
            Difficulty = 3,
            GameGrid = new GameGrid()
        };
        await _donjonService.SaveDonjonAsync(originalDonjon);

        var loadedDonjon = await _donjonService.LoadDonjon("TestDonjon_D3.csv");

        Assert.NotNull(loadedDonjon);
        Assert.Equal("TestDonjon_D3.csv", loadedDonjon.Name);
        Assert.Equal(3, loadedDonjon.Difficulty);
        Assert.NotNull(loadedDonjon.GameGrid);
    }

    [Fact]
    public async Task RequestRandomDonjon_WithAvailableDonjons_ShouldReturnDonjons()
    {
        var donjon1 = new Donjon
        {
            Name = "Donjon1",
            Difficulty = 1,
            GameGrid = new GameGrid()
        };
        var donjon2 = new Donjon
        {
            Name = "Donjon2",
            Difficulty = 2,
            GameGrid = new GameGrid()
        };

        await _donjonService.SaveDonjonAsync(donjon1);
        await _donjonService.SaveDonjonAsync(donjon2);

        var randomDonjons = await _donjonService.RequestRandomDonjon(2);

        Assert.NotNull(randomDonjons);
        Assert.True(randomDonjons.Count > 0);
        Assert.True(randomDonjons.Count <= 2);
    }

    [Fact]
    public async Task RequestRandomDonjon_WithNoDonjons_ShouldReturnEmptyList()
    {
        var randomDonjons = await _donjonService.RequestRandomDonjon(5);

        Assert.NotNull(randomDonjons);
        Assert.Empty(randomDonjons);
    }

    [Fact]
    public async Task RequestRandomDonjon_WithMoreRequestedThanAvailable_ShouldReturnOnlyAvailable()
    {
        var donjon = new Donjon
        {
            Name = "SingleDonjon",
            Difficulty = 1,
            GameGrid = new GameGrid()
        };
        await _donjonService.SaveDonjonAsync(donjon);

        var randomDonjons = await _donjonService.RequestRandomDonjon(10);

        Assert.NotNull(randomDonjons);
        Assert.Single(randomDonjons);
    }
}
