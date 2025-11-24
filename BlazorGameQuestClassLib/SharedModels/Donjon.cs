using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;
namespace BlazorGameQuestClassLib;

public class Donjon
{
    public GameGrid? GameGrid { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Difficulty { get; set; } = 1;
    public override string ToString()
    {
        var gridText = GameGrid != null ? GameGrid.ToCsv() : "null";

        return $"Donjon Name='{Name}', Difficulty={Difficulty}, GameGrid={gridText}";
    }

    public List<List<Interactable>> AsInteractable(Dictionary<string, List<int>> config)
    {
        if (GameGrid == null) return new();

        List<List<List<int>>> grid = GameGrid.grid;

        List<List<Interactable>> interactables = new List<List<Interactable>>(grid.Count());

        for (int i = 0; i < grid.Count(); i++)
        {

            var row = new List<Interactable>();

            for (int j = 0; j < grid[i].Count(); j++)
            {
                for (int k = 0; k < grid[i][j].Count(); k++)
                {
                    int elt = grid[i][j][k];

                    if (elt != -1)
                    {
                        Interactable interactable = Create(elt, j, i, config);
                        row.Add(interactable);
                    }

                }


            }
            interactables.Add(row);
        }

        return interactables;
    }

    private Interactable Create(int id, int x, int y, Dictionary<string, List<int>> config)
    {
        var factory = new Dictionary<string, Func<Interactable>>()
    {
        { "wall", () => new Wall() },
        { "skeleton", () => new Skeleton() },
        { "coin", () => new Coin() },
        { "door", () => new Door() },
        { "heal",()=> new Heal()},
        { "poison",()=> new Poison()},
        { "key",()=> new Key()},
        { "vent",()=> new Vent()},

    };

        foreach (var entry in config)
        {
            if (entry.Value.Contains(id))
            {
                Console.WriteLine("key "+entry.Key);
                if (factory.TryGetValue(entry.Key, out var createFunc))
                {
                    return InitInteractable(createFunc(), id, x, y, true);
                }
            }
        }

        return InitInteractable(new Deco(), id, x, y, false);
    }


    private Interactable InitInteractable(Interactable obj, int id, int x, int y, bool isActive)
    {
        string tilePath = GameAsset.ListMapTile[id];

        obj.X = x;
        obj.Y = y;
        obj.IsActive = isActive;

        obj.AddAnimation("default", new List<string> { tilePath });
        obj.PlayAnimation("default");

        return obj;
    }


}