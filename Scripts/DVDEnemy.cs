using Godot;
using System;

public partial class DVDEnemy : Enemy
{
    protected override ushort Points => 200;

    Vector2 currentDirection;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.DVDEnemiesSeen++;

        if (GameplayController.RNG.Randf() > 0.5f)
        {
            currentDirection = new Vector2(1, 1);
        }
        else
        {
            currentDirection = new Vector2(-1, 1);
        }
    }

    protected override void HandleMovement(float delta, float movementSpeed)
    {
        Position += currentDirection * (movementSpeed * delta);

        if (Position.X < Transform.Scale.X / 2)
        {
            currentDirection = new Vector2(1, 1);
        }
        else if (Position.X > 1080 - Transform.Scale.X / 2)
        {
            currentDirection = new Vector2(-1, 1);
        }

        if (Position.Y > 1920 + 64)
        {
            ReachedBottom();
        }
    }
}
