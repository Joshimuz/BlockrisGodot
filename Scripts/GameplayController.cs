using Godot;
using System;
using System.Collections.Generic;
using static Godot.TextServer;
using System.Linq;
using static GlobalController;

public partial class GameplayController : Node2D
{
    GlobalController globalController;

    public static RandomNumberGenerator RNG = new RandomNumberGenerator();

    public enum GameplayState
    { 
        Intro, //TODO: Add Gameplay Intro stuff
        Running,
        End //TODO: Add Gameplay Outro/End/Deathscreen/Highscore stuff
    }

    [Export] GameplayState currentGameplayState = GameplayState.Intro;

    [Export] PackedScene packedSpawner;

    [Export] PackedScene packedPlayer;

    [Export] public static RichTextLabel testText;

    Vector2 TouchPosition;

    Player player;
    Spawner spawner;

    int i = 100;

    Dictionary<int, Vector2> touchDic = new Dictionary<int, Vector2>();

    public ulong CurrentScore = 0;
    public sbyte CurrentLives = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        globalController = GetNode<GlobalController>("/root/GlobalController");

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
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (CurrentLives <= 0)
        {
            currentGameplayState = GameplayState.End;
            
            // Report the latest score to stats
            Stats.LatestScore = CurrentScore;

            globalController.ChangeGameState(GlobalController.GameState.MainMenu);
        }

        HandlePlayerInput();

        testText.Text = CurrentScore.ToString() + "\n" + CurrentLives.ToString();
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