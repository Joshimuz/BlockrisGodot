using Godot;
using System;
using static Spawner;
using System.Collections.Generic;
using static BasicEnemy;

public partial class BasicEnemy : Enemy
{
    public override ushort Points { get; set; } = 100;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.BasicEnemiesSeen++;
    }

    public class Spawnable : Spawner.ISpawnable
    {
        public string PackedSceneFilePath => "res://PackedNodes/Enemies/Basic.tscn";

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
            (0f, 1.2f, 7),
            (1.2f, 1.4f, 6),
            (1.4f, 1.6f, 5),
            (1.6f, 1.8f, 4),
            (1.8f, 2f, 3),
            (2f, 2.2f, 2),
            (2.2f, float.PositiveInfinity, 1),
        };
    }
}
