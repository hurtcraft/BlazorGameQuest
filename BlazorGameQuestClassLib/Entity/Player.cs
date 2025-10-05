
namespace BlazorGameQuestClassLib
{
    public class Player(int Id, string Name, int Score)
    {
        public int Id { get; set; } = Id;
        public string Name { get; set; } = Name;
        public int Score { get; set; } = Score;
        
    }
}

