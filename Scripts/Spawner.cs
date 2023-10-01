using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public partial class Spawner : Timer
{
    //List<(byte weight, PackedScene type)> CurrentSpawnableEnimes = new();

    List<ISpawnable> ShuffleBag = new List<ISpawnable>();

    // byte weight, Packed type, float minimumDifficulty, float maximumDifficulty

    public interface ISpawnable
    {
        public string PackedSceneFilePath { get; }

        public byte NumberToSpawn { get; set; }

        public bool SpawnAfter { get; }

        public bool ShouldSpawn();
    }

    List<ISpawnable> spawnables = new List<ISpawnable>();

    public override void _Ready()
    {
		Timeout += TimerUp;
        WaitTime = 1f + GameplayController.RNG.RandfRange(-0.1f, 0.1f);

        // Create the spawnable list
        spawnables.Add(new BasicEnemy.Spawnable());
        spawnables.Add(new SpikeyEnemy.Spawnable());
        spawnables.Add(new DVDEnemy.Spawnable());
        spawnables.Add(new OneUpEnemy.Spawnable());
        spawnables.Add(new TriangleDVDEnemy.Spawnable());
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void TimerUp()
	{
        // If the shuffle bag is empty, go through list of spawnables and
        // ask them to add themselves to the new bag
        if (ShuffleBag.Count == 0)
        {
            foreach (ISpawnable spawnable in spawnables)
            {
                if (spawnable.ShouldSpawn())
                {
                    for (int i = 0; i < spawnable.NumberToSpawn; i++)
                    {
                        ShuffleBag.Add(spawnable);
                    }
                }
            }
        }

        GD.Print("ShuffleBag Contains: ");
        foreach (ISpawnable spawnable in ShuffleBag)
        {
            GD.Print(spawnable.PackedSceneFilePath + ", ");
        }

        int k = GameplayController.RNG.RandiRange(0, ShuffleBag.Count - 1);

        bool spawnAgain = ShuffleBag[k].SpawnAfter;

        Enemy newEmemy = ResourceLoader
                .Load<PackedScene>(ShuffleBag[k].PackedSceneFilePath).Instantiate<Enemy>();

        AddSibling(newEmemy);

        newEmemy.GlobalPosition
             = new Vector2(GameplayController.RNG.RandfRange(0 + 64, 1080 - 64), 0);

        ShuffleBag.RemoveAt(k);
        //newEnemy.Position
            //= new Vector2(GameplayController.RNG.RandfRange(0 + 64, 1080 - 64), 0);

        if (spawnAgain)
        {
            TimerUp();
        }
    }
}
