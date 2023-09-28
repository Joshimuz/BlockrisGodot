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

        defaultScale = 96f;

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
