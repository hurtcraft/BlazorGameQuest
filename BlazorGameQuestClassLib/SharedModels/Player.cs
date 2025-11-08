using System.Drawing;
using BlazorGameQuestClassLib.AbstractModels;
using BlazorGameQuestClassLib.Intefaces;

namespace BlazorGameQuestClassLib
{
    public class Player : Interactable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public bool IsBlocked { get; set; } = false;

    }
}