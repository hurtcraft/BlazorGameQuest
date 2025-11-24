

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Key : Interactable
{
    private int Reward=500;
    public override void Interact(IInteractable other)
    {
        if(other is Player player && IsActive)
        {
            player.Score+=Reward;
            IsActive=false;
        }
    }
}