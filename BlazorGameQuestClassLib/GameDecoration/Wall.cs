

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Wall : Interactable
{
    // public override string GetCurrentFramePath()
    // {

    // }
    public override void Interact(IInteractable other)
    {
        Console.WriteLine("mur touche "+X+" "+Y+" "+other);
    }
}