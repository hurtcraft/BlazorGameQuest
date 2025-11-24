

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Heal : Interactable
{
    private int HealPoints=10;
    public override void Interact(IInteractable other)
    {
        if(other is Player player && IsActive)
        {
            if( player.Health==player.MaxHealth) return;
            player.Health+=HealPoints;
            IsActive=false;
        }
    }
}