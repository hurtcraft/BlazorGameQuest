using BlazorGameQuestClassLib.Intefaces;

namespace BlazorGameQuestClassLib
{
    public interface IInteractable : IMovable, ICollidable, IAnimable
    {
        bool IsActive { get; set; }

        void Interact(IInteractable other);
    }
}