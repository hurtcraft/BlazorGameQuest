
using BlazorGameQuestClassLib;

public class GameEntity
{
    public Player? Player { get; set; }


    private List<List<IInteractable>> Map{ get; set; } = new List<List<IInteractable>>();
    
    public void InitMap(Donjon donjon)
    {
        GameGrid? gameGrid = donjon.GameGrid;
        FactoryInteractable.create(2);
    }



}