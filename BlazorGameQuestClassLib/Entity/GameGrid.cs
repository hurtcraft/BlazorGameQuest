namespace BlazorGameQuestClassLib
{
    public class GameGrid
    {

        public static readonly int NB_SPRITE_LARGEUR = 20;
        public static readonly int NB_SPRITE_LONGUEUR = 30;
        public static readonly int SPRITE_SIZE = 32;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<List<int>> grid { get; set; } = new List<List<int>>();

        public GameGrid()
        {
            for (int y = 0; y < NB_SPRITE_LARGEUR; y++)
            {
                var row = new List<int>();
                for (int x = 0; x < NB_SPRITE_LONGUEUR; x++)
                {
                    row.Add(-1);
                }
                grid.Add(row);
            }
        }
        

    }
}