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
                    //Console.WriteLine(elt+" "+i+" "+j); 

                    if (elt != -1)
                    {
                        Interactable interactable = Create(elt, i, j, config);
                        row.Add(interactable);
                        Console.WriteLine(elt+" "+i+" "+j+" "+interactable.GetCurrentFramePath()); 
                    }

                }


                // Console.WriteLine("tile " + string.Join(",", tile));
            }
            interactables.Add(row);
        }
        Console.WriteLine("=== Interactables ===");

        for (int i = 0; i < interactables.Count; i++)
        {
            Console.WriteLine($"Row {i}:");

            for (int j = 0; j < interactables[i].Count; j++)
            {
                var inter = interactables[i][j];
                //Console.WriteLine($"  [{i},{j}] X={inter.X} Y={inter.Y} Frame={inter.GetCurrentFramePath()}");
            }
        }
        return interactables;
    }
    private Interactable Create(int id, int x, int y, Dictionary<string, List<int>> config)
    {

        // Console.WriteLine(id + " " + x + " " + y);
        foreach (var entry in config)
        {
            string key = entry.Key;
            List<int> values = entry.Value;

            if (values.Contains(id))
            {
                switch (key)
                {
                    case "wall":
                        Wall w = new Wall();

                        w.X = x;
                        w.Y = y;
                        w.AddAnimation("default", new List<string> { GameAsset.ListMapTile[id] });
                        w.PlayAnimation("default");
                        return w;

                }
            }


        }
        //temporaire 

        string tilePath = GameAsset.ListMapTile[id];
        Deco deco = new Deco();
        deco.X = x;
        deco.Y = y;
        deco.AddAnimation("default", new List<string> { tilePath });
        deco.PlayAnimation("default");
        return deco;

    }


}