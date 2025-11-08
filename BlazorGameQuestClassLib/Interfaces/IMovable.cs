
namespace BlazorGameQuestClassLib.Intefaces
{
    public interface IMovable
    {
        float X { get; }
        float Y { get; }
        float Speed { get; }

        void Move(float deltaX, float deltaY);
    }
}

