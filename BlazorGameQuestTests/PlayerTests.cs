using BlazorGameQuestClassLib;
using BlazorGameQuestClassLib.AbstractModels;

namespace BlazorGameQuestTests;

// Tests unitaires pour la classe Player : initialisation, mouvements, animations, attaques et collisions
public class PlayerTests
{
    [Fact]
    public void Player_Initialisation_ShouldHaveDefaultValues()
    {
        var player = new Player();

        Assert.Equal(0, player.Id);
        Assert.Equal(string.Empty, player.Name);
        Assert.Equal(0, player.Score);
        Assert.False(player.IsBlocked);
        Assert.True(player.IsActive);
        Assert.Equal(0f, player.X);
        Assert.Equal(0f, player.Y);
        Assert.Equal(1.0f, player.Speed);
    }

    [Fact]
    public void Player_MoveLeft_ShouldDecreaseX()
    {
        var player = new Player { X = 10, Y = 10 };
        float initialX = player.X;

        player.MoveLeft();

        Assert.Equal(initialX - 1, player.X);
        Assert.Equal(10, player.Y);
    }

    [Fact]
    public void Player_MoveRight_ShouldIncreaseX()
    {
        var player = new Player { X = 10, Y = 10 };
        float initialX = player.X;

        player.MoveRight();

        Assert.Equal(initialX + 1, player.X);
        Assert.Equal(10, player.Y);
    }

    [Fact]
    public void Player_MoveUp_ShouldDecreaseY()
    {
        var player = new Player { X = 10, Y = 10 };
        float initialY = player.Y;

        player.MoveUp();

        Assert.Equal(10, player.X);
        Assert.Equal(initialY - 1, player.Y);
    }

    [Fact]
    public void Player_MoveDown_ShouldIncreaseY()
    {
        var player = new Player { X = 10, Y = 10 };
        float initialY = player.Y;

        player.MoveDown();

        Assert.Equal(10, player.X);
        Assert.Equal(initialY + 1, player.Y);
    }

    [Fact]
    public void Player_Stop_ShouldNotMove()
    {
        var player = new Player { X = 10, Y = 10 };
        player.AddAnimation("walk_left", new List<string> { "sprite1.png" });
        player.PlayAnimation("walk_left");

        player.Stop();

        Assert.Equal(10, player.X);
        Assert.Equal(10, player.Y);
    }

    [Fact]
    public void Player_Attack_ShouldPlayAttackAnimation()
    {
        var player = new Player();
        player.AddAnimation("walk_left", new List<string> { "sprite1.png" });
        player.AddAnimation("attack_left", new List<string> { "attack1.png" });
        player.PlayAnimation("walk_left");

        player.Attack();

        Assert.Equal("attack_left", player.CurrentAnimation);
    }

    [Fact]
    public void Player_Attack_WithoutCurrentAnimation_ShouldNotChangeAnimation()
    {
        var player = new Player();
        player.AddAnimation("attack_left", new List<string> { "attack1.png" });

        player.Attack();

        Assert.Equal(string.Empty, player.CurrentAnimation);
    }

    [Fact]
    public void Player_Move_WithCustomSpeed_ShouldRespectSpeed()
    {
        var player = new Player { Speed = 2.0f, X = 0, Y = 0 };

        player.MoveRight();

        Assert.Equal(2.0f, player.X);
    }

    [Fact]
    public void Player_CheckCollision_WithOverlappingBounds_ShouldReturnTrue()
    {
        var player1 = new Player { X = 0, Y = 0 };
        var player2 = new Player { X = 10, Y = 10 };

        bool collision = player1.CheckCollision(player2);

        Assert.True(collision);
    }

    [Fact]
    public void Player_CheckCollision_WithNonOverlappingBounds_ShouldReturnFalse()
    {
        var player1 = new Player { X = 0, Y = 0 };
        var player2 = new Player { X = 100, Y = 100 };

        bool collision = player1.CheckCollision(player2);

        Assert.False(collision);
    }
}
