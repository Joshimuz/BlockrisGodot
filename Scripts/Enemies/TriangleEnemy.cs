using Godot;
using System;
using static Spawner;
using System.Collections.Generic;

public partial class TriangleEnemy : Enemy
{
    public override ushort Points { get; set; } = 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.TriangleEnemiesSeen++;
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

    public class Spawnable : Spawner.ISpawnable
    {
        public string PackedSceneFilePath => "res://PackedNodes/Enemies/Triangle.tscn";

        public byte NumberToSpawn { get; set; } = 0;

        public bool SpawnAfter => false;

        public bool ShouldSpawn()
        {
            NumberToSpawn = 0;

            bool returnValue = false;

            foreach ((float MinDiff, float MaxDiff, byte NumberToSpawn) 
                condition in spawnConditions)
            {
                if (GameplayController.Difficulty > condition.MinDiff
                    && GameplayController.Difficulty < condition.MaxDiff)
                {
                    NumberToSpawn += condition.NumberToSpawn;
                    returnValue = true;
                }
            }

            return returnValue;
        }

        List<(float MinDiff, float MaxDiff, byte NumberToSpawn)> spawnConditions = new()
        {
            (1.05f, 1.5f, 1),
            (1.1f, 1.6f, 1),
            (1.2f, 1.7f, 1),
            (1.3f, float.PositiveInfinity, 1),
        };
    }
}
