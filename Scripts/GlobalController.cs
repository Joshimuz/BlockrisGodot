using Godot;
using System;

public partial class GlobalController : Node
{
    public enum GameState
    {
        Intro,
        MainMenu,
        Gameplay
    }

    GameState currentGameState = GameState.MainMenu;

    PauseMenu pauseMenu;

    bool previousPaused;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ProcessMode = Node.ProcessModeEnum.Always;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        previousPaused = GetTree().Paused;

        Stats.SecondsPlayed += delta;
    }

    public void ChangeGameState(GameState requestedGameState)
    {
        if (requestedGameState == currentGameState) return;

        Stats.CommitToDisk();

        switch (requestedGameState)
        {
            case GameState.Intro:
                //TODO: Add an intro
                break;

            case GameState.MainMenu:
                GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
                GetTree().Paused = false;
                break;

            case GameState.Gameplay:
                GetTree().ChangeSceneToFile("res://Scenes/gameplay.tscn");
                GetTree().Paused = false;
                break;
        }

        currentGameState = requestedGameState;
    }

    public void ChangePauseState(bool pause)
    {
        if (previousPaused != GetTree().Paused 
            || currentGameState != GameState.Gameplay)
        {
            return;
        }

        if (pause && !GetTree().Paused)
        {
            pauseMenu = GD.Load<PackedScene>("res://PackedNodes/PauseMenu.tscn")
                .Instantiate<PauseMenu>();

            GetNode("/root/GameController").AddChild(pauseMenu);

            GetTree().Paused = true;
        }
        else if (!pause && GetTree().Paused)
        {
            pauseMenu.QueueFree();

            GetTree().Paused = false;
        }
    }
    public void ChangePauseState()
    {
        ChangePauseState(!GetTree().Paused);
    }

    public override void _Notification(int notification)
    {
        if (notification == NotificationWMCloseRequest
            || notification == NotificationWMGoBackRequest)
        {
            Stats.CommitToDisk();
        }

        if (notification == NotificationApplicationFocusOut
            || notification == NotificationWMWindowFocusOut)
        {
            ChangePauseState(true);
        }
    }
}

//TODO: Figure out how to center the game in the viewport instead of being to the top left