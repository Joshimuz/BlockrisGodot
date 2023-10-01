using Godot;
using System;
using static Spawner;
using System.Collections.Generic;
using static BasicEnemy;

public partial class BasicEnemy : Enemy
{
    protected override ushort Points => 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.BasicEnemiesSeen++;
    }

    public class Spawnable : Spawner.ISpawnable
    {
        public string PackedSceneFilePath => "res://PackedNodes/BasicEnemy.tscn";

        public byte NumberToSpawn { get; set; } = 7;

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
            (0f, 1.25f ,7),
            (1.25f, 1.5f, 6),
            (1.5f, 1.75f, 5),
            (1.75f, 2f, 4),
            (2f, 2.25f, 3),
            (2f, 2.5f, 2),
            (2f, float.PositiveInfinity, 1),
        };
    }
}
