using System.Drawing;


namespace BlazorGameQuestClassLib.Intefaces
{
    public interface ICollidable
    {
        RectangleF GetBounds();
        bool CheckCollision(ICollidable other);
    }
}
