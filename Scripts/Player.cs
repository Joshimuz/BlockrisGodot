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

	float currentBoostAmount;
	float MaximumBoost = 1f;
	/// <summary>
	/// The amount to multiple delta by every frame for boos regeneration
	/// </summary>
	const float BoostRegenRate = 0.1f;
	
	public Vector2 CurrentDirection;

	Vector2 previousDirection;

	public bool WantsToBoost;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetChild<Area2D>(0).AreaEntered += EnteredCollision;

		currentBoostAmount = MaximumBoost;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Vector2 newPosition = Position;

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

		if (WantsToBoost && currentBoostAmount >= (float)delta)
		{
            newPosition += CurrentDirection * 
				(MovementSpeed * boostMultiplyer * (float)delta);

			// Use boost to a minimum of 0
            currentBoostAmount = MathF.Max(currentBoostAmount -= (float)delta, 0);
        }
		else
		{
            newPosition += CurrentDirection * (MovementSpeed * (float)delta);

            // Regenerate boost to a maximum of MaximumBoost
            currentBoostAmount = MathF.Min(currentBoostAmount 
				+= (float)delta * BoostRegenRate, MaximumBoost);
        }

        newPosition.X = Math.Clamp(newPosition.X, 64, 1080 - 64);

        Position = newPosition;

        previousDirection = CurrentDirection;
		CurrentDirection = Vector2.Zero;
		WantsToBoost = false;

		GameplayController.testText.Text += "\nBoost: " + currentBoostAmount;
    }

	public void EnteredCollision(Area2D otherCollision)
	{
		if (otherCollision.GetParent() is Enemy enemy)
		{
			enemy.OnPlayerHit();
		}
	}
}
