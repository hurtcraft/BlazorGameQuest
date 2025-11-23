using BlazorGameQuestClassLib.AbstractModels;

using System.Drawing;
using BlazorGameQuestClassLib.Intefaces;

namespace BlazorGameQuestClassLib
{
    public class Skeleton : Interactable
    {

        private float moveCooldown = 0.4f;
        private float moveTimer = 0f;
        public override int Health { get; set; } = 50;
        public override int MaxHealth { get; set; } = 50;

        public override int Damage { get; set; } = 10;
        private bool CanMove(float deltaTime)
        {
            moveTimer += deltaTime;
            if (moveTimer < moveCooldown)
                return false;

            moveTimer = 0f;
            return true;
        }

        public bool MoveLeft(float deltaTime)
        {
            if (!CanMove(deltaTime))
                return false;

            Move(-1, 0);
            PlayAnimation("walk_left");
            return true;
        }
        public void ResumeWalk()
        {
            if (CurrentAnimation.Contains("left"))
                PlayAnimation("walk_left");
            else
                PlayAnimation("walk_right");
        }

        public bool MoveRight(float deltaTime)
        {
            if (!CanMove(deltaTime))
                return false;

            Move(1, 0);
            PlayAnimation("walk_right");
            return true;
        }

        public void ChangeDirection(float deltaTime)
        {
            if (string.IsNullOrEmpty(CurrentAnimation))
                return;

            if (CurrentAnimation.Contains("left"))
            {
                MoveRight(deltaTime);
                return;
            }

            if (CurrentAnimation.Contains("right"))
            {
                MoveLeft(deltaTime);
                return;
            }
        }

        public void Die()
        {
            PlayAnimation("death");
            currentFrameIndex = 16;

        }
        public void Attack()
        {
            if (string.IsNullOrEmpty(CurrentAnimation))
                return;

            string direction = GetDirection();

            if (direction != null)
            {
                PlayAnimation($"attack_{direction}");
            }

        }
        private string GetDirection()
        {
            string direction = string.Empty;
            if (CurrentAnimation.Contains("left")) direction = "left";
            else if (CurrentAnimation.Contains("right")) direction = "right";
            return direction;
        }



        public override void Interact(IInteractable other)
        {
            if (other is Player p)
            {
                Attack();
                if (currentFrameIndex == 8)
                {
                    p.Health -= this.Damage;
                }
                if (p.CurrentAnimation.Contains("attack"))
                {
                    Health -= p.Damage;
                    if (Health <= 0)
                    {

                        Health = 0;
                        p.Score += MaxHealth;

                    };

                }

            }

        }
    }

}