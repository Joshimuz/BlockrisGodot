using Godot;
using System;
using static Godot.TextServer;

public abstract partial class Enemy : Sprite2D
{
    /// <summary>
    /// Movement Speed in pixels per second, scaled by the size of the player * MovementSpeedScale
    /// </summary>
    protected float MovementSpeed;
    /// <summary>
    /// The amount to times the scale of the player by, for MovementSpeed, use this to change movement speed
    /// </summary>
    //protected float MovementSpeedScale = 6.25f;
    protected float MovementSpeedScale = 3.125f;

    protected abstract ushort Points { get; }

    protected float defaultScale = 128f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Scale = new Vector2(defaultScale, defaultScale);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        float newScale = defaultScale / GameplayController.LerpedDifficulty;
        Scale = new Vector2(newScale, newScale);

        MovementSpeed = MovementSpeedScale * Transform.Scale.X;

        HandleMovement((float)delta, MovementSpeed);
    }

    protected virtual void HandleMovement(float delta, float movementSpeed)
    {
        Position += Vector2.Down * (movementSpeed * delta);

        if (Position.Y > 1920 + 64)
        {
            ReachedBottom();
        }
    }


    public virtual void OnPlayerHit()
    {
        GameplayController.PlayerScored(Points);

        Stats.BlocksHit++;
        QueueFree();
    }

    protected virtual void ReachedBottom()
    {
        GameplayController.PlayerFouled(Points);

        Stats.BlocksMissed++;
        QueueFree();
    }
}