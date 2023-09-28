using Godot;
using System;
using static Godot.TextServer;

public partial class Player : Sprite2D
{
	/// <summary>
	/// Movement Speed in pixels per second, scaled by the size of the player * MovementSpeedScale
	/// </summary>
	float MovementSpeed;
	/// <summary>
	/// The amount to times the scale of the player by, for MovementSpeed, use this to change movement speed
	/// </summary>
	float MovementSpeedScale = 6.25f;

    float boostMultiplyer = 2;

	float currentBoostAmount;
	float MaximumBoost = 1f;
	/// <summary>
	/// The amount to multiple delta by every frame for boos regeneration
	/// </summary>
	const float BoostRegenRate = 0.25f;
	
	public Vector2 CurrentDirection;

	Vector2 previousDirection;

	public bool WantsToBoost;

	float defaultScale = 128f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetChild<Area2D>(0).AreaEntered += EnteredCollision;

		currentBoostAmount = MaximumBoost;

		Scale = new Vector2(defaultScale, defaultScale);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float newScale = defaultScale / GameplayController.LerpedDifficulty;
		Scale = new Vector2(newScale, newScale);

		MovementSpeed = MovementSpeedScale * (Transform.Scale.X * 1.1f);

		Position = new Vector2(Position.X, 1660 - (Transform.Scale.Y / 2));

        HandleMovement(delta);

        GameplayController.testText.Text += "\nBoost: " + currentBoostAmount;
    }

	void HandleMovement(double delta)
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

			Stats.SecondsBoosted += delta;
        }
		else
		{
            newPosition += CurrentDirection * (MovementSpeed * (float)delta);

            // Regenerate boost to a maximum of MaximumBoost
            currentBoostAmount = MathF.Min(currentBoostAmount 
				+= (float)delta * BoostRegenRate, MaximumBoost);
        }

        newPosition.X = Math.Clamp(newPosition.X, Transform.Scale.X / 2, 
			1080 - Transform.Scale.X / 2);

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
//TODO: Figure out how to handle sprite sizes for scaling instead of Transform scale alone
