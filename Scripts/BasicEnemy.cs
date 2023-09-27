using Godot;
using System;

public partial class BasicEnemy : Enemy
{
    public override void OnPlayerHit()
    {
        GetParent<GameplayController>().CurrentScore += Points;
        QueueFree();
    }

    protected override void ReachedBottom()
    {
        GetParent<GameplayController>().CurrentLives--;
        QueueFree();
    }
}
