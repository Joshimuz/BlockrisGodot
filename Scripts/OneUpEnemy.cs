using Godot;
using System;

public partial class OneUpEnemy : Enemy
{
    protected override ushort Points => 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        MovementSpeedScale = 6.25f;

        // 128 / 1.333 = 96, which was the desired size of the enemy
        // doing it like this to keep the random variation set in base._Ready()
        defaultScale = defaultScale / 1.333f;

        Stats.OneUpEnemiesSeen++;
    }

    public override void OnPlayerHit()
    {
        GameplayController.Lives++;

        Stats.BlocksHit++;
        QueueFree();
    }

    protected override void ReachedBottom()
    {
        Stats.BlocksMissed++;
        QueueFree();
    }
}
