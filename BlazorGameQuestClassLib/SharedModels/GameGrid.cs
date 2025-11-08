namespace BlazorGameQuestClassLib
{
    public class GameGrid
    {

        public static readonly int NB_SPRITE_LARGEUR = 20;
        public static readonly int NB_SPRITE_LONGUEUR = 30;
        public static readonly int SPRITE_SIZE = 32;


        public List<List<List<int>>> grid { get; set; } = new List<List<List<int>>>();

        public static GameGrid StringToGameGrid(string GameGridString)
        {
            GameGrid gameGrid = new GameGrid();


            string[] splittedGameGridString = GameGridString.Split(";");
            int rowIndex;
            int colIndex;
            for (int i = 0; i < splittedGameGridString.Count() - 1; i++)
            {
                rowIndex = i / NB_SPRITE_LONGUEUR; 
                colIndex = i % NB_SPRITE_LONGUEUR;
                string elt = splittedGameGridString[i].Trim();


                gameGrid.grid[rowIndex][colIndex].AddRange(extractTileFromBracket(elt));



            }
            return gameGrid;
        }
        private static List<int> extractTileFromBracket(string s)
        {
            // if (string.IsNullOrWhiteSpace(s))
            //     return new List<int>();

            List<int> res = new List<int>();
            s = s.Trim('[', ']');
            string[] splittedString = s.Split(",");

            foreach (string elt in splittedString)
            {
                int i = int.Parse(elt);
                res.Add(i);

            }

            return res;
        }

        public GameGrid()
        {
            for (int y = 0; y < NB_SPRITE_LARGEUR; y++)
            {
                var row = new List<List<int>>();
                for (int x = 0; x < NB_SPRITE_LONGUEUR; x++)
                {
                    var cell = new List<int> { -1 };

                    row.Add(cell);
                }
                grid.Add(row);  
            }
        }
        public string ToCsv()
        {
            var sb = new System.Text.StringBuilder();

            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[0].Count; j++)
                {
                    string cellString = "[" + string.Join(",", grid[i][j]) + "];";
                    sb.Append(cellString);

                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}