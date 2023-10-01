using Godot;
using System;
using System.Collections.Generic;
using static Godot.TextServer;
using static GlobalController;

public partial class GameplayController : Node2D
{
    GlobalController globalController;

    public static RandomNumberGenerator RNG = new RandomNumberGenerator();

    enum GameplayState
    {
        Intro, //TODO: Add Gameplay Intro stuff
        Running,
        End //TODO: Add Gameplay Outro/End/Deathscreen/Highscore stuff
    }

    GameplayState currentGameplayState = GameplayState.Intro;

    [Export] PackedScene packedSpawner;

    [Export] PackedScene packedPlayer;

    [Export] public static RichTextLabel testText;

    Vector2 TouchPosition;

    Player player;
    Spawner spawner;

    int i = 100;

    Dictionary<int, Vector2> touchDic = new Dictionary<int, Vector2>();

    public static ulong Score { get; private set; } = 0 ;
    public static sbyte Lives = 5;
    public static float Difficulty = 1;
    public static float LerpedDifficulty = 1;
    static uint EnemiesSinceLastIncident = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        globalController = GetNode<GlobalController>("/root/GlobalController");

        Score = 0;
        Lives = 5;
        Difficulty = 1f;
        LerpedDifficulty = 1;
        EnemiesSinceLastIncident = 0;

        currentGameplayState = GameplayState.Running;

        spawner = packedSpawner.Instantiate<Spawner>();
        AddChild(spawner);

        player = packedPlayer.Instantiate<Player>();
        AddChild(player);
        player.Position = new Vector2(540, 1600);

        GetNode<TouchScreenButton>("TouchScreenButton").Pressed += OnPauseButton;

        // Create a new RNG seed
        RNG.Randomize();

        //TODO: Record the RNG seed and store it somewhere for replays

        testText = GetNode<RichTextLabel>("RichTextLabel");

        Background.TargetStarMovespeed = 10;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (Lives <= 0)
        {
            currentGameplayState = GameplayState.End;

            globalController.ChangeGameState(GlobalController.GameState.MainMenu);
        }

        HandlePlayerInput();

        Difficulty += (float)delta * 0.001f;
        LerpedDifficulty = Mathf.Lerp(LerpedDifficulty, Difficulty, (float)delta);

        testText.Text = "Score: " + Score.ToString() + "\nLives: " + Lives.ToString()
            + "\nDiff: " + Difficulty;

        Stats.LatestScore = Score;
        Stats.LatestDifficulty = Difficulty;
    }

    void HandlePlayerInput()
    {
        if (touchDic.Count > 0)
        {
            TouchPosition = Vector2.Zero;

            foreach (Vector2 touchThing in touchDic.Values)
            {
                TouchPosition += touchThing;
            }

            TouchPosition /= touchDic.Count;

            if (TouchPosition.X < 540)
            {
                player.CurrentDirection = Vector2.Left;
            }
            else
            {
                player.CurrentDirection = Vector2.Right;
            }

            if (TouchPosition.X < 200 || TouchPosition.X > 1080-200)
            {
                player.WantsToBoost = true;
            }
        }
    }

    public static void PlayerScored(uint points)
    {
        ulong pointsToAdd = (ulong)(points * Difficulty);

        Score += pointsToAdd;

        //Difficulty += pointsToAdd / 50000f;

        EnemiesSinceLastIncident++;

        Difficulty += (float)(1 - Math.Exp(-0.05 * Mathf.Min(EnemiesSinceLastIncident, 25))) * 0.007f;
    }

    public static void PlayerFouled(uint points)
    {
        Lives--;
        Stats.PointsMissed += (ulong)(points * Difficulty);

        if (Lives <= 0)
        {
            return;
        }

        Difficulty = Math.Max(Difficulty * 0.9f, 1);

        EnemiesSinceLastIncident = 0;
    }

    void OnPauseButton()
    {
        globalController.ChangePauseState();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            //touchThings.Add(new TouchThing(touchEvent.Position, touchEvent.Pressed, false));
            if (touchEvent.Pressed == false)
            {
                touchDic.Remove(touchEvent.Index);
            }
            else
            {
                touchDic[touchEvent.Index] = touchEvent.Position;
            }
        }
        else if (@event is InputEventScreenDrag dragEvent)
        {
            touchDic[dragEvent.Index] = dragEvent.Position;
        }

        if (@event is InputEventKey keyEvent
            && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
        {
            globalController.ChangePauseState();
        }
    }
}


//TODO: Implement alternate/slider control scheme
//TODO: Implement some kind of difficulty scaling
//TODO: Fix out render order and put text above enemies
//TODO: Also track the highest-latest difficulty