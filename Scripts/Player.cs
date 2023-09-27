using Godot;
using System;
using static Godot.TextServer;

public partial class Player : Sprite2D
{
	/// <summary>
	/// Movement Speed in Pixels Per Second
	/// </summary>
	[Export] public float MovementSpeed;

    [Export] float boostMultiplyer;

    float currentBoostAmount; //TODO: Implement Boosting amount
	
	public Vector2 CurrentDirection;

	Vector2 previousDirection;

	public bool WantsToBoost;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetChild<Area2D>(0).AreaEntered += EnteredCollision;

		currentBoostAmount = 100;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Vector2 newPosition = Position;

		//TODO: Recover boost every frame (by delta plz)

        if (CurrentDirection != previousDirection)
        {
            //direction changed
            if (CurrentDirection == Vector2.Left)
            {
                Stats.TimesGoneLeft++;
            }
            else if (CurrentDirection == Vector2.Right)
            {
                Stats.TimesGoneRight++;
            }
        }

		if (WantsToBoost && currentBoostAmount > 0)
		{
            newPosition += CurrentDirection * 
				(MovementSpeed * boostMultiplyer * (float)delta);
			//TODO: Consume Boost
        }
		else
		{
            newPosition += CurrentDirection * (MovementSpeed * (float)delta);
        }

        newPosition.X = Math.Clamp(newPosition.X, 64, 1080 - 64);

        Position = newPosition;

        previousDirection = CurrentDirection;
		CurrentDirection = Vector2.Zero;
		WantsToBoost = false;
    }

	public void EnteredCollision(Area2D otherCollision)
	{
		if (otherCollision.GetParent() is Enemy enemy)
		{
			enemy.OnPlayerHit();
		}
	}
}
