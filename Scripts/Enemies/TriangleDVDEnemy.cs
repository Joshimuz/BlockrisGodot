using Godot;
using System;
using static Spawner;
using System.Collections.Generic;

public partial class TriangleDVDEnemy : Enemy
{
    public override ushort Points { get; set; } = 200;

    Vector2 currentDirection;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.TriangleDVDEnemiesSeen++;

        if (GameplayController.RNG.Randf() > 0.5f)
        {
            currentDirection = new Vector2(1, 1);
        }
        else
        {
            currentDirection = new Vector2(-1, 1);
        }
    }

    protected override void HandleMovement(float delta, float movementSpeed)
    {
        Position += currentDirection * (movementSpeed * delta);

        if (Position.X < Transform.Scale.X / 2)
        {
            currentDirection = new Vector2(1, 1);
        }
        else if (Position.X > 1080 - Transform.Scale.X / 2)
        {
            currentDirection = new Vector2(-1, 1);
        }

        if (Position.Y > 1920 + (Scale.Y / 2))
        {
            ReachedBottom();
        }
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
        public string PackedSceneFilePath => "res://PackedNodes/Enemies/TriangleDVD.tscn";

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
            (1.4f, 2.1f, 1),
            (1.6f, 2.3f, 1),
            (1.8f, 2.5f, 1),
            (2f, float.PositiveInfinity, 1),
        };
    }
}
