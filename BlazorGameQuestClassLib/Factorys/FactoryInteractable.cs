



using System.Text.Json;
using BlazorGameQuestClassLib;

public class FactoryInteractable
{
    private static Dictionary<string, List<int>> dict = new();
    static FactoryInteractable()
    {
        Init();
    }

    public static IInteractable create(int id)
    {
        Console.WriteLine("dict " + dict);

        return new Wall();
    }
    public static void Init()
    {
        string json = File.ReadAllText("conf/donjonElt.json");

        var res = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(json);
        if (res != null)
        {
            dict = res;
        }
        else
        {
            dict = new Dictionary<string, List<int>>();
        }
        Console.WriteLine("dict " + dict);
    }
}