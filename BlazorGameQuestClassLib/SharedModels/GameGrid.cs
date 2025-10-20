namespace BlazorGameQuestClassLib
{
    public class GameGrid
    {

        public static readonly int NB_SPRITE_LARGEUR = 20;
        public static readonly int NB_SPRITE_LONGUEUR = 30;
        public static readonly int SPRITE_SIZE = 32;


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
        public string ToCsv()
        {
            var sb = new System.Text.StringBuilder();

            for (int y = 0; y < grid.Count; y++)
            {
                sb.AppendLine(string.Join(",", grid[y]));
            }

            return sb.ToString();
        }

    }
}