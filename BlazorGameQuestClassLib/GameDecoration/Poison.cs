

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Poison : Interactable
{
    private int PoisonPoints=10;
    public override void Interact(IInteractable other)
    {
        if(other is Player player && IsActive)
        {
            // if( player.Health<=0) return;

            player.Health-=PoisonPoints;
            IsActive=false;
        }
    }
}