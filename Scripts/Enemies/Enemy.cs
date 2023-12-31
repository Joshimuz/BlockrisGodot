using Godot;
using System;
using static Godot.TextServer;

public abstract partial class Enemy : Sprite2D
{
    /// <summary>
    /// Movement Speed in pixels per second, scaled by the size of the enemy * MovementSpeedScale
    /// </summary>
    protected float MovementSpeed;
    /// <summary>
    /// The amount to times the scale of the enemy by, for MovementSpeed, use this to change movement speed
    /// </summary>
    //protected float MovementSpeedScale = 6.25f;
    public float MovementSpeedScale = 3.125f;

    public abstract ushort Points { get; set; }

    public float defaultScale = 128f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Apply slight random scaling for variation in both size and speed
        defaultScale += GameplayController.RNG.RandfRange(-8f, 8f);

        Scale = new Vector2(defaultScale, defaultScale);

        // Apply slight random roation for variation
        RotationDegrees += GameplayController.RNG.RandfRange(-2f, 2f);
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

        if (Position.Y > GlobalController.AnchorB(1920) + (Scale.Y / 2))
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