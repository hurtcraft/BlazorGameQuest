using BlazorGameQuestClassLib;
namespace BlazorGameQuestClassLib;

public class Donjon
{
    public GameGrid? GameGrid { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Difficulty { get; set; } = 1;


}