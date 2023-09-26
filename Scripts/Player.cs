using Godot;
using System;

public partial class Player : Sprite2D
{
	/// <summary>
	/// Movement Speed in Pixels Per Second
	/// </summary>
	[Export] public float MovementSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetChild<Area2D>(0).AreaEntered += EnteredCollision;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void EnteredCollision(Area2D otherCollision)
	{
		if (otherCollision.GetParent() is Enemy enemy)
		{
			enemy.OnPlayerHit();
		}
	}

	public void Move(Vector2 direction, float delta)
	{
		Vector2 newPosition = Position;

		newPosition += direction * (MovementSpeed * delta);

        newPosition.X = Math.Clamp(newPosition.X, 64, 1080 - 64);
    
		Position = newPosition;
	}
}
