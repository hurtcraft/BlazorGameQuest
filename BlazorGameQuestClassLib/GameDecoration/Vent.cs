

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Vent : Interactable
{

    public override void Interact(IInteractable other)
    {
        if(other is Player player)
        {
            //PlayAnimation("open");
            player.OnVent=true;
        }
    }
}