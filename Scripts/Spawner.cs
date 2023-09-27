using Godot;
using System;

public partial class Spawner : Timer
{
    [Export] PackedScene packedBasicEnemy;
    [Export] PackedScene packedSpikeyEnemy;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Timeout += TimerUp;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void TimerUp()
	{
        Enemy newEnemy;

        if (GameplayController.RNG.Randf() > 0.5)
        {
            newEnemy = packedBasicEnemy.Instantiate<Enemy>();
        }
        else
        {
            newEnemy = packedSpikeyEnemy.Instantiate<Enemy>();
        }

        AddSibling(newEnemy);
        newEnemy.Position 
            = new Vector2(GameplayController.RNG.RandfRange(0 + 64, 1080 - 64), 0);
    }
}
