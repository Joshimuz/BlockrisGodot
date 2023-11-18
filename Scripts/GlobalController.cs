using Godot;
using System;
using System.Diagnostics;

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

    public static float AspectRatioNumber;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CalcAspectRatioNumber();

        ProcessMode = Node.ProcessModeEnum.Always;
    }

    public static void CalcAspectRatioNumber()
    {
        AspectRatioNumber = (float)DisplayServer.WindowGetSize().Y
            / (float)DisplayServer.WindowGetSize().X;

        GD.Print(AspectRatioNumber.ToString());
    }

    /// <summary>
    /// Calculate the actual Y position needed to Anchor to the Botton of the screen
    /// </summary>
    /// <param name="OGValue"></param>
    /// <returns></returns>
    public static float AnchorB(float OGValue)
    {
        if (AspectRatioNumber <= 1.7777778f)
        {
            return OGValue;
        }

        return (OGValue / 1.7777778f) * AspectRatioNumber;
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
            || currentGameState != GameState.Gameplay
            || GameplayController.currentGameplayState 
            != GameplayController.GameplayState.Running)
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

            // If the game is in Windowed mode, get the current position of the window
            // and store it for next time
            if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed)
            {
                ProjectSettings.SetSetting("display/window/size/initial_position",
                    DisplayServer.WindowGetPosition());

                ProjectSettings.SaveCustom("override.cfg");
            }
        }

        if (notification == NotificationApplicationFocusOut
            || notification == NotificationWMWindowFocusOut)
        {
            ChangePauseState(true);
        }
    }
}

//TODO: Figure out how to center the game in the viewport instead of being to the top left