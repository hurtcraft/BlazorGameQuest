using System.Collections.Generic;


namespace BlazorGameClient.Game.Utils
{
        public class AnimationHelper
    {
        private readonly Dictionary<string, List<string>> animations;
        private int currentFrameIndex = 0;
        private float timer = 0f;

        public string CurrentAnimation { get; private set; } = string.Empty;

        public string CurrentFrame
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentAnimation) || !animations.ContainsKey(CurrentAnimation))
                    return string.Empty;

                var frames = animations[CurrentAnimation];
                return frames.Count > 0 ? frames[currentFrameIndex] : string.Empty;
            }
        }
        public AnimationHelper(Dictionary<string, List<string>> _animations)
        {
            animations = _animations;
        }
        public static List<string> GetAnimationsFromFolder(string folder,int nbSprite)
        {
            List<string> files = new List<string>();
            for(int i = 0; i < nbSprite; i++)
            {
                Console.WriteLine($"{folder}/{i}.png");
                files.Add( $"{folder}/{i}.png");
            }
            return files;
        }

        public void PlayAnimation(string name)
        {
            if (!animations.ContainsKey(name)) return;

            if (CurrentAnimation != name)
            {
                CurrentAnimation = name;
                currentFrameIndex = 0;
                timer = 0f;
            }
            Console.WriteLine("current animation " + name);
        }

        public void Update(float deltaTime,float FrameRate=12f)
        {
            if (string.IsNullOrEmpty(CurrentAnimation) || !animations.ContainsKey(CurrentAnimation)) return;

            var frames = animations[CurrentAnimation];
            if (frames.Count == 0) return;

            timer += deltaTime;
            float frameDuration = 1f / FrameRate;

            while (timer >= frameDuration)
            {
                timer -= frameDuration;
                currentFrameIndex = (currentFrameIndex + 1) % frames.Count;
            Console.WriteLine("current frame index " + currentFrameIndex);
            }
                

        }

        public void Reset()
        {
            currentFrameIndex = 0;
            timer = 0f;
        }
    }

}


