using System.Drawing;
using System.Dynamic;
using BlazorGameQuestClassLib.AbstractModels;
using BlazorGameQuestClassLib.Intefaces;

namespace BlazorGameQuestClassLib
{
    public class Player : Interactable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public bool IsBlocked { get; set; } = false;
        public override int Health { get; set; } = 100;
        public override int MaxHealth { get; set; } = 100;

        public override int Damage { get; set; } = 10;
        public bool GoNextDonjon { get; set; } = false;

        public bool OnVent { get; set; } = false;

        public void MoveLeft()
        {
            Move(-1, 0);
            PlayAnimation("walk_left");
        }
        public void MoveUp()
        {
            Move(0, -1);
            PlayAnimation("walk_up");

        }

        public void MoveDown()
        {
            Move(0, 1);
            PlayAnimation("walk_down");

        }

        public void MoveRight()
        {
            Move(1, 0);
            PlayAnimation("walk_right");

        }
        public override void Stop()
        {
            Move(0, 0);

            if (string.IsNullOrEmpty(CurrentAnimation))
                return;

            string direction = GetDirection();

            if (direction != null)
            {
                PlayAnimation($"idle_{direction}");
            }
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
            else if (CurrentAnimation.Contains("up")) direction = "up";
            else if (CurrentAnimation.Contains("down")) direction = "down";
            return direction;
        }
    }

}