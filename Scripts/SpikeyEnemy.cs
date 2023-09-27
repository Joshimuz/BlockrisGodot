using Godot;
using System;

public partial class SpikeyEnemy : Enemy
{
    public override void OnPlayerHit()
    {
        GetParent<GameplayController>().CurrentLives--;

        Stats.BlocksHit++;
        QueueFree();
    }

    protected override void ReachedBottom()
    {
        GetParent<GameplayController>().CurrentScore += Points;

        Stats.BlocksMissed++;
        QueueFree();
    }
}
