using Godot;
using System;
using static Spawner;
using System.Collections.Generic;

public partial class OneUpEnemy : Enemy
{
    public override ushort Points { get; set; } = 0;

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
        GameplayController.AddLife();

        Stats.BlocksHit++;
        QueueFree();
    }

    protected override void ReachedBottom()
    {
        Stats.BlocksMissed++;
        QueueFree();
    }

    public class Spawnable : Spawner.ISpawnable
    {
        ulong ScoreLastSpawnedAt = 0;

        float DifficulyLastSpawnedAt = 1f;

        public string PackedSceneFilePath => "res://PackedNodes/Enemies/OneUp.tscn";

        public byte NumberToSpawn { get; set; } = 1;

        public bool SpawnAfter => true;

        public bool ShouldSpawn()
        {
            if (GameplayController.Score > ScoreLastSpawnedAt + 4444
                && GameplayController.Difficulty > DifficulyLastSpawnedAt + 0.1f)
            {
                ScoreLastSpawnedAt = GameplayController.Score;
                DifficulyLastSpawnedAt = GameplayController.Difficulty;
                return true;
            }

            return false;
        }
    }
}
