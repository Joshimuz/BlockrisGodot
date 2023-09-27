using Godot;
using System;
using static Godot.TextServer;

public abstract partial class Enemy : Sprite2D
{
    /// <summary>
    /// Movement Speed in Pixels Per Second
    /// </summary>
    [Export] protected float MovementSpeed;

	[Export] protected ushort Points;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position += new Vector2(0, 1) * (MovementSpeed * (float)delta);

		if (Position.Y > 1920+64)
		{
			ReachedBottom();
        }
    }

    public abstract void OnPlayerHit();

    protected abstract void ReachedBottom();
}
