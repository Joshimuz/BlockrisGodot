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

    static GameplayState currentGameplayState = GameplayState.Intro;

    [Export] PackedScene packedSpawner;

    [Export] PackedScene packedPlayer;

    [Export] public static RichTextLabel testText;

    TextureProgressBar boostBar;

    Node2D gameOverScreen;
    Node2D gameplayUI;

    Vector2 TouchPosition;

    Player player;
    Spawner spawner;

    int i = 100;

    Dictionary<int, Vector2> touchDic = new Dictionary<int, Vector2>();

    public static ulong Score { get; private set; } = 0 ;
    public static sbyte Lives = 5;
    public static float Difficulty = 1;
    static float HighestDifficulty;
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

        GetNode<TouchScreenButton>("GameplayUI/TouchScreenButton").Pressed += OnPauseButton;
        GetNode<TouchScreenButton>("GameOverScreen/TouchScreenButton").Pressed += OnExitButton;

        // Create a new RNG seed
        RNG.Randomize();

        //TODO: Record the RNG seed and store it somewhere for replays

        testText = GetNode<RichTextLabel>("GameplayUI/RichTextLabel");

        boostBar = GetNode<TextureProgressBar>("GameplayUI/BoostBar");
        boostBar.TintProgress = new Color(1f, 0f, 0f, 0f);

        gameOverScreen = GetNode<Node2D>("GameOverScreen");
        gameplayUI = GetNode<Node2D>("GameplayUI");

        gameOverScreen.Visible = false;
        gameOverScreen.ProcessMode = ProcessModeEnum.Disabled;

        gameplayUI.Visible = false;
        gameplayUI.ProcessMode = ProcessModeEnum.Disabled;

        ChangeGamplayState(GameplayState.Running);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (currentGameplayState == GameplayState.Running)
        {
            if (Lives <= 0)
            {
                ChangeGamplayState(GameplayState.End);
            }

            HandlePlayerInput();

            Difficulty += (float)delta * 0.001f;
            HighestDifficulty = Mathf.Max(HighestDifficulty, Difficulty);
            LerpedDifficulty = Mathf.Lerp(LerpedDifficulty, Difficulty, (float)delta);

            testText.Text = "Score: " + Score.ToString() + "\nLives: " + Lives.ToString()
                + "\nDiff: " + Difficulty;

            Stats.LatestScore = Score;
            Stats.LatestDifficulty = HighestDifficulty;

            HandleUI();
        }
    }

    void ChangeGamplayState(GameplayState requestedGameplayState)
    {
        switch (requestedGameplayState)
        {
            case GameplayState.Intro:
                break;

            case GameplayState.Running:
                spawner = packedSpawner.Instantiate<Spawner>();
                AddChild(spawner);

                player = packedPlayer.Instantiate<Player>();
                AddChild(player);
                player.Position = new Vector2(540, 1600);

                Background.TargetStarMovespeed = 25;

                gameplayUI.Visible = true;
                gameplayUI.ProcessMode = ProcessModeEnum.Inherit;
                break;

            case GameplayState.End:
                spawner.QueueFree();

                gameOverScreen.Visible = true;
                gameOverScreen.ProcessMode = ProcessModeEnum.Inherit;

                gameplayUI.Visible = false;
                gameplayUI.ProcessMode = ProcessModeEnum.Disabled;

                GetNode<RichTextLabel>("GameOverScreen/YouDiedXD").Text
                    += "\n\nScore: " + Score;
                break;
        }

        currentGameplayState = requestedGameplayState;
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

    void HandleUI()
    {
        if (player.currentBoostAmount == player.MaximumBoost)
        {
            boostBar.TintProgress = new Color(0f, 1f, 0f, boostBar.TintProgress.A - 0.01f);
        }
        else
        {
            boostBar.TintProgress = new Color((float)(1 - boostBar.Value), (float)boostBar.Value, 0f, 1f);
        }

        boostBar.Value = player.currentBoostAmount;
        //boostBar.Value = Mathf.Lerp(boostBar.Value, player.currentBoostAmount, 0.1f);
    }

    public static void PlayerScored(uint points)
    {
        if (currentGameplayState != GameplayState.Running)
        {
            return;
        }

        if (points <= 0)
        {
            return;
        }

        ulong pointsToAdd = (ulong)(points * Difficulty);

        Score += pointsToAdd;

        //Difficulty += pointsToAdd / 50000f;

        EnemiesSinceLastIncident++;

        Difficulty += (float)(1 - Math.Exp(-0.05 * Mathf.Min(EnemiesSinceLastIncident, 50))) * 0.007f;
    }

    public static void PlayerFouled(uint points)
    {
        if (currentGameplayState != GameplayState.Running)
        {
            return;
        }

        Lives--;
        Stats.PointsMissed += (ulong)(points * Difficulty);

        if (Lives <= 0)
        {
            return;
        }

        Difficulty = Math.Max(Difficulty - 0.1f, 0.5f);

        EnemiesSinceLastIncident = 0;
    }

    void OnPauseButton()
    {
        globalController.ChangePauseState();
    }

    void OnExitButton()
    {
        globalController.ChangeGameState(GlobalController.GameState.MainMenu);
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
//TODO: Fix out render order and put text above enemies