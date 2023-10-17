using Godot;
using System;
using static GlobalController;

public partial class MainMenuController : Node2D
{
    GlobalController globalController;

    Node2D mainMenuNode;

    OptionsMenu optionsMenu;

    PackedScene packedOptionsMenu =
        ResourceLoader.Load<PackedScene>("res://PackedNodes/OptionsMenu.tscn");

    TouchScreenButton ReturnToMainButton;

    public enum MenuState
    {
        Main,
        Options,
        Stats, //TODO: Implement Stats Screen
    }

    MenuState currentMenuState = MenuState.Main;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        globalController = GetNode<GlobalController>("/root/GlobalController");

        GetNode<TouchScreenButton>("Main/StartGameButton").Pressed += StartGameButton;
        GetNode<TouchScreenButton>("Main/OptionsMenuButton").Pressed += OptionsMenuButton;
        GetNode<TouchScreenButton>("Main/QuitButton").Pressed += QuitGameButton;

        ReturnToMainButton = GetNode<TouchScreenButton>("ReturnToMain");
        ReturnToMainButton.Pressed += ReturnToMain;
        ReturnToMainButton.Visible = false;
        ReturnToMainButton.ProcessMode = ProcessModeEnum.Disabled;

        Background.TargetStarMovespeed = 1;

        mainMenuNode = GetNode<Node2D>("Main");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void ChangeMenuState(MenuState requestedMenuState)
    {
        switch (requestedMenuState)
        {
            case MenuState.Main:
                mainMenuNode.Visible = true;
                mainMenuNode.ProcessMode = ProcessModeEnum.Inherit;

                if (currentMenuState == MenuState.Options)
                {
                    optionsMenu.QueueFree();
                }

                ReturnToMainButton.Visible = false;
                ReturnToMainButton.ProcessMode = ProcessModeEnum.Disabled;
                break;

            case MenuState.Options:
                mainMenuNode.Visible = false;
                mainMenuNode.ProcessMode = ProcessModeEnum.Disabled;

                optionsMenu = packedOptionsMenu.Instantiate<OptionsMenu>();
                AddChild(optionsMenu);

                ReturnToMainButton.Visible = true;
                ReturnToMainButton.ProcessMode = ProcessModeEnum.Inherit;
                break;
        }

        currentMenuState = requestedMenuState;
    }

	public void StartGameButton()
	{
        globalController.ChangeGameState(GlobalController.GameState.Gameplay);
    }

    public void OptionsMenuButton()
    {
        ChangeMenuState(MenuState.Options);
    }

    public void ReturnToMain()
    {
        ChangeMenuState(MenuState.Main);
    }

    public void QuitGameButton()
    {
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit();
    }
}