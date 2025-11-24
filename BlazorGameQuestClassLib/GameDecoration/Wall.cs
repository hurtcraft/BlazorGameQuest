

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
        if(other is Skeleton sk)
        {
            Console.WriteLine("SKELETON TOUCHE "+X+" "+Y+" "+other);

        }
        Console.WriteLine("mur touche "+X+" "+Y+" "+other);
    }
}