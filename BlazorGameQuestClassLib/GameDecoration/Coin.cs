

using System.Security.Principal;
using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

public class Coin : Interactable
{
    private int Reward=200;
    public override void Interact(IInteractable other)
    {
        if(other is Player player && IsActive)
        {
            player.Score+=Reward;
            IsActive=false;
        }
    }
}