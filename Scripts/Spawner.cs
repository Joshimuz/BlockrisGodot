using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public partial class Spawner : Timer
{
    [Export] PackedScene basicEnemy;
    [Export] PackedScene spikeyEnemy;
    [Export] PackedScene DVDEnemy;
    [Export] PackedScene OneUpEnemy;

    //List<(byte weight, PackedScene type)> CurrentSpawnableEnimes = new();

    List<Spawnable> ShuffleBag = new List<Spawnable>();

    // byte weight, Packed type, float minimumDifficulty, float maximumDifficulty

    struct Spawnable
    {
        public PackedScene Type;
        public byte Amount;
        public float MinDiff;
        public float MaxDiff;
        public bool OneTime;
        public bool SpawnAgain;

        public Spawnable(PackedScene type, byte amount, float minDiff, 
            float maxDiff, bool oneTime = false, bool spawnAgain = false)
        {
            Type = type; Amount = amount; MinDiff = minDiff; 
            MaxDiff = maxDiff; OneTime = oneTime; SpawnAgain = spawnAgain;
        }
    };

    List<Spawnable> spawnables;

    public override void _Ready()
    {
		Timeout += TimerUp;
        WaitTime = 1f + GameplayController.RNG.RandfRange(-0.1f, 0.1f);

        // Create the spawnable list
        InitSpawnables();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void TimerUp()
	{
        Enemy newEnemy;

        if (ShuffleBag.Count == 0)
        {
            for (int z = 0; z < spawnables.Count; z++)
            {
                if (GameplayController.Difficulty < spawnables[z].MinDiff
                    || GameplayController.Difficulty > spawnables[z].MaxDiff)
                {
                    continue;
                }

                for (int i = 0; i < spawnables[z].Amount; i++)
                {
                    ShuffleBag.Add(spawnables[z]);
                }
            }

            // Remove all spawnables that were to use only one time that got added
            spawnables.RemoveAll((x) => GameplayController.Difficulty > x.MinDiff
                    && GameplayController.Difficulty < x.MaxDiff && x.OneTime);
        }

        int k = GameplayController.RNG.RandiRange(0, ShuffleBag.Count - 1);
        newEnemy = ShuffleBag[k].Type.Instantiate<Enemy>();

        bool spawnAgain = ShuffleBag[k].SpawnAgain;

        ShuffleBag.RemoveAt(k);
        AddSibling(newEnemy);
        newEnemy.Position
            = new Vector2(GameplayController.RNG.RandfRange(0 + 64, 1080 - 64), 0);

        if (spawnAgain)
        {
            TimerUp();
        }
    }

    void InitSpawnables()
    {
        spawnables = new List<Spawnable>
        {
            new(basicEnemy, 7, 0f, 1.25f),
            new(basicEnemy, 6, 1.25f, 1.5f),
            new(basicEnemy, 5, 1.5f, 1.75f),
            new(basicEnemy, 4, 1.75f, 2f),
            new(basicEnemy, 3, 2f, float.PositiveInfinity),

            new(spikeyEnemy, 1, 1.05f, 2f),
            new(spikeyEnemy, 2, 1.1f, 2f),
            new(spikeyEnemy, 2, 1.25f, float.PositiveInfinity),

            new(OneUpEnemy, 1, 1.25f, 1.5f, true, true),
            new(OneUpEnemy, 1, 1.5f, 1.75f, true, true),
            new(OneUpEnemy, 1, 1.75f, 2f, true, true),
            new(OneUpEnemy, 1, 2f, 2.25f, true, true),
            new(OneUpEnemy, 1, 2.25f, 2.5f, true, true),
            new(OneUpEnemy, 1, 2.5f, 2.75f, true, true),

            new(DVDEnemy, 1, 1.5f, float.PositiveInfinity),
            new(DVDEnemy, 2, 1.6f, float.PositiveInfinity),
            new(DVDEnemy, 2, 1.7f, float.PositiveInfinity),
        };
    }

    //TODO: Try new spawning method described below:
    //Make a class for each possible spawnable enemy and create a list with every enemy in it
    //starting from least likely to spawn to most likely. Make a function for SpawningConditions
    //and SpawningProceedure
    //Get a random RNG "weight" value
    //Iterate over the list and check if SpawningCondition == true
    //If so call SpawningProceedure()
    //SpawningCondition can be basic stuff like if the RNG "weight" value is correct or not
    //but can also be more complex stuff like "if Difficulty reached this threashold spawn regardless"
    //SpawningProceedure can be basic stuff like "hurr durr spawn enemy" but it could do more
    //like maybe spawning two! Or in the case of a OneUp it could return false to let
    //the Spawner know to keep iterating through the loop instead of breaking
}
