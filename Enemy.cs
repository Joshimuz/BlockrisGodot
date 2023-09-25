using Godot;
using System;
using static Godot.TextServer;

public partial class Enemy : Sprite2D
{
	enum enemyType
	{
		Basic,
		Spikey
	}

	[Export] enemyType EnemyType;

    /// <summary>
    /// Movement Speed in Pixels Per Second
    /// </summary>
    [Export] public float MovementSpeed;

	[Export] public ushort Points;

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

	public void OnPlayerHit()
	{
		switch (EnemyType)
		{
			case enemyType.Basic:
				GetParent<GameController>().CurrentScore += Points;
				QueueFree();
				break;

			case enemyType.Spikey:
                //TODO: Lose a life
                QueueFree();
                break;
        }    
	}

	void ReachedBottom()
	{
        //TODO: Lose a life
        QueueFree();
    }
}
