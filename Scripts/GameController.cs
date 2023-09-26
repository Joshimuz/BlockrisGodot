using Godot;
using System;
using System.Collections.Generic;
using static Godot.TextServer;
using System.Linq;
using static GlobalController;

public partial class GameController : Node2D
{
    GlobalController globalController;

    [Export] PackedScene packedPlayer;
    [Export] PackedScene packedbasicEnemy;

    [Export] RichTextLabel testText;

    Vector2 TouchPosition;
    Vector2 direction;

    Player player;
    Enemy basicEnemy;

    int i = 100;

    Random rng = new Random();

    Dictionary<int, Vector2> touchDic = new Dictionary<int, Vector2>();

    public ulong CurrentScore = 0;
    public sbyte CurrentLives = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        globalController = GetNode<GlobalController>("/root/GlobalController");

        player = packedPlayer.Instantiate<Player>();
        AddChild(player);
        player.Position = new Vector2(540, 1600);

        GetNode<TouchScreenButton>("TouchScreenButton").Pressed += OnPauseButton;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (CurrentLives <= 0)
        {
            globalController.ChangeGameState(GlobalController.GameState.GameplayEnd);
        }

        HandlePlayerMovement((float)delta);

        i--;

        testText.Text = CurrentScore.ToString() + "\n" + CurrentLives.ToString();

        if (i == 0)
        {
            basicEnemy = packedbasicEnemy.Instantiate<Enemy>();
            AddChild(basicEnemy);
            basicEnemy.Position = new Vector2(rng.Next(0 + 64, 1080 - 64), 0);
            i = 100;
        }
    }

    void HandlePlayerMovement(float delta)
    {
        if (touchDic.Count > 0)
        {
            TouchPosition = Vector2.Zero;

            foreach (Vector2 touchThing in touchDic.Values)
            {
                TouchPosition += touchThing;
            }

            TouchPosition /= touchDic.Count;

            Vector2 direction;

            if (TouchPosition.X < 540)
            {
                direction = Vector2.Left;
            }
            else
            {
                direction = Vector2.Right;
            }

            player.Move(direction, delta);
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