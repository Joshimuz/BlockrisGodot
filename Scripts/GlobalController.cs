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

    [Export] GameState currentGameState = GameState.MainMenu;

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
    }

    public void ChangeGameState(GameState requestedGameState)
    {
        if (requestedGameState == currentGameState) return;

        //TODO: statscontroller.commitToDisk()

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

    public void ChangePauseState()
    {
        if (previousPaused != GetTree().Paused)
        {
            return;
        }

        if (currentGameState == GameState.Gameplay)
        {
            if (!GetTree().Paused)
            {
                pauseMenu = GD.Load<PackedScene>("res://PackedNodes/PauseMenu.tscn")
                    .Instantiate<PauseMenu>();

                GetNode("/root/GameController").AddChild(pauseMenu);
                GetTree().Paused = true;
            }
            else
            {
                pauseMenu.QueueFree();
                GetTree().Paused = false;
            }
        }
        else
        {
            GetTree().Paused = !GetTree().Paused;
        }
    }
}

//TODO: Add Highscores File Saving Stuff