using BlazorGameQuestClassLib;

namespace BlazorGameQuestTests;

// Tests unitaires pour GameGrid : initialisation, conversion en CSV et conversion inverse
public class GameGridTests
{
    [Fact]
    public void GameGrid_Constructor_ShouldInitializeGridWithDefaultValues()
    {
        var gameGrid = new GameGrid();

        Assert.NotNull(gameGrid.grid);
        Assert.Equal(GameGrid.NB_SPRITE_LARGEUR, gameGrid.grid.Count);
        Assert.Equal(GameGrid.NB_SPRITE_LONGUEUR, gameGrid.grid[0].Count);
        
        for (int y = 0; y < GameGrid.NB_SPRITE_LARGEUR; y++)
        {
            for (int x = 0; x < GameGrid.NB_SPRITE_LONGUEUR; x++)
            {
                Assert.Single(gameGrid.grid[y][x]);
                Assert.Equal(-1, gameGrid.grid[y][x][0]);
            }
        }
    }

    [Fact]
    public void GameGrid_StringToGameGrid_WithValidString_ShouldParseCorrectly()
    {
        var gameGrid = new GameGrid();
        string csvString = gameGrid.ToCsv();
        
        var parsedGrid = GameGrid.StringToGameGrid(csvString);

        Assert.NotNull(parsedGrid);
        Assert.Equal(GameGrid.NB_SPRITE_LARGEUR, parsedGrid.grid.Count);
        Assert.Equal(GameGrid.NB_SPRITE_LONGUEUR, parsedGrid.grid[0].Count);
    }

    [Fact]
    public void GameGrid_StringToGameGrid_WithCustomValues_ShouldParseCorrectly()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("[1,2,3];");
        for (int i = 1; i < GameGrid.NB_SPRITE_LARGEUR * GameGrid.NB_SPRITE_LONGUEUR; i++)
        {
            sb.Append("[-1];");
        }
        string csvString = sb.ToString();

        var parsedGrid = GameGrid.StringToGameGrid(csvString);

        Assert.NotNull(parsedGrid);
        Assert.Equal(3, parsedGrid.grid[0][0].Count);
        Assert.Equal(1, parsedGrid.grid[0][0][0]);
        Assert.Equal(2, parsedGrid.grid[0][0][1]);
        Assert.Equal(3, parsedGrid.grid[0][0][2]);
    }

    [Fact]
    public void GameGrid_ToCsv_ShouldGenerateValidFormat()
    {
        var gameGrid = new GameGrid();

        string csv = gameGrid.ToCsv();

        Assert.NotNull(csv);
        Assert.NotEmpty(csv);
        Assert.Contains("[", csv);
        Assert.Contains("]", csv);
        Assert.Contains(";", csv);
    }

    [Fact]
    public void GameGrid_ToCsv_And_StringToGameGrid_ShouldBeReversible()
    {
        var originalGrid = new GameGrid();
        originalGrid.grid[0][0] = new List<int> { 1, 2, 3 };

        string csv = originalGrid.ToCsv();
        var restoredGrid = GameGrid.StringToGameGrid(csv);

        Assert.NotNull(restoredGrid);
        Assert.Equal(originalGrid.grid[0][0].Count, restoredGrid.grid[0][0].Count);
        for (int i = 0; i < originalGrid.grid[0][0].Count; i++)
        {
            Assert.Equal(originalGrid.grid[0][0][i], restoredGrid.grid[0][0][i]);
        }
    }

    [Fact]
    public void GameGrid_Constants_ShouldHaveCorrectValues()
    {
        Assert.Equal(20, GameGrid.NB_SPRITE_LARGEUR);
        Assert.Equal(30, GameGrid.NB_SPRITE_LONGUEUR);
        Assert.Equal(32, GameGrid.SPRITE_SIZE);
    }
}
