using Godot;
using System;

public partial class PauseMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<TouchScreenButton>("UnpauseButton").Pressed += OnUnpauseButton;
        GetNode<TouchScreenButton>("MainMenuButton").Pressed += OnMainMenuButton;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void OnUnpauseButton()
    {
        GetNode<GlobalController>("/root/GlobalController")
                .ChangePauseState();
    }

    public void OnMainMenuButton()
    {
        GetNode<GlobalController>("/root/GlobalController")
            .ChangeGameState(GlobalController.GameState.MainMenu);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent
            && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
        {
            GetNode<GlobalController>("/root/GlobalController")
                .ChangePauseState();
        }
    }
}
