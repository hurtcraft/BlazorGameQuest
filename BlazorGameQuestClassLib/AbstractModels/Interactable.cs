
using System.Drawing;
using BlazorGameQuestClassLib.Intefaces;

namespace BlazorGameQuestClassLib.AbstractModels
{
    public abstract class Interactable : IInteractable
    {

        public bool IsActive { get; set; } = true;
        public float X { get; set; }
        public float Y { get; set; }
        public float Speed { get; set; } = 1.0f;

        protected Dictionary<string, List<string>> AnimationMap { get; set; } = new();
        protected int currentFrameIndex = 0;
        protected float animationTimer = 0f;
        protected float frameDuration = 0.1f;
        public string CurrentAnimation { get; set; } = string.Empty;
        public string CurrentFramePath => GetCurrentFramePath();

        public bool CheckCollision(ICollidable other)
        {
            return GetBounds().IntersectsWith(other.GetBounds());
        }

        public RectangleF GetBounds()
        {
            return new RectangleF(X, Y, GameGrid.SPRITE_SIZE, GameGrid.SPRITE_SIZE);
        }

        public string GetCurrentFramePath()
        {
            if (string.IsNullOrEmpty(CurrentAnimation) || !AnimationMap.ContainsKey(CurrentAnimation))
                return string.Empty;

            var frames = AnimationMap[CurrentAnimation];
            if (frames.Count == 0) return string.Empty;

            return frames[currentFrameIndex];
        }


        public void Interact(IInteractable other)
        {
            throw new NotImplementedException();
        }

        public void Move(float deltaX, float deltaY)
        {
            X += deltaX * Speed;
            Y += deltaY * Speed;
        }

        public void PlayAnimation(string animationName)
        {
            if (!AnimationMap.ContainsKey(animationName)) return;

            if (CurrentAnimation != animationName)
            {
                CurrentAnimation = animationName;
                currentFrameIndex = 0;
                animationTimer = 0f;
            }
        }
        public void UpdateAnimation(float deltaTime)
        {
            if (string.IsNullOrEmpty(CurrentAnimation)) return;
            if (!AnimationMap.ContainsKey(CurrentAnimation)) return;

            var frames = AnimationMap[CurrentAnimation];
            if (frames.Count == 0) return;

            animationTimer += deltaTime;

            while (animationTimer >= frameDuration)
            {
                animationTimer -= frameDuration;
                currentFrameIndex = (currentFrameIndex + 1) % frames.Count;
            }
        }

        public void AddAnimation(string animationName, List<string> animationSprites)
        {
            if (AnimationMap.ContainsKey(animationName))
            {
                AnimationMap[animationName] = animationSprites;
            }
            else
            {
                AnimationMap.Add(animationName, animationSprites);
            }
        }
    }

}

