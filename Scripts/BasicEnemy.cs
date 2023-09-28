using Godot;
using System;

public partial class BasicEnemy : Enemy
{
    protected override ushort Points => 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.BasicEnemiesSeen++;
    }
}
