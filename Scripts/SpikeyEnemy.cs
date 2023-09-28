using Godot;
using System;

public partial class SpikeyEnemy : Enemy
{
    protected override ushort Points => 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.SpikeyEnemiesSeen++;
    }

    public override void OnPlayerHit()
    {
        GameplayController.PlayerFouled(Points);

        Stats.BlocksHit++;
        QueueFree();
    }

    protected override void ReachedBottom()
    {
        GameplayController.PlayerScored(Points);

        Stats.BlocksMissed++;
        QueueFree();
    }
}
