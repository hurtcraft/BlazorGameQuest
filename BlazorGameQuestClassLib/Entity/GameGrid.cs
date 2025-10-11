namespace BlazorGameQuestClassLib
{
    public class GameGrid
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<List<int>> grid { get; set; }
        public readonly int NB_SPRITE_LARGEUR=30;
        public readonly int NB_SPRITE_LONGUEUR = 30;
        public readonly int SPRITE_SIZE = 32;
    }
}