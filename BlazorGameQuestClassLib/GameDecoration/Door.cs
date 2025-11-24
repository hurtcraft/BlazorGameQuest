

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Door : Interactable
{

    public override void Interact(IInteractable other)
    {
        if(other is Player player)
        {
            //PlayAnimation("open");
            player.GoNextDonjon=true;
            Console.WriteLine("DOOR TOUCHE "+X+" "+Y+" "+other);

        }
    }
}