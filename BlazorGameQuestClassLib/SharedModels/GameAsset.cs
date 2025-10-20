using System.Diagnostics;
namespace BlazorGameQuestClassLib;
public static class GameAsset
{
    public static List<string> ListMapTile { get; private set; } = new List<string>();
    public static List<string> ListMobTile { get; private set; } = new List<string>();
    public static int? currentMapTile { get; set; }
    public static int? currentMobTile{ get; set; }
    public static void LoadAssets(string MapTilePath, string MobTilePath)
    {
        ;
        // Chargement des tuiles de carte
        for (int i = 0; i < 100; i++)
        {
            ListMapTile.Add($"{MapTilePath}/tile{i:D3}.png");
        }


        // // Chargement des sprites de monstres 
        // for (int i = 0; i < 20; i++)
        // {
        //     ListMobTile.Add($"{MobTilePath}/mob{i:D3}.png");
        // }
    }

}