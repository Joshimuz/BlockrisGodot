using Godot;
using System;

public partial class BasicEnemy : Enemy
{
    public override void OnPlayerHit()
    {
        GetParent<GameplayController>().CurrentScore += Points;

        Stats.BlocksHit++;
        QueueFree();
    }

    protected override void ReachedBottom()
    {
        GetParent<GameplayController>().CurrentLives--;

        Stats.BlocksMissed++;
        QueueFree();
    }
}
