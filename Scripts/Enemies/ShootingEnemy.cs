using Godot;
using System;
using static Spawner;
using System.Collections.Generic;
using static BasicEnemy;

public partial class ShootingEnemy : Enemy
{
    public override ushort Points { get; set; } = 100;

    Area2D shootingArea;

    Player player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Stats.ShootingEnemiesSeen++;

        shootingArea = GetNode<Area2D>("ShootingArea");

        shootingArea.AreaEntered += EnteredCollision;

        player = GetNode<Player>("/root/GameController/Player");

        shootingArea.GlobalPosition = new Vector2(GlobalPosition.X, player.GlobalPosition.Y);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (shootingArea != null)
        {
            shootingArea.GlobalPosition = new Vector2(GlobalPosition.X,
            player.GlobalPosition.Y - (Scale.Y / 1.25f));
        }
    }

    public void EnteredCollision(Area2D otherCollision)
    {
        // If we entered the shooting block, it's too late to shoot
        if (otherCollision.GetParent() == this)
        {
            shootingArea.QueueFree();
        }

        else if (otherCollision.GetParent() is Player player)
        {
            shootingArea.QueueFree();

            shootingArea = null;

            Enemy newEnemy = ResourceLoader
                .Load<PackedScene>("res://PackedNodes/Enemies/Triangle.tscn")
                .Instantiate<Enemy>();

            newEnemy.GlobalPosition = GlobalPosition;
            newEnemy.MovementSpeedScale = MovementSpeedScale * 12f;
            newEnemy.defaultScale = defaultScale / 4f;
            newEnemy.Points = 0;

            //AddSibling(newEmemy);

            CallDeferred("add_sibling", newEnemy);
        }
    }

    public class Spawnable : Spawner.ISpawnable
    {
        public string PackedSceneFilePath => "res://PackedNodes/Enemies/Shooting.tscn";

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
            (1.6f, 2.3f, 1),
            (1.8f, 2.5f, 1),
            (2f, float.PositiveInfinity, 1),
        };
    }
}
